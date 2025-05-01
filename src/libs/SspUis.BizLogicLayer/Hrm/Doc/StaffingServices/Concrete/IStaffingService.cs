using Microsoft.AspNetCore.Http;
using SspUis.DataLayer.Repositories.Hrm;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public interface IStaffingService : IStatusGeneric
    {
        PagedResult<StaffingListDto> GetList(StaffingSortFilterPageOptions dto);
        PagedResult<StaffingListDto> GetListForHeader(StaffingSortFilterPageOptions dto);
        PagedResult<StaffingListDto> GetListForOwnOrg(StaffingSortFilterPageOptions dto);
        StaffingPostionForNow GetAllStaffingPositionsForQuantity(int positionId, int departmentId, int? organizationId);
        IEnumerable<StaffingPositionDto> GetAllStaffingPositions(DateTime? date = null, int? positionId = null, int? positionClassificationId = null, int? departmentId = null, int? organizationId = null);
        IEnumerable<PositionDto> GetAllStaffingPositions(DateTime? date = null, int? departmentId = null);
        IEnumerable<PositionDto> GetAllStaffingPositionClassifications(DateTime? date = null, int? positionClassificationId = null, int? departmentId = null);
        IEnumerable<StaffingPositionDto> FillStaffingPosition(int staffingTemplateId);
        StaffingPositionDto GetStaffingPosition(GetStaffingPositionDto dto);
        StaffingPositionDto RecalcStaffingCalcKindTables(StaffingPositionDto dto);
        StaffingDto Get();
        IEnumerable<StaffingIndicatorValueDto> FillIndicator(CreateStaffingDlDto dto);
        StaffingDto Get(long id);
        StaffingDto GetClone(long id);
        SelectList<long> AsSelectList();
        HaveId<long> Create(CreateStaffingDlDto dto, bool autoCommit = true);
        void Accept(UpdateStatusStaffingDto dto);
        void Cancel(UpdateStatusStaffingDto dto);
        void Revoke(UpdateStatusStaffingDto dto);
        void Receieved(UpdateStatusStaffingDto dto);
        void Reject(UpdateStatusStaffingDto dto);
        void Send(long id);
        void Update(UpdateStaffingDlDto dto);
        void Delete(long id);
        void SendToArchive(long id);
        void RecallFromArchive(long id);
        IEnumerable<StaffingDtoForChamber> GetPositionsForChamber(int? langId);
    }
}
