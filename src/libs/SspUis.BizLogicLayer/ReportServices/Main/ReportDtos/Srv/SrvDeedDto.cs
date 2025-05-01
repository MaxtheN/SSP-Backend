using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{


    public class SrvDeedDto
    {
      
        public int? RegionId { get; set; }
        public string OrderCode { get; set; }
        public string Name { get; set; }
        public int? DistrictId { get; set; }
        public string GroupName { get; set; }
        public string ServiceName { get; set; }
        public decimal? Sum { get; set; }
        public long Count { get; set; }
        public int OrganizationId { get; set; }
        public int? SrvApplicationOrganizationId {  get; set; }
        public List<DeedGroup> DeedGroup { get; set; }
        public DeedGroupCollection DeedGroupCollection { get; set; }
    }

    public class DeedService
    {
        public string ServiceNames { get; set; }
        public long Count {  get; set; }
        public decimal? Sum { get; set; }

    }

    public class ServiceName
    {
        public int Id { get; set; }
        public string ServiceNames { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }

    public class DeedGroup
    {
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public List<ServiceName> ServiceNames { get; set; }
    }

    public class DeedGroupCollection
    {
        public List<DeedGroup> DeedGroup { get; set; }
    }

}
