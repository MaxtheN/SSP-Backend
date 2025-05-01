using System;

namespace SspUis.BizLogicLayer.ReportServices
{

    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateNameAttribute : Attribute
    {

        public TemplateNameAttribute(string name)
        {
            _name = name;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
        }
        

    }

}