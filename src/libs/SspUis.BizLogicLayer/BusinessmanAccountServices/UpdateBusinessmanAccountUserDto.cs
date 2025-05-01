using AutoMapper;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer.EfClasses;
using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class UpdateBusinessmanAccountUserDto : UpdateUserInfoDlDto, ILinkToEntity<BusinessmanUser>
    {
        public MyUpdateContractorDto Contractor { get; set; } = new();
    }
}
