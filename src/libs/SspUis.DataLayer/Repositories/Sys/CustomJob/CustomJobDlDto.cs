using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using GenericServices;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class CustomJobDlDto<TDto> : EntityDto<TDto, CustomJob>
        where TDto : CustomJobDlDto<TDto>
    {
        [LocalizedRange(0, int.MaxValue)]
        [LocalizedRequired] 
        public int JobTypeId { get; set; }
        [Required]
        public bool IsForceUpdate { get; set; } 
        public string ExtendData { get; set; }  
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? OrganizationId { get; set; }


    }
}
