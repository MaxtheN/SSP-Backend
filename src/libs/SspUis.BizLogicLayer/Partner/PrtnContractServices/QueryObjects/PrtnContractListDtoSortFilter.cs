using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public static class PrtnContractListDtoSortFilter
    {
        public static IQueryable<PrtnContractListDto> SortFilter(this IQueryable<PrtnContractListDto> query, PrtnDocumentSortFilterOptions options, Organization? organization = null)
        {
            var a = query.Count();
            query = query.DocumentFilter(options);

            if (options.RegionId.HasValue)
                query = query.Where(a => a.ContractorRegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => a.ContractorDistrictId == options.DistrictId.Value);

            if (options.MfyId.HasValue)
                query = query.Where(a => a.MfyId == options.MfyId.Value);

            if (options.PrtnContractTypeId.HasValue)
                query = query.Where(a => a.PrtnContractTypeId == options.PrtnContractTypeId.Value);
    
            if (options.HasCertificate.HasValue)
                query = query.Where(a => a.PrtnCertificateId.HasValue == options.HasCertificate.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a => options.ContractorInn == a.ContractorInn);

            if (!string.IsNullOrEmpty(options.OkedCode))
                query = query.Where(a => a.OkedCode == options.OkedCode);

            if (options.HasSearch())
                query = query.Where(a => a.PrtnContractType.ToLower().Contains(options.Search.ToLower())
                    || a.DocNumber.ToLower().Contains(options.Search.ToLower())
                    || a.Status.ToLower().Contains(options.Search.ToLower())
                    || a.Contractor.ToLower().Contains(options.Search.ToLower())
                    || a.ContractorInn.ToLower().Contains(options.Search.ToLower())
                );

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");

            else
                query = query.OrderByDescending(a => a.Id);


            if(options.StatusId == 27 && options.PrtnContractTypeId == 3)
            {
                var result = new List<PrtnContractListDto>();

                foreach (var item in query.ToList())
                {
                    if (item.Signed.Count() > 1)
                    {
                        result.Add(item);
                    }
                    else if (item.Signed.Count() == 1)
                    {
                        var newItem = new PrtnContractListDto
                        {
                            Id = item.Id,
                            PrtnContractType = item.PrtnContractType,
                            DocNumber = item.DocNumber,
                            Status = item.Status,
                            Contractor = item.Contractor,
                            ContractorInn = item.ContractorInn,
                            ContractorRegionId = item.ContractorRegionId,
                            ContractorDistrictId = item.ContractorDistrictId,
                            MfyId = item.MfyId,
                            PrtnContractTypeId = item.PrtnContractTypeId,
                            PrtnCertificateId = item.PrtnCertificateId,
                            OkedCode = item.OkedCode,
                            ApplicationDocNumber = item.ApplicationDocNumber,
                            NextSigner = item.NextSigner,
                            NewVacanciesCount = item.NewVacanciesCount,
                            ModifiedAt = item.ModifiedAt,
                            ApplicationDocOn = item.ApplicationDocOn,
                            ApplicationId = item.ApplicationId,
                            ContractorDistrict = item.ContractorDistrict,
                            ContractorGovShare = item.ContractorGovShare,
                            ContractorHasGovShare = item.ContractorHasGovShare,
                            ContractorId = item.ContractorId,
                            ContractorRegestrationDate = item.ContractorRegestrationDate,
                            ContractorRegion = item.ContractorRegion,
                            CreatedAt = item.CreatedAt,
                            DocOn = item.DocOn,
                            StatusId = item.StatusId,
                            Id2 = item.Id2,
                            LastSigner = item.LastSigner,
                            Signed = item.Signed,
                            Mfy = item.Mfy,
                            NotSigned = item.NotSigned,
                            Oked = item.Oked,
                            PrtnCertificateStatus = item.PrtnCertificateStatus,
                            PrtnCertificateStatusId = item.PrtnCertificateStatusId,

                            // Change only these fields
                            OrganizationId = organization.Id,
                            Organization = organization.ShortName,
                            OrganizationInn = organization.Inn
                        };

                        result.Add(newItem);
                    }
                }

                return result.AsQueryable();
            }
    
            return query;
        }
    }
}
