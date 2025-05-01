using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using SspUis.Core;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnCertificateDlDto<TDto> : EntityDto<TDto, PrtnCertificate>
        where TDto : PrtnCertificateDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }
        [LocalizedRequired]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long PrtnContractId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ContractorId { get; set; }
        [LocalizedRequired]
        public DateOnly ExpireOn { get; set; }
        [LocalizedStringLength(250)]
        public string FirstSign { get; set; }
        [LocalizedStringLength(250)]
        public string SecondSign { get; set; }
        public string PrtnContractDocNumber { get; set; }
        public DateOnly PrtnContractDocOn { get; set; }
        public int PrtnContractTypeId { get; set; }
        public string ContractorInn { get; set; }
        public int OrganizationId { get; set; }
        public override PrtnCertificate CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.Id2 = Guid.NewGuid();
            entity.StatusId = StatusIdConst.FORMED;
            entity.TableId = TableIdConst.DOC_PRTN_CERTIFICATE;
            return entity;
        }
    }
}
