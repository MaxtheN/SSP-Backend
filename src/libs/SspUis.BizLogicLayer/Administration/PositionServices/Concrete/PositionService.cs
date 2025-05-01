using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.IO;
using System.Linq;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.PositionServices
{
    public class PositionService
        : BaseEntityService<Position, PositionListDto, PositionDto, CreatePositionDlDto, UpdatePositionDlDto, IPositionRepository>
        , IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;

        public PositionService(IUnitOfWork unitOfWork, IAuthService authService, IStorageService storageService, ICultureHelper cultureHelper)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _storageService = storageService;
            _cultureHelper = cultureHelper;
        }

        protected override IQueryable<PositionListDto> SortFilter(IQueryable<PositionListDto> query, SortFilterPageOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options).Where(a => a.StateId == StateIdConst.ACTIVE);
        }

        public override PagedResult<PositionListDto> GetList(SortFilterPageOptions options)
        {
            var result = SortFilter(GetQuery<PositionListDto>(), options)
                               .AsPagedResult(options);
            return result;
        }

        public override PositionDto Get()
        {
            var dto = base.Get();
            return dto;
        }

        public override PositionDto Get(int id)
        {
            var dto = GetQuery<PositionDto>().FirstOrDefault(a => a.Id == id);
            if (dto == null)
                AddError("Запись не найденà");
            return dto;
        }

        //public SelectList<int> AllAsSelectList()
        //{
        //    return Repository.AllAsQueryable.Include(a => a.Translates).AllAsSelectList();
        //}
        public SelectList<int> AsSelectList(bool fromOrganizationalStructure = false)
        {
            if (fromOrganizationalStructure)
            {
                var organizationalStructureId = _authService.Organization.OrganizationalStructureId;

                if (!organizationalStructureId.HasValue)
                    return new SelectList<int>();

                var organizationalStructure = Repository.Context
                                                    .Set<OrganizationalStructure>()
                                                    .FirstOrDefault(a => a.Id == organizationalStructureId);

                if (organizationalStructure != null)
                {
                    Repository.Context.Entry(organizationalStructure)
                                       .Collection(a => a.StructurePosition)
                                       .Query()
                                       .Include(a => a.Position).ThenInclude(a => a.Translates)
                                       .Include(a => a.PositionType).ThenInclude(a => a.Translates)
                                       .Include(a => a.PositionCategory).ThenInclude(a => a.Translates)
                                       .Include(a => a.TariffScale)//.ThenInclude(a => a.Translates) // translate include qilinsa error chiqyapti
                                       .Include(a => a.Rank)
                                       .Load();

                    if (!organizationalStructure!.StructurePosition.Any())
                        return new SelectList<int>();

                    return organizationalStructure.StructurePosition.AsSelectList();
                }
            }

            return Repository.AllAsQueryable.Include(a => a.Translates)
                              .AsSelectList();
        }

        public Stream SaveAsExecel(TableSortFilterPageOptions dto)
        {
            var data = GetQuery<PositionDto>()
                        //.SortFilter(dto)
                        .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.POSITIONS));

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            if (IsValid && data != null)
            {
                var namerange = excelPackage.Workbook.Names["ImportRow1"];
                var currentrow = namerange.Start.Row;
                var ws = namerange.Worksheet;
                int i = 1;
                foreach (var item in data)
                {
                    ws.InsertRow(currentrow, 1, namerange.Start.Row);
                    ws.Cells[currentrow, 1].Value = i++;
                    ws.Cells[currentrow, 2].Value = item.FullName;
                    //ws.Cells[currentrow, 3].Value = item.ParentOrganization; // TODO: exceldan olish kerak shu ustunni
                    ws.Cells[currentrow, 4].Value = item.State;
                    currentrow++;
                }
                ws.DeleteRow(namerange.Start.Row, 1);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }

        public override HaveId<int> Create(CreatePositionDlDto dto)
        {
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var entity = Repository.Create(dto);
                CombineStatuses(Repository);

                if (IsValid)
                {
                    _unitOfWork.Save();

                    ///Edoc schema uchun Departmenti sync qilish.
                    if (!HasErrors)
                        CreatePositionForEdocSchema(entity);

                    if (HasErrors)
                        return null;
                     transaction.Commit();

                    return HaveId.Create(entity.Id);
                }
            }
            catch (Exception e)
            {
                AddError(e.Message);
                _unitOfWork.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
            return null;
        }

        public override void Update(UpdatePositionDlDto dto)
        {
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var entity = Repository.Update(dto);
                CombineStatuses(Repository);
                if (IsValid)
                {
                    _unitOfWork.Save();
                    ///Edoc schema uchun Departmenti sync qilish.
                    if (!HasErrors)
                        UpdatePositionForEdocSchema(entity);

                    if (HasErrors)
                        return;
                      transaction.Commit();
                }
            }
            catch (Exception e)
            {
                AddError(e.Message);
                _unitOfWork.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private IQueryable<TDto> GetQuery<TDto>()
          where TDto : class
        {
            return Repository.ReadAsNoTracked<TDto>();
        }

        #region Edoc Position

        public void SyncEdocPosition(IDbContextTransaction outTransaction = null)
        {
            var departments = _unitOfWork.Context.Set<Position>().ToList();

            var transaction = outTransaction == null ? _unitOfWork.BeginTransaction() : outTransaction;
            try
            {
                foreach (var item in departments)
                {
                    CreatePositionForEdocSchema(item);
                }
                if (HasErrors)
                    return;

                if (outTransaction == null)
                    transaction.Commit();
            }
            catch (Exception e)
            {
                AddError(e.Message);
                _unitOfWork.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private void CreatePositionForEdocSchema(Position dto)
        {
            try
            {
                var exsist = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Position>()
                                        .Any(x => x.Id == dto.Id);
                if (!exsist)
                {
                    var position = new Ssp.DataLayer.EFClasses.Edoc.Position()
                    {
                        Id = dto.Id,
                        OrderCode = dto.OrderCode,
                        DateOfCreated = DateTime.Now,
                        FullName = dto.FullName,
                        ShortName = dto.ShortName,
                        UniqueId = Guid.NewGuid().ToString(),
                        StateId = dto.StateId,
                        OrganizationId = _authService.Organization.Id,
                        Code = dto.Id.ToString("00000"),
                        IndexCode = dto.IndexCode,
                        CreatedUserId = dto.CreatedUserId.HasValue ? dto.CreatedUserId.Value : 1,
                        PositionTypeId = null
                    };
                    _unitOfWork.Context.Add(position);
                    _unitOfWork.Save();
                }
                else
                {
                    UpdatePositionForEdocSchema(dto);
                }

            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }
        }

        private void UpdatePositionForEdocSchema(Position dto)
        {
            try
            {
                var position = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Position>()
                                        .FirstOrDefault(x => x.Id == dto.Id);
                if (position is not null)
                {
                    position.FullName = dto.FullName;
                    position.ShortName = dto.ShortName;
                    position.StateId = dto.StateId;
                    position.DateOfModified = DateTime.Now;
                    position.ModifiedUserId = _authService.User.Id;
                    position.IndexCode = dto.IndexCode;

                    _unitOfWork.Context.Update(position);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }
        }

       
        #endregion
    }
}
