using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class MemshipPaymentOrderFileDlDto :
        EntityDto<MemshipPaymentOrderFileDlDto, 
            MemshipPaymentOrderFile>, IHaveIdProp<Guid>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
