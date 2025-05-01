using GenericServices;
using SspUis.Cor;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class DocumentChatRepository 
        : BaseEntityRepository<long, DocumentChat, CreateDocumentChatDlDto, UpdateDocumentChatDlDto>,
        IDocumentChatRepository
    {
        private readonly IAuthService _authService;
        public DocumentChatRepository(ICrudServices crudServices, IAuthService authService)
            : base(crudServices)
        {
            this._authService = authService;
        }

        protected override void OnCreate(DocumentChat entity, CreateDocumentChatDlDto dto)
        {
            if (_authService.Contractor != null)
            {
                entity.ContractorId = _authService.Contractor.Id;
                entity.AppId = AppIdConst.MY;
            }
            else
            {
                entity.OrganizationId = _authService.User.OrganizationId;
                entity.UserId = _authService.User.Id;
                entity.AppId = AppIdConst.ERP;
            }

            base.OnCreate(entity, dto);
        }

        protected override IQueryable<DocumentChat> InjectFilter(IQueryable<DocumentChat> query)
        {
            return query.Where(a => a.StateId != StateIdConst.PASSIVE);
        }
    }
}