using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories;

public interface IBaseApplication<TApplication>
where TApplication : IDocument<long>
{
    long Id { get; set; }
    TApplication Application { get; set; }
}


public interface IBaseApplicationEntity : IBaseApplication<Application>
{

}
