using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateAnswerDlDto : AnswerDlDto<UpdateAnswerDlDto>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(0, int.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(0, int.MaxValue)]
    public int StateId { get; set; }
}
