using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;

public class ArbitrationResultFileDto :
	ArbitrationResultFileDlDto,
	ILinkToEntity<ArbitrationResultFile>
{
    public DateTime CreatedAt { get; set; }
}
