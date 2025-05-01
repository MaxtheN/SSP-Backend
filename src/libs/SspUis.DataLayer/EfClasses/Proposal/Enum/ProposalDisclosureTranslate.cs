using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Proposal
{
    [Table("enum_proposal_disclosure_translate", Schema = "propos")]
    [Index(nameof(OwnerId), nameof(LanguageId), nameof(ColumnName), Name = "ix_enum_proposal_disclosure_translate__owner_lang")]
    public partial class ProposalDisclosureTranslate : EnumTranslateEntity<ProposalDisclosureTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual ProposalDisclosure Owner { get; set; }
    }
}