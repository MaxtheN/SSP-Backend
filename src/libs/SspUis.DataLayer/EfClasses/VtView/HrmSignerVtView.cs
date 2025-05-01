using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Keyless]
    public class HrmSignerVtView
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Column("doc_number")]
        public string DocNumber { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("details")]
        public string Details { get; set; }
        [Column("tableid")]
        public int TableId { get; set; }
        [Column("employee_fullnames")]
        public string Employee { get; set; }
        [Column("employee_manage_ids")]
        public long[]? EmployeeManageIds { get; set; }
    }
}
