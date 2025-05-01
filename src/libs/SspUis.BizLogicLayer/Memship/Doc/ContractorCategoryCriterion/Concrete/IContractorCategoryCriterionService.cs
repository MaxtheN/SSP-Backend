using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IContractorCategoryCriterionService
    : IBaseEntityService<long, ContractorCategoryCriterion, ContractorCategoryCriterionListDto, ContractorCategoryCriterionDto, CreateContractorCategoryCriterionDlDto, UpdateContractorCategoryCriterionDlDto, ContractorCategoryCriterionSortFilterOptions>
{
    PagedResult<ContractorCategoryCriterionListDto> GetList(ContractorCategoryCriterionSortFilterOptions options);
    ContractorCategoryCriterionDto Get();
    ContractorCategoryCriterionDto Get(long id);
    SelectList<long> AsSelectList();
    HaveId<long> Create(CreateContractorCategoryCriterionDlDto dto);
    void Accept(UpdateStatusContractorCategoryCriterionDto dTo);
    void Cancel(UpdateStatusContractorCategoryCriterionDto dTo);
    void Update(UpdateContractorCategoryCriterionDlDto dto);
    void Delete(long id);
}
