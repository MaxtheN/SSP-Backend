using GenericServices;
using SspUis.DataLayer.EfClasses.Public.Hl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;
    public class PersonLogDto : ILinkToEntity<PersonLog>
    {
        public int Id { get; set; }
        public string Pinfl { get; set; }
        public string PassportSeria { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportDate { get; set; }
        public DateTime? PassportExpiration { get; set; }
        public string PassportDivName { get; set; }
        public int PersonId { get; set; }
        public string Person { get; set; }
        public int EmployeeId { get; set; }
    }

