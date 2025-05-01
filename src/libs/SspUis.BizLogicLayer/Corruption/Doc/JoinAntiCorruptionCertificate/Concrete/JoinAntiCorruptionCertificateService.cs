using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Spire.Pdf.Conversion;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core.Configurations;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories.Corruption;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Corruption
{
    public class JoinAntiCorruptionCertificateService
        : BaseEntityService<long,
            JoinAntiCorruptionCertificate,
            JoinAntiCorruptionCertificateListDto,
            JoinAntiCorruptionCertificateDto,
            CreateJoinAntiCorruptionCertificateDlDto,
            UpdateJoinAntiCorruptionCertificateDlDto,
            IJoinAntiCorruptionCertificateRepository,
            JoinAntiCorruptionCertificateSortFilterOptions>, IJoinAntiCorruptionCertificateService
    {

        private readonly IConvertService _pdfConverter;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;
        private readonly INumberService _numberService;
        private readonly SystemConf _systemConf;
        public JoinAntiCorruptionCertificateService(
            IUnitOfWork unitOfWork,
            IConvertService pdfConverter,
            IStorageService storageService,
            ICultureHelper cultureHelper,
            INumberService numberService,
            SystemConf systemConf) : base(unitOfWork)
        {
            _numberService = numberService;
            _storageService = storageService;
            _systemConf = systemConf;
            _pdfConverter = pdfConverter;
            _cultureHelper = cultureHelper;
        }

        public PagedResult<JoinAntiCorruptionCertificateListDto> GetList(JoinAntiCorruptionCertificateSortFilterOptions dto)
        {
            var result = Repository.ReadAsNoTracked<JoinAntiCorruptionCertificateListDto>()
                                    .SortFilter(dto)
                                    .AsPagedResult(dto);
            
            return result;
        }

        public JoinAntiCorruptionCertificateDto Get(long id)
        {
            return Repository.ById<JoinAntiCorruptionCertificateDto>(id);
        }

        public async ValueTask<byte[]> DownloadPdf(Guid id2, string? lang)
        {
            lang ??= "uz-latn";

            var wordFile = _storageService.GetStaticFile(
               StaticFileConst.WordTemplate.GetFileName(
                   lang,
                   StaticFileConst.WordTemplate.JOIN_ANTICORRUPTION_CERTIFICATE)
               );
            var lan = UnitOfWork.Context.Set<Language>()
                    .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;

            var certificate = UnitOfWork.Context.Set<JoinAntiCorruptionCertificate>()
                .Include(c => c.JoinAntiCorruptionResultTable)
                .ThenInclude(c => c.Owner)
                .Include(x => x.Contractor)
                .FirstOrDefault(c => c.Id2 == id2);

            if (certificate is null)
            {
                AddError("Bunday sertifikat mavjud emas !");
                return null;
            }

            var link = _systemConf.QrImagePrintMy + "/MemshipCertificate/DownloadPdf?id2=" + certificate.Id2.ToString();
            var qrCode = QRCodeHelper.GeneratePng(link, 256, 256);
            var qrImage = new MemoryStream(qrCode);
            var plh = new Placeholders();
            plh.ImagePlaceholders.Add("QrCode", new ImageElement
            {
                Dpi = 256,
                MemStream = qrImage,
            });

            var expireOn = certificate.ExpireOn is null ? "Nomuayyan muddat" : certificate.ExpireOn.Value.ToString("dd.MM.yyyy");
            var expireOnEng = certificate.ExpireOn is null ? "Indefinite period" : certificate.ExpireOn.Value.ToString("dd.MM.yyyy");

			plh.TextPlaceholders.Add(nameof(certificate.Contractor.FullName), certificate.Contractor.FullName ?? "");
            plh.TextPlaceholders.Add(nameof(certificate.DocNumber), certificate.DocNumber);
            plh.TextPlaceholders.Add(nameof(certificate.DocOn), certificate.DocOn.ToString("dd.MM.yyyy"));
            plh.TextPlaceholders.Add(nameof(certificate.ExpireOn), expireOn);
            plh.TextPlaceholders.Add("ExpireOnEng", expireOnEng);
            plh.TextPlaceholders.Add("ResultDocNumber", certificate.JoinAntiCorruptionResultTable?.Owner?.DocNumber ?? "");
            plh.TextPlaceholders.Add("ResultDocOn", certificate.JoinAntiCorruptionResultTable?.Owner?.DocOn.ToString("dd.MM.yyyy") ?? "");
            wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();

            return await _pdfConverter.DocxToPdfAsync(wordFile, new());
        }

    }
}