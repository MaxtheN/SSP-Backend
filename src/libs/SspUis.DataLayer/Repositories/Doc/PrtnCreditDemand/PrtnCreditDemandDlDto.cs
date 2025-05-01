using System;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnCreditDemandDlDto<TDto> : EntityDto<TDto, PrtnCreditDemand>
        where TDto : PrtnCreditDemandDlDto<TDto>
    {
        [LocalizedRequired]
        public DateOnly DocOn { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }
        [LocalizedRequired]
        public string ImplementedProjectName { get; set; }
        [LocalizedRequired]
        public double ProjectCost { get; set; }
        [LocalizedRequired]
        public double OwnInvestment { get; set; }
        [LocalizedRequired]
        public double ForeignInvestment { get; set; }
        [LocalizedRequired]
        public double PrivillageBankCredit { get; set; }
        [LocalizedRequired]
        public int BankId { get; set; }
        public override PrtnCreditDemand CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.FORMED;
            return entity;
        }
    }
}
