using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using SspUis.Core;
using AutoMapper;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IPrtnApplicationDlDto
    {
        public int PrtnContractTypeId { get; set; }
        public int NewVacanciesCount { get; set; }
        public bool ChooseLocation { get; set; }
        public int? ChoosedRegionId { get; set; }
        public int? ChoosedDistrictId { get; set; }
        public long MfyId { get; set; }
        public List<PrtnApplicationGraphDlDto> Graphs { get; set; }
        
    }
}
