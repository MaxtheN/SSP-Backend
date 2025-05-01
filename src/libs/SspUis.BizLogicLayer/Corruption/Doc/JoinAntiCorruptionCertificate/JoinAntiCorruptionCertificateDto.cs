using System;
using System.ComponentModel.DataAnnotations.Schema;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories.Corruption;

namespace SspUis.BizLogicLayer.Corruption
{

    public class JoinAntiCorruptionCertificateDto
        : UpdateJoinAntiCorruptionCertificateDlDto,
        ILinkToEntity<JoinAntiCorruptionCertificate>,
        IDocument
    {
        public long Id { get; set; }
        public Guid Id2 { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorDirector { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
    }
}
