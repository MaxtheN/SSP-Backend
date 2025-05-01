using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class UserRepository : BaseEntityRepository<int, User, CreateUserDlDto, UpdateUserDlDto>, IUserRepository
    {
        public UserRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<User> ByIdQuery()
        {
            return base.ByIdQuery().Include(a => a.UserRoles).ThenInclude(a => a.Role).Include(a => a.Person);
        }

        public User ByUserName(string userName)
        {
            var user = ByIdQuery().FirstOrDefault(a => a.UserName == userName);
            if (user == null)
            {
                AddEntityNotFoundError();
            }
            return user;
        }

        protected override void CreateValidate(CreateUserDlDto dto)
        {
            var query = DbSet.AsQueryable();
            if (query.Any(a => a.UserName == dto.UserName))
            {
                AddError("Это имя пользователя занято", dto.UserName);
            }
            Validate(null, dto);
        }

        protected override void UpdateValidate(User entity, UpdateUserDlDto dto)
        {
            var query = DbSet.AsQueryable();
            if (query.Any(a => a.Id != dto.Id && a.UserName == dto.UserName))
            {
                AddError("Это имя пользователя занято", dto.UserName);
            }
            Validate(entity, dto);
        }

        private void Validate<TDto>(User entity, UserDlDto<TDto> dto)
            where TDto : UserDlDto<TDto>
        {
            if (dto.Roles == null || !dto.Roles.Any())
                AddError("Роль для пользователя еще не выбрана");
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (!string.IsNullOrEmpty(dto.Email) && query.ByEmail(dto.Email, isIncludePassive: true).Any())
                AddError($"Ползователь с этим Email ({dto.Email}) уже существует.", nameof(dto.Email));
            //if (dto.Pinfl.NullOrEmpty() && query.ByPinfl(dto.Pinfl, isIncludePassive: true).Any())
            //    AddError($"Страна с этим ИНПС ({dto.Pinfl}) уже существует.", nameof(dto.Pinfl));
        }

        public void ClearErrors()
        {
            _errors?.Clear();
        }
    }
}
