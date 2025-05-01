using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.QualificationCategoryServices
{
    public class QualificationCategoryDto : UpdateQualificationCategoryDlDto, ILinkToEntity<QualificationCategory>, IInfoHl
    {
        public string State { get; set; } = null!;
        public new List<QualificationCategoryTranslateDto> Translates { get; set; } = new();
    }
}
