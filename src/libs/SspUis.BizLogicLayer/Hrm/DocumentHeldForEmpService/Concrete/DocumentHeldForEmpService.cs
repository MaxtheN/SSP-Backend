using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;
public class DocumentHeldForEmpService : StatusGenericHandler, IDocumentHeldForEmpService
{
    private readonly DbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public DocumentHeldForEmpService(IUnitOfWork unitOfWork, IAuthService authService, DbContext context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _authService = authService;
    }
    public PagedResult<HrmEmpVtView> GetData(DocumentHeldForEmpFilterOption dto)
    {
        bool hasError = false;
        var query = GetQuery(dto, out hasError);
        if (hasError)
            return null;

        return query.AsPagedResult(dto);
    }

    public IQueryable<HrmEmpVtView> GetQuery(DocumentHeldForEmpFilterOption dto, out bool hasError)
    {
        hasError = false;

        if (dto.PersonId != null)
        {
            var query = _context.Set<HrmEmpVtView>().FromSqlRaw($@"
SELECT
    de.id,
    de.doc_number,
    de.status_id,
    de.details,
    de.doc_on,
    sta.full_name as status,
    {TableIdConst.DOC_APPOINT_EMPLOYEE} AS tableid,
    ARRAY_AGG(DISTINCT det.employee_manage_id) FILTER (WHERE det.employee_manage_id IS NOT NULL) as employee_manage_ids,
    ARRAY_AGG(DISTINCT per.id) FILTER (WHERE per.id IS NOT NULL) as person_ids,
    STRING_AGG(DISTINCT per.full_name, ', ') AS employee_fullnames
FROM
    hrm.doc_appoint_employee de
LEFT JOIN
    hrm.doc_appoint_employee_table det ON de.id = det.owner_id
LEFT JOIN
    hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
    public.hl_person per ON emp.person_id = per.id
LEFT JOIN
    public.enum_status sta ON de.status_id = sta.id
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
    ARRAY_AGG(DISTINCT det.employee_manage_id) FILTER (WHERE det.employee_manage_id IS NOT NULL) as employee_manage_ids,
	ARRAY_AGG(DISTINCT per.id) FILTER (WHERE per.id IS NOT NULL) as person_ids,
    STRING_AGG(DISTINCT per.full_name, ', ') AS employee_fullnames
FROM
    hrm.doc_chastisement de
LEFT JOIN
    hrm.doc_chastisement_table det ON de.id = det.owner_id
LEFT JOIN
    hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
    public.hl_person per ON emp.person_id = per.id
LEFT JOIN
    public.enum_status sta ON de.status_id = sta.id
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
    ARRAY_AGG(DISTINCT det.employee_manage_id) FILTER (WHERE det.employee_manage_id IS NOT NULL) as employee_manage_ids,
	ARRAY_AGG(DISTINCT per.id) FILTER (WHERE per.id IS NOT NULL) as person_ids,
    STRING_AGG(DISTINCT per.full_name, ', ') AS employee_fullnames
FROM
    hrm.doc_employee_leave_order de
LEFT JOIN
    hrm.doc_employee_leave_order_table det ON de.id = det.owner_id
LEFT JOIN
    hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
    public.hl_person per ON emp.person_id = per.id
LEFT JOIN
    public.enum_status sta ON de.status_id = sta.id
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
    ARRAY_AGG(DISTINCT manage.id) FILTER (WHERE manage.id IS NOT NULL AND manage.end_on IS NULL AND manage.is_deleted IS false) as employee_manage_ids,
    ARRAY_AGG(DISTINCT per.id) FILTER (WHERE per.id IS NOT NULL) as person_ids,
    STRING_AGG(DISTINCT per.full_name, ', ') AS employee_fullnames
FROM
    hrm.doc_employee_send_train de
LEFT JOIN
    hrm.doc_employee_send_train_table det ON de.id = det.owner_id
LEFT JOIN
    hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
    hrm.sys_employee_manage manage ON emp.id = manage.employee_id 
LEFT JOIN
    public.hl_person per ON emp.person_id = per.id
LEFT JOIN
    public.enum_status sta ON de.status_id = sta.id
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
    ARRAY_AGG(DISTINCT det.employee_manage_id) FILTER (WHERE det.employee_manage_id IS NOT NULL) as employee_manage_ids,
	ARRAY_AGG(DISTINCT per.id) FILTER (WHERE per.id IS NOT NULL) as person_ids,
    STRING_AGG(DISTINCT per.full_name, ', ') AS employee_fullnames
FROM
    hrm.doc_order_to_send_business_trip de
LEFT JOIN
    hrm.doc_order_to_send_business_trip_table det ON de.id = det.owner_id
LEFT JOIN
    hrm.hl_employee emp ON det.employee_id = emp.id
LEFT JOIN
    public.hl_person per ON emp.person_id = per.id
LEFT JOIN
    public.enum_status sta ON de.status_id = sta.id
GROUP BY
    de.id, de.doc_number, de.status_id, sta.full_name, de.doc_on, de.details").Where(a => a.StatusId == StatusIdConst.ACCEPTED).AsNoTracking();

            if (dto.StatusId.HasValue)
                query = query.Where(x => x.StatusId == dto.StatusId);

            if (dto.TableId.HasValue)
                query = query.Where(x => x.TableId == dto.TableId);

            if (dto.PersonId.HasValue)
                query = query.Where(x => x.PersonIds.Any(a => a == dto.PersonId.Value));

            return query;
        }
        else
        {
            hasError = true;
            AddError("Нет доступа");
            return Enumerable.Empty<HrmEmpVtView>().AsQueryable();
        }
    }
}