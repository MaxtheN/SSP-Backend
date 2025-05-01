/*using FastMember;
using UzAvtoYul.Core.OpenXML.Attibutes;
using UzAvtoYul.Core.OpenXML.ActionResults;
using System;
using System.IO;
using System.Reflection;

namespace SspUis.BizLogicLayer.ReportServices
{
    public interface IWordTemplate
    {
        void SetWorker(WordprocessingWorker worker, string prefix = "");
        
        WordResult AsWordResult();
    }

    public abstract class WordTemplate : IWordTemplate
    {
*//*
        public static WordResult AsWordResult(IWordTemplate wordTemplate, string fileDownloadName)
        {
            return new WordResult(AsWordWorker(wordTemplate), fileDownloadName);
        }
        public static WordResult AsWordResult(IWordTemplate wordTemplate, string fileDownloadName,string templateName)
        {
            return new WordResult(AsWordWorker(wordTemplate,templateName), fileDownloadName);
        }*//*
        public static WordprocessingWorker AsWordWorker(IWordTemplate wordTemplate)
        {
            var templateNameAttr = wordTemplate.GetType().GetCustomAttributes(typeof(TemplateNameAttribute), false)[0] as TemplateNameAttribute;

            return WordTemplate.AsWordWorker(wordTemplate, templateNameAttr.Name);
        }
       
        public static WordprocessingWorker AsWordWorker(IWordTemplate wordTemplate, string templateName)
        {
            return AsWordWorker(wordTemplate, Path.Combine(Settings.WordTemplatesFolder, CultureHelper.CurrentCultureName), templateName);
        }
*//*
        public static WordprocessingWorker AsWordWorker(IWordTemplate wordTemplate, string templatePath, string templateName)
        {
            var sourcePath = HttpContext.Current.Server.MapPath(string.IsNullOrEmpty(templateName) ? templatePath : Path.Combine(templatePath, templateName));
            var worker = new WordprocessingWorker(sourcePath);

            wordTemplate.SetWorker(worker);
            worker.Save();

            return worker;
        }*//*

        public static void SetWorker(IWordTemplate wordTemplate, WordprocessingWorker worker, string prefix = "")
        {
            Type type = wordTemplate.GetType();
            TypeAccessor accessor = TypeAccessor.Create(type);
            string iWordTemplate = "IWordTemplate";

            foreach (var member in accessor.GetMembers())
            {
                if (member.Type.GetInterface(iWordTemplate) != null)
                {
                    MethodInfo setWorker = member.Type.GetMethod("SetWorker");

                    setWorker.Invoke(accessor[wordTemplate, member.Name], new object[] { worker, string.Concat(prefix, member.Name, ".") });
                }
            }
        }

        public abstract WordResult AsWordResult();

        public WordResult AsWordResult(string fileDownloadName)
        {
            return WordTemplate.AsWordResult(this, fileDownloadName);
        }
        *//*public WordResult AsWordResult(string fileDownloadName,string templateName)
        {
            return WordTemplate.AsWordResult(this, fileDownloadName,templateName);
        }*//*
        public WordprocessingWorker AsWordWorker()
        {
            return WordTemplate.AsWordWorker(this);
        }

        public WordprocessingWorker AsWordWorker(string templateName)
        {
            return WordTemplate.AsWordWorker(this, templateName);
        }

        public WordprocessingWorker AsWordWorker(string templatePath, string templateName)
        {
            return WordTemplate.AsWordWorker(this, templatePath, templateName);
        }

        public virtual void SetWorker(WordprocessingWorker worker, string prefix = "")
        {
            WordTemplate.SetWorker(this, worker, prefix);
        }
    }
}*/