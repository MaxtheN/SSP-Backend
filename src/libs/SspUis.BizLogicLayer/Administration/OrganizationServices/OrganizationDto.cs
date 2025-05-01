using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationDto : UpdateOrganizationDlDto, ILinkToEntity<Organization>
    {
        public string State { get; internal set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Oked { get; set; }
        public string Parent { get; set; }
        public string SignOrganizationType { get; set; }
        public string OrganizationLegalForm { get; set; }
        public string OrganizationGroup { get; set; }
        public string IncomingDocReceiverEmployee { get; set; }
        new public List<OrganizationSignDto> Signs { get; set; } = new();
        public List<OrganizationSignDto> ExpiredSigns { get; set; } = new();
        new public List<OrganizationSettlementAccountDto> SettlementAccounts { get; set; } = new();
        new public List<OrganizationTranslateDto> Translates { get; set; } = new();
        public new List<OrganizationFileDto> Files { get; set; } = new();
    }


    public class RegionOrganizationIdDto
    {
        public int Id { get; set; }
    }
}
