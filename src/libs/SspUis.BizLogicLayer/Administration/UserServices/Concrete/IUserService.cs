using Microsoft.EntityFrameworkCore.Storage;
using SspUis.BizLogicLayer.Edoc.AppointmentOrderService;
using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Integration.MSPD.GSP;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.UserServices
{
    public interface IUserService : IStatusGeneric
    {
        PagedResult<UserListDto> GetList(UserSortFilterPageOptions dto);
        UserDto Get();
        UserDto Get(int id);
        PagedSelectList<int> AsSelectList(UserSortFilterPageOptions options);
        HaveId<int> Create(CreateUserDto dto);
        void Update(UpdateUserDlDto dto);
        //void Delete(int id);
        Task<CreateUserDto> GetByPassportData(GSPPersonInfoRequestForUserDto dto);
        Stream SaveAsExecel(UserSortFilterPageOptions dto);
        bool IsUserNameBusy(CheckUserNameDto dto);
        Task ImportJusticeUser(List<ImportUserDlDto> listDto);
        public void SyncEdocUser(IDbContextTransaction outTransaction = null);
        void UpdateUserForEdocSchema(User dto, AppointmentOrderDto? appointmentEmployee = null);
        AppointmentOrderDto AppointmentOrderMap(AppointEmployee dto, long employeeManageId);
    }
}
