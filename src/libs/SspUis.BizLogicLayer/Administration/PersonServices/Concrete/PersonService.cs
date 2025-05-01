using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using SspUis.Integration.DigitizationCenter.Services.GSP;
using StatusGeneric;
using WEBASE;
using WEBASE.Integration.Manuals.Services;
using WEBASE.Integration.MSPD.Client;
using WEBASE.Integration.MSPD.FHDYO;
using WEBASE.Integration.MSPD.GSP;
using WEBASE.Models;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.PersonServices
{
    public class PersonService : StatusGenericHandler, IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMspdUnitOfWork _mspdUnitOfWork;
        private readonly IUOWIntegrationManuals _uowIntegrationManuals;
        private readonly IStorageService _storageService;
        private readonly IDigitizationCenterGspService _gspService;

        public PersonService(IUnitOfWork unitOfWork,
                             IMspdUnitOfWork mspdUnitOfWork,
                             IStorageService storageService,
                             IUOWIntegrationManuals uowIntegrationManuals, IDigitizationCenterGspService gspService)
        {
            _repository = unitOfWork.PersonRepository;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _mspdUnitOfWork = mspdUnitOfWork;
            _gspService = gspService;
            _uowIntegrationManuals = uowIntegrationManuals;
        }

        public PersonDto Get()
        {
            return new PersonDto();
        }

        public PersonDto Get(int id)
        {
            var dto = _repository.ById<PersonDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreatePersonDlDto dto)
        {

            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                if (dto.PictureId != null) 
                { 
                    _storageService.MoveToPersistent(DocumentStorageConst.HL_PERSON_FILES, $"{entity.Id}", (Guid)dto.PictureId);
                    CombineStatuses(_storageService);
                }

                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdatePersonDlDto dto)
        {
            _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);

            if (IsValid)
                _unitOfWork.Save();
        }

        public void AddOrUpdateFiles(int personId, Guid pictureId)
        {
            var existingPerson = Get(personId);

            if (existingPerson == null)
            {
                AddError("Not Found");
                return;
            }

            try
            {
                _repository.UpdatePersonFiles(personId, pictureId);
            }
            catch (Exception ex)
            {
                AddError($"Error updating person files: {ex.Message}");
                return;
            }

            _storageService.ResolveMarkedFiles(DocumentStorageConst.HL_PERSON_FILES, $"{personId}");

            if (_repository.IsValid)
                _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            try
            {
                var entity = _repository.Delete(id);
                CombineStatuses(_repository);

                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }

        public async Task<PersonDto> GetByPassportData(GSPPersonInfoRequestDto dto)
        {
            var birthDate = dto.DateOfBirth.AsDateOnly();
            var person = _repository.ReadAsNoTracked<PersonDto>()
                                    .FirstOrDefault(a => a.PassportSeria == dto.Seria
                                                      && a.PassportNumber == dto.Number
                                                      && a.BirthDate == birthDate);
            if (person == null)
            {
                var status = await GetFromGsp(dto);
                CombineStatuses(status);
                person = status.Result;
            }
            return person;
        }
        private async Task<IStatusGeneric<PersonDto>> GetFromGsp(GSPPersonInfoRequestDto dto)
        {
            var res = new StatusGenericHandler<PersonDto>();
            var status = await _mspdUnitOfWork.GSP.GetPersonInfoAsync(dto);
            if (status.IsValid)
            {
                var wbCodes = status.Result.LoadCodes(_uowIntegrationManuals);
                var birthCountry = _unitOfWork.CountryRepository.ByWbCode(wbCodes.BirthCountryWbCode);
                var nationality = _unitOfWork.NationalityRepository.ByWbCode(wbCodes.NationalityWbCode);
                var citizenship = _unitOfWork.CitizenshipRepository.ByWbCode(wbCodes.CitizenshipWbCode);
                var livingRegion = _unitOfWork.RegionRepository.ByWbCode(wbCodes.LivingRegionWbCode);
                var livingDistrict = _unitOfWork.DistrictRepository.ByWbCode(wbCodes.LivingDistrictWbCode);
                var gender = _unitOfWork.Context.Set<Gender>()
                                                .AsQueryable()
                                                .Include(a => a.Translates)
                                                .FirstOrDefault(a => a.Id == status.Result.person_sex);
                var birthDate = status.Result.person_birth_date.AsDateOnly();
                res.SetResult(new PersonDto
                {
                    Pinfl = status.Result.person_pin,
                    PassportSeria = status.Result.person_passport_seria,
                    PassportNumber = status.Result.person_passport_number,
                    PassportDate = status.Result.person_passport_date,
                    PassportExpiration = status.Result.person_passport_expiration,
                    SurnameLatin = status.Result.person_surname_latin,
                    NameLatin = status.Result.person_name_latin,
                    PatronymLatin = status.Result.person_patronym_latin,
                    SurnameEng = status.Result.person_surname_eng,
                    NameEng = status.Result.person_name_eng,
                    BirthDate = birthDate,
                    PassportDivName = status.Result.person_passport_div_name,
                    BirthCountryId = birthCountry?.Id,
                    BirthCountry = birthCountry?.Translates.AsQueryable().FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? birthCountry?.FullName,
                    //BirthRegionId = _unitOfWork.RegionRepository.ByWbCode(birthRegionWbCode)?.Id,
                    BirthDistrict = status.Result.birth_place,
                    GenderId = gender?.Id,
                    Gender = gender?.Translates.AsQueryable().FirstOrDefault(GenderTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? gender?.FullName,
                    NationalityId = nationality?.Id,
                    Nationality = nationality?.Translates.AsQueryable().FirstOrDefault(NationalityTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? nationality?.FullName,
                    CitizenshipId = citizenship?.Id,
                    Citizenship = citizenship?.Translates.AsQueryable().FirstOrDefault(CitizenshipTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? citizenship?.FullName,
                    LivingRegionId = livingRegion?.Id,
                    LivingRegion = livingRegion?.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? livingRegion?.FullName,
                    LivingDistrictId = livingDistrict?.Id,
                    LivingDistrict = livingDistrict?.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? livingDistrict?.FullName,
                });
            }
            res.CombineStatuses(status);
            return res;
        }
        //public async Task<PersonDto> GetByPassportDataFromDigital(GSPNewApiRequestDto dto)
        //{
        //    var birthDate = DateTime.Parse(dto.birth_date);
        //    var person = _repository.ReadAsNoTracked<PersonDto>()
        //                            .FirstOrDefault(a => a.PassportSeria == dto.document.Substring(0,2)
        //                                              && a.PassportNumber == dto.document.Substring(2)
        //                                              && a.BirthDate == birthDate.AsDateOnly());
        //    if (person == null)
        //    {
        //        var formattedBirthDate = birthDate.ToString("yyyy-MM-dd");
        //        dto.birth_date = formattedBirthDate;
        //        var status = await GetFromDigital(dto);
        //        CombineStatuses(status);
        //        person = status.Result;
        //    }
        //    return person;
        //}
        public async Task<PersonDto> GetByPassportDataFromDigital(GSPNewApiRequestDto dto)
        {
            DateTime birthDate;
            if (DateTime.TryParseExact(dto.birth_date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
            {
                // Use birthDate in your logic
                var person = _repository.ReadAsNoTracked<PersonDto>()
                                        .FirstOrDefault(a => a.PassportSeria == dto.document.Substring(0, 2)
                                                          && a.PassportNumber == dto.document.Substring(2)
                                                          && a.BirthDate == birthDate.AsDateOnly());
                if (person == null)
                {
                    var formattedBirthDate = birthDate.ToString("yyyy-MM-dd");
                    dto.birth_date = formattedBirthDate;
                    var status = await GetFromDigital(dto);
                    CombineStatuses(status);
                    person = status.Result;
                }
                return person;
            }
            else
            {
                // Handle the case where the string could not be parsed into a valid DateTime object
                Console.WriteLine("Invalid birth date format");
                // return null or throw an exception depending on your requirements
                return null;
            }
        }

        private async Task<IStatusGeneric<PersonDto>> GetFromDigital(GSPNewApiRequestDto dto)
        {
            var res = new StatusGenericHandler<PersonDto>();
            var response = await _gspService.GetFromGSP(dto);

            if (response != null && response.Any())
            {
                foreach (var data in response)
                {
                    //var birthCountry = _unitOfWork.CountryRepository.ByWbCode(data.birthcountry);
                    //var nationality = _unitOfWork.NationalityRepository.ByWbCode(data.nationality);
                    //var citizenship = _unitOfWork.CitizenshipRepository.ByWbCode(data.citizenship);
                    //var livingRegion = _unitOfWork.RegionRepository.ByWbCode(data.birthplace);
                    var livingDistrict = _unitOfWork.DistrictRepository.ByWbCode(data.birthplace);
                    //var birhtRegion = _unitOfWork.RegionRepository.ByWbCode(data.birthplace.Substring(0, 4));
                    var gender = _unitOfWork.Context.Set<Gender>()
                        .AsQueryable()
                        .Include(a => a.Translates)
                        .FirstOrDefault(a => a.Id == data.sex);
                    var birthDate = DateTime.Parse(data.birth_date);
                  

                    res.SetResult(new PersonDto
                    {
                     
                        Pinfl = data.current_pinpp,
                        SurnameLatin = data.surnamelat,
                        NameLatin = data.namelat,
                        PatronymLatin = data.patronymlat,
                        SurnameEng = data.engsurname,
                        NameEng = data.engname,
                        BirthDate = birthDate.AsDateOnly(),
                        BirthCountryId = (data.birthcountryid == 182) ? 211 : data.birthcountryid,
                        BirthCountry = data.birthcountry,
                        BirthDistrict = data.birthplace,
                        //BirthRegionId = birhtRegion?.Id??0,
                        //BirthRegion = birhtRegion?.FullName??String.Empty,
                        GenderId = gender?.Id,
                        Gender = gender?.Translates.AsQueryable().FirstOrDefault(GenderTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? gender?.FullName,
                        ////NationalityId = data.nationalityid,
                        Nationality = data.nationality,
                        //CitizenshipId = data.citizenshipid,
                        Citizenship = data.citizenship,
                        ////LivingRegionId = livingRegion?.Id,
                        ////LivingRegion = livingRegion?.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? livingRegion?.FullName,
                        LivingDistrict = data.birthplace,
                        PassportSeria = dto.document.Substring(0,2),
                        PassportNumber = dto.document.Substring(2),
                        PassportDate = DateTime.Parse(data.documents.Where(a => a.document == dto.document).FirstOrDefault().datebegin),
                        PassportExpiration = DateTime.Parse(data.documents.Where(a => a.document == dto.document).FirstOrDefault().dateend),
                    });
                }
            }
            else
            {
                res.AddError("No data found in the response.");
            }

            //res.CombineStatuses(response); 
            return res;
        }


        private async Task<IStatusGeneric<ChildInfoResult>> GetChildInfoByCertAsync(PersonFilterDto dto)
        {
            var result = await _mspdUnitOfWork.FHDYO.GetChildInfoByCertAsync(
                series: dto.Seria,
                number: dto.Number);

            //var  result = await _mspdUnitOfWork.FHDYO.GetChildInfoByCertAsync(
            //   new WEBASE.Integration.MSPD.Models.FHDYO.ChildInfoByCertRequestDto
            //   {
            //       Id = 12,
            //       CertSeries = dto.Seria,
            //       CertNumber = dto.Number
            //   });
            return result;
        }

        private void Validation<TDto>(PersonDlDto<TDto> dto, Person entity)
            where TDto : PersonDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (!dto.Inn.NullOrEmpty() && query.ByInn(dto.Inn, isIncludePassive: true).Any())
                AddError($"Человек с этим ИНН ({dto.Inn}) уже существует.", nameof(dto.Inn));
            if (!dto.Pinfl.NullOrEmpty() && query.ByPinfl(dto.Pinfl, isIncludePassive: true).Any())
                AddError($"Человек с этим ИНПС ({dto.Pinfl}) уже существует.", nameof(dto.Pinfl));

            if (dto.PassportExpiration < DateTime.Today)
            {
                AddError($"Срок действия паспорта истек.", $"{dto.PassportSeria}{dto.PassportNumber}.", $"{dto.PassportExpiration}");
            }
        }

        public async Task<Stream> CheckExcelData(IFormFile formFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(formFile.OpenReadStream());

            var namerange = excelPackage.Workbook.Names["ImportRow"];

            var ws = namerange.Worksheet;
            var n = ws.Cells.Rows;

            for (int i = 2; i < n; i++)
            {
                var person = await GetByPassportData(new GSPPersonInfoRequestDto
                {
                    DateOfBirth = DateTime.ParseExact(ws.Cells[i, 7].Value.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    Seria = ws.Cells[i, 12].Value.ToString().Substring(0, 2),
                    Number = ws.Cells[i, 12].Value.ToString().Substring(2)
                });
                ws.Cells[i, 32].Value = person == null ? "mavjud emas" : "mavjud";
            }
            var result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
            result.Position = 0;
            return result;
        }

        public async Task<PersonDto> GetChildFromGsp(PersonFilterDto dto)
        {
            dto.Seria = dto.Seria.ToUpper();

            var status = await GetChildInfoByCertAsync(dto);

            CombineStatuses(status);

            if (IsValid)
            {
                var child = new ChildInfoData();
                if (status.Result is null || status.Result.Items is null || status.Result.Items.Count == 0)
                {
                    AddError("Child Not Found");
                    return null;
                }

                foreach (var item in status.Result.Items)
                {
                    if (item.BirthDate.Year == dto.DateOfBirth.Year)
                    {
                        child = item;
                        if (item.BirthDate.Month != dto.DateOfBirth.Date.Month)
                        {
                            AddError("Child Not Found");
                            return null;
                        }

                    }
                }


                var gender = _unitOfWork.Context.Set<Gender>()
                                                .AsQueryable()
                                                .Include(a => a.Translates)
                                                .FirstOrDefault(a => a.Id == child.GenderCode);

                //metrka bo'ganichun uzb fuqarosi hisoblanib 234 olinvoti
                var citizenship = _unitOfWork.Context.Set<Citizenship>().Where(a => a.Id == 234).FirstOrDefault();

                var result = new PersonDto
                {
                    Pinfl = child.Pnfl,
                    PassportSeria = child.CertSeries,
                    PassportNumber = child.CertNumber,
                    PassportDate = child.CertBirthDate,
                    PatronymLatin = child.Patronym,
                    NameLatin = child.Name,
                    // Bazada nullable emas  DocExpireOn = DateTime.Now,
                    BirthDate = dto.DateOfBirth.AsDateOnly(),
                    GenderId = child.GenderCode,
                    Gender = gender?.Translates.AsQueryable().FirstOrDefault(GenderTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? gender?.FullName,
                    CitizenshipId = citizenship?.Id,
                    Citizenship = citizenship?.Translates.AsQueryable().FirstOrDefault(CitizenshipTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? citizenship?.FullName,
                    StateId = StateIdConst.ACTIVE
                };
                result.FullName = StringUtility.GetFullFIO(child.Surname, child.Name, result.PatronymLatin);
                result.ShortName = StringUtility.GetFIO(result.FullName);

                return result;
            }
            return null;
        }

        #region Files
        public IStorageFileInfo UploadFile(StorageFile file)
        {
            if (!IsSupportedFileFormat(file))
            {
                AddError("File formati to'g'ri emas / Неверный формат файла : " + file.FileName);
            }

            if (file.FileSize > 200 * 1024)
            {
                AddError("Fayl hajmi fayl chegarasidan oshib ketdi / Размер файла превышает лимит для файла : " + file.FileName);
            }

            var result = _storageService.SaveTemp(DocumentStorageConst.HL_PERSON_FILES, file);
            CombineStatuses(_storageService);
            return IsValid ? result.First() : null;
        }

        private bool IsSupportedFileFormat(StorageFile file)
        {
            var allowedFormats = new[] { ".png", ".jpg", ".jpeg" };

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedFormats.Contains(fileExtension);
        }

        public StorageFile DownloadFile(Guid fileId)
        {
            var file = _storageService.GetTempFile(DocumentStorageConst.HL_PERSON_FILES, fileId);
            if (file == null)
            {
                _storageService.ClearErrors();
                var entity = _unitOfWork.Context.Set<Person>().FirstOrDefault(a => a.PictureId == fileId);
                file = _storageService.GetFile(DocumentStorageConst.HL_PERSON_FILES, entity.Id.ToString(), fileId);
                if (file == null)
                {
                    AddError("По вашему запросу запись не найдено");
                    return null;
                }
            }
            return file;
        }

        public void DeleteFile(Guid fileId)
        {
            _storageService.DeleteTemp(DocumentStorageConst.HL_PERSON_FILES, fileId);

            _storageService.ClearErrors();
            var entity = _unitOfWork.Context.Set<Person>().FirstOrDefault(a => a.PictureId == fileId);
            if (entity != null)
            {
                var file = _storageService.GetFile(DocumentStorageConst.HL_PERSON_FILES, entity.Id.ToString(), fileId);
                if (file != null)
                    _storageService.Delete(DocumentStorageConst.HL_PERSON_FILES, entity.Id.ToString(), fileId);

                entity.PictureId = null;
                _unitOfWork.Save();
            }
        }
        #endregion
    }
}
