using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfCode;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class TimesheetRepository : BaseEntityRepository<long, Timesheet, CreateTimesheetDlDto, UpdateTimesheetDlDto, UpdateStatusTimesheetDlDto>, ITimesheetRepository
{
    private readonly IAuthService _authService;
    private readonly ICrudServices _crudServices;
    private readonly IUnitOfWork _unitOfWork;

    public TimesheetRepository(
        ICrudServices crudServices,
        IUnitOfWork unitOfWork,
        IAuthService authService) : base(crudServices)
    {
        _authService = authService;
        _crudServices = crudServices;
        _unitOfWork = unitOfWork;
    }

    protected override IQueryable<Timesheet> ByIdQuery()
    {
        return base.ByIdQuery()
            .Include(a => a.Tables.Where(b => !b.IsDeleted))
            .ThenInclude(a => a.TableDays.Where(b => !b.IsDeleted))
            .AsSplitQuery();
    }

    protected override IQueryable<Timesheet> ByIdQuery(bool applyFilter)
    {
        return base.ByIdQuery(applyFilter)
            .Include(a => a.Tables.Where(b => !b.IsDeleted))
            .ThenInclude(a => a.TableDays.Where(b => !b.IsDeleted))
            .AsSplitQuery();
    }

    protected override void OnCreate(Timesheet entity, CreateTimesheetDlDto dto)
    {
        entity.OrganizationId = _authService.User.OrganizationId;

        SetEntityProperties(entity, dto);
    }

    protected override void OnUpdate(Timesheet entity, UpdateTimesheetDlDto dto)
    {
        SetEntityProperties(entity, dto);
    }

    public Timesheet Fill(UpdateTimesheetDlDto dto, Action<Timesheet> validation = null)
    {
        try
        {
            Timesheet entity = null;

            if (dto.Id == 0)
            {
                entity = dto.CreateEntity();

                if (IsValid)
                {
                    if (validation != null)
                        validation(entity);
                }

                entity.OrganizationId = _authService.User.OrganizationId;
                entity.MonthOn = new DateTime(dto.Year, dto.Month, 1);

                if (HasErrors)
                    return null!;

                DbSet.Add(entity);
                Context.Entry(entity).State = EntityState.Added;
            }
            else
            {
                entity = AllAsQueryable.FirstOrDefault(a => a.Id == dto.Id);

                if (IsValid)
                {
                    if (validation != null)
                        validation(entity);
                }

                if (HasErrors)
                    return null!;

                dto.UpdateEntityHead(entity);

                var entityForTables = dto.CreateEntity();
                foreach (var table in entityForTables.Tables)
                {
                    table.OwnerId = entity.Id;
                    table.TempTableDays = table.TableDays;
                    table.TableDays = new HashSet<TimesheetTableDay>();
                }

                string connectionString = Context.Database.GetConnectionString();
                var builder = new DbContextOptionsBuilder<EfCoreContext>();
                builder.UseNpgsql(connectionString);

                using (var dbContext = new EfCoreContext(builder.Options))
                {
                    dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                    dbContext.Config.AutoSetProperties.Enabled = false;
                    dbContext.AddRange(entityForTables.Tables);
                    dbContext.SaveChanges();
                }

                var tableDays = entityForTables.Tables.SelectMany(a => a.TempTableDays.Select(b =>
                {
                    b.OwnerId = a.Id;
                    return b;
                }));

                DataTable dtTimeSheetTableDay = new DataTable("doc_timesheet_table_day", "hrm");
                dtTimeSheetTableDay.Columns.Add("owner_id", typeof(long));
                dtTimeSheetTableDay.Columns.Add("timesheet_indicator_id", typeof(int));
                dtTimeSheetTableDay.Columns.Add("date_on", typeof(DateOnly));
                dtTimeSheetTableDay.Columns.Add("plan_days", typeof(int));
                dtTimeSheetTableDay.Columns.Add("plan_hours", typeof(decimal));
                dtTimeSheetTableDay.Columns.Add("fact_days", typeof(int));
                dtTimeSheetTableDay.Columns.Add("fact_hours", typeof(decimal));
                dtTimeSheetTableDay.Columns.Add("day_off_hours", typeof(decimal));
                dtTimeSheetTableDay.Columns.Add("night_hours", typeof(decimal));
                dtTimeSheetTableDay.Columns.Add("hourly", typeof(decimal));
                dtTimeSheetTableDay.Columns.Add("maintenance_hours", typeof(decimal));

                foreach (var item1 in tableDays)
                {
                    DataRow drTimeSheetTableDay = dtTimeSheetTableDay.NewRow();
                    drTimeSheetTableDay["owner_id"] = item1.OwnerId;
                    drTimeSheetTableDay["timesheet_indicator_id"] = item1.TimesheetIndicatorId;
                    drTimeSheetTableDay["date_on"] = item1.DateOn;
                    drTimeSheetTableDay["plan_days"] = item1.PlanDays;
                    drTimeSheetTableDay["plan_hours"] = item1.PlanHours;
                    drTimeSheetTableDay["fact_days"] = item1.FactDays;
                    drTimeSheetTableDay["fact_hours"] = item1.FactHours;
                    drTimeSheetTableDay["day_off_hours"] = item1.DayOffHours;
                    drTimeSheetTableDay["night_hours"] = item1.NightHours;
                    drTimeSheetTableDay["hourly"] = item1.Hourly ?? (object)DBNull.Value;
                    drTimeSheetTableDay["maintenance_hours"] = item1.MaintenanceHours ?? (object)DBNull.Value;

                    dtTimeSheetTableDay.Rows.Add(drTimeSheetTableDay);
                }

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (var writer = conn.BeginBinaryImport("COPY hrm.doc_timesheet_table_day (owner_id, timesheet_indicator_id, date_on, plan_days, " +
                        "plan_hours, fact_days, fact_hours, day_off_hours, night_hours, hourly, maintenance_hours) FROM STDIN (FORMAT BINARY)"))
                    {
                        foreach (DataRow row in dtTimeSheetTableDay.Rows)
                        {
                            writer.StartRow();
                            foreach (object item in row.ItemArray)
                            {
                                writer.Write(item);
                            }
                        }
                        writer.Complete();
                    }
                }

                if (HasErrors) return null!;
                Context.Entry(entity).State = EntityState.Modified;
            }
            return entity;
        }
        catch(Exception ex)
        {
            AddError(ex.Message);
            return null!;
        }
    }
    
    public TimesheetTableWithDaysDlDto UpdateTable(TimesheetTableWithDaysDlDto dto)
    {
        var entity = _crudServices.Context.Set<TimesheetTable>().Include(a => a.TableDays)
                .FirstOrDefault(a => a.Id == dto.Id);

        dto.OwnerId = entity.OwnerId;
        dto.UpdateEntity(entity!);

        var timeSheetIndicatorIds = _crudServices.Context.Set<TimesheetIndicator>().IsActive().Select(a => a.Id);

        var workDays = dto.TableDays.Where(a => timeSheetIndicatorIds.Contains(a.TimesheetIndicatorId)).ToList();
        var factHours = workDays.Any() ? workDays.Sum(a => a.FactHours) : 0;
        var factDays = workDays.Any() ? workDays.Sum(a => a.FactDays) : 0;

        entity.FactHours = factHours;
        entity.FactDays = factDays;

        return dto;
    }

    public void ClearTables(long id)
    {
        var entity = AllAsQueryable
                        .Include(a => a.Tables.Where(b => !b.IsDeleted))
                        .FirstOrDefault(a => a.Id == id);

        if (entity != null)
        {
            foreach (var table in entity.Tables)
                table.MarkAsDeleted();
            Context.Entry(entity).State = EntityState.Modified;
        }
    }

    private void SetEntityProperties<TDto>(Timesheet entity, TimesheetDlDto<TDto> dto)
           where TDto : TimesheetDlDto<TDto>
    {
        entity.MonthOn = new DateTime(dto.Year, dto.Month, 1);
    }

    protected override IQueryable<Timesheet> InjectFilter(IQueryable<Timesheet> query)
        => query.Where(a => a.OrganizationId == _authService.User.OrganizationId && a.StatusId != StatusIdConst.DELETED);
}
