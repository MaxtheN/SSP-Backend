using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationCourtApplicationDlDto<TDto> : BaseApplicationDlDto<TDto, ArbitrationCourtApplication>
    where TDto : ArbitrationCourtApplicationDlDto<TDto>
{
    public ArbitrationCourtApplicationDlDto()
    {
        Application = new()
        {
            ApplicationTypeId = ApplicationTypeIdConst.ARBITRATION,
            ContractorPositionName = string.Empty
        };
    }

    public string ECourtNumber { get; set; }
    [LocalizedStringLength(50)]
    public string ContractorPhonber { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int ContractorResponsibleTypeId { get; set; }
    public long? ContractorId { get; set; }
    public string ResponsibleInn { get; set; }
    public string ContractorInn { get; set; }
    [LocalizedStringLength(250)]
    public string ContractorAddress { get; set; }
    [LocalizedRequired]
    public int OrganizationId { get; set; }
    [LocalizedStringLength(50)]
    public string ResponsiblePhonber { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int ResponsibleTypeId { get; set; }
    [LocalizedStringLength(250)]
    public string ResponsibleAddress { get; set; }
    [LocalizedRange(1, long.MaxValue)]
    public long? ResponsibleContractorId { get; set; }
    public decimal Amount { get; set; }
    public decimal ArbitrationAmount { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int CurrencyId { get; set; }
    public int ArbitrationApplicationTypeId { get; set; }
    public int? ArbitrationCourtId { get; set; }
    public DateTime? DiscussionDate { get; set; }
    public int? ArbitrationCourtResultId { get; set; }
    public decimal? AllocatedDivided { get; set; }
    public bool CanByDivided { get; set; }

    #region Foreign props
    public bool IsForeignContractor { get; set; } = false;
    public bool IsForeignResponsible { get; set; } = false;
    [LocalizedStringLength(50)]
    public string? ForeignContractorInn { get; set; }
    [LocalizedStringLength(50)]
    public string? ForeignResponsibleInn { get; set; }
    [LocalizedStringLength(500)]
    public string? ForeignContractorName { get; set; }
    [LocalizedStringLength(500)]
    public string? ForeignResponsibleName { get; set; }
    #endregion

    public List<ArbitrationCourtApplicationFileDlDto> Files { get; set; }
    public List<ArbitrationCourtApplicationSignerDlDto> Signer { get; set; }

    protected override Action<IMappingExpression<TDto, ArbitrationCourtApplication>> AlterMapping =>
            cfg => cfg
            .ForMember(x => x.Files, x => x.Ignore())
            .ForMember(x => x.Application, x => x.Ignore())
            .ForMember(x => x.Signer, x => x.Ignore());
    public override ArbitrationCourtApplication CreateEntity()
    {
        //var entity = base.CreateEntity();
        //entity.IsCreatedFromMy = true;
        ArbitrationCourtApplication entity;
        if (AlterCreateMapping != null)
        {
            entity = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                AlterCreateMapping(cfg.CreateMap<TDto, ArbitrationCourtApplication>());
            }).CreateMapper().Map<ArbitrationCourtApplication>((TDto)this);
        }
        else if (AlterMapping != null)
        {
            entity = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                AlterMapping(cfg.CreateMap<TDto, ArbitrationCourtApplication>());
            }).CreateMapper().Map<ArbitrationCourtApplication>((TDto)this);
        }
        else
        {
            entity = new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
            {
                cfg.CreateMap<TDto, ArbitrationCourtApplication>();
            }).CreateMapper().Map<ArbitrationCourtApplication>((TDto)this);
        }
        entity.Application = Application.CreateEntity();
        //var entity = _entity;
        if (!IsForeignContractor)
            entity.Application.ContractorId = ContractorId.Value;

        entity.Application.StatusId = StatusIdConst.CREATED;
        entity.Application.ApplicationTypeId = ApplicationTypeIdConst.ARBITRATION;
        //entity.Application.DocNumber = Application.DocNumber;
        entity.Application.TableId = TableIdConst.CLAIM__DOC_ARBITRATION_APPLICATION;
        entity.Application.CreatedUserId = (int?)ServiceProvider.AuthService.UserId;
        entity.CreatedUserId = (int?)ServiceProvider.AuthService.UserId;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_ARBITRATION_APPLICATION, Files.Select(a => a.Id).ToList());
        Signer.AddTo(entity.Signer);

        int i = 0;
        foreach (var item in entity.Files)
        {
            item.StepId = Files[i++].StepId;
        }
        return entity;
    }

    public override void UpdateEntity(ArbitrationCourtApplication entity)
    {
        //base.UpdateEntity(entity);
        //ArbitrationCourtApplication entity;
        //_entity = entity;
        MapperConfiguration mapperConfiguration = null;
        mapperConfiguration = ((AlterUpdateMapping != null) ? new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
        {
            AlterUpdateMapping(cfg.CreateMap<TDto, ArbitrationCourtApplication>());
        }) : ((AlterMapping == null) ? new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TDto, ArbitrationCourtApplication>();
        }) : new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
        {
            AlterMapping(cfg.CreateMap<TDto, ArbitrationCourtApplication>());
        })));
        mapperConfiguration.CreateMapper().Map((TDto)this, entity);


        Application.UpdateEntity(entity.Application);
        //var entity = _entity;
        if (!IsForeignContractor)
            entity.Application.ContractorId = ContractorId.Value;

        entity.Application.StatusId = StatusIdConst.MODIFIED;
        entity.Application.ApplicationTypeId = ApplicationTypeIdConst.ARBITRATION;
        //entity.Application.DocNumber = Application.DocNumber;
        entity.Application.TableId = TableIdConst.CLAIM__DOC_ARBITRATION_APPLICATION;
        entity.Application.ModifiedUserId = (int?)ServiceProvider.AuthService.UserId;
        entity.ModifiedUserId = (int?)ServiceProvider.AuthService.UserId;
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_ARBITRATION_APPLICATION, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        Signer.ApplyChangesTo<long, ArbitrationCourtApplicationSignerDlDto, ArbitrationCourtApplicationSigner>(entity.Signer);

        int i = 1;
        foreach (var item in entity.Signer)
        {
            item.SignOrder = i++;
            item.StepId = StepIdConst.NEED_DISCUSSION;
        }

        i = 0;
        foreach (var item in entity.Files)
        {
            item.StepId = Files[i++].StepId;
        }
    }
}
