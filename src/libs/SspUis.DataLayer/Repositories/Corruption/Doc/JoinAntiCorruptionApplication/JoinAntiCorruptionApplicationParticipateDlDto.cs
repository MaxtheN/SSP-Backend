using GenericServices;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using WEBASE.Models;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class JoinAntiCorruptionApplicationParticipateDlDto : EntityDto<JoinAntiCorruptionApplicationParticipateDlDto, JoinAntiCorruptionApplicationParticipate>, IHaveIdProp<long>, ILinkToEntity<JoinAntiCorruptionApplicationParticipate>
    {
        public long Id { get; set; }
        public int OrderNumber { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public int YearIn { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string InvestigationOrganization { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string BasisForInvestigation { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string InvestigatedPersonFio { get; set; }
        [LocalizedRequired]
        public string InvestigatedResult { get; set; }
    }
}
