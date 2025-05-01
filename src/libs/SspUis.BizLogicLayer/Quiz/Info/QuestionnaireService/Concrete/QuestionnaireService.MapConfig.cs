using AutoMapper;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.QuestionnaireService;

public partial class QuestionnaireService
{
    public IMapper Configuration() =>
        new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateQuestionGroupDlDto, UpdateQuestionGroupDlDto>().ReverseMap();
            cfg.CreateMap<CreateQuestionDlDto, UpdateQuestionDlDto>().ReverseMap();
            cfg.CreateMap<CreateAnswerDlDto, UpdateAnswerDlDto>().ReverseMap();
        }).CreateMapper();
}
