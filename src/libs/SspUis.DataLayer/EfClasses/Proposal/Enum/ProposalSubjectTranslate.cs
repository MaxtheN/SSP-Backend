using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Proposal
{
    [Table("enum_proposal_subject_translate", Schema = "propos")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ix_enum_proposal_subject_translate__owner_lang")]
    public partial class ProposalSubjectTranslate : EnumTranslateEntity<ProposalSubjectTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ProposalSubject.Translates))]
        public virtual ProposalSubject Owner { get; set; }
    }
}