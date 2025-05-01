using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.PartisanshipServices.Concrete
{
    public class PartisanshipService : BaseEntityService<Partisanship, PartisanshipListDto, PartisanshipDto, CreatePartisanshipDlDto, UpdatePartisanshipDlDto, IPartisanshipRepository>, IPartisanshipService
    {
        public PartisanshipService(IUnitOfWork unitOfWork, IAuthService authService)
        : base(unitOfWork)
        {
        }

        public override PagedResult<PartisanshipListDto> GetList(SortFilterPageOptions dto)
        {
            var result = Repository.ReadAsNoTracked<PartisanshipListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public override PartisanshipDto Get()
        {
            return new PartisanshipDto();
        }

        public override PartisanshipDto Get(int id)
        {
            var dto = Repository.ById<PartisanshipDto>(id);
            CombineStatuses(Repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return Repository.AllAsQueryable
                .AsSelectList();
        }

        public override HaveId<int> Create(CreatePartisanshipDlDto dto)
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

        public override void Update(UpdatePartisanshipDlDto dto)
        {
            Repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(Repository);
            if (IsValid)
                UnitOfWork.Save();
        }

        public override void Delete(int id)
        {
            try
            {
                Repository.Delete(id);
                CombineStatuses(Repository);
                if (IsValid)
                    UnitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }

        private void Validation<TDto>(PartisanshipDlDto<TDto> dto, Partisanship entity)
            where TDto : PartisanshipDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
