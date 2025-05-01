using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class DebtService : BaseEntityService<long, Debt, DebtListDto, DebtDto, CreateDebtDlDto, UpdateDebtDlDto, IDebtRepository, DebtSortFilterOption>, IDebtService
{
    private readonly IAuthService _authService;
    private readonly INumberService _numberService;

    public DebtService(IUnitOfWork unitOfWork, IAuthService authService, INumberService numberService)
        : base(unitOfWork)
    {
        _authService = authService;
        _numberService = numberService;
    }

    public override PagedResult<DebtListDto> GetList(DebtSortFilterOption options)
    {
        var result = Repository.ReadAsNoTracked<DebtListDto>()
            .SortFilter(options)
            .ToList()
            .AsQueryable()
            .AsPagedResult(options);
        return result;

    }

    public SelectList<long> AsSelectList()
    {
        return Repository.AllAsQueryable
            .AsSelectList();
    }

    public override DebtDto Get()
    {
        return
            new()
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext("").Item2
            };
    }

    public override DebtDto Get(long id)
    {
        var dto = base.Get(id);
        return dto;
    }

    public HaveId<long> Create(CreateDebtDlDto dto)
    {
        var entity = Repository.Create(dto, ent => Validation(dto, ent));
        CombineStatuses(Repository);
        if (IsValid)
        {
            UnitOfWork.Save();
            return HaveId.Create(entity.Id);
        }
        return null;
    }

    public void Update(UpdateDebtDlDto dto)
    {
        Repository.Update(dto, ent => Validation(dto, ent));
        CombineStatuses(Repository);
        if (IsValid)
            UnitOfWork.Save();
    }

    public override void Delete(long id)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusDebtDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                Repository.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplyStatus(ent.StatusId, statusDto.StatusId))
                        AddError("Нет доступа");
                });
                UnitOfWork.Save();

                if (IsValid)
                {
                    transaction.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
    }
    private void Validation<TDto>(DebtDlDto<TDto> dto, Debt entity)
        where TDto : DebtDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
}
