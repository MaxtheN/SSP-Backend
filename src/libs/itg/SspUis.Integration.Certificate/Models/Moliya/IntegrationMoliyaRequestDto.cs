using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Certificate
{
    public class IntegrationMoliyaRequestDto
    {
        public ContractorInfo contractor_info { get; set; }
        public Address address { get; set; }
        public Bank bank { get; set; }
        public ContractInfo contract_info { get; set; }
        public Certificate certificate { get; set; }
        public List<ContactGraph> contact_graphs { get; set; }
    }
    public class Address
    {
        public int region_id { get; set; }
        public string region { get; set; }
        public int district_id { get; set; }
        public string district { get; set; }
        public string address { get; set; }
    }

    public class Bank
    {
        public string bank_code { get; set; }
        public string bank_name { get; set; }
    }

    public class Certificate
    {
        public string doc_on { get; set; }
        public int certificate_number { get; set; }
        public string expire_on { get; set; }
        public string cancel_on { get; set; }
        public string certificate_link { get; set; }
        public int new_vacancies_count { get; set; }
    }

    public class ContactGraph
    {
        public int year_in { get; set; }
        public int month_in { get; set; }
        public int new_vacancies_count { get; set; }
    }

    public class ContractInfo
    {
        public int contractor_type_id { get; set; }
        public string contractor_type { get; set; }
        public string contractor_doc_on { get; set; }
        public int contractor_number { get; set; }
        public string contractor_link { get; set; }
    }

    public class ContractorInfo
    {
        public int inn { get; set; }
        public string name { get; set; }
        public string director { get; set; }
        public int oked { get; set; }
        public string oked_name { get; set; }
        public string guid { get; set; }
    }

}
