using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.Appeal;
using SspUis.BizLogicLayer.Claim;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.Memship;
using SspUis.Core;
using SspUis.Core.Extensions;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Billing.Services;
using StatusGeneric;
using System;
using System.Linq;
using System.Threading.Tasks;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;

namespace SspUis.BizLogicLayer.Doc.SignDocumentManage;

public class SignDocumentManageService : StatusGenericHandler, ISignDocumentManageService
{
    private readonly IApplicationForCourtService _applicationForCourtService;
    private IUnitOfWork _unitOfWork;
    private readonly IWbImzoService _wbImzoService;
    private readonly IAppointEmployeeService _employeeService;
    private readonly IChastisementService _chastisementService;
    private readonly ITempCalcKindService _tempCalcKindService;
    private readonly IEmployeeLeaveOrderService _employeeLeaveOrderService;
    private readonly IRecallLeaveService _recallLeaveService;
    private readonly IEmployeeSendStudyService _employeeSendStudyService;
    private readonly IWorkDayOffService _workdayOfService;
    private readonly IEmployeeSendTrainService _employeeSendTrainService;
    private readonly IMemshipContractService _memshipContractService;
    private readonly ISrvContractService _srvContractService;
    private readonly ISrvCompleteService _srvCompleteService;
    private readonly ISrvDeedService _srvDeedService;
    private readonly IAppealApplicationService _appealApplication;
    private readonly IDualContractService _dualContractService;
    private readonly IBillingService _billingService;

    public SignDocumentManageService(IUnitOfWork unitOfWork,
                                     IWbImzoService wbImzoService,
                                     IMemshipContractService memshipContractService,
                                     ISrvContractService srvContractService,
                                     ISrvCompleteService srvCompleteService,
                                     IAppointEmployeeService employeeService,
                                     IAppealApplicationService appealApplicationService,
                                     IBillingService billingService,
                                     IDualContractService dualContractService,
                                     IChastisementService chastisementService,
                                     ITempCalcKindService tempCalcKindService,
                                     IRecallLeaveService recallLeaceService,
                                     IWorkDayOffService workDayOffService,
                                     IEmployeeSendStudyService employeeSendStudyService,
                                     IEmployeeSendTrainService employeeSendTrainService,
                                     IApplicationForCourtService applicationForCourtService,
                                     ISrvDeedService srvDeedService,
                                     IEmployeeLeaveOrderService employeeLeaveOrderService)
    {
        _applicationForCourtService = applicationForCourtService;
        _unitOfWork = unitOfWork;
        _wbImzoService = wbImzoService;
        _employeeService = employeeService;
        _chastisementService = chastisementService;
        _tempCalcKindService = tempCalcKindService;
        _employeeLeaveOrderService = employeeLeaveOrderService;
        _recallLeaveService = recallLeaceService;
        _employeeSendStudyService = employeeSendStudyService;
        _workdayOfService = workDayOffService;
        _employeeSendTrainService = employeeSendTrainService;
        _memshipContractService = memshipContractService;
        _srvContractService = srvContractService;
        _srvCompleteService = srvCompleteService;
        _srvDeedService = srvDeedService;
        _appealApplication = appealApplicationService;
        _dualContractService = dualContractService;
        _billingService = billingService;
    }
    public async Task<WbImzoSignInformResponseDto> PostSignResponse2(WbImzoSignInformDto dto)
    {
        WbImzoSignInformResponseDto resultDto = new();

        ApiResult<WbImzoSignRequestDto> signResult = await _wbImzoService.GetByRequestIdAsync(dto);
        if (!signResult.IsSuccess || signResult.Response is null)
            CombineStatuses(signResult.GetStatusGeneric());

        if (IsValid && signResult.Response is not null)
        {
            var signedPerson = signResult.Response.SignRequestUsers
                    .FirstOrDefault(a => a.SignatureMethodId == SignatureMethodIdConst.E_IMZO ?
                                         dto.UserKey == a.UserKey : dto.UserKey == a.UserPhoneNumber.NormalizePhoneNumber() &&
                                         a.IsSigned == true);

            if (signedPerson != null && !signedPerson.ActionTypeId.HasValue)
            {
                AddError("Imzolovchi topilmadi");
                resultDto.ErrorMessage = "Error";
                resultDto.ErrorCode = "002";
                resultDto.Comment = String.Join(" ", _errors);

                return resultDto;
            }

            if (signResult.Response.TableId == TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT)
            {
                #region A'zolik Shartnomasi
                var Contract = _unitOfWork.Context.Set<MemshipContract>().Include(s => s.Status).FirstOrDefault(s => s.Id == signResult.Response.DocumentId && s.StatusId != StatusIdConst.DELETED);

                if (Contract == null)
                {
                    AddError("A'zolik shartnomasi topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                Contract.StatusId = signedPerson.DocStatusId;

                var DataFile = _memshipContractService.SaveFile(Contract.Id, JsonConvert.SerializeObject(Contract), "data.txt");

                Contract.Signs.Add(new()
                {
                    OwnerId = Contract.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = Contract.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });


                _unitOfWork.Save();

                _memshipContractService.CreateDocumentChangeLog(Contract.Id,$"A'zolik shartnomasi {signedPerson.UserInfo} tomonidan imzolandi");
                CombineStatuses(_memshipContractService);
                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_SERVICE_CONTRACT)
            {
                #region Xizmatlar Shartnomasi

                var Contract = _unitOfWork.Context.Set<ServiceContract>()
                                                               .Include(a => a.Application).ThenInclude(a => a.ServiceApplication)
                                                               .Include(a => a.Groups).ThenInclude(a => a.Tables)
                                                               .FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey
                                                                && s.StatusId != StatusIdConst.DELETED);
                if (Contract == null)
                {
                    AddError("Shartnoma topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                Contract.StatusId = signedPerson.DocStatusId;
                var DataFile = _srvContractService.SaveFile(Contract.Id, JsonConvert.SerializeObject(Contract), "data.txt");

                Contract.Signs.Add(new()
                {
                    OwnerId = Contract.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = Contract.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    foreach (var group in Contract.Groups)
                    {
                        foreach (var table in group.Tables)
                        {
                            var complete = _srvCompleteService.Create(new CreateCompletedServiceDlDto
                            {
                                DocNumber = Contract.DocNumber,
                                DocOn = DateOnly.FromDateTime(DateTime.Now),
                                ContractorId = Contract.ContractorId,
                                ServiceContractId = Contract.Id,
                                //EmployeeManageId = _authService.User.EmployeeManageId,
                                ServiceApplicationId = Contract.Application.ServiceApplication.Id,
                            });
                        }
                    }
                }

                _unitOfWork.Save();
                _srvContractService.CreateDocumentChangeLog(Contract.Id,$"Xizmatlar shartnomasi {signedPerson.UserInfo} tomonidan imzolandi");

                CombineStatuses(_srvContractService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion 
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_SERVICE_DEED)
            {
                #region Xizmatlar Dalolatnomasi
                var deed = _unitOfWork.Context.Set<ServiceDeed>()
                                                        .Include(a => a.Groups).ThenInclude(a => a.Tables)
                                                        .Include(a => a.SrvContract)
                                                        .FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (deed == null)
                {
                    AddError("Dalolatnoma topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                deed.StatusId = signedPerson.DocStatusId;

                var DataFile = _srvDeedService.SaveFile(deed.Id, JsonConvert.SerializeObject(deed), "data.txt");

                deed.Signs.Add(new()
                {
                    OwnerId = deed.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = deed.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });

                foreach (var group in deed.Groups)
                {
                    foreach (var table in group.Tables)
                    {
                        var complete = _srvCompleteService.Create(new CreateCompletedServiceDlDto
                        {
                            DocNumber = deed.DocNumber,
                            DocOn = DateOnly.FromDateTime(DateTime.Now),
                            ContractorId = deed.Id,
                            ServiceContractId = deed.SrvContractorId,
                            //EmployeeManageId = _authService.User.EmployeeManageId,
                            ServiceApplicationId = deed.Application.ServiceApplication.Id,
                        });
                    }
                }

                _srvDeedService.CreateDocumentChangeLog(deed.Id, $"Xizmatlar dalolatnomasi {signedPerson.UserInfo} tomonidan imzolandi");
                _unitOfWork.Save();

                CombineStatuses(_srvContractService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.APPEAL__DOC_APPEAL_APPLICATION)
            {
                #region Murojat arizasi
                var application = _unitOfWork.Context.Set<AppealApplication>()
                                                                   .FirstOrDefault(s => s.Id == (int)signResult.Response.DocumentId && s.StatusId != StatusIdConst.DELETED);
                if (application == null)
                {
                    AddError("Ariza topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                application.StatusId = signedPerson.DocStatusId;
                var DataFile = _appealApplication.SaveFile(application.Id, JsonConvert.SerializeObject(application), "data.txt");

                application.Signs.Add(new()
                {
                    OwnerId = application.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = application.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });


                _unitOfWork.Save();
                _appealApplication.CreateDocumentChangeLog(application.Id, signedPerson.DocStatusId);

                CombineStatuses(_appealApplication);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;

                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_DUAL_CONTRACT)
            {
                #region Dual ta'lim shartnomasi

                var dual = _unitOfWork.Context.Set<DualContract>()
                                                        .FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey
                                                         && s.StatusId != StatusIdConst.DELETED);

                if (dual == null)
                {
                    AddError("Dual ta'lim shartnomasi topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }


                dual.StatusId = signedPerson.DocStatusId;
                var DataFile = _dualContractService.SaveFile(dual.Id, JsonConvert.SerializeObject(dual), "data.txt");

                dual.Signs.Add(new()
                {
                    OwnerId = dual.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = dual.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });

                _dualContractService.CreateDocumentChangeLog(dual.Id, signedPerson.DocStatusId);
                _unitOfWork.Save();

                CombineStatuses(_dualContractService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                if (IsValid)
                {
                    if (dual.ExternalId != null)
                    { await _billingService.HeldByContractor((int)dual.ExternalId); }
                }

                CombineStatuses(_billingService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DUALEDU__DOC_DUAL_APPLICATION)
            {
                #region Dual Arizasi 

                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.CLAIM__DOC_ARBITRATION_APPLICATION)
            {
                #region Hakamlik arizasi

                #endregion
            }
            else if (signResult.Response.TableId == TableIdConst.DOC_APPOINT_EMPLOYEE)
            {
                #region Kadrlar hisobi Shaxsiy tarkib buyuruqlarini imzolash
                var contract = _unitOfWork.Context.Set<AppointEmployee>()
                                                               .FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey
                                                                && s.StatusId != StatusIdConst.DELETED);


                if (signedPerson.DocStatusId == 0 || contract == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                UpdateStatusAppointEmployeeDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                contract.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();
                _employeeService.CreateDocumentChangeLog(contract.Id, signedPerson.DocStatusId);

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    await _employeeService.AcceptAsync(updateStatus);
                }

                CombineStatuses(_employeeService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;

                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.HRM__DOC_CHASTISEMENT)
            {
                #region Kadrlar hisobi Intizomiy jazo qollash
                var contract = _unitOfWork.Context.Set<Chastisement>().FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || contract == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                UpdateStatusChastisementDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;
                contract.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();

                _chastisementService.CreateDocumentChangeLog(contract.Id, signedPerson.DocStatusId);

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    _chastisementService.Accept(updateStatus);
                }

                CombineStatuses(_chastisementService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }
            else if (signResult.Response.TableId == TableIdConst.DOC_TEMP_CALC_KIND)
            {
                #region Kadrlar hisobi Bir martalik tolovlar berish to'g'risidagi buyruq
                var contract = _unitOfWork.Context.Set<TempCalcKind>().FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || contract == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                UpdateStatusTempCalcKindDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                _tempCalcKindService.CreateDocumentChangeLog(contract.Id, signedPerson.DocStatusId);
                contract.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    _tempCalcKindService.Accept(updateStatus);
                }

                CombineStatuses(_tempCalcKindService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }
            else if (signResult.Response.TableId == TableIdConst.HRM__DOC_ORDER_TO_SEND_TO_BUSINESS_TRIP)
            {
                #region Xizmat safariga yuborish buyuruqlarini imzolash
                var contract = _unitOfWork.Context.Set<OrderToSendBusinessTrip>().FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || contract == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                UpdateStatusTempCalcKindDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;


                contract.StatusId = updateStatus.StatusId;

                _unitOfWork.Save(); ///errorr

                _tempCalcKindService.CreateDocumentChangeLog(contract.Id, signedPerson.DocStatusId);

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    _tempCalcKindService.Accept(updateStatus);
                }

                CombineStatuses(_tempCalcKindService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }
            else if (signResult.Response.TableId == TableIdConst.DOC_EMPLOYEE_LEAVE_ORDER)
            {
                #region Ta'tilga yuborish buyruqlarini imzolash , Bola parvarishlash, Xomiladorlik va tug'ish ta'til buyuruqlarini imzolash
                var employeeLeaveOrder = _unitOfWork.Context.Set<EmployeeLeaveOrder>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || employeeLeaveOrder == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                UpdateStatusEmployeeLeaveOrderDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                employeeLeaveOrder.StatusId = updateStatus.StatusId;
                _unitOfWork.Save();

                _employeeLeaveOrderService.CreateDocumentChangeLog(employeeLeaveOrder.Id, signedPerson.DocStatusId);


                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                    _employeeLeaveOrderService.Accept(updateStatus);

                CombineStatuses(_employeeLeaveOrderService);
                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_RECALL_LEAVE)
            {
                #region Mexnat ta'tilidan yoki o'z hisobidan ta'tildan chaqirib olish buyruq
                var recallLeave = _unitOfWork.Context.Set<RecallLeave>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || recallLeave == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                UpdateStatusRecallLeaveDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                recallLeave.StatusId = updateStatus.StatusId;
                _unitOfWork.Save();

                _recallLeaveService.CreateDocumentChangeLog(recallLeave.Id, signedPerson.DocStatusId);



                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                    _recallLeaveService.Accept(updateStatus);

                CombineStatuses(_recallLeaveService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_EMPLOYEE_SEND_STUDY)
            {
                #region O'quv ta'tili buyuruqlari
                var employeeSendStudy = _unitOfWork.Context.Set<EmployeeSendStudy>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || employeeSendStudy == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }


                UpdateStatusEmployeeSendStudyDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;
                employeeSendStudy.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();

                _employeeSendStudyService.CreateDocumentChangeLog(employeeSendStudy.Id, signedPerson.DocStatusId);

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                    _employeeSendStudyService.Accept(updateStatus);

                CombineStatuses(_employeeSendStudyService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;

                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_WORK_DAY_OFF)
            {
                #region Bayram (ishlanmaydigon) kunlari ishga jalb etish buyuruqlari
                var workDayOff = _unitOfWork.Context.Set<WorkDayOff>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                var nextSigner = workDayOff.Signer.FirstOrDefault(a => a.SignOrder == signedPerson.SignPriority);

                if (signedPerson.DocStatusId == 0 || workDayOff == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                UpdateStatusWorkDayOffDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                workDayOff.StatusId = updateStatus.StatusId;


                nextSigner.DataFile = _workdayOfService.SaveFile(workDayOff.Id, JsonConvert.SerializeObject(dto), "data.txt");
                nextSigner.SignedAt = DateTime.Now;
                nextSigner.SignedUserInfo = signedPerson.UserInfo;

                _unitOfWork.Context.Entry(nextSigner).State = EntityState.Modified;
                _unitOfWork.Save();
                _workdayOfService.CreateDocumentChangeLog(workDayOff.Id, signedPerson.DocStatusId);


                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                    _workdayOfService.Accept(updateStatus);

                CombineStatuses(_workdayOfService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_EMPLOYEE_SEND_TRAIN)
            {
                #region Malaka oshirishga yuborish buyruqni imzolash
                var employeeSendTrain = _unitOfWork.Context.Set<EmployeeSendTrain>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                var signer = employeeSendTrain.Signer.FirstOrDefault(a => a.SignOrder == signedPerson.SignPriority);

                if (signedPerson.DocStatusId == 0 || employeeSendTrain == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                UpdateStatusEmployeeSendTrainDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                employeeSendTrain.StatusId = updateStatus.StatusId;

                signer.DataFile = _employeeSendTrainService.SaveFile(employeeSendTrain.Id, JsonConvert.SerializeObject(employeeSendTrain), "data.txt");
                signer.SignedAt = DateTime.Now;
                signer.SignedUserInfo = signedPerson.UserInfo;

                _unitOfWork.Save();

                _employeeSendTrainService.CreateDocumentChangeLog(employeeSendTrain.Id, signedPerson.DocStatusId);

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                    _employeeSendTrainService.Accept(updateStatus);


                CombineStatuses(_employeeSendTrainService);
                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT)
            {
                #region Davo arizasi 
                var doc = _unitOfWork.Context.Set<ApplicationForCourt>().Include(s => s.Signs).FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (doc == null)
                {
                    AddError("Document topilmadi");
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                doc.StatusId = signedPerson.DocStatusId;

                var DataFile = _applicationForCourtService.SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");

                doc.Signs.Add(new()
                {
                    OwnerId = doc.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    SignedUserInfo = signedPerson.UserInfo
                });

                _unitOfWork.Save();

                _applicationForCourtService.CreateDocumentChangeLog(doc.Id, signedPerson.DocStatusId);

                CombineStatuses(_applicationForCourtService);

                if (HasErrors)
                {
                    resultDto.ErrorMessage = "Error";
                    resultDto.ErrorCode = "002";
                    resultDto.Comment = String.Join(" ", _errors);

                    return resultDto;
                }

                resultDto.ErrorMessage = "OK";
                resultDto.ErrorCode = "001";
                resultDto.Comment = string.Empty;

                return resultDto;
                #endregion
            }
        }

        resultDto.ErrorMessage = "OK";
        resultDto.ErrorCode = "001";
        resultDto.Comment = string.Empty;

        return resultDto;
    }
    public async Task<bool> PostSignResponse(WbImzoSignInformDto dto)
    {
        ApiResult<WbImzoSignRequestDto> signResult = await _wbImzoService.GetByRequestIdAsync(dto);
        if (!signResult.IsSuccess || signResult.Response is null)
            CombineStatuses(signResult.GetStatusGeneric());

        if (IsValid && signResult.Response is not null)
        {
            var signedPerson = signResult.Response.SignRequestUsers
                    .FirstOrDefault(a => a.SignatureMethodId == SignatureMethodIdConst.E_IMZO ?
                                         dto.UserKey == a.UserKey : dto.UserKey == a.UserPhoneNumber.NormalizePhoneNumber() &&
                                         a.IsSigned == true);

            if (signedPerson != null && !signedPerson.ActionTypeId.HasValue)
            {
              return true;
            }

            if (signResult.Response.TableId == TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT)
            {
                #region A'zolik Shartnomasi
                var Contract = _unitOfWork.Context.Set<MemshipContract>().Include(s => s.Status).FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (Contract == null)
                {
                    return true;
                }

                Contract.StatusId = signedPerson.DocStatusId;
                
                var DataFile = _memshipContractService.SaveFile(Contract.Id, JsonConvert.SerializeObject(Contract), "data.txt");

                Contract.Signs.Add(new()
                {
                    OwnerId = Contract.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = Contract.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });


                _unitOfWork.Save();

                _memshipContractService.CreateDocumentChangeLog(Contract.Id);

                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_SERVICE_CONTRACT)
            {
                #region Xizmatlar Shartnomasi

                var Contract = _unitOfWork.Context.Set<ServiceContract>()
                                                               .Include(a => a.Application).ThenInclude(a => a.ServiceApplication)
                                                               .Include(a => a.Groups).ThenInclude(a => a.Tables)
                                                               .FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey
                                                                && s.StatusId != StatusIdConst.DELETED);
                if (Contract == null)
                {
                    return true;
                }

                Contract.StatusId = signedPerson.DocStatusId;
                var DataFile = _srvContractService.SaveFile(Contract.Id, JsonConvert.SerializeObject(Contract), "data.txt");

                Contract.Signs.Add(new()
                {
                    OwnerId = Contract.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = Contract.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });

                CombineStatuses(_srvContractService);

                if (HasErrors)
                    return true;

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    foreach (var group in Contract.Groups)
                    {
                        foreach (var table in group.Tables)
                        {
                            var complete = _srvCompleteService.Create(new CreateCompletedServiceDlDto
                            {
                                DocNumber = Contract.DocNumber,
                                DocOn = DateOnly.FromDateTime(DateTime.Now),
                                ContractorId = Contract.ContractorId,
                                ServiceContractId = Contract.Id,
                                //EmployeeManageId = _authService.User.EmployeeManageId,
                                ServiceApplicationId = Contract.Application.ServiceApplication.Id,
                            });
                        }
                    }
                }

                _unitOfWork.Save();
                _srvContractService.CreateDocumentChangeLog(Contract.Id);

                return true;
                #endregion 
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_SERVICE_DEED)
            {
                #region Xizmatlar Dalolatnomasi
                var deed = _unitOfWork.Context.Set<ServiceDeed>()
                                                        .Include(a => a.Groups).ThenInclude(a => a.Tables)
                                                        .Include(a => a.SrvContract)
                                                        .FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (deed == null)
                {
                    return true;
                }

                deed.StatusId = signedPerson.DocStatusId;

                var DataFile = _srvDeedService.SaveFile(deed.Id, JsonConvert.SerializeObject(deed), "data.txt");

                deed.Signs.Add(new()
                {
                    OwnerId = deed.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = deed.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });

                CombineStatuses(_srvContractService);

                if (HasErrors)
                    return true;

                foreach (var group in deed.Groups)
                {
                    foreach (var table in group.Tables)
                    {
                        var complete = _srvCompleteService.Create(new CreateCompletedServiceDlDto
                        {
                            DocNumber = deed.DocNumber,
                            DocOn = DateOnly.FromDateTime(DateTime.Now),
                            ContractorId = deed.Id,
                            ServiceContractId = deed.SrvContractorId,
                            //EmployeeManageId = _authService.User.EmployeeManageId,
                            ServiceApplicationId = deed.Application.ServiceApplication.Id,
                        });
                    }
                }

                _srvDeedService.CreateDocumentChangeLog(deed.Id);
                _unitOfWork.Save();

                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.APPEAL__DOC_APPEAL_APPLICATION)
            {
                #region Murojat arizasi
                var application = _unitOfWork.Context.Set<AppealApplication>()
                                                                   .FirstOrDefault(s => s.Id == (int)signResult.Response.DocumentId && s.StatusId != StatusIdConst.DELETED);
                if (application == null)
                    return true;

                application.StatusId = signedPerson.DocStatusId;
                var DataFile = _appealApplication.SaveFile(application.Id, JsonConvert.SerializeObject(application), "data.txt");

                application.Signs.Add(new()
                {
                    OwnerId = application.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = application.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });

                CombineStatuses(_srvContractService);

                if (HasErrors)
                    return true;

                _unitOfWork.Save();
                _appealApplication.CreateDocumentChangeLog(application.Id, signedPerson.DocStatusId);

                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_DUAL_CONTRACT)
            {
                #region Dual ta'lim shartnomasi

                var dual = _unitOfWork.Context.Set<DualContract>()
                                                        .FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey
                                                         && s.StatusId != StatusIdConst.DELETED);

                if (dual == null)
                    return true;


                dual.StatusId = signedPerson.DocStatusId;
                var DataFile = _dualContractService.SaveFile(dual.Id, JsonConvert.SerializeObject(dual), "data.txt");

                dual.Signs.Add(new()
                {
                    OwnerId = dual.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    StatusId = dual.StatusId,
                    SignedUserInfo = signedPerson.UserInfo
                });

                CombineStatuses(_srvContractService);

                if (HasErrors)
                    return true;

                _dualContractService.CreateDocumentChangeLog(dual.Id, signedPerson.DocStatusId);
                _unitOfWork.Save();

                if (IsValid)
                {
                    if (dual.ExternalId != null)
                    { await _billingService.HeldByContractor((int)dual.ExternalId); }
                }

                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DUALEDU__DOC_DUAL_APPLICATION)
            {
                #region Dual Arizasi 

                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.CLAIM__DOC_ARBITRATION_APPLICATION)
            {
                #region Hakamlik arizasi

                #endregion
            }
            else if (signResult.Response.TableId == TableIdConst.DOC_APPOINT_EMPLOYEE)
            {
                #region Kadrlar hisobi Shaxsiy tarkib buyuruqlarini imzolash
                var contract = _unitOfWork.Context.Set<AppointEmployee>()
                                                               .FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey
                                                                && s.StatusId != StatusIdConst.DELETED);


                if (signedPerson.DocStatusId == 0 || contract == null)
                    return true;
                UpdateStatusAppointEmployeeDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                contract.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();
                _employeeService.CreateDocumentChangeLog(contract.Id, signedPerson.DocStatusId);

                try
                {
                    if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                    {
                        await _employeeService.AcceptAsync(updateStatus);
                    }

                }
                catch (Exception ex)
                {
                    AddError($"{ex}");
                }
                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.HRM__DOC_CHASTISEMENT)
            {
                #region Kadrlar hisobi Intizomiy jazo qollash
                var contract = _unitOfWork.Context.Set<Chastisement>().FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || contract == null)
                    return true;

                UpdateStatusChastisementDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;


                contract.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();

                _chastisementService.CreateDocumentChangeLog(contract.Id, signedPerson.DocStatusId);

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    _chastisementService.Accept(updateStatus);
                }

                return true;
                #endregion
            }
            else if (signResult.Response.TableId == TableIdConst.DOC_TEMP_CALC_KIND)
            {
                #region Kadrlar hisobi Bir martalik tolovlar berish to'g'risidagi buyruq
                var contract = _unitOfWork.Context.Set<TempCalcKind>().FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || contract == null)
                    return true;

                UpdateStatusTempCalcKindDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                _tempCalcKindService.CreateDocumentChangeLog(contract.Id, signedPerson.DocStatusId);
                contract.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    _tempCalcKindService.Accept(updateStatus);
                }

                return true;
                #endregion
            }
            else if (signResult.Response.TableId == TableIdConst.HRM__DOC_ORDER_TO_SEND_TO_BUSINESS_TRIP)
            {
                #region Xizmat safariga yuborish buyuruqlarini imzolash
                var contract = _unitOfWork.Context.Set<OrderToSendBusinessTrip>().FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || contract == null)
                    return true;

                UpdateStatusTempCalcKindDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;


                contract.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();

                _tempCalcKindService.CreateDocumentChangeLog(contract.Id, signedPerson.DocStatusId);

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                {
                    _tempCalcKindService.Accept(updateStatus);
                }

                return true;
                #endregion
            }
            else if (signResult.Response.TableId == TableIdConst.DOC_EMPLOYEE_LEAVE_ORDER)
            {
                #region Ta'tilga yuborish buyruqlarini imzolash , Bola parvarishlash, Xomiladorlik va tug'ish ta'til buyuruqlarini imzolash
                var employeeLeaveOrder = _unitOfWork.Context.Set<EmployeeLeaveOrder>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || employeeLeaveOrder == null)
                    return true;

                UpdateStatusEmployeeLeaveOrderDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                employeeLeaveOrder.StatusId = updateStatus.StatusId;
                _unitOfWork.Save();

                _employeeLeaveOrderService.CreateDocumentChangeLog(employeeLeaveOrder.Id, signedPerson.DocStatusId);
                try
                {
                    if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                        _employeeLeaveOrderService.Accept(updateStatus);
                }
                catch (Exception ex)
                {
                    AddError(ex.Message);
                }
                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_RECALL_LEAVE)
            {
                #region Mexnat ta'tilidan yoki o'z hisobidan ta'tildan chaqirib olish buyruq
                var recallLeave = _unitOfWork.Context.Set<RecallLeave>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || recallLeave == null)
                    return true;

                UpdateStatusRecallLeaveDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                recallLeave.StatusId = updateStatus.StatusId;
                _unitOfWork.Save();

                _recallLeaveService.CreateDocumentChangeLog(recallLeave.Id, signedPerson.DocStatusId);

                try
                {
                    if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                        _recallLeaveService.Accept(updateStatus);
                }
                catch (Exception ex)
                {
                    AddError(ex.Message);
                }
                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_EMPLOYEE_SEND_STUDY)
            {
                #region O'quv ta'tili buyuruqlari
                var employeeSendStudy = _unitOfWork.Context.Set<EmployeeSendStudy>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                if (signedPerson.DocStatusId == 0 || employeeSendStudy == null)
                    return true;

                UpdateStatusEmployeeSendStudyDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;
                employeeSendStudy.StatusId = updateStatus.StatusId;

                _unitOfWork.Save();

                _employeeSendStudyService.CreateDocumentChangeLog(employeeSendStudy.Id, signedPerson.DocStatusId);

                try
                {
                    if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                        _employeeSendStudyService.Accept(updateStatus);
                }
                catch (Exception ex)
                {
                    AddError(ex.Message);
                }
                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_WORK_DAY_OFF)
            {
                #region Bayram (ishlanmaydigon) kunlari ishga jalb etish buyuruqlari
                var workDayOff = _unitOfWork.Context.Set<WorkDayOff>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                var nextSigner = workDayOff.Signer.FirstOrDefault(a => a.SignOrder == signedPerson.SignPriority);

                if (signedPerson.DocStatusId == 0 || workDayOff == null)
                    return true;

                UpdateStatusWorkDayOffDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                workDayOff.StatusId = updateStatus.StatusId;


                nextSigner.DataFile = _workdayOfService.SaveFile(workDayOff.Id, JsonConvert.SerializeObject(dto), "data.txt");
                nextSigner.SignedAt = DateTime.Now;
                nextSigner.SignedUserInfo = signedPerson.UserInfo;

                _unitOfWork.Context.Entry(nextSigner).State = EntityState.Modified;
                _unitOfWork.Save();
                _workdayOfService.CreateDocumentChangeLog(workDayOff.Id, signedPerson.DocStatusId);


                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                    _workdayOfService.Accept(updateStatus);

                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.DOC_EMPLOYEE_SEND_TRAIN)
            {
                #region Malaka oshirishga yuborish buyruqni imzolash
                var employeeSendTrain = _unitOfWork.Context.Set<EmployeeSendTrain>()
                                                            .FirstOrDefault(x => x.WebImzoSecretKey == signResult.Response.SecretKey && x.StatusId != StatusIdConst.DELETED);

                var signer = employeeSendTrain.Signer.FirstOrDefault(a => a.SignOrder == signedPerson.SignPriority);

                if (signedPerson.DocStatusId == 0 || employeeSendTrain == null)
                    return true;

                UpdateStatusEmployeeSendTrainDto updateStatus = new();
                updateStatus.StatusId = signedPerson.DocStatusId;

                employeeSendTrain.StatusId = updateStatus.StatusId;

                signer.DataFile = _employeeSendTrainService.SaveFile(employeeSendTrain.Id, JsonConvert.SerializeObject(employeeSendTrain), "data.txt");
                signer.SignedAt = DateTime.Now;
                signer.SignedUserInfo = signedPerson.UserInfo;

                _unitOfWork.Save();

                _employeeSendTrainService.CreateDocumentChangeLog(employeeSendTrain.Id, signedPerson.DocStatusId);

                if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                    _employeeSendTrainService.Accept(updateStatus);

                return true;
                #endregion
            }

            else if (signResult.Response.TableId == TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT)
            {
                #region Davo arizasi 
                var doc = _unitOfWork.Context.Set<ApplicationForCourt>().Include(s => s.Signs).FirstOrDefault(s => s.WebImzoSecretKey == signResult.Response.SecretKey && s.StatusId != StatusIdConst.DELETED);

                if (doc == null)
                {
                    return true;
                }

                doc.StatusId = signedPerson.DocStatusId;

                var DataFile = _applicationForCourtService.SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");

                doc.Signs.Add(new()
                {
                    OwnerId = doc.Id,
                    DataFile = DataFile,
                    SignedAt = DateTime.Now,
                    SignedUserInfo = signedPerson.UserInfo
                });

                _unitOfWork.Save();

                _applicationForCourtService.CreateDocumentChangeLog(doc.Id, signedPerson.DocStatusId);

                return true;
                #endregion
            }
        }

        return true;
    }

    public async Task UpdateStatus(UpdateStatusWebImzoDto dto)
    {
        if (TableIdConst.DOC_EMPLOYEE_SEND_TRAIN == dto.TableID)
        {
            #region Malaka oshirishga yuborish buyruqni imzolash
            var employeeSendTrain = _unitOfWork.Context.Set<EmployeeSendTrain>()
                                                                     .Include(a => a.Signer)
                                                                         .ThenInclude(a => a.EmployeeManage)
                                                                             .ThenInclude(em => em.Employee)
                                                                                 .ThenInclude(e => e.Organization)
                                                                     .Include(a => a.Signer)
                                                                         .ThenInclude(a => a.EmployeeManage)
                                                                             .ThenInclude(em => em.Employee)
                                                                                 .ThenInclude(e => e.Person)
                                                                     .FirstOrDefault(a => a.Id == dto.DocId);

            if (employeeSendTrain.WebImzoRequestId is not null)
            {
                var signers = employeeSendTrain.Signer.Where(a => a.SignedAt == null);
                var bossSigner = employeeSendTrain.Signer.Max(a => a.SignOrder);
                string userKey;
                ApiResult<WbImzoSignRequestDto> signResultData = new();
                foreach (var signer in signers)
                {
                    if (signer.SignOrder == bossSigner)
                    {
                        signResultData = await _wbImzoService.GetByRequestIdAsync(new WbImzoSignInformDto()
                        {
                            SignRequestId = employeeSendTrain.WebImzoRequestId.Value,
                            SecretKey = employeeSendTrain.WebImzoSecretKey,
                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Organization.Inn) ? signer.EmployeeManage.Employee.Organization.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                        });

                        userKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Organization.Inn) ? signer.EmployeeManage.Employee.Organization.Inn : signer.EmployeeManage.Employee.Person.Pinfl;
                    }

                    else
                    {
                        signResultData = await _wbImzoService.GetByRequestIdAsync(new WbImzoSignInformDto()
                        {
                            SignRequestId = employeeSendTrain.WebImzoRequestId.Value,
                            SecretKey = employeeSendTrain.WebImzoSecretKey,
                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Person.Inn) ? signer.EmployeeManage.Employee.Person.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                        });

                        userKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Person.Inn) ? signer.EmployeeManage.Employee.Person.Inn : signer.EmployeeManage.Employee.Person.Pinfl;
                    }

                    if (!signResultData.IsSuccess || signResultData.Response is null)
                        CombineStatuses(signResultData.GetStatusGeneric());

                    if (IsValid && signResultData.Response is not null)
                    {
                        var signedPerson = signResultData.Response.SignRequestUsers
                                .FirstOrDefault(a => a.SignatureMethodId == SignatureMethodIdConst.E_IMZO ?
                                                     userKey == a.UserKey : userKey == a.UserPhoneNumber.NormalizePhoneNumber() &&
                                                     a.IsSigned == true);

                        if (signedPerson is null)
                            continue;

                        UpdateStatusEmployeeSendTrainDto updateStatus = new();
                        updateStatus.StatusId = signedPerson.DocStatusId;

                        employeeSendTrain.StatusId = updateStatus.StatusId;

                        signer.DataFile = _employeeSendTrainService.SaveFile(employeeSendTrain.Id, JsonConvert.SerializeObject(employeeSendTrain), "data.txt");
                        signer.SignedAt = DateTime.Now;
                        signer.SignedUserInfo = signedPerson.UserInfo;

                        _unitOfWork.Save();

                        _employeeSendTrainService.CreateDocumentChangeLog(employeeSendTrain.Id, signedPerson.DocStatusId);

                        if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                            _employeeSendTrainService.Accept(updateStatus);

                        if (HasErrors)
                            return;
                        return;
                    }
                }
            }
            #endregion
        }

        else if (TableIdConst.DOC_WORK_DAY_OFF == dto.TableID)
        {
            #region Bayram (ishlanmaydigon) kunlari ishga jalb etish buyuruqlari
            var doc = _unitOfWork.Context.Set<WorkDayOff>()
                                                         .Include(a => a.Signer)
                                                             .ThenInclude(a => a.EmployeeManage)
                                                                 .ThenInclude(em => em.Employee)
                                                                     .ThenInclude(e => e.Organization)
                                                         .Include(a => a.Signer)
                                                             .ThenInclude(a => a.EmployeeManage)
                                                                 .ThenInclude(em => em.Employee)
                                                                     .ThenInclude(e => e.Person)
                                                         .FirstOrDefault(a => a.Id == dto.DocId);

            if (doc.WebImzoRequestId is not null)
            {
                var signers = doc.Signer.Where(a => a.SignedAt == null);
                var bossSigner = doc.Signer.Max(a => a.SignOrder);
                string userKey;
                ApiResult<WbImzoSignRequestDto> signResultData = new();
                foreach (var signer in signers)
                {
                    if (signer.SignOrder == bossSigner)
                    {
                        signResultData = await _wbImzoService.GetByRequestIdAsync(new WbImzoSignInformDto()
                        {
                            SignRequestId = doc.WebImzoRequestId.Value,
                            SecretKey = doc.WebImzoSecretKey,
                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Organization.Inn) ? signer.EmployeeManage.Employee.Organization.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                        });

                        userKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Organization.Inn) ? signer.EmployeeManage.Employee.Organization.Inn : signer.EmployeeManage.Employee.Person.Pinfl;
                    }

                    else
                    {
                        signResultData = await _wbImzoService.GetByRequestIdAsync(new WbImzoSignInformDto()
                        {
                            SignRequestId = doc.WebImzoRequestId.Value,
                            SecretKey = doc.WebImzoSecretKey,
                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Person.Inn) ? signer.EmployeeManage.Employee.Person.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                        });

                        userKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Person.Inn) ? signer.EmployeeManage.Employee.Person.Inn : signer.EmployeeManage.Employee.Person.Pinfl;
                    }

                    if (!signResultData.IsSuccess || signResultData.Response is null)
                        CombineStatuses(signResultData.GetStatusGeneric());

                    if (IsValid && signResultData.Response is not null)
                    {
                        var signedPerson = signResultData.Response.SignRequestUsers
                                .FirstOrDefault(a => a.SignatureMethodId == SignatureMethodIdConst.E_IMZO ?
                                                     userKey == a.UserKey : userKey == a.UserPhoneNumber.NormalizePhoneNumber() &&
                                                     a.IsSigned == true);

                        if (signedPerson is null)
                            continue;

                        UpdateStatusWorkDayOffDto updateStatus = new();
                        updateStatus.StatusId = signedPerson.DocStatusId;

                        doc.StatusId = updateStatus.StatusId;

                        signer.DataFile = _workdayOfService.SaveFile(doc.Id, JsonConvert.SerializeObject(dto), "data.txt");
                        signer.SignedAt = DateTime.Now;
                        signer.SignedUserInfo = signedPerson.UserInfo;

                        _unitOfWork.Context.Entry(signer).State = EntityState.Modified;
                        _unitOfWork.Save();
                        _workdayOfService.CreateDocumentChangeLog(doc.Id, signedPerson.DocStatusId);
                        if (signedPerson.DocStatusId == StatusIdConst.SIGNED)
                            _workdayOfService.Accept(updateStatus);

                        return;
                    }
                }
            }
            #endregion
        }
        else if (TableIdConst.DOC_EMPLOYEE_SEND_STUDY == dto.TableID)
        {
            #region O'quv ta'tili buyuruqlari
            #endregion
        }
    }
    public async Task GetErrors()
    {

    }
}