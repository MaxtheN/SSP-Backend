using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class TranslateDto<TTranslateDto, TEntity, TTranslateColumn> : EntityDto<TTranslateDto, TEntity>, IHaveUniqueForeignKey
        where TTranslateDto : EntityDto<TTranslateDto, TEntity>
        where TEntity : class
        where TTranslateColumn : struct
    {
        [LocalizedRequired]
        public virtual TTranslateColumn ColumnName { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int LanguageId { get; set; }
        [LocalizedRequired]
        public string TranslateText { get; set; } = string.Empty;

        public object GetUniqueForeignKey() => $"{ColumnName}_{LanguageId}";
    }
}
