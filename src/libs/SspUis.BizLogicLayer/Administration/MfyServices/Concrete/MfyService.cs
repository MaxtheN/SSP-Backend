using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using SspUis.DataLayer.EfClasses;
using SspUis.Integration.OnlineMahalla;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.MfyServices
{
    public class MfyService :
        StatusGenericHandler,
        IMfyService
    {
        private readonly IMfyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IOnlineMahallaService _onlineMahallaService;

        public MfyService(IUnitOfWork unitOfWork,
                          IAuthService authService,
                          IOnlineMahallaService onlineMahallaService)
        {
            _repository = unitOfWork.MfyRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _onlineMahallaService = onlineMahallaService;
        }

        public PagedResult<MfyListDto> GetList(SortFilterPageOptions dto)
            => _repository.ReadAsNoTracked<MfyListDto>()
                            .SortFilter(dto)
                            .AsPagedResult(dto);

        public MfyDto Get()
            => new MfyDto();

        public MfyDto Get(long id)
        {
            var dto = _repository.ById<MfyDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<long> AsSelectList(int? regionId, int? districtId)
            => _repository.AllAsQueryable.Where(a => (regionId.HasValue ? a.RegionId == regionId : true) &&
                                                     (districtId.HasValue ? a.DistrictId == districtId : true))
                                             .AsSelectList();

        public void CreateFromIntegration(List<OnlineMahallaDataDto> dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var externalIdCheck = dto.Any(a => a.MfyId == 0);

                if (externalIdCheck)
                {
                    AddError("Невозможно указать 0 для внешнего идентификатора.");
                    transaction.Rollback();
                    return;
                }

                foreach (var mfy in dto)
                {
                    var oldMfy = _unitOfWork.Context.Set<Mfy>()
                                                    .FirstOrDefault(m => m.ExternalId == mfy.MfyId);

                    if (oldMfy == null)
                    {
                        var district = _unitOfWork.Context.Set<District>()
                            .FirstOrDefault(a => a.Soato == mfy.DistrictSoatoCode.ToString());

                        if (district == null)
                        {
                            AddError($"В этом районе (\"{mfy.MfyNameRu}\") указан неверный код района.");
                            transaction.Rollback();
                            return;
                        }

                        var region = _unitOfWork.Context.Set<Region>()
                            .FirstOrDefault(a => a.Soato == mfy.RegionSoatoCode.ToString());

                        if (district == null)
                        {
                            AddError($"В этом районе (\"{mfy.MfyNameRu}\") указан неверный региональный код.");
                            transaction.Rollback();
                            return;
                        }

                        var createDto = new CreateMfyDlDto()
                        {
                            ExternalId = mfy.MfyId,
                            ShortName = mfy.MfyNameRu,
                            FullName = mfy.MfyNameRu,
                            DistrictId = district.Id,
                            RegionId = region.Id,
                            ExternalLastUpdatedId = mfy.UpdateId
                        };

                        Create(createDto);

                        if (HasErrors)
                        {
                            transaction.Rollback();
                            return;
                        }
                    }
                    else
                    {
                        var district = _unitOfWork.Context.Set<District>()
                            .FirstOrDefault(a => a.Soato == mfy.DistrictSoatoCode.ToString());

                        if (district == null)
                        {
                            AddError($"В этом районе (\"{mfy.MfyNameRu}\") указан неверный код района.");
                            transaction.Rollback();
                            return;
                        }

                        var region = _unitOfWork.Context.Set<Region>()
                            .FirstOrDefault(a => a.Soato == mfy.RegionSoatoCode.ToString());

                        if (region == null)
                        {
                            AddError($"В этом районе (\"{mfy.MfyNameRu}\") указан неверный региональный код.");
                            transaction.Rollback();
                            return;
                        }

                        var updateDto = new UpdateMfyDlDto()
                        {
                            Id = mfy.MfyId,
                            ExternalId = mfy.MfyId,
                            ShortName = mfy.MfyNameRu,
                            FullName = mfy.MfyNameRu,
                            DistrictId = district.Id,
                            RegionId = region.Id,
                            StateId = mfy.State == 1 ? 1 : 2,
                            ExternalLastUpdatedId = mfy.UpdateId
                        };

                        Update(updateDto);

                        if (HasErrors)
                        {
                            transaction.Rollback();
                            return;
                        }
                    }
                }

                if (IsValid)
                    transaction.Commit();
            }
        }

        public async Task SyncMfy()
        {
            long maxUpdatedId = _unitOfWork.Context.Set<Mfy>().Max(a => a.ExternalLastUpdatedId);

            var externalDto = await _onlineMahallaService.Get(maxUpdatedId);

            CombineStatuses(_onlineMahallaService);

            if (HasErrors)
                return;

            CreateFromIntegration(externalDto);

            if (HasErrors)
                return;
        }

        private HaveId<long> Create(CreateMfyDlDto dto)
        {
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        private void Update(UpdateMfyDlDto dto)
        {
            _repository.Update(dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        /* private void Delete(long id)
         {
             try
             {
                 _repository.Delete(id);
                 CombineStatuses(_repository);
                 if (IsValid)
                     _unitOfWork.Save();
             }
             catch (DbUpdateException)
             {
                 AddError("Запись не может быть удален");
             }
         }*/
    }
}