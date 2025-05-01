using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NotBudgetContractorDlDto<TDto> : EntityDto<TDto, NotBudgetContractor>
        where TDto : NotBudgetContractorDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string Name { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Inn { get; set; }
    }
}
