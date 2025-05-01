using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OfficeOpenXml;
//using RestSharp.Extensions;
using SspUis.BizLogicLayer.Administration.ContractorServices;
using SspUis.BizLogicLayer.IntegrationServices.Finance.Concrete;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Bandlik.Models;
using SspUis.Integration.Bandlik.Services;
using SspUis.Integration.BankCredit;
using SspUis.Integration.BankCredit.Models;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.Bojxona.Services;
using SspUis.Integration.DavAktiv;
using SspUis.Integration.Finance.Models;
using SspUis.Integration.Finance.Services;
using SspUis.Integration.Investitsiya.Models;
using SspUis.Integration.Investitsiya.Services;
using SspUis.Integration.MarkaziyBank;
using SspUis.Integration.Soliq;
using SspUis.Integration.Soliq.Models;
using SspUis.Integration.TadbirkorFund;
using WEBASE;
using WEBASE.EF;
using WEBASE.i18n;
using WEBASE.Integration.Manuals.Services;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ContractorServices
{
    public class ContractorService : BaseEntityService<long, Contractor, ContractorListDto, ContractorDto,
        CreateContractorDlDto, UpdateContractorDlDto, IContractorRepository>, IContractorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ISoliqContractorService _soliqService;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;
        private readonly IBojxonaService _bojxonaService;
        private readonly IBankCreditService _bankCreditService;
        private readonly IBandlikService _bandlikService;
        private readonly IDavAktivContractorService _davAktivContractorService;
        private readonly ITadbirkorFundContractorService _tadbirkorFundContractorService;
        private readonly IMarkaziyBankContractorService _markaziyBankContractorService;
        private readonly IUOWIntegrationManuals _uowIntegrationManuals;
        private readonly INotBudgetContractorRepository _notBudgetContractorRepository;
        private readonly IInvestitsiyaService _investitsiyaService;
        private readonly IFinanceService _financeService;

        public ContractorService(IUnitOfWork unitOfWork,
                                 IAuthService authService,
                                 IUOWIntegrationManuals uowIntegrationManuals,
                                 INotBudgetContractorRepository notBudgetContractorRepository,
                                 ISoliqContractorService soliqService,
                                 IBankCreditService bankCreditService,
                                 IBojxonaService bojxonaService,
                                 IBandlikService bandlikService,
                                 IDavAktivContractorService davAktivContractorService,
                                 ITadbirkorFundContractorService tadbirkorFundContractorService,
                                 IMarkaziyBankContractorService markaziyBankContractorService,
                                 IStorageService storageService,
                                 ICultureHelper cultureHelper,
                                 IInvestitsiyaService investitsiyaService,
                                 IFinanceService financeService)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _uowIntegrationManuals = uowIntegrationManuals;
            _notBudgetContractorRepository = notBudgetContractorRepository;
            _soliqService = soliqService;
            _bankCreditService = bankCreditService;
            _storageService = storageService;
            _cultureHelper = cultureHelper;
            _bojxonaService = bojxonaService;
            _bandlikService = bandlikService;
            _davAktivContractorService = davAktivContractorService;
            _tadbirkorFundContractorService = tadbirkorFundContractorService;
            _markaziyBankContractorService = markaziyBankContractorService;
            _investitsiyaService = investitsiyaService;
            _financeService = financeService;
        }
        protected override IQueryable<ContractorListDto> SortFilter(IQueryable<ContractorListDto> query, SortFilterPageOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options);
        }
        public SelectList<long> AsSelectList()
        {
            return Repository.AllAsQueryable.AsSelectList();
        }
        public PagedSelectList<long> AsPagedSelectList(ContractorPagedSelectListOptions options)
        {
            return Repository.ReadAsNoTracked<ContractorListDto>().AsPagedResult(options).AsPagedSelectList();
        }
        public PagedResult<ContractorListDto> GetListForEmloyee(SortFilterPageOptions dto)
        {
            var result = Repository.ReadAsNoTracked<ContractorListDto>(applyFilter: false)
                                    .SortFilter(dto)
                                    .AsPagedResult(dto);
            return result;
        }
        public IQueryable<ContractorListDto> GetListByIds(long[] contractorIds)
        {
            var result = Repository.ReadAsNoTracked<ContractorListDto>(applyFilter: false)
                                    .Where(x => contractorIds.Contains(x.Id));
            return result;
        }
        public override HaveId<long> Create(CreateContractorDlDto dto)
        {
            var entity = Repository.Create(dto);
            CombineStatuses(Repository);
            if (IsValid)
            {
                UnitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }
        public async Task UpdateFromSoliq(long id)
        {
            var ent = Repository.ById(id);
            var dto = await GetByInnFromSoliq(ent.Inn);
            if (dto != null)
            {
                dto.Id = ent.Id;
                var updateDto = new UpdateContractorDlDto
                {
                    Id = dto.Id,
                    Inn = ent.Inn,
                    ShortName = dto.ShortName,
                    FullName = dto.FullName,
                    Pinfl = dto.Pinfl,
                    OkedId = dto.OkedId,
                    BankId = dto.BankId,
                    CountryId = dto.CountryId,
                    RegionId = dto.RegionId,
                    DistrictId = dto.DistrictId,
                    Address = dto.Address,
                    Accounter = dto.Accounter,
                    Director = dto.Director,
                    PhoneNumber = dto.PhoneNumber,
                    Contact = dto.Contact,
                    VatCode = dto.VatCode,
                    OrganizationLegalFormId = dto.OrganizationLegalFormId,
                    RegistrationDate = dto.RegistrationDate,
                    StateId = StateIdConst.ACTIVE,
                    SettlementAccounts = ent.SettlementAccounts.Select(a => new ContractorSettlementAccountDlDto
                    {
                        AccountCode = a.AccountCode,
                        AccountName = a.AccountName,
                        BankId = a.BankId,
                        Id = a.Id,
                        StateId = a.StateId
                    }).ToList(),
                };

                foreach (var bSetAcc in dto.SettlementAccounts.Select(a => (ContractorSettlementAccountDlDto)a).ToList())
                {
                    if (updateDto.SettlementAccounts.Any(a => a.AccountCode == bSetAcc.AccountCode))
                        continue;
                    updateDto.SettlementAccounts.Add(bSetAcc);
                }


                Repository.Update(updateDto);
                CombineStatuses(Repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
        }
        public async Task UpdateAllFromSoliq()
        {
            var contractors = Repository.AllAsQueryable.IsActive()
                                                       .Where(a => a.Inn != null
                                                               && a.OpfId == null)
                                                       .Select(a => new { a.Id, a.Inn })
                                                       .ToArray();
            ContractorDto dto;
            for (int i = 0; i < contractors.Length; i++)
            {
                dto = await GetByInnFromSoliq(contractors[i].Inn);
                if (dto != null)
                {
                    dto.Id = contractors[i].Id;
                    Repository.Update(new UpdateContractorDlDto
                    {
                        Id = dto.Id,
                        Inn = contractors[i].Inn,
                        ShortName = dto.ShortName,
                        FullName = dto.FullName,
                        Pinfl = dto.Pinfl,
                        OkedId = dto.OkedId,
                        BankId = dto.BankId,
                        CountryId = dto.CountryId,
                        RegionId = dto.RegionId,
                        DistrictId = dto.DistrictId,
                        Address = dto.Address,
                        Accounter = dto.Accounter,
                        Director = dto.Director,
                        PhoneNumber = dto.PhoneNumber,
                        Contact = dto.Contact,
                        VatCode = dto.VatCode,
                        OrganizationLegalFormId = dto.OrganizationLegalFormId,
                        RegistrationDate = dto.RegistrationDate,
                        StateId = StateIdConst.ACTIVE,
                        OpfId = dto.OpfId,
                        Kfs = dto.Kfs,
                        Soogu = dto.Soogu,
                        SooguRegistrator = dto.SooguRegistrator,
                        RegistrationNumber = dto.RegistrationNumber,
                        BusinessFund = dto.BusinessFund,
                        VillageCode = dto.VillageCode,
                        VillageName = dto.VillageName,
                        TaxRate = dto.TaxRate,
                        AvgNumberEmployees = dto.AvgNumberEmployees,
                        MonthlyNumberEmployees = dto.MonthlyNumberEmployees,
                        SettlementAccounts = dto.SettlementAccounts.Select(a => (ContractorSettlementAccountDlDto)a).ToList(),
                    });
                }
                Thread.Sleep(10);
                if (i % 500 == 0 && i > 0)
                {
                    _unitOfWork.Save();
                    _unitOfWork.Context.ChangeTracker.Clear();
                }
            }
            _unitOfWork.Save();
        }
        public async Task UpdateAllFromDavAktiv()
        {
            var contractors = Repository.AllAsQueryable.IsActive().Where(a => a.Inn != null && a.GovShare == null).Select(a => new { a.Id, a.Inn }).ToArray();
            ContractorDto dto;
            DavAktivContractorDto davAktivDto;
            for (int i = 0; i < contractors.Length; i++)
            {
                Thread.Sleep(10);
                if (i % 500 == 0 && i > 0)
                {
                    _unitOfWork.Save();
                    _unitOfWork.Context.ChangeTracker.Clear();
                }
                davAktivDto = await GetByInnFromDavAktiv(contractors[i].Inn);
                if (davAktivDto == null)
                    continue;
                dto = Get(contractors[i].Id);
                if (dto != null)
                {
                    dto.Id = contractors[i].Id;
                    Repository.Update(new UpdateContractorDlDto
                    {
                        Id = dto.Id,
                        Inn = contractors[i].Inn,
                        ShortName = dto.ShortName,
                        FullName = dto.FullName,
                        Pinfl = dto.Pinfl,
                        OkedId = dto.OkedId,
                        BankId = dto.BankId,
                        CountryId = dto.CountryId,
                        RegionId = dto.RegionId,
                        DistrictId = dto.DistrictId,
                        Address = dto.Address,
                        Accounter = dto.Accounter,
                        Director = dto.Director,
                        PhoneNumber = dto.PhoneNumber,
                        Contact = dto.Contact,
                        VatCode = dto.VatCode,
                        OrganizationLegalFormId = dto.OrganizationLegalFormId,
                        RegistrationDate = dto.RegistrationDate,
                        StateId = StateIdConst.ACTIVE,
                        SettlementAccounts = dto.SettlementAccounts.Select(a => (ContractorSettlementAccountDlDto)a).ToList(),
                        GovShare = decimal.Parse(davAktivDto.Share1)
                    });
                }
            }
            _unitOfWork.Save();
        }
        public async Task<ContractorDto> GetByInn(string inn)
        {
            var dto = Repository.ReadAsNoTracked<ContractorDto>()
                                .FirstOrDefault(a => a.Inn == inn);
            if (dto == null)
            {
                //dto = await GetByInnFromGnk(inn);
                dto = await GetByInnFromSoliq(inn);
                if (dto == null)
                {
                    //AddError($"Soliqdan ma'lumot topilmadi. Inn {inn} / Äàííûå íå íàéäåíû. ÈÍÍ {inn}");
                    return null;
                }
                dto.LoadSettlementAccountsToBase();
            }
            return dto;
        }
        public async Task<ContractorDto> GetByPinfl(string pinfl)
        {
            var dto = Repository.ReadAsNoTracked<ContractorDto>()
                                .FirstOrDefault(a => a.Pinfl == pinfl);

            if (dto == null)
            {
                dto = await GetByPinflFromSoliq(pinfl);
                if (dto == null)
                {
                    return null;
                }
                dto.LoadSettlementAccountsToBase();
            }

            return dto;
        }
        public UpdateContractorOkedsDto UpdateOkeds(UpdateContractorOkedsDto dto)
        {
            var contractor = Repository.ById(_authService.Contractor.Id);
            contractor.Okeds.UpdateFromForeignKeys(dto.Okeds.Select(a => a.Id).ToList());
            UnitOfWork.Context.Entry(contractor).State = EntityState.Modified;
            UnitOfWork.Save();
            return GetOkeds();
        }
        public UpdateContractorOkedsDto GetOkeds()
        {
            var contractor = Repository.ById(_authService.Contractor.Id);
            var dto = new UpdateContractorOkedsDto
            {
                Okeds = UnitOfWork.Context.Set<ContractorOked>()
                                          .Include(a => a.Oked)
                                          .ThenInclude(a => a.Translates)
                                          .Where(a => a.OwnerId == contractor.Id)
                                          .Select(a => new UpdateContractorOkedDto
                                          {
                                              Id = a.OkedId,
                                              Code = a.Oked.Code,
                                              Name = a.Oked.Translates.AsQueryable()
                                                                      .FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name)).TranslateText ?? a.Oked.FullName
                                          })
                                          .ToList()
            };

            return dto;
        }
        public async Task<ContractorDto> GetByInnFromSoliq(string inn)
        {
            var soliqContractor = await _soliqService.GetByInn(inn);
            CombineStatuses(_soliqService);
            if (soliqContractor == null)
                return null;
            var dto = new ContractorDto
            {
                InnOrPinfl = soliqContractor.Company.Tin,
                Inn = soliqContractor.Company.Tin,
                ShortName = soliqContractor.Company.ShortName,
                FullName = soliqContractor.Company.Name,
                Address = soliqContractor.CompanyBillingAddress.StreetName,
                RegistrationDate = soliqContractor.Company.RegistrationDate.AsDateOnly(),
                OpfId = soliqContractor.Company.Opf,
                Kfs = soliqContractor.Company.Kfs,
                Soogu = soliqContractor.Company.Soogu,
                SooguRegistrator = soliqContractor.Company.SooguRegistrator,
                RegistrationNumber = soliqContractor.Company.RegistrationNumber,
                BusinessFund = soliqContractor.Company.BusinessFund,
                VillageCode = soliqContractor.Company.VillageCode,
                VillageName = soliqContractor.Company.VillageName,
                TaxRate = soliqContractor.Company.TaxRate,
                BusinessType = soliqContractor.Company.BusinessType,
                AvgNumberEmployees = soliqContractor.CompanyExtraInfo?.AvgNumberEmployees,
                MonthlyNumberEmployees = soliqContractor.CompanyExtraInfo?.MonthlyNumberEmployees
            };
            if (soliqContractor.Director != null)
            {
                dto.Director = $"{soliqContractor.Director.LastName} {soliqContractor.Director.FirstName} {soliqContractor.Director.MiddleName ?? ""}";
                dto.DirectorSeria = soliqContractor.Director.PassportSeries;
                dto.DirectorNumber = soliqContractor.Director.PassportNumber;
                dto.DirectorPinfl = soliqContractor.Director.Pinfl;
                dto.DirectorBirthDate = soliqContractor.Director.BirthDate;
            }
            if (soliqContractor.Accountant != null)
            {
                dto.Accounter = $"{soliqContractor.Accountant.LastName} {soliqContractor.Accountant.FirstName} {soliqContractor.Accountant.MiddleName ?? ""}";
            }

            var oked = UnitOfWork.OkedRepository.ByCode(soliqContractor.Company.Oked);
            if (oked != null)
            {
                dto.OkedId = oked.Id;
                dto.Oked = oked.Translates.AsQueryable()
                           .FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? oked?.FullName;
            }


            Bank bank;
            foreach (var companyBank in soliqContractor.CompanyBanks.Where(a => !string.IsNullOrEmpty(a.Mfo)
                                                                             && !string.IsNullOrEmpty(a.BankName)
                                                                             && string.IsNullOrEmpty(a.StatusName)))
            {
                bank = UnitOfWork.BankRepository.ByCode(companyBank.Mfo);
                if (bank == null)
                {
                    UnitOfWork.BankRepository.Create(new CreateBankDlDto
                    {
                        Code = companyBank.Mfo,
                        BankName = companyBank.BankName
                    });
                    CombineStatuses(UnitOfWork.BankRepository);
                    if (HasErrors)
                        return null;
                    UnitOfWork.Save();
                    bank = UnitOfWork.BankRepository.ByCode(companyBank.Mfo);
                }
                if (dto.BankId == null)
                {
                    dto.BankId = bank.Id;
                    dto.Bank = bank.Translates.AsQueryable()
                               .FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? bank?.BankName;
                }

                var settlement = new ContractorSettlementAccountDto
                {
                    BankId = bank.Id,
                    BankCode = bank.Code,
                    Bank = bank.BankName,
                    AccountCode = companyBank.PaymentAccount,
                    AccountName = companyBank.PaymentAccount,
                    StateId = StateIdConst.ACTIVE
                };

                if (!dto.SettlementAccounts.Any(x => x.AccountCode == settlement.AccountCode && x.BankId == settlement.BankId))
                    dto.SettlementAccounts.Add(settlement);
            }

            var district = UnitOfWork.DistrictRepository.BySoato($"{soliqContractor.CompanyBillingAddress.District.Code}");
            if (district != null)
            {
                dto.DistrictId = district.Id;
                dto.District = district.Translates.AsQueryable()
                                       .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? district.FullName;
                var region = UnitOfWork.RegionRepository.ById(district.RegionId);
                var country = UnitOfWork.CountryRepository.ById(region.CountryId);
                dto.CountryId = country.Id;
                dto.Country = country.Translates.AsQueryable()
                                   .FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? country.FullName;

                dto.RegionId = region.Id;
                dto.Region = region.Translates.AsQueryable()
                                   .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? region.FullName;
            }
            return dto;
        }
        public async Task<ContractorDto> GetByPinflFromSoliq(string pinfl)
        {
            var contractor = await _soliqService.GetByPinfl(pinfl);
            CombineStatuses(_soliqService);

            if (contractor is null)
                return null;

            if (contractor.Entrepreneur is null)
                return null;

            if (contractor.Entrepreneur.EntrepreneurshipAddress is null)
                return null;

            if (contractor.Entrepreneur.EntrepreneurshipAddress.SoatoCode == 0)
                return null;

            var dto = new ContractorDto
            {
                //ShortName = contractor.FullName.Uzb ?? contractor.FullName.Rus ?? "",
                //FullName = contractor.FullName.Uzb ?? contractor.FullName.Rus ?? "",
                //InnOrPinfl = contractor.Pinfl.ToString(),
                //Pinfl = contractor.Pinfl.ToString(),
                //Address = contractor.Address,
                //Accounter = contractor.FullName.Uzb ?? contractor.FullName.Rus ?? "",
                //Director = "",
                //PhoneNumber = "",
                //Contact = "",
                //VatCode = "",
                //RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
                //Soogu = "",
                //SooguRegistrator = "",
                //RegistrationNumber = "",

                ShortName = contractor.Entrepreneur.FormName is not null
                    ? contractor.Entrepreneur.FormName.Uzb ?? contractor.Entrepreneur.FormName.Rus ?? contractor.Entrepreneur.FormName.Eng
                    : "",
                FullName = contractor.Entrepreneur.FormName is not null
                    ? contractor.Entrepreneur.FormName.Uzb ?? contractor.Entrepreneur.FormName.Rus ?? contractor.Entrepreneur.FormName.Eng
                    : "",
                InnOrPinfl = contractor.Pinfl.ToString(),
                Inn = contractor.Tin is null ? null : contractor.Tin.ToString(),
                Pinfl = contractor.Pinfl.ToString(),
                Address = contractor.Address ?? "",
                Accounter = contractor.Entrepreneur.EntrepreneurshipDirector is not null
                    ? $"{contractor.Entrepreneur.EntrepreneurshipDirector.FirstName ?? ""} " +
                      $"{contractor.Entrepreneur.EntrepreneurshipDirector.LastName ?? ""} " +
                      $"{contractor.Entrepreneur.EntrepreneurshipDirector.MiddleName ?? ""}"
                    : "",
                Director = contractor.Entrepreneur.EntrepreneurshipDirector is not null
                    ? $"{contractor.Entrepreneur.EntrepreneurshipDirector.FirstName ?? ""} " +
                      $"{contractor.Entrepreneur.EntrepreneurshipDirector.LastName ?? ""} " +
                      $"{contractor.Entrepreneur.EntrepreneurshipDirector.MiddleName ?? ""}"
                    : "",
                PhoneNumber = contractor.Entrepreneur.PhoneNumber is not null ? contractor.Entrepreneur.PhoneNumber.ToString() : "",
                Contact = contractor.Entrepreneur.EntrepreneurshipContact is not null
                ? $"Mobile: {contractor.Entrepreneur.EntrepreneurshipContact.MobilePhoneNumber} " +
                  $"// Cell: {contractor.Entrepreneur.EntrepreneurshipContact.CellPhoneNumber}"
                : "",
                VatCode = contractor.Entrepreneur.VatNumber ?? "",
                RegistrationDate = DateTime.TryParse(contractor.Entrepreneur.RegistrationDate, out DateTime dateTime)
                    ? dateTime.AsDateOnly()
                    : DateOnly.FromDateTime(DateTime.Today),
                Soogu = "",
                SooguRegistrator = "",
                RegistrationNumber = contractor.Entrepreneur.RegistrationId,
                VillageCode = contractor.VillageCode is not null ? int.Parse(contractor.VillageCode) : null,
                VillageName = contractor.VillageName ?? "",
                TaxRate = 0
            };

            int correctSoato = contractor.Entrepreneur.EntrepreneurshipAddress.SoatoCode == 1714367
                ? 1714401367
                : contractor.Entrepreneur.EntrepreneurshipAddress.SoatoCode;
            
            var district = UnitOfWork.DistrictRepository.BySoato($"{correctSoato}");
            if (district is not null)
            {
                dto.DistrictId = district.Id;
                dto.District = district.Translates.AsQueryable()
                                       .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                                       ?? district.FullName;

                var region = UnitOfWork.RegionRepository.ById(district.RegionId);
                dto.RegionId = region.Id;
                dto.Region = region.Translates.AsQueryable()
                                   .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                                   ?? region.FullName;

                var country = UnitOfWork.CountryRepository.ById(region.CountryId);
                dto.CountryId = country.Id;
                dto.Country = country.Translates.AsQueryable()
                                   .FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                                   ?? country.FullName;
            }

            return dto;
        }
        public async Task<DavAktivContractorDto> GetByInnFromDavAktiv(string inn)
        {
            var dto = await _davAktivContractorService.GetByInn(inn);
            return dto;
        }
        public async Task<TadbirkorFundContractorDto> GetByInnFromTadbirkorFund(string inn)
        {
            var dto = await _tadbirkorFundContractorService.GetByInn(inn);
            return dto;
        }
        public async Task<List<MarkaziyBankContractorCreditHistoryDto>> GetMarkaziyBankCreditHistoryByInn(MarkaziyBankContractorCreditHistoryRequestDto requestDto)
        {
            var fromDate = requestDto.FromDate.ToString("yyyy-MM-dd");
            var toDate = requestDto.ToDate.ToString("yyyy-MM-dd");
            var dto = await _markaziyBankContractorService.GetCreditHistoryByInn(requestDto.Inn, fromDate, toDate);
            return dto;
        }
        public async Task<SoliqContractorByTinDto> GetFromSoliq(string inn)
        {
            var dto = await _soliqService.GetByInn(inn);
            return dto;
        }
        public async Task<SoliqContractorDebtByTinDataDto> GetDebtFromSoliq(string inn, int year)
        {
            var dto = await _soliqService.GetDebtByInn(inn, year);
            CombineStatuses(_soliqService);
            if (HasErrors)
            {
                return null;
            }
            return dto;
        }
        public async Task<FarmerRefundByInnDataDto> GetFarmerRefundByInnFromSoliq(string inn, int year)
        {
            var dto = await _soliqService.GetFarmerRefundByInn(inn, year);
            CombineStatuses(_soliqService);
            if (HasErrors)
            {
                return null;
            }
            return dto;
        }
        public async Task<List<ImtiyozDataByInnDataDto>> GetImtiyozDataByInnFromSoliq(string inn, int year)
        {
            var dto = await _soliqService.GetImtiyozDataByInn(inn, year);
            CombineStatuses(_soliqService);
            if (HasErrors)
            {
                return null;
            }
            return dto;
        }
        public async Task<SoliqContractorEmployeeCountByTinDataDto> GetEmployeeCountFromSoliq(string inn, int year, int month)
        {
            var dto = await _soliqService.GetEmployeeCountByInn(inn, year, month);
            CombineStatuses(_soliqService);
            return dto;
        }
        public async Task<SoliqContractorFinanceBenefitByTinDataDto> GetFinanceBenefitFromSoliq(string inn, int year, int period)
        {
            var dto = await _soliqService.GetFinanceBenefitByInn(inn, year, period);
            return dto;
        }
        public async Task<GTDFromBojxonaDto> GetGTDFromBojxona(GetGTDByInnRequestDto dto)
        {
            var data = await _bojxonaService.GetGTDByInn(dto);
            CombineStatuses(_bojxonaService);
            if (data == null)
            { return new GTDFromBojxonaDto(); }
            if (dto.CountryId != null)
            {
                data = data.Where(a => a.EkimcountryCode == _unitOfWork.Context.Countries.FirstOrDefault(a => a.Id == dto.CountryId).Code).ToList();

            }
            var country = new Country();
            foreach (var item in data)
            {
                country = _unitOfWork.Context.Countries.Include(a => a.Translates).FirstOrDefault(a => a.Code == item.EkimcountryCode);
                item.EkimcountryFullName = country.Translates.AsQueryable()
                    .FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?
                        .TranslateText ?? country.FullName;
            }
            var result = new GTDFromBojxonaDto();
            result.Imports = data.Where(a => a.Mode == "ИМ40").ToList();
            result.Exports = data.Where(a => a.Mode == "ЭК10").ToList();
            return result;
        }
        public decimal? GetAylanmaByInn(string innPinfl)
        {
            if (innPinfl.Length != 9 && innPinfl.Length != 14)
            {
                AddError("Inn yoki Pinfl yaroqsiz");
                return null;
            }
            else if (innPinfl.Length == 9)
            {
                return _unitOfWork.Context.ContractorAylanmas.FirstOrDefault(a => a.Inn == innPinfl)?.Amount;
            }
            else
            {
                return _unitOfWork.Context.ContractorAylanmas.FirstOrDefault(a => a.Pinfl == innPinfl)?.Amount;
            }
        }
        public async Task<GetStatisticByInnDataDto> GetStatisticFromBandlik(string inn)
        {
            var data = await _bandlikService.GetStatisticByInn(inn);
            CombineStatuses(_bandlikService);
            return data;
        }
        public async Task<GetDaftarBySoatoDataDto> GetDaftarBySoatoFromBandlik(GetDaftarBySoatoQuery dto)
        {
            var data = await _bandlikService.GetDaftarBySoato(dto);
            CombineStatuses(_bandlikService);
            return data;
        }
        public async Task<GetStatisticByInnDataDto> GetInfoEmpByInnFromBandlik(string inn)
        {
            var data = await _bandlikService.GetStatisticByInn(inn);
            CombineStatuses(_bandlikService);
            return data;
        }
        public async Task<BankCreditApplicationsResponse> GetApplicationsFromBankCredit(string inn, int offerSigned = 1)
        {
            var data = await _bankCreditService.GetApplications(inn, offerSigned);
            CombineStatuses(_bankCreditService);
            return data;
        }
        public async Task<List<BankCreditContractStatusResponse>> GetContractStatusFromBankCredit()
        {
            var data = await _bankCreditService.GetContractStatus();
            CombineStatuses(_bankCreditService);
            return data;
        }
        public async Task<List<InvestmentContract>> GetInvestmentContracts(InvestitsiyaRequestDto dto)
        {
            var data = await _investitsiyaService.GetInvestmentContracts(dto);
            CombineStatuses(_investitsiyaService);
            return data;
        }
        public async Task<Stream> SaveAsExecelFromBojxona(GetGTDByInnRequestDto dto)
        {
            var Fulldata = await GetGTDFromBojxona(dto);
            var data = Fulldata.Imports;
            data.AddRange(Fulldata.Exports);

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRINT_FROM_BOJXONA_LIST));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                var ws = importRow.Worksheet;
                int currentRow = importRow.Start.Row + 1;
                int index = 1;
                foreach (var item in data)
                {
                    foreach (var good in item.Goods)
                    {
                        var column = 1;
                        ws.InsertRow(currentRow, 1, importRow.Start.Row);
                        ws.Cells[currentRow, column++].Value = index++;
                        ws.Cells[currentRow, column++].Value = item.Mode;
                        ws.Cells[currentRow, column++].Value = item.Organizationtin;
                        ws.Cells[currentRow, column++].Value = item.Organization1name;
                        ws.Cells[currentRow, column++].Value = item.Organization1adress;
                        ws.Cells[currentRow, column++].Value = item.Organization2name;
                        ws.Cells[currentRow, column++].Value = item.Organization2adress;
                        ws.Cells[currentRow, column++].Value = item.EkimcountryFullName;
                        ws.Cells[currentRow, column++].Value = good.ProductName;
                        ws.Cells[currentRow, column++].Value = good.CodeTiftnGoods;
                        ws.Cells[currentRow, column++].Value = good.UnitGoods;
                        ws.Cells[currentRow, column++].Value = good.AdditionalUnitgoods;
                        ws.Cells[currentRow, column++].Value = good.NetMassGoods;
                        ws.Cells[currentRow, column++].Value = good.ValueGoods;
                        ws.Cells[currentRow, column++].Value = item.Typetransport;
                        ws.Cells[currentRow, column++].Value = item.Typeincoterms;
                        ws.Cells[currentRow, column++].Value = good.NumContract;
                        ws.Cells[currentRow, column++].Value = item.Year;
                        //ws.Cells[currentRow, column++].Value = good.NumberGoods;
                        currentRow++;
                    }
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }
        public void AddContactInfo(long ownerId, int contactTypeId, string contact)
        {
            var canCommit = UnitOfWork.CurrentTransaction == null;
            using var transaction = canCommit ? UnitOfWork.BeginTransaction() : null;

            try
            {
                var entity = Repository.AddContactInfo(ownerId, contactTypeId, contact, ent =>
                {
                    var contractor = Repository.ById(ownerId);
                    if (contractor.Contacts.Any(a => a.ContactTypeId == contactTypeId && a.Contact == contact))
                        AddError($"Такая {0} электронная почта уже зарегистрирована");
                });
                CombineStatuses(Repository);

                if (IsValid)
                {
                    UnitOfWork.Save();
                    if (canCommit)
                        transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                if (canCommit)
                    transaction.Rollback();
            }
        }
        public void ImportNotBudgetContractorInn(List<ImportNotBudgetNotBudgetContractorInnDlDto> listDto)
        {
            var existsContractors = _notBudgetContractorRepository.AllAsQueryable
                .Where(a => listDto.Select(a => a.ContractorInn).Contains(a.Inn))
                .Select(a => a.Inn)
                .ToList();

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                foreach (var contractorDto in listDto)
                {
                    if (existsContractors.Contains(contractorDto.ContractorInn))
                        continue;

                    _notBudgetContractorRepository.Create(new CreateNotBudgetContractorDlDto
                    {
                        Name = contractorDto.Contractor,
                        Inn = contractorDto.ContractorInn
                    });
                    CombineStatuses(_notBudgetContractorRepository);
                    if (HasErrors)
                        return;

                    existsContractors.Add(contractorDto.ContractorInn);
                }

                if (IsValid)
                {
                    _unitOfWork.Save();
                    transaction.Commit();
                }
            }
        }
        public void CreateContractorOffers(ContractorOfferDto dto)
        {
            try
            {
                var contractor = _unitOfWork.Context.Set<Contractor>()
                    .FirstOrDefault(c => c.Id == dto.ContractorId && c.StateId != StateIdConst.PASSIVE);

                if (contractor == null)
                {
                    AddError($"{dto.ContractorId} ID ли Поудратчи(Contractor) мавжуд емас");
                    return;
                };

                var unique = _unitOfWork.Context.Set<ContractorOffers>()
                    .Any(a => a.OfferId == dto.OfferId && a.ContractorId == dto.ContractorId);

                if (contractor.IsLastOffer && unique)
                {
                    AddError("Бу пудратчилар(contractor) аллақачон таклиф(offer) мавжуд");
                    return;
                }

                if (!unique)
                {
                    var entity = Repository
                        .CreateContractorOffers(dto, new ContractorOffers());
                }

                if (!contractor.IsLastOffer)
                    contractor.IsLastOffer = true;

                _unitOfWork.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message}\n\n{ex.InnerException.Message}");
                return;
            }
        }
		public async Task<ContractorDto> SearchByInnPnfl(string innpnfl)
		{
			//var res = UnitOfWork.Context.Set<ContractorDto>().FirstOrDefault(x => x.Inn == innpnfl || x.Pinfl == innpnfl);
            var contractor =  Repository.ReadAsNoTracked<ContractorDto>()
                .FirstOrDefault(x => x.Inn == innpnfl || x.Pinfl == innpnfl);

            if (contractor == null)
            {
                if(innpnfl.Length > 10)
                    contractor = await GetByPinflFromSoliq(innpnfl);
                else 
                    contractor = await GetByInnFromSoliq(innpnfl);
                if (contractor == null)
                {
                    AddError("Soliqda ma'lumot topilmadi");
                    return null;
                }
                else
                {
					var mc = new AutoMapper.MapperConfiguration(cfg =>
					{
						cfg.CreateMap<ContractorDto, CreateContractorDlDto>();
					});
					var createContractorDlDto = mc.CreateMapper().Map<CreateContractorDlDto>(contractor);

					var contractorEntity = Create(createContractorDlDto);

					CombineStatuses(this);
					if (HasErrors)
						return null;
					_unitOfWork.Save();
					var ent = Repository.ReadAsNoTracked<ContractorDto>()
				        .FirstOrDefault(x => x.Id == contractorEntity.Id);
					return ent;
				}
            }

            return contractor;
        }
        public void UpdateSettlementAccount(UpdateContractorSettlementAccountDlDto dto)
        {
            Repository.UpdateContractorSettlementAccount(dto);
            CombineStatuses(Repository);
            if (HasErrors) return;

            UnitOfWork.Context.SaveChanges();
        }
        public void ChangeBasicSettlementAccounting(long contractorId, long settlementAccountId)
        {
            Repository.ChangeMainSettlementAccounting(contractorId, settlementAccountId);
            CombineStatuses(Repository);
            if (HasErrors) return;

            UnitOfWork.Context.SaveChanges();
        }
        //public async Task<PagedResult<GetPayDocsDto>> GetPayDocsFilter(GetPayDocsSortFilterOptions options)
        //{
        //    var data = await _financeService.GetPayDocsAsync();
            
        //    return data.AsQueryable().SortFilter(options).AsPagedResult(options);
        //}

        #region Edoc Contractor
        private void SyncEdocContractor()
        {
            var organizations = _unitOfWork.Context.Set<Contractor>()
                                    .Include(x => x.Oked).ToList();

            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                foreach (var item in organizations)
                {
                    CreateContractorForEdocSchema(item);
                }
                if (HasErrors)
                    return;

                transaction.Commit();
            }
            catch (Exception e)
            {
                AddError(e.Message);
                _unitOfWork.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }
        private void CreateContractorForEdocSchema(Contractor dto)
        {
            try
            {
                var exsist = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Contractor>()
                                        .Any(x => x.Id == dto.Id);
                if (!exsist)
                {
                    var contractor = new Ssp.DataLayer.EFClasses.Edoc.Contractor()
                    {
                        Id = (int)dto.Id,
                        RegionId = dto.RegionId,
                        Accounter = !string.IsNullOrEmpty(dto.Accounter) ? dto.Accounter : "ACCOUNTER",
                        Address = dto.Address,
                        Director = dto.Director,
                        ContactInfo = dto.Contact,
                        DateOfCreated = DateTime.Now,
                        DistrictId = dto.DistrictId,
                        FullName = dto.FullName,
                        ShortName = dto.ShortName,
                        Inn = dto.Inn,
                        CreatedUserId = _authService.User.Id,
                        Email = "SSP@gmail.com",
                        Oked = dto.Oked.Code,
                        MobileNumber = dto.PhoneNumber,
                        VatCode = !string.IsNullOrEmpty(dto.VatCode) ? dto.VatCode : "000",
                    };

                    foreach (var i in dto.SettlementAccounts)
                    {
                        contractor.Accounts.Add(new Ssp.DataLayer.EFClasses.Edoc.ContractorAccount()
                        {
                            AccountName = i.AccountName,
                            BankId = i.BankId,
                            Code = i.AccountCode,
                            CreatedUserId = _authService.User.Id,
                            DateOfCreated = DateTime.Now,
                            StateId = i.StateId,
                        });
                    }
                    _unitOfWork.Context.Add(contractor);
                    _unitOfWork.Save();
                }
                else
                {
                    UpdateContractorForEdocSchema(dto);
                }

            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }
        }
        private void UpdateContractorForEdocSchema(Contractor dto)
        {
            try
            {
                var contractor = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Contractor>()
                                        .Include(x => x.Accounter)
                                        .FirstOrDefault(x => x.Id == dto.Id);
                if (contractor is not null)
                {
                    contractor.RegionId = dto.RegionId;
                    contractor.Accounter = !string.IsNullOrEmpty(dto.Accounter) ? dto.Accounter : "ACCOUNTER";
                    contractor.Address = dto.Address;
                    contractor.Director = dto.Director;
                    contractor.DistrictId = dto.DistrictId;
                    contractor.FullName = dto.FullName;
                    contractor.ShortName = dto.ShortName;
                    contractor.Inn = dto.Inn;
                    contractor.Oked = dto.Oked.Code;
                    contractor.ContactInfo = dto.Contact;
                    contractor.MobileNumber = dto.PhoneNumber;
                    contractor.VatCode = !string.IsNullOrEmpty(dto.VatCode) ? dto.VatCode : "000";
                    _unitOfWork.Context.Update(contractor);
                    _unitOfWork.Save();
                }

                foreach (var i in dto.SettlementAccounts)
                {
                    if (contractor.Accounts.Any(x => x.Code == i.AccountCode && x.BankId == i.BankId))
                    {
                        var account = contractor.Accounts.First(x => x.Code == i.AccountCode && x.BankId == i.BankId);
                        account.AccountName = i.AccountName;
                        account.StateId = i.StateId;
                        account.DateOfModified = DateTime.Now;
                        account.ModifiedUserId = _authService.User.Id;
                    }
                    else
                        contractor.Accounts.Add(new Ssp.DataLayer.EFClasses.Edoc.ContractorAccount()
                        {
                            AccountName = i.AccountName,
                            BankId = i.BankId,
                            Code = i.AccountCode,
                            CreatedUserId = _authService.User.Id,
                            DateOfCreated = DateTime.Now,
                            StateId = i.StateId,
                        });
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
            }
        }

        
        #endregion
    }
}
