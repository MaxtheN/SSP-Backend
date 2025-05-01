using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Doc.PrtnContractServices
{
    public class PrtnContractSortFilterPageOptions : DocumentSortFilterOptions
    {
        public int? PrtnContractTypeId { get; set; }
    }
}
