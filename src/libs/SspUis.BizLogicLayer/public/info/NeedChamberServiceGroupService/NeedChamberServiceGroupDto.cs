using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer
{
    public class NeedChamberServiceGroupDto 
        : UpdateNeedChamberServiceGroupDlDto,
        ILinkToEntity<NeedChamberServiceGroup>
    {
        public string State { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
