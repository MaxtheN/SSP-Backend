using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_contact_type_translate", Schema = "my")]
    public partial class ContactTypeTranslate : TranslateEntity<ContactTypeTranslate, TranslateColumn>
    {
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ContactType.Translates))]
        public virtual ContactType Owner { get; set; }
    }
}
