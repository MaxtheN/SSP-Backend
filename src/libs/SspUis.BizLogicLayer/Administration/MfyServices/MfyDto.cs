using GenericServices;
using System.Collections.Generic;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.MfyServices
{
    public class MfyDto : UpdateMfyDlDto, ILinkToEntity<Mfy>
    {
        public string State { get; internal set; }
        public string Region { get; set; }
        public string District { get; set; }
    }
}
