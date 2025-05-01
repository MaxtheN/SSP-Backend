using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using Quartz;

namespace WEBASE.QuartzForHrm.Services
{
    public class AppointEmployeeJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public AppointEmployeeJob(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _authService.ResetUserName("admin");
            var appointEmployees = _unitOfWork.Context.AppointEmployeeTables.ToList();

            foreach (var appointEmployee in appointEmployees)
            {
                if (appointEmployee.EndOn == DateOnly.FromDateTime(DateTime.Today) && appointEmployee.Owner.StatusId == StatusIdConst.ACCEPTED && appointEmployee.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE)
                {
                    var employeeManage = _unitOfWork.Context.EmployeeManages.FirstOrDefault(a => a.Id == appointEmployee.EmployeeManageId);

                    employeeManage.EndOn = DateOnly.FromDateTime(DateTime.Today);
                    employeeManage.IsDeleted = true;

                    _unitOfWork.Save();
                }

            }
        }
    }
}