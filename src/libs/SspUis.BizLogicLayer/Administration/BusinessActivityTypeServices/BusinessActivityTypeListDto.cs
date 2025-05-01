using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.BusinessActivityTypeServices
{
    public class BusinessActivityTypeListDto : ILinkToEntity<BusinessActivityType>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public long ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public int BankId { get; set; }
        public string Bank { get; set; }    
        public string BankCode { get; set; }    
        public long? HelpAmount { get; set; }
        public string RealEmployeesCount { get; set; }
        public int BusinessCtorId { get; set; }
        public string BusinessCtorName { get; set; }
        public int FinancialHelpId { get; set; }
        public string FinancialHelp { get; set; }
        public string NewEmployeesCount { get; set; }
    }
}
