using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer.Claim
{
    public class ApplicationForCourtForGetDto
    {
        public long ClaimApplicationId { get; set; }
        public long? MediationId { get; set; }
        public long? MediationPlanId { get; set; }
        public ClaimApplicationIntegrationFileUrlDto claimApplicationIntegrationFileUrl { get; set; } = new();
        public ClaimApplicationFileIdDto ClaimApplicationFileIds { get; set; } = new();
        public MediationFileIdDto MediationFileIds { get; set; } = new();
    }
    public class ClaimApplicationIntegrationFileUrlDto
    {
        public string ClaimApplicationLink { get; set; }
        public string? MediationLink { get; set; }
        public string? MediationPLanLink { get; set; }
    }
    public class ClaimApplicationFileIdDto
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();
        public List<string> FileNames { get; set; } = new List<string>();
    }
    public class MediationFileIdDto
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();
        public List<string> FileNames { get; set; } = new List<string>();
    }
}
