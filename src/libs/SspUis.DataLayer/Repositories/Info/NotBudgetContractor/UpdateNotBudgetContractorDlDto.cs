using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using SspUis.Core.Security;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateNotBudgetContractorDlDto : NotBudgetContractorDlDto<UpdateNotBudgetContractorDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long Id { get; set; }
    }
}
