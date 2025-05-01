using System;
using System.Linq;

namespace SspUis.Core;

public class StatusIdConst
{
    /// <summary>
    /// Создан
    /// </summary>
    public const int CREATED = 1;

    /// <summary>
    /// Проведено
    /// </summary>
    public const int ACCEPTED = 2;

    /// <summary>
    /// Не проведено
    /// </summary>
    public const int NOT_ACCEPTED = 3;

    /// <summary>
    /// Изменен
    /// </summary>
    public const int MODIFIED = 4;

    /// <summary>
    /// Удален
    /// </summary>
    public const int DELETED = 5;

    /// <summary>
    /// Выполняется
    /// </summary>
    public const int EXECUTING = 6;

    /// <summary>
    /// Архивный
    /// </summary>
    public const int ARCHIVED = 7;

    /// <summary>
    /// Отправлен
    /// </summary>
    public const int SENT = 8;

    /// <summary>
    /// Доставлен
    /// </summary>
    public const int BANK_RECEIVED = 9;

    /// <summary>
    /// Не доставлен
    /// </summary>
    public const int BANK_NOT_RECEIVED = 10;

    /// <summary>
    /// Оплачен
    /// </summary>
    public const int PAID = 11;

    /// <summary>
    /// Ожидание
    /// </summary>
    public const int WAITING = 12;

    /// <summary>
    /// Отчет принят
    /// </summary>
    public const int REPORT_ACCEPTED = 13;

    /// <summary>
    /// Выполнен
    /// </summary>
    public const int FULL_FILLED = 14;

    /// <summary>
    /// Готов к отправке
    /// </summary>
    public const int READY_TO_SEND = 15;

    /// <summary>
    /// Принят
    /// </summary>
    public const int RECEIVED = 16;

    /// <summary>
    /// Утвержден
    /// </summary>
    public const int APPROVED = 17;

    /// <summary>
    /// Зарегистрирован
    /// </summary>
    public const int REGISTERED = 18;

    /// <summary>
    /// Отправлен в вышестоящую организацию
    /// </summary>
    public const int SENT_PARENT = 19;

    /// <summary>
    /// Согласовано
    /// </summary>
    public const int AGREED = 20;

    /// <summary>
    /// Подписан
    /// </summary>
    public const int SIGNED = 21;

    /// <summary>
    /// Отозван
    /// </summary>
    public const int REVOKED = 23;

    /// <summary>
    /// Отменен
    /// </summary>
    public const int CANCELED = 24;

    /// <summary>
    /// Отклонен
    /// </summary>
    public const int REJECTED = 25;

    /// <summary>
    /// Формирован
    /// </summary>
    public const int FORMED = 26;

    /// <summary>
    /// Подписывается
    /// </summary>
    public const int SIGNING = 27;

    /// <summary>
    /// Прошел экспертизу
    /// </summary>
    public const int PASS_EXPERTISE = 28;

    /// <summary>
    /// Не прошел экспертизу
    /// </summary>
    public const int NOT_PASS_EXPERTISE = 29;

    /// <summary>
    /// Отправлено на рассмотрение
    /// </summary>
    public const int SENT_FOR_REVIEW = 30;

    /// <summary>
    /// Отправлен на экспертизу
    /// </summary>
    public const int SENT_FOR_EXPERTISE = 31;

    /// <summary>
    /// Неудачно
    /// </summary>
    public const int FAILED = 32;

    /// <summary>
    /// Законченный
    /// </summary>
    public const int FINISHED = 33;

    /// <summary>
    /// Отправляется
    /// </summary>
    public const int SENDING = 34;

    //---------------------------- CORRUPSIYA UCHUN QO'SHILDI ----------------------------
    /// <summary>
    /// SSP tasdiqladi
    /// </summary>
    public const int ACCEPTED_SSP = 35;

    // <summary>
    /// Ombudsman tasdiqladi
    /// </summary>
    public const int ACCEPTED_OMBUDSMAN = 36;
    // -----------------------------------------------------------------------------------

    //---------------------------- APPEAL ------------------------------------------------

    /// <summary>
    /// Ijroda
    /// </summary>
    public const int IN_EXECUTION = 37;

    /// <summary>
    /// Ijrosi taminlangan.
    /// </summary>
    public const int EXECUTED = 38;

    /// <summary>
    /// Javob xati shakllandi
    /// </summary>
    public const int HAS_EDOC_RESPONSE = 39;

    // -----------------------------------------------------------------------------------

    public static int[] CanEditStatuses = new int[] { CREATED, MODIFIED, REVOKED };
    public static int[] CanRevokeStatuses = new int[] { SENT };
    public static int[] CanAcceptStatuses = new int[] { SENT };
    public static int[] CanDeleteStatuses = new int[] { CREATED, REVOKED, MODIFIED };
    public static int[] CanNotAcceptStatuses = new int[] { SENT };
    public static int[] CanAgreeStatuses = new int[] { SENT };
    public static int[] CanCancelStatuses = new int[] { ACCEPTED, SENT };
    public static int[] CanSignStatuses = new int[] { SENT, AGREED };
    public static int[] CanNotRejectStatuses = new int[] { SENT };
    public static int[] CanModifyStatus = { CREATED, MODIFIED, NOT_ACCEPTED, BANK_NOT_RECEIVED };
    public static int[] CanModifyForHrmStatus = { CREATED, MODIFIED, NOT_ACCEPTED, BANK_NOT_RECEIVED, REJECTED };

    #region SRV
    public static bool CanApplySrvDocStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT => new int[] { CREATED, MODIFIED, CANCELED }.Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, CANCELED }.Contains(currentStatusId),
            CANCELED => new int[] { ACCEPTED }.Contains(currentStatusId),
            ACCEPTED => new int[] { CREATED, MODIFIED, CANCELED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED, REJECTED, CANCELED, REVOKED }.Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanApplySrvApplicationStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            RECEIVED => new int[] { SENT }.Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            CANCELED => new int[] { SENT, CREATED, ACCEPTED, RECEIVED, MODIFIED }.Contains(currentStatusId),
            ACCEPTED => new int[] { CREATED, SENT, MODIFIED, RECEIVED }.Contains(currentStatusId),
            REJECTED => new int[] { SENT, ACCEPTED, MODIFIED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED, REJECTED, CANCELED, REVOKED }.Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanApplySrvContractStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SIGNING => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            REJECTED => new int[] { SIGNING, CREATED, MODIFIED }.Contains(currentStatusId),
            SIGNED => new int[] { SIGNING }.Contains(currentStatusId),
            CANCELED => new int[] { CREATED, MODIFIED, SIGNING }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, SIGNING, REJECTED, MODIFIED }.Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanApplySrvDeedStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SIGNING => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            REJECTED => new int[] { SIGNING, CREATED, MODIFIED }.Contains(currentStatusId),
            SIGNED => new int[] { SIGNING }.Contains(currentStatusId),
            CANCELED => new int[] { CREATED, MODIFIED, SIGNING }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, SIGNING, REJECTED, MODIFIED }.Contains(currentStatusId),
            _ => false,
        };
    }
    #endregion

    #region MEMSHIP
    public static bool CanMemshipApplicationApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED, REVOKED }.Contains(currentStatusId),
            REVOKED => currentStatusId == SENT_FOR_REVIEW,
            SENT_FOR_REVIEW => new int[] { CREATED, REVOKED, MODIFIED }.Contains(currentStatusId),
            ACCEPTED => new int[] { CREATED, MODIFIED, SENT_FOR_REVIEW }.Contains(currentStatusId),
            REJECTED => new int[] { SENT_FOR_REVIEW, ACCEPTED, MODIFIED }.Contains(currentStatusId),
            CANCELED => new int[] { ACCEPTED,FORMED }.Contains(currentStatusId),
            DELETED => new int[] { }.Contains(currentStatusId),
            _ => false
        };
    }

    public static bool CanMemshipContractApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            SENT_FOR_REVIEW => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            SIGNING => new int[] { SENT_FOR_REVIEW, CREATED, MODIFIED }.Contains(currentStatusId),
            SIGNED => new int[] { SIGNING,CREATED, SENT_FOR_REVIEW, SIGNING }.Contains(currentStatusId),
            REJECTED => new int[] { SENT_FOR_REVIEW, SIGNED }.Contains(currentStatusId),
            CANCELED => new int[] { SIGNING, SENT_FOR_REVIEW, CREATED, MODIFIED, SIGNED }.Contains(currentStatusId),
            _ => false
        };
    }
    #endregion

    #region HRM
    public static bool CanDualApplicationApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT => new int[] { CREATED, REVOKED, MODIFIED }.Contains(currentStatusId),
            REVOKED => currentStatusId == SENT,
            REJECTED => new int[] { SENT }.Contains(currentStatusId),
            ACCEPTED => new int[] { SENT }.Contains(currentStatusId),
            CANCELED => new int[] { ACCEPTED }.Contains(currentStatusId),
            MODIFIED => true,
            _ => false
        };
    }
    public static bool CanDualContractApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SIGNED => new int[] { SIGNING, MODIFIED }.Contains(currentStatusId),
            REJECTED => new int[] { SIGNING }.Contains(currentStatusId),
            _ => false
        };
    }
    public static bool CanCandidateConfirmationStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT_FOR_REVIEW => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            RECEIVED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            REJECTED => new int[] { CREATED, MODIFIED, RECEIVED }.Contains(currentStatusId),
            ACCEPTED => new int[] { SENT_FOR_REVIEW }.Contains(currentStatusId),
            CANCELED => new int[] { ACCEPTED }.Contains(currentStatusId),
            _ => false
        };
    }
    public static bool CanEmployeeMissedDayApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            CREATED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.Contains(currentStatusId),
            APPROVED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, REJECTED }.Contains(currentStatusId),
            REJECTED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.Contains(currentStatusId),
            _ => true
        };
    }
    public static bool CanStateAssetApplicationApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            ACCEPTED => new int[] { SENT_FOR_REVIEW }.Contains(currentStatusId),
            REJECTED => new int[] { SENT_FOR_REVIEW }.Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, REVOKED }.Contains(currentStatusId),
            SENT_FOR_REVIEW => new int[] { SENDING }.Contains(currentStatusId),
            _ => false,
        };
    }

    public static bool CanMonoApplicationApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT => new int[] { CREATED, REVOKED }.Contains(currentStatusId),
            REVOKED => currentStatusId == SENT,
            REJECTED => new int[] { SENT, CREATED, SENT_FOR_REVIEW }.Contains(currentStatusId),
            ACCEPTED => new int[] { SENT, CREATED, SENT_FOR_REVIEW }.Contains(currentStatusId),
            CANCELED => new int[] { ACCEPTED }.Contains(currentStatusId),
            _ => false
        };
    }

    public static bool CanApplyHrmDocStatus(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            RECEIVED => new int[] { SENT, ARCHIVED }.Contains(currentStatusId),
            ARCHIVED => new int[] { RECEIVED }.Contains(currentStatusId),
            REJECTED => new int[] { SENT, RECEIVED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED, REJECTED, CANCELED, REVOKED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED, BANK_NOT_RECEIVED, REJECTED, REVOKED, CANCELED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            SENT => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED, BANK_NOT_RECEIVED, REJECTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            READY_TO_SEND => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            CANCELED => new int[] { ACCEPTED }.Contains(currentStatusId),
            FINISHED => new int[] { ACCEPTED, READY_TO_SEND }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }

    public static bool CanApplyAppointEmployee(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            SIGNING => new int[] { CREATED, NOT_ACCEPTED, SIGNING }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNING, CREATED, NOT_ACCEPTED, MODIFIED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanExecutionApplicationApplyStatus(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, CANCELED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, CANCELED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            SIGNED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { CANCELED, CREATED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanApplyEmployeeLeaveOrder(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            SIGNING => new int[] { CREATED, NOT_ACCEPTED, SIGNING }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNING }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            SIGNED => new int[] { SIGNING, ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanApplyEmployeeMissedDays(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            APPROVED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { APPROVED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanApplyChastisement(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            SIGNING => new int[] { CREATED, SIGNING, ACCEPTED, SIGNING, MODIFIED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNING, CREATED, MODIFIED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }

    public static bool CanApplyTimesheet(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNING, CREATED, MODIFIED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }
    #endregion

    #region KPI
    public static bool CanKpiRatingEmployee(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNING, CREATED, MODIFIED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanKpiGrating(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNING, CREATED, MODIFIED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanKpiPlanForEmployee(int currentStatusId, int newStatusId, int[] ignoredStatusIds = null)
    {
        return newStatusId switch
        {
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNING, CREATED, MODIFIED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.SafeUnion(ignoredStatusIds).Contains(currentStatusId),
            _ => false,
        };
    }
    #endregion


    #region CLAIM
    public static bool CanClaimApplicationApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT => new int[] { CREATED, REVOKED }.Contains(currentStatusId),
            REVOKED => currentStatusId == SENT,
            REJECTED => new int[] { SENT }.Contains(currentStatusId),
            ACCEPTED => new int[] { SENT }.Contains(currentStatusId),
            CANCELED => new int[] { ACCEPTED, SENT }.Contains(currentStatusId),
            _ => false
        };
    }
    public static bool CanApplicationForCourtApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.Contains(currentStatusId),
            REJECTED => new int[] { SENT }.Contains(currentStatusId),
            SENT => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.Contains(currentStatusId),
            ACCEPTED => new int[] { SENT, MODIFIED, CREATED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED }.Contains(currentStatusId),
            _ => false
        };
    }

    public static bool CanApplicationForCourtSendStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SIGNED => new int[] { ACCEPTED }.Contains(currentStatusId),
            SENT => new int[] {MODIFIED, CREATED }.Contains(currentStatusId),
            _ => false
        };
    }
    public static bool CanArbitrationCourtApplicationApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT_FOR_REVIEW => new int[] { CREATED, MODIFIED, CANCELED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, CANCELED }.Contains(currentStatusId),
            CANCELED => new int[] { CREATED, MODIFIED, SENT_FOR_REVIEW }.Contains(currentStatusId),
            ACCEPTED => new int[] { CREATED, MODIFIED, SENT_FOR_REVIEW }.Contains(currentStatusId),
            _ => false,
        };
    }
    #endregion

    #region   DUAL
    public static bool CanSubsidyRequestApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED, REVOKED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, REJECTED }.Contains(currentStatusId),
            //SENT => new int[] { CREATED, MODIFIED, REVOKED }.Contains(currentStatusId),
            REVOKED => new int[] { SENT_FOR_REVIEW }.Contains(currentStatusId),
            REJECTED => new int[] { SENT_FOR_REVIEW }.Contains(currentStatusId),
            CANCELED => new int[] { SENT_FOR_REVIEW }.Contains(currentStatusId),
            SENT_FOR_REVIEW => new int[] { CREATED, MODIFIED, REVOKED }.Contains(currentStatusId),
            ACCEPTED => new int[] { SENT_FOR_REVIEW }.Contains(currentStatusId),
            _ => false
        };
    }
    #endregion

    #region   APPEAL
    public static bool CanAppealApplicationApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED, IN_EXECUTION }.Contains(currentStatusId),
            SENT => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            SENT_FOR_REVIEW => new int[] { CREATED, MODIFIED, SENT }.Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNED }.Contains(currentStatusId),
            REJECTED => new int[] { SENT }.Contains(currentStatusId),
            IN_EXECUTION => new int[] { SENT, CREATED, MODIFIED }.Contains(currentStatusId),
            HAS_EDOC_RESPONSE => new int[] { IN_EXECUTION }.Contains(currentStatusId),
            EXECUTED => new int[] { HAS_EDOC_RESPONSE, EXECUTED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            _ => false
        };
    }
    public static bool CanCallCenterAppealApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED, IN_EXECUTION }.Contains(currentStatusId),
            SENT => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            SENT_FOR_REVIEW => new int[] { CREATED, MODIFIED, SENT }.Contains(currentStatusId),
            ACCEPTED => new int[] { SIGNED }.Contains(currentStatusId),
            REJECTED => new int[] { SENT }.Contains(currentStatusId),
            IN_EXECUTION => new int[] { SENT, CREATED, MODIFIED }.Contains(currentStatusId),
            HAS_EDOC_RESPONSE => new int[] { IN_EXECUTION, HAS_EDOC_RESPONSE }.Contains(currentStatusId),
            EXECUTED => new int[] { HAS_EDOC_RESPONSE, IN_EXECUTION }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            _ => false
        };
    }
    #endregion

    #region CORRUPTION
    public static bool CanApplyAntiCorruptionResultStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT => new int[] { CREATED }.Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, SENT, MODIFIED }.Contains(currentStatusId),
            NOT_ACCEPTED => new int[] { ACCEPTED }.Contains(currentStatusId),
            ACCEPTED => new int[] { CREATED, SENT, MODIFIED, CANCELED, NOT_ACCEPTED }.Contains(currentStatusId),
            REJECTED => new int[] { SENT }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, NOT_ACCEPTED, REJECTED, CANCELED, REVOKED }.Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanJoinAntiCorruptionApplicationApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            SENT => new int[] { CREATED, REVOKED }.Contains(currentStatusId),

            ACCEPTED_SSP => new int[] { SENT }.Contains(currentStatusId),
            ACCEPTED_OMBUDSMAN => new int[] { ACCEPTED_SSP }.Contains(currentStatusId),
            ACCEPTED => new int[] { ACCEPTED_OMBUDSMAN }.Contains(currentStatusId),
            REVOKED => currentStatusId == SENT,
            REJECTED => new int[] { SENT, ACCEPTED_OMBUDSMAN, ACCEPTED_SSP }.Contains(currentStatusId),
            CANCELED => new int[] { ACCEPTED }.Contains(currentStatusId),
            _ => false
        };
    }
    #endregion

    #region ArbitrationDiscussion
    #endregion

    public static bool CanApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            ACCEPTED => CanAcceptStatuses.Contains(currentStatusId),
            DELETED => CanDeleteStatuses.Contains(currentStatusId),
            REVOKED => CanRevokeStatuses.Contains(currentStatusId),
            REJECTED => CanNotAcceptStatuses.Contains(currentStatusId),
            MODIFIED => CanEditStatuses.Contains(currentStatusId),
            APPROVED => CanModifyStatus.Contains(currentStatusId),
            CANCELED => new int[] { FORMED }.Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanMemshipCertificateApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            CANCELED => new int[] { FORMED }.Contains(currentStatusId),
            _ => false
        };
    }
    public static bool CanApplicationApplyStatus(int currentStatusId, int newStatusId)  //30   2
    {
        return newStatusId switch
        {
            SENT_FOR_REVIEW => new int[] { CREATED, MODIFIED, SENT, SENDING }.Contains(currentStatusId),
            FULL_FILLED => currentStatusId == SENT_FOR_REVIEW,
            ACCEPTED => new int[] { SENT_FOR_REVIEW, FULL_FILLED }.Contains(currentStatusId),
            //ACCEPTED => currentStatusId == SENT_FOR_REVIEW,
            DELETED => CanDeleteStatuses.Contains(currentStatusId),
            REJECTED => new int[] { SENT_FOR_REVIEW, FULL_FILLED }.Contains(currentStatusId),
            MODIFIED => new int[] { CREATED, MODIFIED, SENT }.Contains(currentStatusId),
            CANCELED => currentStatusId == ACCEPTED,
            SENT => new int[] { SENDING }.Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanContractApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            REVOKED => currentStatusId == SENT,
            PASS_EXPERTISE => new int[] { SENT_FOR_EXPERTISE/*NOT_PASS_EXPERTISE*/ }.Contains(currentStatusId),
            SENT_FOR_EXPERTISE => new int[] { NOT_PASS_EXPERTISE, PASS_EXPERTISE, SIGNING }.Contains(currentStatusId),
            NOT_PASS_EXPERTISE => currentStatusId == SENT_FOR_EXPERTISE,
            SIGNING => new int[] { PASS_EXPERTISE, SIGNING }.Contains(currentStatusId),
            REJECTED => new int[] { PASS_EXPERTISE, SIGNING, NOT_PASS_EXPERTISE }.Contains(currentStatusId),
            SIGNED => new int[] { SIGNING }.Contains(currentStatusId),
            CANCELED => new int[] { SIGNED }.Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanCertificateApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            CANCELED => new int[] { FORMED }.Contains(currentStatusId),
            _ => false,
        };
    }
    public static bool CanAdditionalAgreementApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED, REJECTED }.Contains(currentStatusId),
            SIGNING => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            SIGNED => new int[] { SIGNING }.Contains(currentStatusId),
            REJECTED => new int[] { SIGNING, SIGNED, CREATED, MODIFIED }.Contains(currentStatusId),
            _ => false
        };
    }

    #region   Arbitration

    public static bool CanArbitrationResultApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            SIGNED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            _ => false
        };
    }

    public static bool CanArbitrationDiscussionApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            SIGNED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            _ => false
        };
    }

    public static bool CanArbitrationDelayApplyStatus(int currentStatusId, int newStatusId)
    {
        return newStatusId switch
        {
            MODIFIED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            SIGNING => new int[] { CREATED, MODIFIED, SIGNING }.Contains(currentStatusId),
            SIGNED => new int[] { CREATED, MODIFIED, SIGNING }.Contains(currentStatusId),
            DELETED => new int[] { CREATED, MODIFIED }.Contains(currentStatusId),
            _ => false
        };
    }

    #endregion
}
