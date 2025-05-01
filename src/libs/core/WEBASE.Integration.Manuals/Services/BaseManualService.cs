using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBASE.Integration.Manuals.Services
{
    public class BaseManualService<T>
        where T : class
    {
        private readonly string fileName;

        protected List<T> Data { get; set; }

        public BaseManualService(string fileName)
        {
            this.fileName = fileName;
            LoadData();
        }

        private void LoadData()
        {
            var file = $"app_data/{fileName}";
#if DEBUG
            file = $"../WEBASE.Integration.Manuals/{file}";
#endif
            var json = File.ReadAllText(file, Encoding.UTF8);
            Data = JsonConvert.DeserializeObject<List<T>>(json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
