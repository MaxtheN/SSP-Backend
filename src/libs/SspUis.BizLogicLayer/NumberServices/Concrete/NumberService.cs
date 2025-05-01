using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Public.Sys;
using StatusGeneric;
using WEBASE;

namespace SspUis.ServiceLayer.NumberServices
{
    public class NumberService : StatusGenericHandler, INumberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly Dictionary<int, string> _regionPrexDict = new Dictionary<int, string>
        {
            {196,"N-TN" },
            {180,"N-TV" },
            {178,"N-AN" },
            {187,"N-BX" },
            {185,"N-JZ" },
            {193,"N-QR" },
            {179,"N-QD" },
            {183,"N-NV" },
            {197,"N-NA" },
            {184,"N-SN" },
            {195,"N-SD" },
            {186,"N-SR" },
            {181,"N-FA" },
            {182,"N-XZ" },
            {1,"N-UZ" }
        };

        public NumberService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public (int, string) GetNext(string document)
        {
            int orgId = 0;
            if (_authService.User?.OrganizationId != null)
                orgId = _authService.User.OrganizationId;

            return GetNext(document, document, orgId);
        }

        public (int, string) GetNext(string document, int organizationId, int? externalSourceTypeId = null) =>
            GetNext(document, document, organizationId, externalSourceTypeId);
        public (int, string) GetNext(string document, int organizationId, int regionId, int districtId, int? externalSourceTypeId = null) =>
           GetNext(document, document, organizationId, externalSourceTypeId, regionId, districtId);

        public (int, string) GetNext(string templateDocument, string numberDocument, int organizationId, int? externalSourceTypeId = null, int regionId = 1, int districtId = 1)
        {
            var templateEntity = _unitOfWork.Context.Set<NumberTemplate>().GetActual(templateDocument);
            int nextNumber = 0;
            var organization = _unitOfWork.OrganizationRepository.ById(organizationId);
            //var regionPrexDict = _regionPrexDict[_authService.User.OrganizationId];

            if (templateEntity == null)
                AddError($"Шаблон для документа ({templateDocument}) не найдено");
            else
                nextNumber = GetNextNumber(numberDocument, organizationId);

            if (IsValid)
            {
                string result = Regex.Replace(templateEntity.Template, @"{[a-zA-Z]+(:[0-9a-zA-Z]+)?}", match =>
                {
                    var key = match.Value.TrimStart('{').TrimEnd('}');
                    int indexOf = key.IndexOf(':');
                    string format = indexOf != -1 && key.Length > indexOf + 1 ? key.Substring(indexOf + 1) : null;
                    key = indexOf != -1 ? key.Substring(0, indexOf) : key;
                    switch (key)
                    {
                        case "nextnumber":
                            if (format.NullOrEmpty())
                                return nextNumber.ToString();
                            return string.Format("{0:" + format + "}", nextNumber);
                        case "financeyear":
                            return DateTime.Now.Year.ToString();
                        case "shortfinanceyear":
                            return DateTime.Now.Year.ToString()[2..];
                        case "orgcode":
                            return organization.OrderCode;
                        case "externalsourcetype":
                            return externalSourceTypeId.ToString();
                        case "regionprexdict":
                            return $"{_unitOfWork.Context.Set<RegionPrefix>().FirstOrDefault(x => x.RegionId == regionId)?.Prefix ?? ""}M";
                        case "districtprexdict":
                            return $"{_unitOfWork.Context.Set<DistrictPrefix>().FirstOrDefault(x => x.DistrictId == districtId)?.Code ?? "-"}";
                        default:
                            return "";
                    }
                });

                SaveCurrent(nextNumber, numberDocument, organizationId);

                return (nextNumber, result);
            }

            return default;
        }

        private (string, string) GetRegionDistrict(int regionId = 1, int districtId = 1)
        {
            var prefix = _unitOfWork.Context.Set<RegionPrefix>().FirstOrDefault(x => x.RegionId == regionId);
            var code = _unitOfWork.Context.Set<DistrictPrefix>().FirstOrDefault(x => x.DistrictId == districtId);

            return ($"{prefix.Prefix}M", $"{code.Code}");
        }

        private int GetNextNumber(string document, int organizationId)
        {
            var entity = GetEntity();
            if (entity == null)
            {
                _unitOfWork.Context.Set<NumberTemplate>().Lock(document);
                entity = GetEntity();

                if (entity == null)
                {
                    entity = Number.Create(document, organizationId, DateTime.Now.Year);
                    _unitOfWork.Context.Set<Number>().Add(entity);
                    _unitOfWork.Context.Entry(entity).State = EntityState.Added;
                    _unitOfWork.Save();
                }
            }

            _unitOfWork.Context.Set<Number>().Lock(entity.Id);
            entity = GetEntity();
            return entity.CurrentNumber + 1;

            Number GetEntity()
            {
                return _unitOfWork.Context.Set<Number>().AsNoTracking().GetCurrent(document, organizationId);
            }
        }

        private void SaveCurrent(int number, string document, int organizationId)
        {
            var entity = _unitOfWork.Context.Set<Number>().GetCurrent(document, organizationId);

            if (entity.CurrentNumber != number - 1)
                throw new Exception($"{nameof(NumberService)}.{nameof(SaveCurrent)}() method called not thread-safe manner");

            entity.CurrentNumber = number;

            _unitOfWork.Context.SaveChanges();
        }

    }
}
