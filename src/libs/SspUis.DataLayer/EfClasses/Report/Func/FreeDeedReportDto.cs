using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Report.Func
{
    [Keyless]
    public class FreeDeedReportDto
    {
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("region_name")]
        public string RegionName { get; set; }
        
        [Column("district_id")]
        public int DistrictId { get; set; }
        
        [Column("district_name")]
        public string DistrictName { get; set; }

        [Column("need_chamber_service_name")]
        public string ServiceName { get; set; }

        [Column("chamber_id")]
        public int ServiceId { get; set; }
        [Column("need_chamber_service_group_name")]
        public string GroupName { get; set; }
        [Column("chamber_group_id")]
        public int GroupId { get; set; }
        [Column("app_count")]
        public int Count { get; set; }
        [Column("organization_id")]
        public int OrganizationId {  get; set; }

    }
}



