using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Ssp.DataLayer.EFClasses.Edoc;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DigitizationCenter;
using StatusGeneric;
using WEBASE;
using WEBASE.Security;

namespace SspUis.BizLogicLayer.IntegrationServices.Xodim
{
    public class WorkActivity : StatusGenericHandler, IWorkActivity
    {
        private readonly IDigitizationCenterMehnatService _digitizationCenterMehnatService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Core.Security.IAuthService _authService;

        public WorkActivity(IEmployeeService employeeService,
            IUnitOfWork unitOfWork,
            IDigitizationCenterLoginService digitizationCenterService,
            IDigitizationCenterMehnatService digitizationCenterMehnatService,
            Core.Security.IAuthService authService)
        {
            _repository = unitOfWork.EmployeeRepository;
            _employeeService = employeeService;
            _unitOfWork = unitOfWork;
            _digitizationCenterMehnatService = digitizationCenterMehnatService;
            _authService = authService;
        }

        public int GetXodimDCount()
        {
            _authService.ResetUserName("webaseadmin");
            //var allEmployees = _repository.AllAsQueryable.Where(t=> t.StateId == StateIdConst.ACTIVE).Include(p=>p.Person);
            var allEmployees = _repository.AllAsQueryable.Where(t => t.StateId == StateIdConst.ACTIVE && t.PlaceOfWorks.Count() == 0)  // Фильтр по активному состоянию, если нужно
            .Include(p => p.Person)
            .GroupBy(t => t.PersonId)
            .Where(g => g.Count() == 1)// Фильтрация по количеству элементов в группе
            .Select(g => g.First())  // Выбор первого элемента из каждой группы
            .ToList();

            return allEmployees.Count;
        }


        public async Task UpdateWorkActivity()
        {

            _authService.ResetUserName("webaseadmin");
            //var allEmployees = _repository.AllAsQueryable.Where(t=> t.StateId == StateIdConst.ACTIVE).Include(p=>p.Person);
            var allEmployees = _repository.AllAsQueryable.Where(t => t.StateId == StateIdConst.ACTIVE && t.PlaceOfWorks.Count() == 0)  // Фильтр по активному состоянию, если нужно
            .Include(p => p.Person)
            .GroupBy(t => t.PersonId)
            .Where(g => g.Count() == 1)  // Фильтрация по количеству элементов в группе
            .Select(g => g.First())  // Выбор первого элемента из каждой группы
            .ToList();

            foreach (var item in allEmployees)
            {
                var person = item.Person;

                if (person == null) continue; 
                var dto = new WorkPositionHistoryRequestDto()
                {
                    pin = person.Pinfl
                };

                var maybeData = _digitizationCenterMehnatService.GetMehnatHistory(dto).Result;
                if (maybeData == null) 
                {
                    continue;
                }  

                List<Experience> data = maybeData.Experiences;


                UpdateEmployeeDlDto updateEmployeeDlDto = new UpdateEmployeeDlDto()
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    PersonId = person.Id,
                    PhoneNumber = item.PhoneNumber,
                    StateId = item.StateId,
                    HasMilitary = item.HasMilitary,
                    
                    Person = new UpdatePersonDlDto()
                    {
                        Id = person.Id,
                        BirthCountryId = person.BirthCountryId,
                        BirthDate = person.BirthDate,
                        BirthDistrictId = person.BirthDistrictId,
                        CitizenshipId = person.CitizenshipId,
                        GenderId = person.GenderId,
                        Inn = person.Inn,
                        LivingDistrictId = person.LivingDistrictId,
                        LivingRegionId = person.LivingRegionId,
                        NameEng = person.NameEng,
                        NameLatin = person.NameLatin,
                        NationalityId = person.NationalityId,
                        PassportDate = person.PassportDate,
                        PassportDivName = person.PassportDivName,
                        PassportExpiration = person.PassportExpiration,
                        PassportNumber = person.PassportNumber,
                        PassportSeria = person.PassportSeria,
                        PatronymLatin = person.PatronymLatin,
                        PictureId = person.PictureId,
                        Pinfl = person.Pinfl,
                        StateId = person.StateId,
                        SurnameEng = person.SurnameEng,
                        SurnameLatin = person.SurnameLatin
                    },
                    PlaceOfWorks = new List<EmployeePlaceOfWorkDlDto>()

                };

                foreach (var experience in data)
                {
                    EmployeePlaceOfWorkDlDto placeOfWork = new EmployeePlaceOfWorkDlDto()
                    {  
                        
                        AdditionId = experience.ActionTypeId,
                        ContractorName = experience.CompanyName,
                        EndOn = experience.EndDate == null ? null : DateOnly.Parse(experience.EndDate.ToString()),
                        PositionName = experience.PositionName,
                        StartOn = DateOnly.Parse(experience.StartDate),
                        EmploymentTypeId = experience.WorkType == null ? 1 : experience.WorkType.Value

                    };

                    updateEmployeeDlDto.PlaceOfWorks.Add(placeOfWork);
                }

                await _employeeService.Update(updateEmployeeDlDto, true);
            }
        }

     
    }
}
