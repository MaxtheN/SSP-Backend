using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_election_member_translate", Schema = "hrm")]
    public partial class ElectionMemberTranslate : TranslateEntity<ElectionMemberTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ElectionMember.Translates))]
        public virtual ElectionMember Owner { get; set; }
    }
}
