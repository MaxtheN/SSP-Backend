using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using Ssp.DataLayer.EFClasses.Edoc;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using StatusGeneric;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Edoc.AppointmentOrderService
{
    public class AppointmentOrderService : StatusGenericHandler, IAppointmentOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public AppointmentOrderService(IUnitOfWork unitOfWork,
            IAuthService authService)
        {
            this._unitOfWork = unitOfWork;
            this._authService = authService;
        }

        public void Create(User dto, AppointmentOrderDto appointDto)
        {
            var appointmentOrder = new AppointmentOrder
            {
                CreatedUserId = 1,
                OrganizationId = appointDto.OrganizationId,
                StatusId = StatusIdConst.ACCEPTED,
                Date = DateTime.Today,
                Details = "received from SSP",
                Number = appointDto.DocNumber.Length > 14
                    ? $"SSP - {appointDto.DocNumber.Substring(0, 14)}" 
                    : $"SSP - {appointDto.DocNumber}",
                DateOfCreated = DateTime.Now,
            };
            appointmentOrder.Appointments.Add(new Appointment
            {
                Date = appointDto.StartOn,
                OrderNumber = 1,
                AppointmentTypeId = appointDto.AppointmentTypeId,
                DepartmentId = appointDto.DepartmentId,
                PositionId = appointDto.PositionId,
                Category = null,
                Details = "received from SSP",
                CreatedUserId = 1,
                EmployeeId = appointDto.EmployeeId,
                Rate = appointDto.EmployeeRate,
                DateOfCreated = DateTime.Now,
                EndDate = appointDto.AppointmentTypeId == EmpAppointOrderTypeIdConst.DISMISSAL ? appointDto.StartOn : null,
            });
            var employee = _unitOfWork.Context.Set<Employee>().FirstOrDefault(x => x.Id == appointDto.EmployeeId);
            if (employee != null)
            {
                employee.UserId = dto.Id;
                _unitOfWork.Context.Set<Employee>().Update(employee);
            }
            _unitOfWork.Context.Set<AppointmentOrder>().Add(appointmentOrder);
        }

        public void Update(User dto, AppointmentOrderDto newDto)
        {
            var appointmentOrder = _unitOfWork.Context.Set<AppointmentOrder>()
                .Include(x => x.Appointments).ThenInclude(x => x.Employee).ThenInclude(x => x.User)
                .FirstOrDefault(x => x.Appointments.Any(a => !a.EndDate.HasValue && a.Employee.User.Id == dto.Id && a.AppointmentTypeId == EmpAppointOrderTypeIdConst.HIRE)
                                     && x.StatusId == StatusIdConst.ACCEPTED);

            if (newDto.AppointmentTypeId != EmpAppointOrderTypeIdConst.DISMISSAL)
            {
                if (appointmentOrder != null)
                {
                    var appointment = appointmentOrder.Appointments.First();
                    appointmentOrder.OrganizationId = newDto.OrganizationId;
                    appointmentOrder.StatusId = newDto.StatusId;
                    appointmentOrder.Date = newDto.DocOn;
                    appointmentOrder.Details = "received from SSP";
                    appointmentOrder.Number = $"SSP - {(newDto.DocNumber.Length > 14 ? newDto.DocNumber.Substring(0, 14) : newDto.DocNumber)}";
                    appointmentOrder.ModifiedUserId = 1;
                    appointmentOrder.DateOfModified = DateTime.Now;

                    appointment.Date = newDto.StartOn;
                    appointment.AppointmentTypeId = EmpAppointOrderTypeIdConst.HIRE;
                    appointment.DepartmentId = newDto.DepartmentId;
                    appointment.PositionId = newDto.PositionId;
                    appointment.Category = null;
                    appointment.Employee.UserId = dto.Id;
                    appointment.EmployeeId = newDto.EmployeeId;
                    appointment.Rate = newDto.EmployeeRate;
                    appointment.DateOfModified = DateTime.Now;
                    var employee = _unitOfWork.Context.Set<Employee>().FirstOrDefault(x => x.Id == appointment.EmployeeId);
                    if (employee != null && appointment.EmployeeId != newDto.EmployeeId)
                    {
                        employee.UserId = null;
                        _unitOfWork.Context.Set<Employee>().Update(employee);
                    }
                    _unitOfWork.Context.Set<AppointmentOrder>().Update(appointmentOrder);
                }
                else
                {
                    Create(dto, newDto);
                }
            }
            else
            {
                appointmentOrder = _unitOfWork.Context.Set<AppointmentOrder>()
                        .Include(x => x.Appointments).ThenInclude(x => x.Employee).ThenInclude(x => x.User)
                        .OrderByDescending(a => a.Id)
                        .FirstOrDefault(x => x.Appointments.Any(a => a.Employee.User.Id == dto.Id
                                && a.AppointmentTypeId == EmpAppointOrderTypeIdConst.HIRE
                            )
                            && x.StatusId == StatusIdConst.ACCEPTED
                        );

                if (newDto.StatusId == StatusIdConst.ACCEPTED)
                {
                    if (appointmentOrder != null)
                    {
                        var appointment = appointmentOrder.Appointments.First();
                        appointment.EndDate = newDto.StartOn;
                        _unitOfWork.Context.Set<AppointmentOrder>().Update(appointmentOrder);
                    }
                    Create(dto, newDto);
                }
                else
                {
                    if (appointmentOrder != null)
                    {
                        var appointment = appointmentOrder.Appointments.First();
                        appointment.EndDate = null;
                        _unitOfWork.Context.Set<AppointmentOrder>().Update(appointmentOrder);
                    }

                    appointmentOrder = _unitOfWork.Context.Set<AppointmentOrder>()
                        .Include(x => x.Appointments).ThenInclude(x => x.Employee).ThenInclude(x => x.User)
                        .FirstOrDefault(x => x.Appointments.Any(a => a.Employee.UserId == dto.Id
                            && a.AppointmentTypeId == EmpAppointOrderTypeIdConst.DISMISSAL)
                            && x.StatusId == StatusIdConst.ACCEPTED);
                    if (appointmentOrder != null)
                    {
                        appointmentOrder.StatusId = StatusIdConst.DELETED;
                        _unitOfWork.Context.Set<AppointmentOrder>().Update(appointmentOrder);
                    }
                }
            }
        }

        public void Delete(User dto)
        {
            var appointmentOrder = _unitOfWork.Context.Set<AppointmentOrder>()
                .Include(x => x.Appointments).ThenInclude(x => x.Employee).ThenInclude(x => x.User)
                .FirstOrDefault(x => x.Appointments.Any(a => !a.EndDate.HasValue &&
                                                              a.Employee.User.Id == dto.Id &&
                                                              a.AppointmentTypeId == EmpAppointOrderTypeIdConst.HIRE) &&
                                     x.StatusId == StatusIdConst.ACCEPTED);
            if (appointmentOrder != null)
            {
                var table = appointmentOrder.Appointments.First(x=> x.Employee.User.Id == dto.Id);
                var employee = _unitOfWork.Context.Set<Employee>().FirstOrDefault(x => x.Id == table.EmployeeId);
                if (employee != null)
                {
                    employee.UserId = null;
                    appointmentOrder.Appointments.First(x => x.Employee.User.Id == dto.Id).EndDate = DateTime.Now;
                    _unitOfWork.Context.Set<Employee>().Update(employee);
                }
                appointmentOrder.StatusId = StatusIdConst.DELETED;
                _unitOfWork.Context.Set<AppointmentOrder>().Update(appointmentOrder);
            }
        }

    }
}
