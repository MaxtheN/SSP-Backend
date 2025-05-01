using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Appeal.Info.ExternalDocFromEdocService
{
    public class ExternalDocFromEdocDto : ILinkToEntity<ExternalDocumentFromEdoc>
    {
        public int Id { get; set; }
        public DateTime? TermExecution { get; set; }
        public string Assignment { get; set; }
        public int? OrganizationId { get; set; }
        public int? ProcessId { get; set; }
        public long? AppealAplicationtId { get; set; }
        public long? CallCenterAppealId { get; set; }
        public string RegNumber { get; set; }
        public string Organization { get; set; }
        public DateTime? OutgoingDocCreatedData { get; set; }
        public DateTime? RegDate { get; set; }
    }
}
