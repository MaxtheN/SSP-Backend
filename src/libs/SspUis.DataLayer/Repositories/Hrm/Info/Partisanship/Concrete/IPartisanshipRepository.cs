using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IPartisanshipRepository : IBaseEntityRepository<int, Partisanship, CreatePartisanshipDlDto, UpdatePartisanshipDlDto>
    {
    }
}
