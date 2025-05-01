using SspUis.Integration.Bojxona.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bojxona.Services
{
    public interface IBojxonaService: IStatusGeneric
    {
        Task<List<GetGTDByInnDataDto>> GetGTDByInn(GetGTDByInnRequestDto dto);
    }
}
