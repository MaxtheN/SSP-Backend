using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IOrganizationalStructureRepository : IBaseEntityRepository<int, OrganizationalStructure, CreateOrganizationalStructureDlDto, UpdateOrganizationalStructureDlDto>
    {
        OrganizationalStructure ByWbCode(string wbCode);
    }
}
