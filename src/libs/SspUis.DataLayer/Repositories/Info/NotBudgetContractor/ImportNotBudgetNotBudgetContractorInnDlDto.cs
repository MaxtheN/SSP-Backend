using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ImportNotBudgetNotBudgetContractorInnDlDto
    {
        [LocalizedRequired]
        [Column("Contractor_name")]
        public string Contractor { get; set; }

        [LocalizedRequired]
        [Column("Not_recommended_tins")]
        public string ContractorInn { get; set; }

    }
}
