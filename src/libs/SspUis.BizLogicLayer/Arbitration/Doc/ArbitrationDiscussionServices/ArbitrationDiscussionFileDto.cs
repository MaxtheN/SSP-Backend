using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;

public class ArbitrationDiscussionFileDto :
	ArbitrationDiscussionFileDlDto,
	ILinkToEntity<ArbitrationDiscussionFile>
{
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }

}
