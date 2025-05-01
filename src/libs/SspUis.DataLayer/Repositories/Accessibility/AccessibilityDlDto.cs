using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AccessibilityDlDto<TDto> : EntityDto<TDto, Accessibility>
        where TDto : AccessibilityDlDto<TDto>
    {
        public bool HasAccess { get; set; }

        public override void UpdateEntity(Accessibility entity)
        {
            base.UpdateEntity(entity);
        }
    }
}