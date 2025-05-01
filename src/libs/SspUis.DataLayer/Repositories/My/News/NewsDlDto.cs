using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NewsDlDto<TDto> : EntityDto<TDto, News>
        where TDto : NewsDlDto<TDto>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortContent { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public List<NewsTranslateDlDto> Translates { get; set; } = new();
        public List<UpdateTagDlDto> Tags { get; internal set; } = new();
        public NewsImageDlDto Image { get; set; }
        protected override Action<IMappingExpression<TDto, News>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore())
            .ForMember(x => x.Tags, x => x.Ignore())
            .ForMember(x => x.Image, x => x.Ignore());

        public override News CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            entity.Tags.AddFromForeignKeys(Tags.Select(a => a.Id).ToList());
            if (Image != null)
            {
                var newsImages = new List<NewsImage>();
                //if (entity.Image != null)
                //{
                //    newsImages.Add(entity.Image);
                //}
                newsImages.AddFromTempFiles(DocumentStorageConst.NEWS_IMAGE, new List<Guid> { Image.Id });
                entity.Image = newsImages[0];
            }
            return entity;
        }

        public override void UpdateEntity(News entity)
        {
            base.UpdateEntity(entity);
            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
            entity.Tags.UpdateFromForeignKeys(Tags.Select(a => a.Id).ToList());
            if (Image != null)
            {
                var newsImages = new List<NewsImage>();
                if (entity.Image != null)
                {
                    newsImages.Add(entity.Image);
                }
                newsImages.UpdateFromFiles(DocumentStorageConst.NEWS_IMAGE, entity.Id.ToString(), new List<Guid> { Image.Id });
                entity.Image = newsImages[0];
            }
        }
    }

}
