using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Salary.Core.Model;
using WEBASE.Salary.Core;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindService : StatusGenericHandler, ICalculationKindService
    {
        private readonly ICalculationKindRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public CalculationKindService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.CalculationKindRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<CalculationKindListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<CalculationKindListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public CalculationKindDto Get()
        {
            return new CalculationKindDto();
        }

        public CalculationKindDto Get(int id)
        {
            var dto = _repository.ById<CalculationKindDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(int? workScheduleKindId = null)
        {
            return _repository.AllAsQueryable
                .AsSelectList(workScheduleKindId);
        }

        public HaveId<int> Create(CreateCalculationKindDlDto dto)
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdateCalculationKindDlDto dto)
        {
            _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                CombineStatuses(_repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }

        private void Validation<TDto>(CalculationKindDlDto<TDto> dto, CalculationKind entity)
            where TDto : CalculationKindDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }

        public List<CalculationKindCore> GetAllCalculationKindCore()
        {
            var calculationKinds = _repository.ReadAsNoTracked<CalculationKindDto>();
            var calculationKindCores = new List<CalculationKindCore>();

            foreach (var calculationKind in calculationKinds)
            {
                calculationKindCores.Add(new CalculationKindCore()
                {
                    ID = calculationKind.Id,
                    Name = calculationKind.ShortName,
                    itemofexpenseid = calculationKind.ItemOfExpenseId,
                    itemofexpensename = calculationKind.ItemOfExpenseCode,
                    CalcType = SalaryCalculationCore.ToCalculationType(calculationKind.CalculationTypeId),
                    CalcMethod = SalaryCalculationCore.ToCalculationMethod(calculationKind.CalculationMethodId.GetValueOrDefault(1)),
                    MinimumValueType = SalaryCalculationCore.ToMinimumValueTypeCore(calculationKind.MinimumValueTypeId),
                    CalcByTimeType = SalaryCalculationCore.ToCalculateByTimeTypeCore(calculationKind.CalculateByTimeTypeId.GetValueOrDefault(1)),
                    ByEnrolment = calculationKind.ByEnrolment,
                    DependOnRate = calculationKind.DependOnRate,
                    UsedCalculationKinds = calculationKind.UsedTables
                        .Select(bd => new CalculationKindCore.UsedCalculationKind()
                        {
                            CalculationKindID = bd.FormedCalculationKindId,
                            CalcFromInSum = bd.CalcFromInSum,
                            QuantityOfMinValue = bd.QuantityOfMinimumValue.GetValueOrDefault(0),
                            MinimumValueType = SalaryCalculationCore.ToMinimumValueTypeCore(bd.MinimumValueTypeId)
                        }).ToList(),
                    Percentage = calculationKind.Percents.OrderByDescending(x => x.DateOn).FirstOrDefault()?.PercentRate
                });
            }

            return calculationKindCores;
        }
        
        public CalculationKindDto GetByMethod(int calculationMethodId)
        {
            var dto = _repository.ReadAsNoTracked<CalculationKindDto>()
                .FirstOrDefault(a => a.CalculationMethodId == calculationMethodId);
            CombineStatuses(_repository);
            return dto;
        }
    }
}
