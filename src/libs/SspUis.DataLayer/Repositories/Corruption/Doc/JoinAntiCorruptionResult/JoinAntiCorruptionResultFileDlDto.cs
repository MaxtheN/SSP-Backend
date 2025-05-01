using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.Models;
using GenericServices;

namespace SspUis.DataLayer.Repositories.Corruption
{
    public class JoinAntiCorruptionResultFileDlDto : EntityDto<JoinAntiCorruptionResultFileDlDto, JoinAntiCorruptionResultFile>, IHaveIdProp<Guid>, ILinkToEntity<JoinAntiCorruptionResultFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    
    }
}
