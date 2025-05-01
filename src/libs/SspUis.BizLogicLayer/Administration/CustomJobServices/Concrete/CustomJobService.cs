using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.CustomJobServices;

public class CustomJobService : StatusGenericHandler, ICustomJobService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractorService _contractorService;
    private readonly ICustomJobRepository _repository;
    private readonly IAuthService _authService;
    private readonly ICustomJobRunnerFactory _customJobRunnerFactory;
    private readonly IConfiguration _configuration;
    private readonly SystemConf _systemConf;

    public CustomJobService(
        IUnitOfWork unitOfWork,
        IContractorService contractorService,
        ICustomJobRepository repository,
        IAuthService authService,
        ICustomJobRunnerFactory customJobRunnerFactory,
        SystemConf systemConf,
    IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _contractorService = contractorService;
        _repository = repository;
        _authService = authService;
        _customJobRunnerFactory = customJobRunnerFactory;
        _configuration = configuration;
        _systemConf = systemConf;
    }

    public HaveId<long> Create(CreateCustomJobDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {

            try
            {
                var entity = _repository.Create(dto);


                CombineStatuses(_repository);
                if (HasErrors)
                    return null;

                _unitOfWork.Save();

                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public CustomJobDto Get()
    {


        return new CustomJobDto
        {

        };
    }

    public CustomJobDto Get(long id)
    {
        var dto = GetQuery<CustomJobDto>().FirstOrDefault(a => a.Id == id);

        return dto;
    }

    public PagedResult<CustomJobListDto> GetList(CustomJobSortFilterOptions options)
    {
        var result = GetQuery<CustomJobListDto>()
                .SortFilter(options)
                .AsPagedResult(options);

        return result;
    }

    public HaveId<long> Update(UpdateCustomJobDlDto dto)
    {
        var entity = _repository.ById(dto.Id);
        if (entity == null)
        {
            AddError("Malumot topilmadi");
            return null;
        }


        using (var transaction = _unitOfWork.BeginTransaction())
        {
            _unitOfWork.Context.CustomJobs.Lock(dto.Id);

            try
            {
                _repository.Update(dto);
                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();

                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                UpdateStatus(new UpdateStatusCustomJobDto
                {
                    Id = entity.Id,
                }, StatusIdConst.MODIFIED);
                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }
    }

    public HaveId<long> UpdateStatus(UpdateStatusCustomJobDto dto, int statusId)
    {
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
        bool canCommit = _unitOfWork.CurrentTransaction == null;
        try
        {
            var updateDto = new UpdateStatusCustomJobDlDto()
            {
                Id = dto.Id,
                StatusId = statusId
            };

            if (HasErrors)
            {

                return null;
            }

            var entity = _repository.UpdateStatus(updateDto);
            CombineStatuses(_repository);

            if (HasErrors)
            {
                return null;
            }

            _unitOfWork.Save();
            if (canCommit)
            {
                transaction.Commit();
            }
            return HaveId.Create(entity.Id);

        }
        catch
        {
            transaction?.Rollback();
            throw;
        }
        finally
        {
            if (canCommit) { transaction.Dispose(); }
        }
    }

    public void Delete(long id)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {

            try
            {
                _unitOfWork.Context.CustomJobs.Lock(id);

                var entity = _repository.ById(id);
                if (entity.StatusId == StatusIdConst.EXECUTING)
                {
                    AddError("You can't delete while executing");
                    return;
                }
                UpdateStatus(new UpdateStatusCustomJobDto
                {
                    Id = entity.Id
                }, StatusIdConst.DELETED);

                if (HasErrors)
                    return;
                _unitOfWork.Save();
                CombineStatuses(_repository);
                if (HasErrors)
                    transaction.Rollback();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public async Task<HaveId<long>> Approve(UpdateStatusCustomJobDto dto)
    {
        CustomJob entity = null;
        entity = _repository.ById(dto.Id);
        if (entity == null)
            AddError("Malumot topilmadi");
        else if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
            AddError("Тасдиқлаш мумкин эмас / Невозможно утвердить");

        if (IsValid)
        {
            string apiUrl = _systemConf.IsLocalHost
                                    ? "http://localhost:5002/hangfire/customjob/Execute"
                                    : _systemConf.IsTest
                                            ? "http://localhost:5781/hangfire/customjob/Execute"
                                            : "http://192.168.1.3:5002/hangfire/customjob/Execute";

            CustomJobParameter dataToSend = new CustomJobParameter
            {
                CustomJobId = entity.Id,
                UserName = _authService.UserName,
                JobTypeId = entity.JobTypeId
            };

            string jsonData = JsonConvert.SerializeObject(dataToSend);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    entity.StatusId = StatusIdConst.WAITING;
                    entity.HangfireJobId = await response.Content.ReadAsStringAsync();
                    _unitOfWork.Save();
                    return HaveId.Create(entity.Id);
                }
                else
                    AddError(response.StatusCode.ToString());
            }
        }
        if (HasErrors)
        {
            AddError("Error exists");
            return null;
        }

        return HaveId.Create(entity.Id);
    }
    public async Task<HaveId<long>> Cancel(long id)
    {
        var entity = _repository.ById(id);
        if(entity == null)
        {
            AddError($"Ma'lumot toplimnadi id: {id}");
        }
        if(entity.StatusId != StatusIdConst.EXECUTING && entity.StatusId != StatusIdConst.WAITING && entity.HangfireJobId == null)
        {
            AddError("Bekor qilolmaysiz");
        }

        if (IsValid)
        {
            string apiUrl = _systemConf.IsLocalHost
                                    ? $"http://localhost:5002/hangfire/customjob/Cancel?jobId={entity.HangfireJobId}"
                                    : _systemConf.IsTest
                                            ? $"http://localhost:5781/hangfire/customjob/Cancel?jobId={entity.HangfireJobId}"
                                            : $"http://192.168.1.3:5002/hangfire/customjob/Cancel?jobId={entity.HangfireJobId}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(new { })));

                if (response.IsSuccessStatusCode)
                {
                    entity.StatusId = StatusIdConst.CANCELED;
                    _unitOfWork.Save();
                    return HaveId.Create(entity.Id);
                }
                else
                    AddError(response.StatusCode.ToString());
            }
        }
        if (HasErrors)
        {
            AddError("Error exists");
            return null;
        }

        return HaveId.Create(entity.Id);
    }

    private IQueryable<TDto> GetQuery<TDto>()
           where TDto : class
    {
        return _repository.ReadAsNoTracked<TDto>();
    }

    public PagedResult<CustomJobListDto> GetTable(CustomJobSortFilerByIdOptions options)
    {
        var result = GetQuery<CustomJobListDto>()
            .Where(s => s.Id == options.Id)
            .AsPagedResult(options);
        return result;
    }
}
