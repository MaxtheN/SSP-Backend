using SspUis.BizLogicLayer.QuestionnaireService;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE;
using WEBASE.Constants;
using WEBASE.Models;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public class ContractorSurveyService : StatusGenericHandler, IContractorSurveyService
{
    private readonly IContractorSurveyRepository _repository;
    private readonly IQuestionnaireService _service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly INumberService _numberService;

    public ContractorSurveyService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        INumberService numberService,
        IQuestionnaireService service)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _numberService = numberService;
        _repository = _unitOfWork.ContractorSurveyRepository;
        _service = service;
    }
    public PagedResult<ContractorSurveyListDto> GetList(ContractorSurveySortFilterPageOptions options)
    {
        var query = _repository.ReadAsNoTracked<ContractorSurveyListDto>()
           .SortFilter(options);

        return query.AsPagedResult(options);
    }
    public HaveId<long> Create(CreateContractorSurveyDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.Create(dto, ent => Validation(dto, ent));

                CombineStatuses(_repository);
                if (HasErrors) return null;

                _unitOfWork.Save();

                if (IsValid)
                {   
                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                var innEx = ex.GetInnermostException();
                AddError($"{innEx.Message}:{innEx.StackTrace}");
                return null;
            }
            finally
            {
                transaction.Dispose();
            }

            return null;
        }
    }
    public ContractorSurveyDto Get()
    {
        var numberResult = _numberService.GetNext(TableIdConst.QUIZ__DOC_CONTRACTOR_SURVEY.ToString());
        return new ContractorSurveyDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Today),
            DocNumber = $"{numberResult.Item2}-{$"{numberResult.Item1}".PadLeft(6, '0')}"
        };
    }
    public ContractorSurveyDto Get(long id)
    {
        var dto = _repository.ById<ContractorSurveyDto>(id);

        QuestionnaireDto questionDto = _service.Get(dto.QuestionnaireId);
        dto.Group = questionDto.Group;

        var whenTextAvailable = dto.Tables.Where(p => p.TextAnswer is not null).Select(a => new
        {
            t = a.TextAnswer,
            g = a.GroupId,
            q = a.QuestionId,
        }).ToDictionary(
            b => new
            {
                group = b.g,
                question = b.q
            },
            b => b.t);

        var data = dto.Tables.Where(p => p.TextAnswer is null).Select(a => new
        {
            g = a.GroupId,
            q = a.QuestionId,
            a = a.AnswerId
        }).ToHashSet();

        foreach (var group in dto.Group)
        {
            foreach (var question in group.Questions)
            {
                if (whenTextAvailable.ContainsKey(new { group = group.Id, question = question.Id }))
                {
                    question.AnswerText = whenTextAvailable[new { group = group.Id, question = question.Id }];
                }
                foreach (var answer in question.Answers)
                {
                    if (data.Contains(new { g = group.Id, q = question.Id, a = answer.Id }))
                    {
                        answer.IsChecked = true;
                    }
                }
            }
        }

        return dto;
    }
    public HaveId<long> Delete(long id)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.ById(id);

                if (entity == null)
                {
                    AddError($"Ҳужжат топилмади. ID: {id}");
                    return null;
                }

                UpdateStatus(new UpdateStatusContractorSurveyDto
                {
                    Id = entity.Id
                }, StatusIdConst.DELETED);

                if (HasErrors)
                    return HaveId.Create(id);

                _unitOfWork.Save();

                CombineStatuses(_repository);
                if (HasErrors)
                    transaction.Rollback();

                if (IsValid)
                {
                    transaction.Commit();
                    return HaveId.Create(id);
                }

                return null;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusContractorSurveyDto dto, int statusId, bool isLockDocument = true)
    {
        var updateDto = new UpdateStatusContractorSurveyDlDto
        {
            Id = dto.Id,
            StatusId = statusId
        };

        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        if (isLockDocument && HasErrors)
        {
            transaction.Rollback();
            return null;
        }
        try
        {
            var entity = _repository.UpdateStatus(updateDto);
            CombineStatuses(_repository);

            if (HasErrors)
                return null;

            _unitOfWork.Save();

            if (canCommit)
            {
                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            return null;
        }
        catch
        {
            if (canCommit)
                transaction?.Rollback();
            throw;
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    private void Validation<TDto>(ContractorSurveyDlDto<TDto> dto, ContractorSurvey entity)
            where TDto : ContractorSurveyDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (dto is CreateContractorSurveyDlDto createDto)
        {
            // validation kerak: qaysiki answerType ga qarab javob berishi kerak yoki text polya yoki bitta javob yoki bir nechta javob
        }
        else
        {

        }
    }
}
