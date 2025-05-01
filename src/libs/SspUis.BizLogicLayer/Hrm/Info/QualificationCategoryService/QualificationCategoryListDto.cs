using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm.QualificationCategoryServices
{
    public class QualificationCategoryListDto :  ILinkToEntity<QualificationCategory>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Details { get; set; } = null!;
        public string State { get; set; } = null!;
    }
}
