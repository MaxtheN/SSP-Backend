using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class QuestionnaireAnswerlDto
        : EntityDto<QuestionnaireAnswerlDto, QuestionnaireAnswer>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long AnswerId { get; set; }
        public int StateId { get; set; }

        public override QuestionnaireAnswer CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            return entity;
        }

        public override void UpdateEntity(QuestionnaireAnswer entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
