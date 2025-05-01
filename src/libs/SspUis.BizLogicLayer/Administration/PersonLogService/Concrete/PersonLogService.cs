using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Administration.PersonLogService;
using SspUis.BizLogicLayer.Administration.PersonLogService.Query;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Public.Hl;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class PersonLogService : BaseEntityService<int, PersonLog,
                    PersonLogListDto,
                    PersonLogDto,
                    CreatePersonLogDlDto,
                    UpdatePersonLogDlDto,
                    IPersonLogRepository,
                    PersonLogSortFilterOption>,  IPersonLogService 
{
    protected readonly IPersonLogRepository _repository;

  

    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public PersonLogService(IUnitOfWork unitOfWork,
                            IStorageService storage,
                               IPersonLogRepository personRepo) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _storageService = storage;
        _repository = personRepo;

    }

    public PagedResult<PersonLogListDto> GetList(PersonLogSortFilterOption dto)
    {
        var result = _repository.ReadAsNoTracked<PersonLogListDto>()
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }
    public PersonLogListDto Get()
    {
        return new PersonLogListDto();
    }
    public PersonLogListDto GetByEmployeeId(int employeeId)
    {
       var res =  _unitOfWork.Context.Set<PersonLog>().Include(x => x.Person)
            .FirstOrDefault(x => x.EmployeeId == employeeId);
        if(res == null)
        {
            AddError("Data not found");
            return null;
        }
        return new()
        {
            EmployeeId = res.EmployeeId,
            PassportSeria = res.PassportSeria,
            Id = res.Id,
            PassportNumber = res.PassportNumber,
            PassportDate = res.PassportDate,
            PassportDivName = res.PassportDivName,
            PassportExpiration = res.PassportExpiration,
            Person = res.Person.FullName,
            PersonId = employeeId,
            Pinfl = res.Pinfl,
        };
    }

    public HaveId<int> Create(CreatePersonLogDlDto dto)
    {
        var entity = _repository.Create(dto, ent => Validation(dto, ent));
        CombineStatuses(_repository);
        if (IsValid)
        {
            _unitOfWork.Save();
            CombineStatuses(_storageService);
            return HaveId.Create(entity.Id);
        }
        return null;
    }
    private void Validation<TDto>(PersonLogDlDto<TDto> dto, PersonLog entity)
          where TDto : PersonLogDlDto<TDto>
    {
        var person = _unitOfWork.Context.Set<Person>().FirstOrDefault(x => x.Pinfl == dto.Pinfl);

        if (person == null)
        {
            AddError($"Pinfl did not fit the old passport Pinfl.", $"{dto.PassportSeria}{dto.PassportNumber}.", $"{dto.PassportExpiration}");
        }
    }

}
