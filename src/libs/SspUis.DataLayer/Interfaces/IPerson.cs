using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.Interfaces
{
    public interface IPerson
    {
        string Pinfl { get; }
        string SurnameLatin { get; set; }
        string NameLatin { get; }
        string PatronymLatin { get; }
        string SurnameEng { get; }
        string NameEng { get; }
    }
}
