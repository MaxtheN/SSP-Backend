using System;
using AutoMapper;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Doc.BaseApplication
{
    public class BaseApplicationDlDto<TDto, TApplicationEntity> : EntityDto<TDto, TApplicationEntity>
      where TDto : BaseApplicationDlDto<TDto, TApplicationEntity>
      where TApplicationEntity : class, IBaseApplicationEntity
    {
        public ApplicationDlDto Application { get; set; }

        protected override Action<IMappingExpression<TDto, TApplicationEntity>> AlterMapping =>
            cfg => cfg
                .ForMember(x => x.Application, x => x.Ignore());

        public override TApplicationEntity CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.Application = Application.CreateEntity();
            return entity;
        }

        public override void UpdateEntity(TApplicationEntity entity)
        {
            Application.UpdateEntity(entity.Application);
            base.UpdateEntity(entity);
        }
    }
}
