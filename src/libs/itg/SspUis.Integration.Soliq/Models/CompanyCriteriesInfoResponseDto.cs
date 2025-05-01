using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class CompanyCriteriesInfoResponseDto
    {
        public bool success { get; set; }
        public string reason { get; set; }
        public CompanyCriteriesData data { get; set; }
    }
    public class CompanyCriteriesData
    {
        public int criteriaAll { get; set; }
        public string type { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
       
    }

    public class Critery
    {
        public int id { get; set; }
        public string nameUz { get; set; }
        public string nameLat { get; set; }
        public string nameRu { get; set; }
        public string methodsUz { get; set; }
        public string methodsLat { get; set; }
        public string methodsRu { get; set; }
        public string procedureUz { get; set; }
        public string procedureLat { get; set; }
        public string procedureRu { get; set; }
        public string eliminateUz { get; set; }
        public string eliminateLat { get; set; }
        public string eliminateRu { get; set; }
        public int? vatPoint { get; set; }
        public int? stPoint { get; set; }
        public int? minusPoint { get; set; }
        public int ord { get; set; }
        public int header { get; set; }
        public int state { get; set; }
        public int active { get; set; }
        public int cabinet { get; set; }
        public string dscr { get; set; }
        public int point { get; set; }
        public string tin { get; set; }
        public int maxPoint { get; set; }
        public bool isPrivilege { get; set; }
    }
    public class CompanyInfo
    {
        public SoliqContractorByTinOkedDetail OkedDetail { get; set; }
        public string pkey { get; set; }
        public string name { get; set; }
        public string tin { get; set; }
        public int criteriaAll { get; set; }
        public string nameUz { get; set; }
        public string nameRu { get; set; }
        public string nameLat { get; set; }
        public string type { get; set; }
        public int? taxpayerType { get; set; }
        public int soato { get; set; }
        public int regionID { get; set; }
        public string regionNameUz { get; set; }
        public string regionNameRu { get; set; }
        public string regionNameLat { get; set; }
        public int districtID { get; set; }
        public string districtNameUz { get; set; }
        public string districtNameRu { get; set; }
        public string districtNameLat { get; set; }
        public int levelTo { get; set; }
        public int levelFrom { get; set; }
        public int maxPoint { get; set; }
        public string taxpayername { get; set; }
        public string taxpayername_ru { get; set; }
        public string taxpayername_uz_cyrl { get; set; }
        public string taxpayer_name_uz_latn { get; set; }
    }
}
