using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.BizLogicLayer.Corruption;
using SspUis.Core.Security;
using SspUis.Core;

namespace SspUis.BizLogicLayer.HrmCorruption
{
    public class JoinAntiCorruptionResultListDtoConfig : PerDtoConfig<JoinAntiCorruptionResultListDto, JoinAntiCorruptionResult>
    {
        public override Action<IMappingExpression<JoinAntiCorruptionResult, JoinAntiCorruptionResultListDto>> AlterReadMapping =>
            cfg => cfg
               .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
               .TranslateText ?? ent.Status.FullName))

            .ForMember(x => x.CanAccept, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : StatusIdConst.CanApplyAntiCorruptionResultStatus(ent.StatusId, StatusIdConst.ACCEPTED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.JoinAntiCorruptionResultAccept) && ent.Signs.Any(x => x.SignedAt == null && x.SignUserId == (int)ServiceProvider.AuthService.UserId)))

            .ForMember(x => x.CanEdit, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : StatusIdConst.CanApplyAntiCorruptionResultStatus(ent.StatusId, StatusIdConst.MODIFIED)
                                  && ServiceProvider.AuthService.HasPermission(ModuleCode.JoinAntiCorruptionApplicationEdit)))

            .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : StatusIdConst.CanApplyAntiCorruptionResultStatus(ent.StatusId, StatusIdConst.CANCELED)
                            && ServiceProvider.AuthService.HasPermission(ModuleCode.JoinAntiCorruptionResultCancel)))

            .ForMember(x => x.CanDelete, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? false
                        : StatusIdConst.CanApplyAntiCorruptionResultStatus(ent.StatusId, StatusIdConst.DELETED)
                            && ServiceProvider.AuthService.HasPermission(ModuleCode.JoinAntiCorruptionResultDelete)))
            ;
    }
}


