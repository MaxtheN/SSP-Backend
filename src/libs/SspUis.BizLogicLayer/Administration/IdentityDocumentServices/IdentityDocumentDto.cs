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

namespace SspUis.BizLogicLayer.IdentityDocumentServices
{
    public class IdentityDocumentDto : UpdateIdentityDocumentDlDto, ILinkToEntity<IdentityDocument>
    {
        public string State { get; internal set; }
        new public List<IdentityDocumentTranslateDto> Translates { get; set; } = new();
    }
}
