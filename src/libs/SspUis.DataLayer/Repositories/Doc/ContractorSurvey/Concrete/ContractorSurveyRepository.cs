using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ContractorSurveyRepository : BaseEntityRepository<long, ContractorSurvey, CreateContractorSurveyDlDto, UpdateContractorSurveyDlDto>, IContractorSurveyRepository
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public ContractorSurveyRepository(ICrudServices crudServices,
        IAuthService authService, IUnitOfWork unitOfWork)
        : base(crudServices)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    protected override IQueryable<ContractorSurvey> ByIdQuery()
        => AllAsQueryable.Include(a => a.Groups).ThenInclude(x => x.Questions).ThenInclude(x => x.Answers);
    protected override void OnCreate(ContractorSurvey entity, CreateContractorSurveyDlDto dto)
    {
        if(_authService.Contractor == null)
        {
            AddError("Contractor is null. // Info: ContractorSurveyRepository");
            return;
        }
        entity.ContractorId = _authService.Contractor.Id;
    }
    public ContractorSurvey UpdateStatus(UpdateStatusContractorSurveyDlDto dto)
    {
        var entity = ById(dto.Id);

        if (HasErrors)
            return null;

        dto.UpdateEntity(entity);
        Context.Entry(entity).State = EntityState.Modified;

        return entity;
    }
    protected override IQueryable<ContractorSurvey> InjectFilter(IQueryable<ContractorSurvey> query)
        => query.Where(a => _authService.User == null || a.ContractorId == _authService.Contractor.Id
            && a.StatusId != StatusIdConst.DELETED);
}
