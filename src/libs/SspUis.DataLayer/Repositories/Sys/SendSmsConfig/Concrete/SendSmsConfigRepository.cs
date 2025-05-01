using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SendSmsConfigRepository 
        : BaseEntityRepository<int, SendSmsConfig, CreateSendSmsConfigDlDto, UpdateSendSmsConfigDlDto>,
        ISendSmsConfigRepository
    {
        public SendSmsConfigRepository(ICrudServices crudServices) 
            : base(crudServices)
        { }

        protected override void CreateValidate(CreateSendSmsConfigDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(SendSmsConfig entity, UpdateSendSmsConfigDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(SendSmsConfig entity, SendSmsConfigDlDto<TDto> dto)
            where TDto : SendSmsConfigDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
