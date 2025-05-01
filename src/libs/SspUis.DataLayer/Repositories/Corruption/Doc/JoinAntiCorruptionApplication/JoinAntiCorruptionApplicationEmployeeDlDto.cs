using GenericServices;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using WEBASE.Models;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class JoinAntiCorruptionApplicationEmployeeDlDto : EntityDto<JoinAntiCorruptionApplicationEmployeeDlDto, JoinAntiCorruptionApplicationEmployee>, IHaveIdProp<long>, ILinkToEntity<JoinAntiCorruptionApplicationEmployee>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Person { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string Position { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string PhoneNumber { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Email { get; set; }
    }
}
