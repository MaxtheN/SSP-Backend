using GenericServices;
using SspUis.DataLayer.EfClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ChangeUserLanguageDlDto : EntityDto<ChangeUserLanguageDlDto, User>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int LanguageId { get; set; }
    }
}
