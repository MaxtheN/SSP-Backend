using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;
using SspUis.Core;

namespace SspUis.DataLayer.Repositories
{
    public class CreateCustomJobDlDto : CustomJobDlDto<CreateCustomJobDlDto>
    {
        public override CustomJob CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            return entity;
        }

        public override void UpdateEntity(CustomJob entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
