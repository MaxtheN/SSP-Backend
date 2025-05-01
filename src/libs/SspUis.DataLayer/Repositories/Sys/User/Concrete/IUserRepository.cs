using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IUserRepository : IBaseEntityRepository<int, User, CreateUserDlDto, UpdateUserDlDto>
    {
        User ByUserName(string userName);
        void ClearErrors();
    }
}
