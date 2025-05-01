using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SspUis.BizLogicLayer
{
    public class ServicePriceDto : UpdateServicePriceDlDto, ILinkToEntity<ServicePrice>, IDocument
    {
        new public List<ServicePriceGroupDto> Groups { get; set; } = new();
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        #region Actions
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        #endregion
        public ServicePriceDto Clone(ref ServicePriceDto dto)
        {
            return new ServicePriceDto
            {
                Id = 0,
                StatusId = dto.StatusId,
                Status = dto.Status,
                OrganizationId = dto.OrganizationId,
                Organization = dto.Organization,
                CreatedAt = dto.CreatedAt,
                DocOn = dto.DocOn,
                DocNumber = dto.DocNumber,
                Details = dto.Details,
                Groups = dto.Groups.Select(g => new ServicePriceGroupDto
                {
                    Id = 0,
                    GroupId = g.GroupId,
                    Group = g.Group,
                    CreatedAt = g.CreatedAt,
                    Tables = g.Tables.Select(t => new ServicePriceGroupTableDto
                    {
                        Id = 0,
                        NeedChamberServiceId = t.NeedChamberServiceId,
                        NeedChamberService = t.NeedChamberService,
                        IsConcrete = t.IsConcrete,
                        BeginCoef = t.BeginCoef,
                        EndCoef = t.EndCoef,
                        ConcreteCoef = t.ConcreteCoef,
                    }).ToList()
                }).ToList()
            };
        }
    }
}