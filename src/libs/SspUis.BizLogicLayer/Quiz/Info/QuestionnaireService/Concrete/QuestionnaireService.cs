using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.AnswerService;
using SspUis.BizLogicLayer.QuestionGroupService;
using SspUis.BizLogicLayer.QuestionService;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Quiz.Enum;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Models;
using WEBASE.Security;

namespace SspUis.BizLogicLayer.QuestionnaireService;

public partial class QuestionnaireService : StatusGenericHandler, IQuestionnaireService
{
    private readonly IQuestionnaireRepository _repository;
    private readonly IQuestionGroupRepository _questionGroupRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public QuestionnaireService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _repository = unitOfWork.QuestionnaireRepository;
        _questionGroupRepository = unitOfWork.QuestionGroupRepository;
        _questionRepository = unitOfWork.QuestionRepository;
        _answerRepository = unitOfWork.AnswerRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }
    public PagedResult<QuestionnaireListDto> GetList(QuestionnaireSortFilterOptionsDto dto)
    {
        var result = _repository.ReadAsNoTracked<QuestionnaireListDto>()
                        .SortFilter(dto)
                        .AsPagedResult(dto);
        return result;
    }
    public SelectList<long> AsSelectList() => _repository.AllAsQueryable.AsSelectList();
    public QuestionnaireDto Get() => new QuestionnaireDto();
    public QuestionnaireDto Get(long id)
    {
        var questionnaire = _unitOfWork.Context.Set<Questionnaire>()
            .IsActive()
            .Include(x => x.State).ThenInclude(x => x.Translates)
            .Include(x => x.Translates)
            .FirstOrDefault(x => x.Id == id);

        if(questionnaire is null)
        {
            AddError("Бундай анкета мавжуд емас !");
            return null;
        }

        return new QuestionnaireDto
        {
            Id = questionnaire.Id,
            StateId = questionnaire.StateId,
            State = questionnaire.State.Translates.AsQueryable()
                               .FirstOrDefault(StateTranslate.GetExpr(
                                   TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                               ?.TranslateText ?? questionnaire.State.FullName,
            OrderNumber = questionnaire.OrderNumber,
            Title = questionnaire.Translates.AsQueryable()
                               .FirstOrDefault(QuestionnaireTranslate.GetExpr(
                                   TranslateQuestionnaire.title, ServiceProvider.CultureHelper.CurrentCulture.Id))
                               ?.TranslateText ?? questionnaire.Title,
            Details = questionnaire.Details,
            QuestionnaireTypeId = questionnaire.QuestionnaireTypeId,
            Translates = questionnaire.Translates.Select(t => new IhmaInv.BizLogicLayer.QuestionServices.QuestionnaireTranslateDto
            {
                LanguageId = t.LanguageId,
                Language = t.Language.FullName
            }).ToList(),

            Group = _unitOfWork.Context.Set<QuestionnaireGroup>()
                .IsActive()
                .Where(x => x.OwnerId == questionnaire.Id)
                .Select(y => new QuestionGroupDto
                {
                    Id = y.GroupId,
                    StateId = y.Group.StateId,
                    State = y.Group.State.Translates.AsQueryable()
                               .FirstOrDefault(StateTranslate.GetExpr(
                                   TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                               .TranslateText ?? y.Group.State.FullName,

                    OrderNumber = y.Group.OrderNumber,
                    Title = y.Group.Translates.AsQueryable()
                               .FirstOrDefault(QuestionGroupTranslate.GetExpr(
                                   TranslateQuestionGroup.title, ServiceProvider.CultureHelper.CurrentCulture.Id))
                               .TranslateText ?? y.Group.Title,

                    Translates = y.Group.Translates.Select(t => new IhmaInv.BizLogicLayer.QuestionGroupService.QuestionGroupTranslateDto
                    {
                        LanguageId = t.LanguageId,
                        Language = t.Language.FullName
                    }).ToList(),

                    Questions = _unitOfWork.Context.QuestionnaireQuestions
                        .AsQueryable().IsActive()
                        .Where(x => x.OwnerId == y.Id)
                        .Select(r => new QuestionDto
                        {
                            Id = r.Question.Id,
                            StateId = r.Question.StateId,
                            State = r.Question.State.Translates.AsQueryable()
                                   .FirstOrDefault(StateTranslate.GetExpr(
                                       TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                   .TranslateText ?? r.Question.State.FullName,
                            OrderNumber = r.Question.OrderNumber,
                            QuestionText = r.Question.Translates.AsQueryable()
                                   .FirstOrDefault(QuestionTranslate.GetExpr(
                                       TranslateQuestion.question_text, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                   .TranslateText ?? r.Question.QuestionText,
                            AnswerTypeId = r.Question.AnswerTypeId,
                            AnswerType = r.Question.AnswerType.Translates.AsQueryable()
                                .FirstOrDefault(AnswerTypeTranslate.GetExpr(
                                    TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                .TranslateText ?? r.Question.AnswerType.FullName,
                            Translates = r.Question.Translates.Select(t => new IhmaInv.BizLogicLayer.QuestionServices.QuestionTranslateDto
                            {
                                LanguageId = t.LanguageId,
                                Language = t.Language.FullName
                            }).ToList(),
                            Answers = _unitOfWork.Context.QuestionnaireAnswers
                                .AsQueryable().IsActive()
                                .Where(x => x.OwnerId == r.Id)
                                .Select(x => new AnswerDto
                                {
                                    Id = x.AnswerId,
                                    OrderNumber = x.Answer.OrderNumber,
                                    AnswerText = x.Answer.Translates.AsQueryable()
                                       .FirstOrDefault(AnswerTranslate.GetExpr(
                                           TranslateAnswer.answer_text, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                       .TranslateText ?? x.Answer.AnswerText,
                                    StateId = x.Answer.StateId,
                                    State = x.Answer.State.Translates.AsQueryable()
                                       .FirstOrDefault(StateTranslate.GetExpr(
                                           TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                       .TranslateText ?? x.Answer.State.FullName,
                                    Translates = x.Answer.Translates.Select(t => new IhmaInv.BizLogicLayer.AnswerServices.AnswerTranslateDto
                                    {
                                        LanguageId = t.LanguageId,
                                        Language = t.Language.FullName
                                    }).ToList(),
                                    IsChecked = false,
                                }).ToList(),
                            Hint = r.Question.Hint,
                        }).ToList(),
                }).ToList(),
        };
    }
    public HaveId<long> Create(CreateQuestionnaireDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                foreach (var group in dto.Group)
                {
                    if (group.Id == 0)
                    {
                        var questionGroupDto = Configuration().Map<CreateQuestionGroupDlDto>(group);
                        var questionGroupEntity = _questionGroupRepository.Create(questionGroupDto);

                        CombineStatuses(_questionGroupRepository);
                        if (HasErrors) return null;

                        _unitOfWork.Save();
                        group.Id = questionGroupEntity.Id;
                    }

                    List<QuestionnaireQuestionDlDto> tempQuestion = new();
                    foreach (var question in group.Questions)
                    {
                        if (question.Id == 0)
                        {
                            var questionDto = Configuration().Map<CreateQuestionDlDto>(question);
                            var questionEntity = _questionRepository.Create(questionDto);

                            CombineStatuses(_questionRepository);
                            if (HasErrors) return null;

                            _unitOfWork.Save();
                            question.Id = questionEntity.Id;
                        }

                        List<QuestionnaireAnswerlDto> tempAnswer = new();
                        foreach (var answer in question.Answers.Where(p => p.Id == 0))
                        {
                            if (answer.Id == 0)
                            {
                                var answerDto = Configuration().Map<CreateAnswerDlDto>(answer);
                                var answerEntity = _answerRepository.Create(answerDto);

                                if (HasErrors) return null;

                                _unitOfWork.Save();
                                answer.Id = answerEntity.Id;
                            }
                            if (!tempAnswer.Any(p => p.Id == answer.Id))
                                tempAnswer.Add(new QuestionnaireAnswerlDto
                                {
                                    AnswerId = answer.Id
                                });
                        }

                        if (!tempQuestion.Any(p => p.QuestionId == question.Id))
                        {
                            tempQuestion.Add(new QuestionnaireQuestionDlDto
                            {
                                QuestionId = question.Id,
                                Answers = tempAnswer
                            });
                        }
                    }
                    if (HasErrors) return null;

                    if (!dto.GroupsOnlyIds.Any(p => p.GroupId == group.Id))
                    {
                        dto.GroupsOnlyIds.Add(
                            new QuestionnaireGroupDlDto
                            {
                                GroupId = group.Id,
                                Questions = tempQuestion,
                                StateId = StateIdConst.ACTIVE
                            });
                    }
                }

                var entity = _repository.Create(dto);
                CombineStatuses(_repository);
                if (HasErrors) return null;

                if (IsValid)
                {
                    _unitOfWork.Save();
                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                AddError($"{ex.Message} : {ex.InnerException}");
            }
        }
        return null;
    }
    public void Update(UpdateQuestionnaireDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                dto.Group.ForEach(group =>
                {
                    if (group.Id == 0)
                    {
                        var questionGroupDto = Configuration().Map<CreateQuestionGroupDlDto>(group);
                        var questionGroupEntity = _questionGroupRepository.Create(questionGroupDto);

                        CombineStatuses(_questionGroupRepository);
                        if (HasErrors) return;

                        _unitOfWork.Save();
                        group.Id = questionGroupEntity.Id;
                    }
                    else _questionGroupRepository.Update(group);

                    List<QuestionnaireQuestionDlDto> tempQuestion = new();
                    group.Questions.ForEach(ques =>
                    {
                        if (ques.Id == 0)
                        {
                            var questionDto = Configuration().Map<CreateQuestionDlDto>(ques);
                            var questionEntity = _questionRepository.Create(questionDto);

                            CombineStatuses(_questionRepository);
                            if (HasErrors) return;

                            _unitOfWork.Save();
                            ques.Id = questionEntity.Id;
                        }
                        else _questionRepository.Update(ques);

                        List<QuestionnaireAnswerlDto> tempAnswer = new();
                        ques.Answers.ForEach(ans =>
                        {
                            if (ans.Id == 0)
                            {
                                var answerDto = Configuration().Map<CreateAnswerDlDto>(ans);
                                var answerEntity = _answerRepository.Create(answerDto);

                                CombineStatuses(_questionRepository);
                                if (HasErrors) return;

                                _unitOfWork.Save();
                                ans.Id = answerEntity.Id;
                            }
                            else _answerRepository.Update(ans);

                            if (!tempAnswer.Any(p => p.Id == ans.Id))
                                tempAnswer.Add(new QuestionnaireAnswerlDto
                                {
                                    AnswerId = ans.Id,
                                });
                        });

                        if (!tempQuestion.Any(p => p.QuestionId == ques.Id))
                        {
                            tempQuestion.Add(new QuestionnaireQuestionDlDto
                            {
                                QuestionId = ques.Id,
                                Answers = tempAnswer
                            });
                        }
                    });

                    if (HasErrors) return;
                    if (!dto.GroupsOnlyIds.Any(p => p.GroupId == group.Id))
                    {
                        dto.GroupsOnlyIds.Add(
                            new QuestionnaireGroupDlDto
                            {
                                GroupId = group.Id,
                                Questions = tempQuestion,
                                StateId = group.StateId
                            });
                    }
                });
                _repository.Update(dto);
                CombineStatuses(_repository);
                if (HasErrors) return;

                if (IsValid)
                {
                    _unitOfWork.Save();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                AddError($"{ex.Message} : {ex.InnerException}");
            }
        }
    }
}