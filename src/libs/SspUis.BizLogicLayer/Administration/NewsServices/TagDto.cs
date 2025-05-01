using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.NewsServices
{
    public class TagDto : UpdateTagDlDto, ILinkToEntity<Tag>
    {
    }
}
