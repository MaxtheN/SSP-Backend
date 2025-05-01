using System;
using System.Linq;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE;
using WEBASE.Integration.MSPD.Client;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class ArbitrationJudgeService : StatusGenericHandler, IArbitrationJudgeService
{
    private readonly IArbitrationJudgeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IMspdUnitOfWork _mspdUnitOfWork;
    private readonly IPersonService _personService;

    public ArbitrationJudgeService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IMspdUnitOfWork mspdUnitOfWork,
        IPersonService personService)
    {
        _repository = unitOfWork.ArbitrationJudgeRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _mspdUnitOfWork = mspdUnitOfWork;
        this._personService = personService;
    }

    public PagedResult<ArbitrationJudgeListDto> GetList(ArbitrationJudgeSortFilterOptions dto)
    {
        var query = _repository.ReadAsNoTracked<ArbitrationJudgeListDto>();
        query = query.SortFilter(dto);
        var result = query.AsPagedResult(dto);
        return result;
    }

    public ArbitrationJudgeDto Get()
    {
        return new ArbitrationJudgeDto();
    }

    public ArbitrationJudgeDto Get(int id)
    {
        var dto = _repository.ById<ArbitrationJudgeDto>(id);
        CombineStatuses(_repository);
        return dto;
    }

    public SelectList<int> AsSelectList()
    {
        return _repository.AllAsQueryable
                        .AsSelectList();
    }

    public async Task<HaveId<int>> Create(CreateArbitrationJudgeDlDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        using var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
        try
        {
            dto.PersonId = await SetPersonId(dto.PassportNumber, dto.PassportSeria, dto.BirthDate);
            if (HasErrors)
            {
                if (canCommit)
                    transaction.Rollback();
                return null;
            }
            var entity = _repository.Create(dto, ent => Validation(dto, ent));

            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();

            if (IsValid && canCommit)
                transaction.Commit();
            return HaveId.Create(entity.Id);
        }
        catch (Exception ex)
        {
            AddError(ex.Message + "Inner: " + ex.InnerException);
            transaction.Rollback();
        }
        return null;
    }

    private async ValueTask<int> SetPersonId(string passportNumber, string passportSeria, DateOnly birthDate)
    {
        var gspPerson = await _personService.GetByPassportData(new()
        {
            Seria = passportSeria,
            Number = passportNumber,
            DateOfBirth = birthDate.AsDateTime(),
        }) ?? throw new($"Ushbu passport malumoti bo'yicha inson topilmadi: {passportNumber}-{passportSeria}  {birthDate}");
        if (gspPerson.Id == 0)
        {
            var mc = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonDto, CreatePersonDlDto>();
            });

            var createPersonDlDto = mc.CreateMapper().Map<CreatePersonDlDto>(gspPerson);

            var personId = _personService.Create(createPersonDlDto);
            CombineStatuses(_personService);
            if (HasErrors)
                return 0;
            _unitOfWork.Save();
            return personId.Id;
        }
        else
        {
            return gspPerson.Id;
        }
    }

    public async ValueTask Update(UpdateArbitrationJudgeDlDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        using var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
        try
        {
            dto.PersonId = await SetPersonId(dto.PassportNumber, dto.PassportSeria, dto.BirthDate);
            _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();

            if (IsValid && canCommit)
                transaction.Commit();

        }
        catch (Exception ex)
        {
            AddError(ex.Message + "Inner: " + ex.InnerException);
            transaction.Rollback();
        }
    }

    public void Delete(int id)
    {
        var entity = _repository.ById(id);
        if (entity == null)
        {
            AddError("Item not found");
            return;
        }
        try
        {
            //_repository.Update(new()
            //{
            //    Id = entity.Id,
            //    //FirstName = entity.FirstName,
            //    //LastName = entity.LastName,
            //    //MiddleName = entity.MiddleName,
            //    StateId = StateIdConst.PASSIVE
            //});
            entity.StateId = StateIdConst.PASSIVE;
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
        catch (Exception ex)
        {
            AddError(ex.Message + "Inner: " + ex.InnerException);
        }
    }

    private void Validation<TDto>(ArbitrationJudgeDlDto<TDto> dto, ArbitrationJudge entity)
        where TDto : ArbitrationJudgeDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
}
