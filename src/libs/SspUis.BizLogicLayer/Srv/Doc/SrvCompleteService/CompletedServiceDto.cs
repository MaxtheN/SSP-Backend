using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer
{
    public class CompletedServiceDto : UpdateCompletedServiceDlDto, ILinkToEntity<CompletedService>, IDocument
    {
        public string ContractorInn { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorRegion { get; set; }
        public string ContractorDistrict { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string OrganizationName { get; set; }
    }
}
