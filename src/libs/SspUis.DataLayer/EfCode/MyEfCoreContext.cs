using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.My;
using SspUis.DataLayer.EfClasses.Report;
using SspUis.DataLayer.EfClasses.Report.Func;
using System;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode
{
    public partial class EfCoreContext : BaseDbContext
    {
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationTypeStep> ApplicationTypeSteps { get; set; }
        public virtual DbSet<ApplicationTypeStepTranslate> ApplicationTypeStepTranslates { get; set; }
        public virtual DbSet<Contractor> Contractors { get; set; }
        public virtual DbSet<MonoApplication> MonoApplications { get; set; }
        public virtual DbSet<MonoApplicationFile> Files { get; set; }
        public virtual DbSet<MonoApplicationItemTable> ItemTables { get; set; }
        public virtual DbSet<MonoApplicationStudentTable> StudentTables { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<BusinessmanUser> BusinessmanUsers { get; set; }
        public virtual DbSet<BusinessmanUserLog> BusinessmanUserLogs { get; set; }
        public virtual DbSet<BusinessmanUserDeviceLog> BusinessmanUserDeviceLogs { get; set; }
        public virtual DbSet<BusinessmanUserTrustedDevice> BusinessmanUserTrustedDevices { get; set; }
        public virtual DbSet<BusinessmanUserInContractor> BusinessmanUserInContractors { get; set; }
        public virtual DbSet<BusinessmanUserContractorLog> BusinessmanUserContractorLog { get; set; }
        public virtual DbSet<SmsCode> SmsCodes { get; set; }
        public virtual DbSet<NotBudgetContractor> NotBudgetContractors { get; set; }
        public virtual DbSet<MonoApplicationBandlikResult> MonoApplicationBandlikResults { get; set; }

        #region Functions

        [DbFunction("clear_contractor_info", Schema = "my")]
        public IQueryable<ClearContractorInfoDto> ClearContractorInfo(int contractorId, bool isOnlyDocuments) =>
            FromExpression(() => ClearContractorInfo(contractorId, isOnlyDocuments));

        [DbFunction("get_soliq_report_by_contractor", Schema = "exapidata")]
        public IQueryable<EfClasses.Exapidata.GetSoliqReportByContractorDto>
           GetSoliqReportByContractor(
               long? pr_id,
               string pr_inn,
               int? pr_year,
               int? pr_region_id,
               int? pr_district_id,
               bool pr_by_region,
               bool pr_by_district,
               bool pr_by_contractor,
               bool pr_has_certificate,
               int? pr_language_id
            ) =>
           FromExpression(() => GetSoliqReportByContractor(pr_id, pr_inn, pr_year, pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_by_contractor, pr_has_certificate, pr_language_id));

        [DbFunction("get_tax_report_by_contractor", Schema = "partner")]
        public IQueryable<EfClasses.Exapidata.GetTaxReportByContractorForPartnerDto>
          GetTaxReportByContractor(
              long? pr_id,
              string pr_inn,
              int? pr_start_year,
              int? pr_end_year,
              int? pr_start_month,
              int? pr_end_month,
              int? pr_region_id,
              int? pr_district_id,
              bool pr_by_region,
              bool pr_by_district,
              bool pr_by_contractor,
              bool pr_has_certificate,
              int? pr_language_id
           ) =>
          FromExpression(() => GetTaxReportByContractor(pr_id, pr_inn, pr_start_year, pr_end_year, pr_start_month, pr_end_month, pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_by_contractor, pr_has_certificate, pr_language_id));

        [DbFunction("get_report_by_type", Schema = "appeal")]
        public IQueryable<AppealReportDto>
            GetReportByAppealType(
            int[] array,
            int? pr_region_id,
            int? pr_district_id,
            int? pr_language_id
            ) =>
            FromExpression(() => GetReportByAppealType(array, pr_region_id, pr_district_id, pr_language_id));

        [DbFunction("call_center_appeal_report_by_oked_type", Schema = "appeal")]
        public IQueryable<CallCenterAppealReportByOkedTypeDto>
            CallCenterAppealReportByOkedType(
            bool pr_by_oked_type_id,
            DateOnly? pr_from_doc_date,
            DateOnly? pr_to_doc_date,
            int? pr_language_id
            ) =>
            FromExpression(() => CallCenterAppealReportByOkedType(pr_by_oked_type_id, pr_from_doc_date, pr_to_doc_date, pr_language_id));

        [DbFunction("get_memship_docs_info", Schema = "memship")]
        public IQueryable<EfClasses.Memship.MemshipDocsInfoDto>
           GetMemshipDocsInfo(
               int? pr_region_id,
               int? pr_memship_contract_type_id,
               int? pr_contractor_category_id,
               DateOnly? pr_from_doc_date,
               DateOnly? pr_to_doc_date,
               bool? pr_is_old,
               bool? pr_is_pinfl,
               int? pr_language_id
            ) =>
           FromExpression(() => GetMemshipDocsInfo(
               pr_region_id,
               pr_memship_contract_type_id,
               pr_contractor_category_id,
               pr_from_doc_date,
               pr_to_doc_date,
               pr_is_old,
               pr_is_pinfl,
               pr_language_id));

        [DbFunction("get_memship_docs_info_reestr", Schema = "memship")]
        public IQueryable<EfClasses.Memship.MemshipDocsInfoReestrModelDto>
           GetMemshipDocsInfoReestr(
               int? pr_region_id,
               int? pr_district_id,
               int? pr_memship_contract_type_id,
               int? pr_contractor_category_id,
               DateOnly? pr_from_doc_date,
               DateOnly? pr_to_doc_date,
               bool? pr_is_old,
               bool? pr_is_pinfl,
               int? pr_language_id,
               string? pr_search,
               int? pr_offset,
               int? pr_limit
            ) =>
           FromExpression(() => GetMemshipDocsInfoReestr(
               pr_region_id,
               pr_district_id,
               pr_memship_contract_type_id,
               pr_contractor_category_id,
               pr_from_doc_date,
               pr_to_doc_date,
               pr_is_old,
               pr_is_pinfl,
               pr_language_id,
               pr_search,
               pr_offset,
               pr_limit));

        [DbFunction("get_memship_docs_info_reestr_count", Schema = "memship")]
        public IQueryable<EfClasses.Memship.CountResult> GetMemshipDocsInfoReestrCount(
            int? pr_region_id,
            int? pr_district_id,
            int? pr_memship_contract_type_id,
            int? pr_contractor_category_id,
            DateOnly? pr_from_doc_date,
            DateOnly? pr_to_doc_date,
            bool? pr_is_old,
            bool? pr_is_pinfl,
            int? pr_language_id,
            string? pr_search
            ) =>
            FromExpression(() => GetMemshipDocsInfoReestrCount(
                pr_region_id,
                pr_district_id,
                pr_memship_contract_type_id,
                pr_contractor_category_id,
                pr_from_doc_date,
                pr_to_doc_date,
                pr_is_old,
                pr_is_pinfl,
                pr_language_id,
                pr_search
            ));


        [DbFunction("get_bojxona_imtiyoz_report_by_contractor", Schema = "exapidata")]
        public IQueryable<BojxonaImtiyozReportByContractorDto>
          BojxonaImtiyozReportByContractor(
               long? pr_contractor_id,
               string pr_contractor_inn,
               int? pr_region_id,
               int? pr_district_id,
               bool? pr_by_region,
               bool? pr_by_district,
               bool? pr_by_contractor,
               int? pr_year,
               int? pr_contract_type_id,
               bool? pr_by_contract_type,
               bool? pr_has_certificate,
               int? pr_language_id
           ) =>
          FromExpression(() => BojxonaImtiyozReportByContractor(pr_contractor_id, pr_contractor_inn, pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_by_contractor, pr_year, pr_contract_type_id, pr_by_contract_type, pr_has_certificate, pr_language_id));

        [DbFunction("get_credit_demand_report_by_bank", Schema = "partner")]
        public IQueryable<EfClasses.Report.PrtnCreditDemandInfoByBankDto> GetPrtnCreditDemandInfoByBank(
               long? pr_contractor_id,
               string pr_contractor_inn,
               int? pr_region_id,
               int? pr_district_id,
               bool pr_by_region,
               bool pr_by_district,
               bool pr_by_contractor,
               int? pr_bank_id,
               bool pr_by_bank,
               int? pr_main_bank_id,
               bool pr_by_main_bank,
               int? pr_contract_type_id,
               bool pr_by_contract_type,
               int? pr_language_id
            ) =>
           FromExpression(() => GetPrtnCreditDemandInfoByBank(pr_contractor_id, pr_contractor_inn, pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_by_contractor, pr_bank_id, pr_by_bank, pr_main_bank_id, pr_by_main_bank, pr_contract_type_id, pr_by_contract_type, pr_language_id));

        [DbFunction("get_tadbirkor_fund_report", Schema = "partner")]
        public IQueryable<EfClasses.Report.TadbirkorFundReportDto> GetTadbirkorFundReport(
               long? pr_contractor_id,
               string pr_contractor_inn,
               int? pr_region_id,
               int? pr_district_id,
               bool pr_by_region,
               bool pr_by_district,
               bool pr_by_contractor,
               int? pr_bank_id,
               bool pr_by_bank,
               int? pr_contract_type_id,
               bool pr_by_contract_type,
               int? pr_language_id
            ) =>
           FromExpression(() => GetTadbirkorFundReport(pr_contractor_id, pr_contractor_inn, pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_by_contractor, pr_bank_id, pr_by_bank, pr_contract_type_id, pr_by_contract_type, pr_language_id));
        [DbFunction("get_report_prtn_certificate_with_contractor", Schema = "partner")]
        public IQueryable<ReportPrtnCertificateWithContractorDto> ReportOnProjectImplementationAndBenefitsGranted(
              int? pr_region_id,
               int? pr_language_id,
              bool by_contract,
              int? pr_district_id
           ) =>
          FromExpression(() => ReportOnProjectImplementationAndBenefitsGranted(pr_region_id, pr_language_id, by_contract, pr_district_id));

        [DbFunction("get_report_prtn_certificate_with_graph", Schema = "partner")]
        public IQueryable<ReportPrtnCertificateWithContractorDto> GetReportPrtnCertificateWithGraph(
              int? pr_region_id,
              int? pr_start_year,
             int? pr_start_month,
             int? pr_end_month,
             int? pr_end_year,
             int? pr_language_id,
              bool by_contract,
              int? pr_district_id
           ) =>
          FromExpression(() => GetReportPrtnCertificateWithGraph(pr_region_id, pr_start_year,
            pr_start_month,
            pr_end_month,
            pr_end_year,
            pr_language_id, by_contract, pr_district_id));

        [DbFunction("get_report_doc_execution_application_contractor", Schema = "partner")]
        public IQueryable<ReportPrtnExecutionApplicationContractorDto> GetReportExecutionApplicationContractor(
            int? pr_region_id,
            int? pr_start_year,
            int? pr_start_month,
            int? pr_end_month,
            int? pr_end_year,
            int? pr_language_id,
            bool by_contract,
            int? pr_district_id
       ) =>
        FromExpression(() => GetReportExecutionApplicationContractor(pr_region_id, pr_start_year,
            pr_start_month,
            pr_end_month,
            pr_end_year,
            pr_language_id, by_contract, pr_district_id));



        [DbFunction("get_tax_qqs_aylanma", Schema = "exapidata")]
        public IQueryable<GetTaxQqsAylanmaDto> GetTaxQqsAylanmaReport(
         long? pr_contractor_id,
         string pr_contractor_inn,
         int? pr_region_id,
         int? pr_district_id,
         bool pr_by_region,
         bool pr_by_district,
         bool pr_by_contractor,
         int? pr_year,
         int? pr_month,
         int? pr_language_id
      ) =>
     FromExpression(() => GetTaxQqsAylanmaReport(pr_contractor_id, pr_contractor_inn, pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_by_contractor, pr_year, pr_month, pr_language_id));

        [DbFunction("get_tax", Schema = "partner")]
        public IQueryable<TaxCreditreportDto> GetTaxCreditReport(
               long? pr_contractor_id,
               string pr_contractor_inn,
               int? pr_region_id,
               int? pr_district_id,
               bool? pr_by_region,
               bool? pr_by_district,
               bool? pr_by_contractor,
               int? pr_year,
               int? pr_contract_type_id,
               bool? pr_by_contract_type,
               bool? pr_has_certificate,
               int? pr_language_id
            ) =>
            FromExpression(() => GetTaxCreditReport(pr_contractor_id, pr_contractor_inn, pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_by_contractor, pr_year, pr_contract_type_id, pr_by_contract_type, pr_has_certificate, pr_language_id));



        [DbFunction("get_region_or_district", Schema = "srv")]
        public IQueryable<FreeDeedReportDto> GetRegionOrDistrict(
            DateOnly? pr_start_date,
            DateOnly? pr_end_date,
            int? pr_region_id,
            int? pr_district_id,
            int? pr_language_id

       ) =>
       FromExpression(() => GetRegionOrDistrict(pr_start_date, pr_end_date, pr_region_id, pr_district_id, pr_language_id));


        [DbFunction("get_bank_credit_report", Schema = "exapidata")]
        public IQueryable<BankCreditReportFuncDto> GetBankCreditReport(
            string pr_year,
            int? pr_region_id,
            int? pr_district_id,
            string pr_inn,
            string pr_bank_mfo
            ) =>
        FromExpression(() => GetBankCreditReport(pr_year, pr_region_id, pr_district_id, pr_inn, pr_bank_mfo));
        
        [DbFunction("get_bank_credit_application_report_by_region_and_district", Schema = "exapidata")]
        public IQueryable<BankCreditApplicationReportByRegionAndDistrictDto> GetBankCreditApplicationReportByRegionAndDistrict(
          long? pr_contractor_id,
          string pr_contractor_inn,
          int? pr_region_id,
          int? pr_district_id,
          bool? pr_by_region,
          bool? pr_by_district,
          bool? pr_by_contractor,
          int? pr_contract_type_id,
          bool? pr_by_contract_type,
          bool? pr_has_certificate,
          int? pr_language_id
       ) =>
       FromExpression(() => GetBankCreditApplicationReportByRegionAndDistrict(pr_contractor_id, pr_contractor_inn, pr_region_id, pr_district_id, pr_by_region, pr_by_district, pr_by_contractor, pr_contract_type_id, pr_by_contract_type, pr_has_certificate, pr_language_id));

        [DbFunction("get_employee_turnstile_report", Schema = "hrm")]
        public IQueryable<EmployeeTurnstileReportDto>
            GetEmployeeTurnstileReport(
                DateTime in_start_date,
                DateTime in_end_date,
                int in_org_id,
                int in_language_id,
                TimeSpan? in_start_work_time,
                TimeSpan? in_end_work_time,
                TimeSpan? in_start_lunch_time,
                TimeSpan? in_end_lunch_time,
                string in_employee_fullname,
                TimeSpan in_office_in_time,
                TimeSpan in_office_out_time,
                TimeSpan in_calc_night_hour,
                bool in_is_late,
                bool in_is_left_early,
                bool in_is_break_lunch_time
             ) =>
         FromExpression(() => GetEmployeeTurnstileReport(in_start_date, in_end_date, in_org_id, in_language_id, in_start_work_time, in_end_work_time, in_start_lunch_time, in_end_lunch_time, in_employee_fullname, in_office_in_time, in_office_out_time, in_calc_night_hour, in_is_late, in_is_left_early, in_is_break_lunch_time));

        [DbFunction("get_employee_turnstile_report_by_id", Schema = "hrm")]
        public IQueryable<EmployeeTurnstileReportByIdDto>
               GetEmployeeTurnstileReportById(
                   DateTime in_start_date,
                   DateTime in_end_date,
                   int in_org_id,
                   int in_language_id,
                   int? in_employee_id,
                   TimeSpan? in_start_work_time,
                   TimeSpan? in_end_work_time,
                   TimeSpan in_office_in_time,
                   TimeSpan in_office_out_time,
                   TimeSpan in_calc_night_hour
                ) =>
            FromExpression(() => GetEmployeeTurnstileReportById(in_start_date, in_end_date, in_org_id, in_language_id, in_employee_id, in_start_work_time, in_end_work_time, in_office_in_time, in_office_out_time, in_calc_night_hour));
        [DbFunction("get_region_or_district", Schema = "public")]
        public IQueryable<RegionOrDistrict> RegionOrDistrict(
            int? pr_region_id,
            int? pr_language_id,
            int? pr_district_id
             ) =>
             FromExpression(() => RegionOrDistrict(pr_region_id,
                                          pr_language_id, pr_district_id));
        #endregion

        private void MyOnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(ClearContractorInfo),
                    new[] {
                        typeof(int),
                        typeof(bool)
                    }));
            modelBuilder
                .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetSoliqReportByContractor),
                    new[] {
                        typeof(long?),
                        typeof(string),
                        typeof(int?),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool),
                        typeof(bool),
                        typeof(bool),
                        typeof(bool),
                        typeof(int?)
                    }));
            modelBuilder
                .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetTaxReportByContractor),
                    new[] {
                        typeof(long?),
                        typeof(string),
                        typeof(int?),
                        typeof(int?),
                        typeof(int?),
                        typeof(int?),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool),
                        typeof(bool),
                        typeof(bool),
                        typeof(bool),
                        typeof(int?)
                    }));
            modelBuilder
                .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(BojxonaImtiyozReportByContractor),
                    new[] {
                        typeof(long?),
                        typeof(string),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(int?)
                    }));
            modelBuilder
               .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetPrtnCreditDemandInfoByBank),
                   new[] {
                        typeof(long?),
                        typeof(string),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool),
                        typeof(bool),
                        typeof(bool),
                        typeof(int?),
                        typeof(bool),
                        typeof(int?),
                        typeof(bool),
                        typeof(int?),
                        typeof(bool),
                        typeof(int?)
                   }));
            modelBuilder
              .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetTadbirkorFundReport),
                  new[] {
                        typeof(long?),
                        typeof(string),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool),
                        typeof(bool),
                        typeof(bool),
                        typeof(int?),
                        typeof(bool),
                        typeof(int?),
                        typeof(bool),
                        typeof(int?)
                  }));
            modelBuilder
             .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetTaxCreditReport),
                 new[] {
                        typeof(long?),
                        typeof(string),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(int?)
                 }));
            modelBuilder
           .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetRegionOrDistrict),
               new[] {
                        typeof(DateOnly?),
                        typeof(DateOnly?),
                        typeof(int?),
                        typeof(int?),
                        typeof(int?)
               }));
            modelBuilder
                .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetBankCreditReport),
                new[] {
                    typeof(string),
                    typeof(int?),
                    typeof(int?),
                    typeof(string),
                    typeof(string)
                }));
            modelBuilder
             .HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetBankCreditApplicationReportByRegionAndDistrict),
                 new[] {
                        typeof(long?),
                        typeof(string),
                        typeof(int?),
                        typeof(int?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(int?),
                        typeof(bool?),
                        typeof(bool?),
                        typeof(int?)
                 }));

            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(CallCenterAppealReportByOkedType),
                new[]
                {
                    typeof(bool),
                    typeof(DateOnly?),
                    typeof(DateOnly?),
                    typeof(int?),
                }));
            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetMemshipDocsInfoReestrCount)));
            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetReportByAppealType),
                new[] {
                    typeof(int[]),
                    typeof(int),
                    typeof(int),
                    typeof(int?)
                }));
            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetEmployeeTurnstileReport),
                new[] {
                    typeof(DateTime),
                    typeof(DateTime),
                    typeof(int),
                    typeof(int),
                    typeof(TimeSpan?),
                    typeof(TimeSpan?),
                     typeof(TimeSpan?),
                    typeof(TimeSpan?),
                    typeof(string),
                    typeof(TimeSpan),
                    typeof(TimeSpan),
                    typeof(TimeSpan),
                    typeof(bool),
                    typeof(bool),
                    typeof(bool)
                }));
            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(ReportOnProjectImplementationAndBenefitsGranted),
               new[] {
                    typeof(int?),
                    typeof(int?),
                    typeof(bool),
                    typeof(int?)
               }));
            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetReportPrtnCertificateWithGraph),
              new[] {
                    typeof(int?),
                    typeof(int?),
                    typeof(int?),
                    typeof(int?),
                    typeof(int?),
                    typeof(int?),
                    typeof(bool),
                    typeof(int?)
              }));
            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetReportExecutionApplicationContractor),
             new[] {
                    typeof(int?),
                    typeof(int?),
                    typeof(int?),
                    typeof(int?),
                    typeof(int?),
                    typeof(int?),
                    typeof(bool),
                    typeof(int?)
             }));

            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(GetEmployeeTurnstileReportById),
                    new[] {
                        typeof(DateTime),
                        typeof(DateTime),
                        typeof(int),
                        typeof(int),
                        typeof(int?),
                        typeof(TimeSpan?),
                        typeof(TimeSpan?),
                        typeof(TimeSpan),
                        typeof(TimeSpan),
                        typeof(TimeSpan)
                    }));
            modelBuilder.HasDbFunction(typeof(EfCoreContext).GetMethod(nameof(RegionOrDistrict),
                    new[] {
                        typeof(int?),
                        typeof(int?),
                        typeof(int?)
                    }));
        }
    }
}
