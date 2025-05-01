using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
//using RestSharp.Extensions;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.BankServices
{
    public class BankService
        : BaseEntityService<Bank, BankListDto, BankDto, CreateBankDlDto, UpdateBankDlDto, IBankRepository>
        , IBankService
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public BankService(IUnitOfWork unitOfWork, IAuthService authService)
            : base(unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        protected override IQueryable<BankListDto> SortFilter(IQueryable<BankListDto> query, SortFilterPageOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options);
        }

        public SelectList<int> AsSelectList()
        {
            return Repository.ReadAsNoTracked<BankListDto>().AsSelectList();
        }
        public override HaveId<int> Create(CreateBankDlDto dto)
        {
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var entity = Repository.Create(dto);
                CombineStatuses(Repository);

                if (IsValid)
                {
                    _unitOfWork.Save();

                    ///Edoc schema uchun Bank sync qilish.
                    if (!HasErrors)
                        CreateBankForEdocSchema(entity);

                    if (HasErrors)
                        return null;
                    transaction.Commit();

                    return HaveId.Create(entity.Id);
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, "Message");
                _unitOfWork.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
            return null;
        }

        public override void Update(UpdateBankDlDto dto)
        {
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                var entity = Repository.Update(dto);
                CombineStatuses(Repository);
                if (IsValid)
                {
                    _unitOfWork.Save();
                    ///Edoc schema uchun Bank sync qilish.
                    if (!HasErrors)
                        UpdateBankForEdocSchema(entity);

                    if (HasErrors)
                        return;
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                AddError(e.Message, "Message");
                _unitOfWork.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public void SyncEdocBank()
        {
            var banks = _unitOfWork.Context.Set<Bank>().ToList();

            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                foreach (var item in banks)
                {
                    CreateBankForEdocSchema(item);
                }
                if (HasErrors)
                    return;

                transaction.Commit();
            }
            catch (Exception e)
            {
                AddError(e.Message, "Message");
                _unitOfWork.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private void CreateBankForEdocSchema(Bank dto)
        {
            try
            {
                var exsist = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Bank>()
                                        .Any(x => x.Id == dto.Id);
                if (!exsist)
                {
                    var bank = new Ssp.DataLayer.EFClasses.Edoc.Bank()
                    {
                        Id = dto.Id,
                        OrderCode = dto.OrderCode,
                        BankName = dto.BankName,
                        Code = dto.Code,
                        CreatedUserId = _authService.User.Id,
                        DateOfCreated = DateTime.Now,                         
                        StateId = dto.StateId
                    };
                    _unitOfWork.Context.Add(bank);
                    _unitOfWork.Save();
                }
                else
                {
                    UpdateBankForEdocSchema(dto);
                }

            }
            catch (Exception ex)
            {
                AddError(ex.Message, "Message");
            }
        }

        private void UpdateBankForEdocSchema(Bank dto)
        {
            try
            {
                var bank = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Bank>()
                                        .FirstOrDefault(x => x.Id == dto.Id);
                if (bank is not null)
                {
                    bank.OrderCode = !string.IsNullOrEmpty(dto.OrderCode) ? dto.OrderCode : dto.Id.ToString("0000");
                    bank.BankName = dto.BankName;
                    bank.Code = dto.Code;
                    bank.ModifiedUserId = _authService.User.Id;
                    bank.DateOfModified = DateTime.Now;
                    bank.StateId = dto.StateId;
                    _unitOfWork.Context.Update(bank);
                    _unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message, "Message");
            }
        }
    }
}
