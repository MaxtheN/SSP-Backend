using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.EfClasses
{
    public abstract class TranslateEntity<TTranslateEntity, TTranslateColumn> : TranslateEntity<TTranslateEntity, TTranslateColumn, int>
        where TTranslateEntity : TranslateEntity<TTranslateEntity, TTranslateColumn>
        where TTranslateColumn : struct
    {

    }

    public abstract class TranslateEntity<TTranslateEntity, TTranslateColumn, TOwnerId> : EnumTranslateEntity<TTranslateEntity, TTranslateColumn, TOwnerId>
        where TTranslateEntity : TranslateEntity<TTranslateEntity, TTranslateColumn, TOwnerId>
        where TTranslateColumn : struct
    {
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
    }
}
