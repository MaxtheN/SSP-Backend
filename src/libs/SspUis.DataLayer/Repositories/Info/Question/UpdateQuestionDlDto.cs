using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateQuestionDlDto : QuestionDlDto<UpdateQuestionDlDto>, IHaveIdProp<int>
{
    [LocalizedRequired]
    [LocalizedRange(0, int.MaxValue)]
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(0, int.MaxValue)]
    public int StateId { get; set; }
}