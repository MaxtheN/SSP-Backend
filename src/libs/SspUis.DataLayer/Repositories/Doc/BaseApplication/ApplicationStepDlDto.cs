using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Doc.BaseApplication
{
    public class ApplicationStepDlDto : EntityDto<ApplicationStepDlDto, ApplicationStep>, IHaveIdProp<long>
        , ILinkToEntity<ApplicationStep>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        public int ApplicationTypeStepId { get; set; }
        public string Message { get; set; }
    }
}
