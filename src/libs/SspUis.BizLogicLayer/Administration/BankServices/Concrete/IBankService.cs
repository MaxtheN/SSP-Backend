using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.BankServices
{
    public interface IBankService
        : IBaseEntityService<Bank, BankListDto, BankDto, CreateBankDlDto, UpdateBankDlDto>
    {
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateBankDlDto dto);
        void Update(UpdateBankDlDto dto);
        void SyncEdocBank();
    }
}
