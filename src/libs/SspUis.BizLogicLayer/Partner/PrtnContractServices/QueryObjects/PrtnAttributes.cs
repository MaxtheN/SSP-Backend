using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class PrtnAttributes : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || !(value is IList list))
                return false;

            return list.Count > 0;
        }
        public PrtnAttributes()
        {
            base.ErrorMessage = "Файл {0} бириктирилмаган / Файл {0} не прикреплен";
        }
    }
}
