using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;
public class DocumentHeldForSignService : StatusGenericHandler, IDocumentHeldForSignService
{
    private readonly DbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public DocumentHeldForSignService(IUnitOfWork unitOfWork, IAuthService authService, DbContext context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }
    public PagedResult<HrmSignerVtView> GetData(DocumentHeldForSignFilterOption dto)
    {
        bool hasError = false;
        var query = GetQuery(dto, out hasError);
        if (hasError)
            return null;

        return query.AsPagedResult(dto);
    }

    public IQueryable<HrmSignerVtView> GetQuery(DocumentHeldForSignFilterOption dto, out bool hasError)
    {
        hasError = false;

        if (_authService.User.EmployeeManageId != null)
        {
            var query = _context.Set<HrmSignerVtView>().FromSqlRaw($@"

SELECT
de.id,
de.doc_number,
de.status_id,
de.details,
de.doc_on,
sta.full_name as status,
{TableIdConst.DOC_APPOINT_EMPLOYEE} AS tableid,
ARRAY_AGG(DISTINCT des.employee_manage_id) FILTER (WHERE des.employee_manage_id IS NOT NULL) as employee_manage_ids,
 STRING_AGG(DISTINCT per.full_name,', ') AS employee_fullnames
FROM
hrm.doc_appoint_employee de
LEFT JOIN
hrm.doc_appoint_employee_table det ON de.id = det.owner_id
LEFT JOIN
hrm.doc_appoint_employee_signer des ON de.id = des.owner_id
LEFT JOIN
hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
public.hl_person per ON emp.person_id = per.id
LEFT JOIN public.enum_status sta ON de.status_id = sta.id
GROUP BY
de.id, de.doc_number, de.status_id, sta.full_name, de.doc_on, de.details 

UNION ALL

SELECT
de.id,
de.doc_number,
de.status_id,
de.details,
de.doc_on,
sta.full_name as status,
{TableIdConst.HRM__DOC_CHASTISEMENT} AS tableid,
ARRAY_AGG(DISTINCT des.employee_manage_id) FILTER (WHERE des.employee_manage_id IS NOT NULL) as employee_manage_ids,
 STRING_AGG(DISTINCT per.full_name,', ') AS employee_fullnames
FROM
hrm.doc_chastisement de
LEFT JOIN
hrm.doc_chastisement_table det ON de.id = det.owner_id
LEFT JOIN
hrm.doc_chastisement_signer des ON de.id = des.owner_id
LEFT JOIN
hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
public.hl_person per ON emp.person_id = per.id
LEFT JOIN public.enum_status sta ON de.status_id = sta.id
GROUP BY
de.id, de.doc_number, de.status_id, sta.full_name, de.doc_on, de.details 

UNION ALL

SELECT
de.id,
de.doc_number,
de.status_id,
de.details,
de.doc_on,
 sta.full_name as status,
{TableIdConst.DOC_EMPLOYEE_LEAVE_ORDER} AS tableid,
ARRAY_AGG(DISTINCT des.employee_manage_id) FILTER (WHERE des.employee_manage_id IS NOT NULL) as employee_manage_ids,
 STRING_AGG(DISTINCT per.full_name,', ') AS employee_fullnames
FROM
hrm.doc_employee_leave_order de
LEFT JOIN
hrm.doc_employee_leave_order_table det ON de.id = det.owner_id
LEFT JOIN
hrm.doc_employee_leave_order_signer des ON de.id = des.owner_id
LEFT JOIN
hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
public.hl_person per ON emp.person_id = per.id
LEFT JOIN public.enum_status sta ON de.status_id = sta.id
GROUP BY
de.id, de.doc_number, de.status_id, sta.full_name, de.doc_on, de.details 

UNION ALL

SELECT
de.id,
de.doc_number,
de.status_id,
de.details,
de.doc_on,
sta.full_name as status,
{TableIdConst.DOC_EMPLOYEE_SEND_TRAIN} AS tableid,
ARRAY_AGG(DISTINCT des.employee_manage_id) FILTER (WHERE des.employee_manage_id IS NOT NULL) as employee_manage_ids,
 STRING_AGG(DISTINCT per.full_name,', ') AS employee_fullnames
FROM
hrm.doc_employee_send_train de
LEFT JOIN
hrm.doc_employee_send_train_table det ON de.id = det.owner_id
LEFT JOIN
hrm.doc_employee_send_train_signer des ON de.id = des.owner_id
LEFT JOIN
hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
public.hl_person per ON emp.person_id = per.id
LEFT JOIN public.enum_status sta ON de.status_id = sta.id
GROUP BY
de.id, de.doc_number, de.status_id, sta.full_name, de.doc_on, de.details 

UNION ALL

SELECT
de.id,
de.doc_number,
de.status_id,
de.details,
de.doc_on,
sta.full_name as status,
{TableIdConst.HRM__DOC_ORDER_TO_SEND_TO_BUSINESS_TRIP} AS tableid,
ARRAY_AGG(DISTINCT des.employee_manage_id) FILTER (WHERE des.employee_manage_id IS NOT NULL) as employee_manage_ids,
 STRING_AGG(DISTINCT per.full_name,', ') AS employee_fullnames
FROM
hrm.doc_order_to_send_business_trip de
LEFT JOIN
hrm.doc_order_to_send_business_trip_table det ON de.id = det.owner_id
LEFT JOIN
hrm.doc_order_to_send_business_trip_signer des ON de.id = des.owner_id
LEFT JOIN
hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
public.hl_person per ON emp.person_id = per.id
LEFT JOIN public.enum_status sta ON de.status_id = sta.id
GROUP BY
de.id, de.doc_number, de.status_id, sta.full_name, de.doc_on, de.details 

UNION ALL

SELECT
de.id,
de.doc_number,
de.status_id,
de.details,
de.doc_on,
sta.full_name as status,
{TableIdConst.DOC_EMPLOYEE_SEND_STUDY} AS tableid,
ARRAY_AGG(DISTINCT des.employee_manage_id) FILTER (WHERE des.employee_manage_id IS NOT NULL) as employee_manage_ids,
 STRING_AGG(DISTINCT per.full_name,', ') AS employee_fullnames
FROM
hrm.doc_employee_send_study de
LEFT JOIN
hrm.doc_employee_send_study_table det ON de.id = det.owner_id
LEFT JOIN
hrm.doc_employee_send_study_signer des ON de.id = des.owner_id
LEFT JOIN
hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
public.hl_person per ON emp.person_id = per.id
LEFT JOIN public.enum_status sta ON de.status_id = sta.id
GROUP BY
de.id, de.doc_number, de.status_id, sta.full_name, de.doc_on, de.details 
").Where(a => a.StatusId != StatusIdConst.DELETED && a.EmployeeManageIds.Any(emp => emp == _authService.User.EmployeeManageId.Value)).AsNoTracking();
            if (dto.StatusId.HasValue)
                query = query.Where(x => x.StatusId == dto.StatusId);

            if (dto.TableId.HasValue)
                query = query.Where(x => x.TableId == dto.TableId);

            if (dto.Employee != null)
                query = query.Where(a => dto.Employee != null || dto.Employee.Contains(a.Employee));

            return query;
        }
        else
        {
            hasError = true;
            AddError("Нет доступа");
            return Enumerable.Empty<HrmSignerVtView>().AsQueryable();
        }
    }
}