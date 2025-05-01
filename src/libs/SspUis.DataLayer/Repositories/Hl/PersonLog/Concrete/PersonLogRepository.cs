using GenericServices;
using SspUis.DataLayer.EfClasses.Public.Hl;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;



public class PersonLogRepository : BaseEntityRepository<int, PersonLog, CreatePersonLogDlDto,
                UpdatePersonLogDlDto>, IPersonLogRepository
{
    public PersonLogRepository(ICrudServices crudServices) : base(crudServices)
    {
    }

}
