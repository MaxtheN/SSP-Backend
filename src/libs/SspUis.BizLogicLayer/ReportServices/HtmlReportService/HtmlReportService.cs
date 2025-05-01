using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.OrganizationServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using Application = SspUis.DataLayer.EfClasses.Application;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class HtmlReportService :
        StatusGenericHandler,
        IHtmlReportService
    {
        private readonly IBaseReportService _baseReportService;
        private readonly IApplicationService _applicationService;
        private readonly DbContext _context;
        private readonly IOrganizationService _organizationService;
        private readonly SystemConf _systemConf;
        private string CertificateCancellingHtmlComponent = "<div style=\"margin-left: 240px; margin-bottom: 0; margin-top: 20px; display: flex; color:red; \" ><span><em>Bekor qilingan sana: </em></span> <div style=\" width: 150px; margin-bottom: 0; border-bottom: 1px dashed black; text-align: center; \"> ${Message} </div> </div>";

        public HtmlReportService(
            IBaseReportService baseReportService,
            IApplicationService applicationService,
            DbContext context,
            IOrganizationService organizationService,
            SystemConf systemConf)
        {
            _baseReportService = baseReportService;
            _applicationService = applicationService;
            _context = context;
            _organizationService = organizationService;
            _systemConf = systemConf;
        }

        public string DownloadApplicationAsHtml(long id, Guid? id2 = null, string lang = null)
        {
            //var application = _applicationService.GetPrtnApplication(Id);

            var application = _context.Set<Application>()
                .Include(application => application.PrtnContract)
                .Include(application => application.PrtnApplication)
                    .ThenInclude(prtnapp => prtnapp.PrtnContractType)
                .Include(application => application.Region)
                    .ThenInclude(region => region.Translates)
                .Select(a => new
                {
                    a.Id,
                    a.Id2,
                    a.RegionId,
                    a.DistrictId,
                    a.PrtnApplication.PrtnContractTypeId,
                    a.DocNumber,
                    a.DocOn,
                    a.PrtnApplication.ChooseLocation,
                    a.PrtnApplication.ChoosedRegionId,
                    a.PrtnApplication.ChoosedDistrictId,
                    RegionName = a.PrtnApplication.ChooseLocation == true ? a.PrtnApplication.ChoosedRegion.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, LanguageIdConst.UZ_CYRL)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName : a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, LanguageIdConst.UZ_CYRL)).TranslateText ?? a.Region.FullName,
                    DistrictName = a.PrtnApplication.ChooseLocation == true ? a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, LanguageIdConst.UZ_CYRL)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName : a.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, LanguageIdConst.UZ_CYRL)).TranslateText ?? a.District.FullName,
                    MfyName = a.PrtnApplication.ChooseLocation == true ? a.PrtnApplication.MfyName : a.PrtnApplication.Mfy.FullName,
                    Contractor = a.Contractor.ShortName,
                    ContractorInn = a.Contractor.Inn,
                    ContractorDirector = a.Contractor.Director,
                    PrtnContractTypeFrom = a.PrtnApplication.PrtnContractType.EmployeeRangeFrom.ToString(),
                    PrtnContractTypeTo = a.PrtnApplication.PrtnContractType.EmployeeRangeTo.ToString(),
                }).FirstOrDefault(application => id == 0 && id2 == application.Id2 || id == application.Id);

            CombineStatuses(_applicationService);

            if (HasErrors)
                return null;

            string fileName = "application.html";
            string organizationNameByLocation = "";

            if (application.ChooseLocation)
                organizationNameByLocation
                     = _organizationService.GetOrganizationNameByLocation(
                         application.ChoosedRegionId.Value,
                         application.ChoosedDistrictId.Value,
                         application.PrtnContractTypeId);
            else
                organizationNameByLocation
                    = _organizationService.GetOrganizationNameByLocation(
                        application.RegionId,
                        application.DistrictId,
                        application.PrtnContractTypeId);

            Dictionary<string, string> data
                = new Dictionary<string, string>
                {
                    { "${DocNumber}", application.DocNumber },
                    { "${DocDate}", application.DocOn.ToString(Constants.DATE_FORMAT) },
                    { "${RegionName}", application.RegionName},
                    { "${OrganizationNameByLocation}", organizationNameByLocation },
                    { "${DistrictName}", application.DistrictName},
                    { "${MfyName}", application.MfyName},
                    { "${Contractor}", application.Contractor },
                    { "${ContractorInn}", application.ContractorInn },
                    { "${ContractorDirectorName}", application.ContractorDirector },
                    { "${PrtnContractTypeFrom}", application.PrtnContractTypeFrom },
                    { "${PrtnContractTypeTo}", application.PrtnContractTypeTo },
                    { "${QrImage}", WEBASE.QRCode.QRCodeHelper.GenerateImageAsBase64($"{_systemConf.QrImagePrintPath}/api/Application/PrintApplicationPdf?Id={id2}&lang={lang}") }
                };

            //if (lang == "uz-cyrl")
            //{
            if (application.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                data.Add("${PrtnContractTypeInverval}", $"{application.PrtnContractTypeFrom} тадан ортиқ");
            else
                data.Add("${PrtnContractTypeInverval}", $"{application.PrtnContractTypeFrom} тадан {application.PrtnContractTypeTo} тагача");
            //}else if(lang == "ru"){}else if(lang == "uz-latn")


            var fileString = _baseReportService.ReturnReadyHtmlAsString(filename: fileName,
                                                      data: data,
                                                      tableData: null, lang);

            return fileString;
        }

        public byte[] DownloadApplicationPdf(Guid id2, string lang)
            => _baseReportService.ReturnReadyPdf(DownloadApplicationAsHtml(0, id2, lang));

        public string DownloadCertificateAsHtml(Guid Id2, string lang = null)
        {
            var certificate = _context.Set<PrtnCertificate>()
                            .Include(certificate => certificate.PrtnContract)
                            .Include(certificate => certificate.Contractor)
                            .Include(certificate => certificate.PrtnContractType)
                            .Select(certificate => new
                            {
                                certificate.Id2,
                                certificate.DocNumber,
                                certificate.CancelOn,
                                certificate.Message,
                                certificate.StatusId,
                                DocOn = certificate.DocOn.ToString(Constants.DATE_FORMAT),
                                certificate.Organization.RegionId,
                                certificate.Organization.DistrictId,
                                certificate.PrtnContractTypeId,
                                ContractorName = certificate.Contractor.FullName,
                                ContractTypeName = certificate.PrtnContractType.FullName,
                                ContractTypeId = certificate.PrtnContractTypeId,
                                PrtnApplicationNewVacanciesCount = certificate.PrtnContract.NewVacanciesCount,
                                ContractorInn = certificate.Contractor.Inn,
                                ContractDate = certificate.PrtnContract.DocOn.ToString(Constants.DATE_FORMAT),
                                ContractNumber = certificate.PrtnContract.DocNumber,
                                ContractExpirationDate = certificate.ExpireOn.ToString(Constants.DATE_FORMAT)
                            }).FirstOrDefault(certificate => certificate.Id2 == Id2);


            if (certificate == null)
            {
                AddError("404 - Not found");
                return null;
            }

            var organizationNameByLocation
                = _organizationService.GetOrganizationNameByLocation(
                            certificate.RegionId,
                            certificate.DistrictId.Value,
                            certificate.PrtnContractTypeId);

            string fileName = certificate.ContractTypeId == PrtnContractTypeIdConst._201__ ? "certificate_ministry.html" : "certificate.html";

            Dictionary<string, string> data
                = new Dictionary<string, string>()
                {
                    { "${DocNumber}", certificate.DocNumber },
                    { "${DocDate}", certificate.DocOn},
                    { "${CancelOn}", certificate.StatusId==StatusIdConst.CANCELED && certificate.CancelOn.HasValue
                        ?CertificateCancellingHtmlComponent.Replace("${Message}", 
                            certificate.CancelOn.Value.ToString(Constants.DATE_FORMAT)
                            +" ("+ certificate.Message+")")
                        :""},
                    { "${OrganizationNameByLocation}", organizationNameByLocation },
                    { "${OrganizationNameByLocation2}", organizationNameByLocation },
                    { "${ContractorName}", certificate.ContractorName},
                    { "${ContractDate}", certificate.ContractDate },
                    { "${ContractNumber}", certificate.ContractNumber },
                    { "${ContractorInn}", certificate.ContractorInn },
                    { "${ContractExpirationDate}", certificate.ContractExpirationDate },
                    { "${ContractTypeName}", certificate.ContractTypeName },
                    { "${prtnApplicationNewVacanciesCount}", certificate.PrtnApplicationNewVacanciesCount.ToString() },
                    { "${QrImage}", WEBASE.QRCode.QRCodeHelper.GenerateImageAsBase64($"{_systemConf.QrImagePrintPath}/PrtnCertificate/PrintCertificatePdf?Id2={Id2}&lang={lang}") }
                };

            var fileString = _baseReportService.ReturnReadyHtmlAsString(filename: fileName,
                                                      data: data,
                                                      tableData: null,
                                                      lang);

            return fileString;
        }

        public byte[] DownloadCertificatePdf(Guid Id2, string lang = null)
            => _baseReportService.ReturnReadyPdf(DownloadCertificateAsHtml(Id2, lang));

        public string DownloadPrtnContractAsHtml(Guid Id2, string lang = null)
        {
            lang = "uz-cyrl";
            var prtnContract = _context.Set<PrtnContract>()
                .Include(contract => contract.Contractor)
                    .ThenInclude(contractor => contractor.Region)
                        .ThenInclude(region => region.Translates)
                .Include(contract => contract.Signs)
                    .ThenInclude(s => s.PrtnContractTypeTable)
                .Include(contract => contract.Signs)
                    .ThenInclude(s => s.OrganizationSign)
                .Include(contract => contract.Contractor)
                    .ThenInclude(contractor => contractor.District)
                        .ThenInclude(district => district.Translates)
                .Include(contract => contract.Contractor)
                    .ThenInclude(contractor => contractor.SettlementAccounts)
                        .ThenInclude(settAcc => settAcc.Bank)
                .Include(contract => contract.Organization)
                    .ThenInclude(organization => organization.SettlementAccounts)
                        .ThenInclude(settlAcc => settlAcc.Bank)
                .Include(contract => contract.Organization)
                    .ThenInclude(organization => organization.Signs)
                        .ThenInclude(settlAcc => settlAcc.PrtnContractTypeTable)
                .Include(contract => contract.Organization)
                    .ThenInclude(organization => organization.Region)
                        .ThenInclude(region => region.Translates)
                .Include(contract => contract.Organization)
                    .ThenInclude(organization => organization.District)
                        .ThenInclude(district => district.Translates)
                .Include(contract => contract.Organization)
                    .ThenInclude(organization => organization.Signs)
                //.ThenInclude(settlAcc => settlAcc.User)
                //    .ThenInclude(user => user.Person)
                .Select(
                    contract => new
                    {
                        contract.Id2,
                        contract.DocOn,
                        contract.DocNumber,
                        contract.StatusId,
                        contract.ApplicationId,
                        contract.PrtnContractTypeId,
                        ContractorRegionSoato = contract.Contractor.Region.Soato,
                        ContractorInn = contract.Contractor.Inn,
                        ContractorAddress = contract.Contractor.Address,
                        ContractorBankCode = contract.Contractor.Bank.Code,
                        ContractorBankName = contract.Contractor.Bank.BankName,
                        ContractorSettlementAccountCode = contract.Contractor.SettlementAccounts.OrderByDescending(a => a.IsMain).FirstOrDefault().AccountCode,
                        ContractorRegionName = contract.Contractor.Region.Translates.AsQueryable()
                                        .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,
                                        LanguageIdConst.UZ_CYRL)).TranslateText ??
                                       contract.Contractor.Region.FullName,
                        ContractorDistrictName = contract.Contractor.District.Translates.AsQueryable()
                                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name,
                                        LanguageIdConst.UZ_CYRL)).TranslateText ??
                                       contract.Contractor.District.FullName,
                        OrganizationRegionName = contract.Organization.Region.Translates.AsQueryable()
                                        .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,
                                        LanguageIdConst.UZ_CYRL)).TranslateText ??
                                       contract.Organization.Region.FullName,
                        OrganizationDistrictName = contract.Organization.District.Translates.AsQueryable()
                                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name,
                                        LanguageIdConst.UZ_CYRL)).TranslateText ??
                                       contract.Contractor.District.FullName,
                        ContractorName = contract.Contractor.FullName,
                        ContractorDirectorName = contract.Contractor.Director,
                        OrganizationAddress = contract.Organization.Address,
                        OrganizationInn = contract.Organization.Inn,
                        OrganizationSettltmentAccountCode = contract.Organization.SettlementAccounts.FirstOrDefault().AccountCode,
                        OrganizationBankCode = contract.Organization.SettlementAccounts.FirstOrDefault().Bank.Code,
                        OrganizationBankName = contract.Organization.SettlementAccounts.FirstOrDefault().Bank.BankName,
                        ContractPosition1 = contract.Signs.FirstOrDefault(a => a.PrtnContractTypeTable.OrderNumber == 3).OrganizationSign.ShortName ?? "",
                        ContractPosition2 = contract.Signs.FirstOrDefault(a => a.PrtnContractTypeTable.OrderNumber == 2).OrganizationSign.ShortName ?? "",
                        contract.Organization.RegionId,
                        DistrictId = contract.Organization.DistrictId.Value,
                        PrtnApplicationId = contract.Application.PrtnApplication.Id
                    })
                .FirstOrDefault(contract => contract.Id2 == Id2);

            if (prtnContract == null)
            {
                AddError("404 - Not found");
                return null;
            }

            var graphResult = new List<GrapthYear>();

            var applicationGraph = _context.Set<PrtnApplicationGraph>()
                .Where(a => a.OwnerId == prtnContract.PrtnApplicationId)
                .ToList();

            var applicationGraphYears = applicationGraph.Select(a => a.YearIn).OrderBy(a => a).Distinct().ToList();

            for (int i = applicationGraphYears.Min(); i <= applicationGraphYears.Max(); i++)
            {
                var graphResultItem = new GrapthYear()
                {
                    YearIn = i,
                    TotalForYear = applicationGraph.Where(a => a.YearIn == i).Sum(a => a.NewVacanciesCount),
                    Months = new List<GrapthMonth>()
                };

                for (int j = 1; j <= 12; j++)
                {
                    graphResultItem.Months.Add(new GrapthMonth
                    {
                        MonthIn = j,
                        TotalForMonth = applicationGraph.Where(a => a.MonthIn == j).Sum(a => a.NewVacanciesCount),
                        NewVacanciesCount = applicationGraph.Where(a => a.YearIn == i && a.MonthIn == j).Sum(a => a.NewVacanciesCount)
                    });
                }
                graphResult.Add(graphResultItem);
            }


            var organizationNameByLocation
                = _organizationService.GetOrganizationNameByLocation(
                    prtnContract.RegionId,
                    prtnContract.DistrictId,
                    prtnContract.PrtnContractTypeId);

            var vacancyTableDict = new Dictionary<string, Dictionary<string, List<string>>>();

            vacancyTableDict.Add(
                "vacancyTable",
                new Dictionary<string, List<string>>()
                {
                    {"${Year}", new() },
                    {"${1}", new()},
                    {"${2}",new()},
                    {"${3}",new() },
                    {"${4}",new() },
                    {"${5}",new() },
                    {"${6}",new() },
                    {"${7}",new() },
                    {"${8}",new() },
                    {"${9}",new() },
                    {"${10}",new() },
                    {"${11}",new() },
                    {"${12}",new() },
                    {"${13}",new() }
                });

            //int totalRowValueList = graphResult.FirstOrDefault().TotalForYear;

            for (int i = 0; i < applicationGraphYears.Count(); i++)
            {
                var year = applicationGraphYears[i];

                var totalYear = graphResult[i];

                vacancyTableDict["vacancyTable"]["${Year}"].Add(year.ToString());

                var item = graphResult.FirstOrDefault(a => a.YearIn == year);

                var itemMonths = item.Months;

                for (int j = 0; j < itemMonths.Count(); j++)
                {
                    var month = itemMonths[j];

                    string monthKey = "${" + month.MonthIn.ToString() + "}";
                    vacancyTableDict["vacancyTable"][monthKey].Add(month.NewVacanciesCount.ToString());
                }
                vacancyTableDict["vacancyTable"]["${13}"].Add(item.TotalForYear.ToString());

            }
            //vacancyTableDict["${14}"].Add(graphResult.Sum(a => a.TotalForYear).ToString());
            //vacancyTableDict["vacancyTable"]["${Year}"].Add("Жами:");

            //for (int i = 0; i < totalRowValueList; i++)
            //{
            //    string monthKey = "${" + (++i).ToString() + "}";
            //    vacancyTableDict["vacancyTable"][monthKey].Add(graphResult[0].TotalForYear.ToString());
            //}

            Dictionary<string, string> data
                = new Dictionary<string, string>()
                {
                    {"${DocDate}",prtnContract.DocOn.ToString(Constants.DATE_FORMAT)},
                    {"${DocNumber}",prtnContract.DocNumber},
                    {"${ContractorRegion}",prtnContract.ContractorRegionName },
                    {"${ContractorDistrict}",prtnContract.ContractorDistrictName },
                    {"${ContractorName}",prtnContract.ContractorName},
                    {"${ContractorDirector}",prtnContract.ContractorDirectorName},
                    {"${ContractorAdress}",prtnContract.ContractorAddress},
                    {"${ContractorInn}",prtnContract.ContractorInn},
                    {"${ContractorBankCode}",prtnContract.ContractorBankCode},
                    {"${ContractorBankName}",prtnContract.ContractorBankName},
                    {"${ContractCreatedAddress}",$"{prtnContract.OrganizationDistrictName}"},
                    {"${ContractorSettlementAccount}",prtnContract.ContractorSettlementAccountCode},
                    {"${OrgRegion}",prtnContract.OrganizationRegionName},
                    {"${OrgDistrict}",prtnContract.OrganizationDistrictName},
                    {"${OrgAdress}",prtnContract.OrganizationAddress},
                    {"${OrgBankSettlementAccount}",prtnContract.OrganizationSettltmentAccountCode},
                    {"${OrgBankCode}",prtnContract.OrganizationBankCode},
                    {"${OrgBankName}",prtnContract.OrganizationBankName},
                    {"${OrgInn}",prtnContract.OrganizationInn},
                    {"${ContractorPosition1}",prtnContract.ContractPosition1},
                    {"${ContractorPosition2}",prtnContract.ContractPosition2},
                    {"${OrganizationNameByLocation}",organizationNameByLocation},
                    {"${14}",graphResult.Sum(a => a.TotalForYear).ToString()}
                };

            if (prtnContract.StatusId == StatusIdConst.SIGNING)
            {
                data.Add("${QrImageContractor}", _baseReportService.QRCodeImageAsBase64($"{prtnContract.Id2}\n{prtnContract.DocNumber}\n{prtnContract.DocOn}\n{prtnContract.ContractorDirectorName}\n{prtnContract.ContractorInn}"));
            }

            if (prtnContract.StatusId == StatusIdConst.AGREED)
            {
                data.Add("${QrImagePosition2}", _baseReportService.QRCodeImageAsBase64($"{prtnContract.Id2}\n{prtnContract.DocNumber}\n{prtnContract.DocOn}\n{prtnContract.ContractPosition2}"));

                data.Add("${QrImageContractor}", _baseReportService.QRCodeImageAsBase64($"{prtnContract.Id2}\n{prtnContract.DocNumber}\n{prtnContract.DocOn}\n{prtnContract.ContractorDirectorName}\n{prtnContract.ContractorInn}"));
            }

            if (prtnContract.StatusId == StatusIdConst.SIGNED)
            {
                data.Add("${QrImagePosition1}", _baseReportService.QRCodeImageAsBase64($"{prtnContract.Id2}\n{prtnContract.DocNumber}\n {prtnContract.DocOn}\n {prtnContract.ContractPosition1}"));
                
                data.Add("${QrImagePosition2}", _baseReportService.QRCodeImageAsBase64($"{prtnContract.Id2}\n{prtnContract.DocNumber}\n{prtnContract.DocOn}\n{prtnContract.ContractPosition2}"));

                data.Add("${QrImageContractor}", _baseReportService.QRCodeImageAsBase64($"{prtnContract.Id2}\n{prtnContract.DocNumber}\n{prtnContract.DocOn}\n{prtnContract.ContractorDirectorName}\n{prtnContract.ContractorInn}"));
            }

            //string fileName = "ownership_contract_region_AB.html";
            string fileName = "";

            if (prtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._50_100)
                fileName = "ownership_contract_district_AB.html";
            else if (prtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._101_200)
                if (prtnContract.ContractorRegionSoato == RegionSoatoConst.Karakalpakstan)
                    fileName = "ownership_contract_kr_vk_ab.html";
                else
                    fileName = "ownership_contract_region_AB.html";
            else if (prtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                fileName = "ownership_contract_ministry_AB.html";
            else
            {
                AddError("Неверный тип контракта");
                return null;
            }


            var fileAsString = _baseReportService.ReturnReadyHtmlAsString(
                                                      filename: fileName,
                                                      data: data,
                                                      tableData: vacancyTableDict,
                                                      lang);

            return fileAsString;
        }

        public byte[] DownloadPrtnContractPdf(Guid Id2, string lang = null)
            => _baseReportService.ReturnReadyPdf(DownloadPrtnContractAsHtml(Id2, lang));


        public Stream Print(PdfPrintDto dto)
        {
            dto.Text = HttpUtility.HtmlDecode(dto.Text);

            var workStream = new MemoryStream();

            using (var pdfWriter = new PdfWriter(workStream))
            {
                pdfWriter.SetCloseStream(false);

                var pdfDocument = new PdfDocument(pdfWriter);

                var pageSize = new PageSize(width: 595f, height: 842f);

                pageSize.ApplyMargins(0f, 0f, 0f, 0f, false);

                pdfDocument.SetDefaultPageSize(pageSize);

                var documentConvertProperties = new ConverterProperties();

                using (var document = HtmlConverter.ConvertToDocument(dto.Text,
                                                                      pdfDocument,
                                                                      documentConvertProperties))
                {
                    document.SetBottomMargin(0);
                    document.SetTopMargin(0);
                    document.Flush();
                }
            }

            workStream.Position = 0;
            return workStream;
        }

        public byte[] TestPdf()
        {

            //string fileName = "application.html";
            string fileName = "ownership_contract_district_AB.html";
            //string fileName = "ownership_contract_kr_vk_ab.html";
            //string fileName = "ownership_contract_ministry_AB.html";
            //string fileName = "ownership_contract_region_AB.html";
            //string fileName = "certificate.html";

            Dictionary<string, string> data
                = new Dictionary<string, string>();

            var fileString = _baseReportService.ReturnReadyHtmlAsString(filename: fileName,
                                                      data: data,
                                                      tableData: null);

            return _baseReportService.ReturnReadyPdf(fileString);
        }
    }

    public class GrapthYear
    {
        public int YearIn { get; set; }
        public int TotalForYear { get; set; }
        public List<GrapthMonth> Months { get; set; } = new();
    }

    public class GrapthMonth
    {
        public int MonthIn { get; set; }
        public int TotalForMonth { get; set; }
        public int NewVacanciesCount { get; set; }
    }
}


