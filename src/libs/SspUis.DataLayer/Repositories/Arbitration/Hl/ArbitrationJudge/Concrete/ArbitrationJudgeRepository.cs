using System.Linq;
using GenericServices;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ArbitrationJudgeRepository
        : BaseEntityRepository<
            int,
            ArbitrationJudge,
            CreateArbitrationJudgeDlDto,
            UpdateArbitrationJudgeDlDto>, IArbitrationJudgeRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public ArbitrationJudgeRepository(
            ICrudServices crudServices,
            IAuthService authService,
            IUnitOfWork unitOfWork)
            : base(crudServices)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        protected override void OnCreate(ArbitrationJudge entity, CreateArbitrationJudgeDlDto dto)
        {
            //base.OnCreate(entity, dto);
            var person = _unitOfWork.PersonRepository.ById(dto.PersonId.Value);
            entity.FirstName = person.NameLatin;
            entity.LastName = person.SurnameLatin;
            entity.MiddleName = person.PatronymLatin;
            entity.BirthDate = person.BirthDate;
        }
        protected override IQueryable<ArbitrationJudge> InjectFilter(IQueryable<ArbitrationJudge> query)
        {
            return base.InjectFilter(query).Where(x => x.StateId == StateIdConst.ACTIVE);
        }

    }
}
