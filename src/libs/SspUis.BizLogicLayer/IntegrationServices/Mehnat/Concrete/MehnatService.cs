using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Hrm;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;

namespace SspUis.BizLogicLayer.IntegrationServices
{
    public class MehnatService : StatusGenericHandler, IMehnatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeWorkScheduleService _employeeWorkScheduleService;
        private readonly IEmployeeRepository _repository;
        private readonly IEmployeeService _employeeService;
        private readonly Core.Security.IAuthService _authService;

        public MehnatService(IUnitOfWork unitOfWork, IEmployeeWorkScheduleService employeeWorkScheduleService, IEmployeeService employeeService, Core.Security.IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _employeeWorkScheduleService = employeeWorkScheduleService;
            _repository = unitOfWork.EmployeeRepository;
            _employeeService = employeeService;
            _authService = authService;
        }

        public void UpdateEmployeesWorkData()
        {
            _authService.ResetUserName("webaseadmin");
            // var employees = _employeeService.GetListMethod(new EmployeeSortFilterPageOptions()).Take(10);
            var employees = _unitOfWork.EmployeeRepository.Context.Set<Employee>().Include(s => s.Person).ToList();
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                if (employees == null)
                {
                    transaction.Rollback();
                }
                else
                {

                    foreach (var item in employees)
                    {

                        try
                        {
                           EmployeeWorkScheduleDto worker = _employeeWorkScheduleService.GetTotalWorkYears(new EmployeeWorkScheduleFilterOption
                            {
                                Pinfl = item.Person.Pinfl,
                                EmployeeId = 0
                            });

                            item.MehnatWorkedYear = worker.MehnatYear;
                            item.MehnatWorkedMonth = worker.MehnatMonth;
                            item.MehnatWorkedDay = worker.MehnatDay;


                            _unitOfWork.Save();
                            _unitOfWork.Commit();

                        }
                        catch (Exception)
                        {
                            continue;
                        }


                    }
                }
            }
        }
    }
}
