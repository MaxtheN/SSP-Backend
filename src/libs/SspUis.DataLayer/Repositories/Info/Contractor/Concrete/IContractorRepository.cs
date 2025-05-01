using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IContractorRepository : IBaseEntityRepository<long, Contractor, CreateContractorDlDto, UpdateContractorDlDto>
    {
        Contractor ByInn(string inn);
        Contractor ByPinfl(string pinfl);
        Contractor Update(MyUpdateContractorDlDto updateDto, Action<Contractor> validation = null);
        ContractorContact AddContactInfo(long ownerId, int contactTypeId, string contact, Action<ContractorContact> validation);
        ContractorOffers CreateContractorOffers(ContractorOfferDto dto, ContractorOffers entity);
        void UpdateContractorSettlementAccount(UpdateContractorSettlementAccountDlDto dto);
        void ChangeMainSettlementAccounting(long contartorId, long settlementAccountId);
    }
}
