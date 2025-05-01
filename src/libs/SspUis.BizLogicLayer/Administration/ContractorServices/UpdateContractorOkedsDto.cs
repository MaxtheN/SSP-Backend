using SspUis.BizLogicLayer.OkedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ContractorServices
{
    public class UpdateContractorOkedsDto
    {
        public UpdateContractorOkedsDto()
        {
            Okeds = new();
        }

        public List<UpdateContractorOkedDto> Okeds { get; set; }
    }

    public class UpdateContractorOkedDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
