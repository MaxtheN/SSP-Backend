using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class RoleDlDto<TDto> : EntityDto<TDto, Role>
        where TDto : RoleDlDto<TDto>
    {
        public RoleDlDto()
        {
            Translates = new();
        }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }
        public bool IsDefault { get; set; }
        public List<int> Modules { get; set; } = new List<int>();
        public List<RoleTranslateDlDto> Translates { get; set; }

        protected override Action<IMappingExpression<TDto, Role>> AlterMapping => cfg => cfg
           .ForMember(x => x.Translates, x => x.Ignore());

        public override Role CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            entity.RoleModules.AddFromForeignKeys(Modules);
            return entity;
        }

        public override void UpdateEntity(Role entity)
        {
            entity.RoleModules.UpdateFromForeignKeys(Modules);
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }

    }
}
