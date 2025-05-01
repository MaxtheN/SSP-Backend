using SspUis.BizLogicLayer.Administration.ContractorServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Bandlik.Models;
using SspUis.Integration.BankCredit.Models;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.DavAktiv;
using SspUis.Integration.Finance.Models;
using SspUis.Integration.Investitsiya.Models;
using SspUis.Integration.MarkaziyBank;
using SspUis.Integration.Soliq.Models;
using SspUis.Integration.TadbirkorFund;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ContractorServices
{
    public interface IContractorService : IBaseEntityService<long, Contractor, ContractorListDto, ContractorDto, CreateContractorDlDto, UpdateContractorDlDto>
    {
        PagedResult<ContractorListDto> GetListForEmloyee(SortFilterPageOptions dto);
        Task<ContractorDto> GetByInn(string inn);
        Task<ContractorDto> GetByPinfl(string pinfl);
        Task<ContractorDto> GetByInnFromSoliq(string inn);
        SelectList<long> AsSelectList();
        PagedSelectList<long> AsPagedSelectList(ContractorPagedSelectListOptions options);
        UpdateContractorOkedsDto GetOkeds();
        UpdateContractorOkedsDto UpdateOkeds(UpdateContractorOkedsDto dto);
        void ImportNotBudgetContractorInn(List<ImportNotBudgetNotBudgetContractorInnDlDto> listDto);
        Task<DavAktivContractorDto> GetByInnFromDavAktiv(string inn);
        Task<TadbirkorFundContractorDto> GetByInnFromTadbirkorFund(string inn);
        Task<List<MarkaziyBankContractorCreditHistoryDto>> GetMarkaziyBankCreditHistoryByInn(MarkaziyBankContractorCreditHistoryRequestDto dto);
        Task UpdateAllFromSoliq();
        Task<SoliqContractorByTinDto> GetFromSoliq(string inn);

        Task<SoliqContractorDebtByTinDataDto> GetDebtFromSoliq(string tin, int year);
        Task<SoliqContractorEmployeeCountByTinDataDto> GetEmployeeCountFromSoliq(string tin, int year, int month);
        Task<SoliqContractorFinanceBenefitByTinDataDto> GetFinanceBenefitFromSoliq(string tin, int year, int period);
        Task<FarmerRefundByInnDataDto> GetFarmerRefundByInnFromSoliq(string inn, int year);
        Task<List<ImtiyozDataByInnDataDto>> GetImtiyozDataByInnFromSoliq(string inn, int year);
        Task<GTDFromBojxonaDto> GetGTDFromBojxona(GetGTDByInnRequestDto dto);
        Task<GetStatisticByInnDataDto> GetStatisticFromBandlik(string inn);
        Task<GetDaftarBySoatoDataDto> GetDaftarBySoatoFromBandlik(GetDaftarBySoatoQuery dto);
        Task<BankCreditApplicationsResponse> GetApplicationsFromBankCredit(string inn, int offerSigned = 1);
        Task<List<BankCreditContractStatusResponse>> GetContractStatusFromBankCredit();
        Task<List<InvestmentContract>> GetInvestmentContracts(InvestitsiyaRequestDto dto);
        
        decimal? GetAylanmaByInn(string innPinfl);

        Task UpdateAllFromDavAktiv();
        Task UpdateFromSoliq(long id);
        void AddContactInfo(long ownerId, int contactTypeId, string contact);
        Task<Stream> SaveAsExecelFromBojxona(GetGTDByInnRequestDto dto);
        void CreateContractorOffers(ContractorOfferDto dto);
        void UpdateSettlementAccount(UpdateContractorSettlementAccountDlDto dto);
        IQueryable<ContractorListDto> GetListByIds(long[] contractorIds);
        Task<ContractorDto> SearchByInnPnfl(string innpnfl);

		void ChangeBasicSettlementAccounting(long contractorId, long settlementAccountId);

        //Task<PagedResult<GetPayDocsDto>> GetPayDocsFilter(GetPayDocsSortFilterOptions options);

    }
}
