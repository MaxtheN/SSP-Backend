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
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.OrganizationLegalFormServices
{
    public class OrganizationLegalFormDto : UpdateOrganizationLegalFormDlDto, ILinkToEntity<OrganizationLegalForm>
    {
        public string State { get; set; }
        new public List<OrganizationLegalFormTranslateDto> Translates { get; set; } = new();
    }
}
