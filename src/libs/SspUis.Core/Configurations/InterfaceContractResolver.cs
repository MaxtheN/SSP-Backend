using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Core.Configurations
{
    public class InterfaceContractResolver : DefaultContractResolver
    {
        private readonly Type _InterfaceType;
        public InterfaceContractResolver(Type InterfaceType)
        {
            _InterfaceType = InterfaceType;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            //IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            IList<JsonProperty> properties = base.CreateProperties(_InterfaceType, memberSerialization);
            return properties;
        }
    }

    public class InterfaceContractResolver<T> : InterfaceContractResolver
    {
        public InterfaceContractResolver()
            : base(typeof(T))
        {

        }
    }
}
