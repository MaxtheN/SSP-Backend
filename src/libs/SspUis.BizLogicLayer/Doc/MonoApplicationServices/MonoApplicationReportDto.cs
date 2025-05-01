using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Doc.MonoApplicationServices
{
	public class MonoApplicationReportDto
	{
		public long Id { get; set; }
		public int RegionId { get; set; }
		public string RegionName { get; set; }
		
		public string ContractorName { get; set; }
		public string OrderCode {get;set; }
		public int DistrictId { get; set; }

		public string DistricName { get; set; }
		public int StatusId { get; set; }
		public AllApplicaions AllApplicaions { get; set; }
		public ReviewApplications ReviewApplications { get; set; }
		public AskApplications	AskApplications { get; set; }
	}

	public class ReviewApplications
	{
		
		public int Count { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TotalCost { get; set; }
	}

	public class AskApplications
	{
		public RejectApplicaions RejectApplicaions { get; set; }
		public AcceptApplicaions AcceptApplicaions { get; set; }
	}

	public class RejectApplicaions
	{
		
		public int Count { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TotalCost { get; set; }
	}

	public class AcceptApplicaions
	{
		
		public int Count { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TotalCost { get; set; }
		public decimal SubsidyAmount { get; set; }
	}

	public class AllApplicaions
	{
		
		public int Count { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TotalCost { get; set; }
	}
}
