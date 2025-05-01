using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class Utils
    {

        public static string GetProperyValue(Type type, object obj, string propertyName)
        {
            if (obj == null)
                return string.Empty;
            int index = propertyName.IndexOf(".");

            if (index == -1)
            {
                if (propertyName.EndsWith("()"))
                {
                    string methodName = propertyName.Substring(0, propertyName.Length - 2);
                    MethodInfo method = type.GetMethod(methodName);
                    object result = method.Invoke(obj, null);

                    return result == null ? string.Empty : result.ToString();
                }
                else
                {
                    string format = null;
                    int doublePointIndexOf = propertyName.IndexOf(':');
                    PropertyInfo property = null;
                    string error = null;

                    if (doublePointIndexOf == -1 && TryGetProperty(type, propertyName, out property, ref error))
                    {
                        var displayFormatAttr = (DisplayFormatAttribute[])property.GetCustomAttributes(typeof(DisplayFormatAttribute), false);
                        format = displayFormatAttr.Length > 0 ? displayFormatAttr[0].DataFormatString : WordConstants.DEFAULT_FORMAT;
                    }
                    else if (doublePointIndexOf != -1 && TryGetProperty(type, propertyName.Substring(0, doublePointIndexOf), out property, ref error))
                        format = string.Format(WordConstants.CUSTOM_FORMAT, propertyName.Substring(doublePointIndexOf, propertyName.Length - doublePointIndexOf));

                    return error == null ? string.Format(format, property.GetValue(obj, null) ?? string.Empty) : error;
                }
            }
            else
            {
                PropertyInfo prop = type.GetProperty(propertyName.Substring(0, index));
                string innerPropertyName = propertyName.Substring(index + 1, propertyName.Length - index - 1);

                return GetProperyValue(prop.PropertyType, prop.GetValue(obj, null), innerPropertyName);
            }
        }

        public static bool TryGetProperty(Type type, string propertyName, out PropertyInfo result, ref string errorMessage)
        {
            result = type.GetProperty(propertyName);

            if (result == null)
            {
                errorMessage = string.Format("Property \"{0}\" not found in type \"{1}\"", propertyName, type.Name);
                return false;
            }
            else if (result.GetCustomAttributes(typeof(IgnoreAttribute), false).Length > 0)
            {
                errorMessage = string.Format("\"{0}\" in type \"{1}\" is ignore property");
                return false;
            }

            return true;
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties().Where(a => a.GetCustomAttributes(typeof(IgnoreAttribute), false).Length == 0);
        }
/*        public static void exportExcel(System.Data.DataTable data, string reportName)
        {
            var wb = new XLWorkbook();

            // Add DataTable as Worksheet
            var ws = wb.Worksheets.Add(data, "Sheet1");
            // Create Response
            HttpResponse response = HttpContext.Current.Response;

            //Prepare the response
            response.Clear();
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".xlsx");

            //Flush the workbook to the Response.OutputStream
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(response.OutputStream);
                MyMemoryStream.Close();
            }

            response.End();
        }
        public static void exportExcel(System.Data.DataTable data, string reportName,bool customize)
        {
            var wb = new XLWorkbook();

            // Add DataTable as Worksheet
            var ws = wb.Worksheets.Add("Sheet1");
            // Create Response
            HttpResponse response = HttpContext.Current.Response;

            //Prepare the response
            response.Clear();
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".xlsx");

            //Flush the workbook to the Response.OutputStream
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(response.OutputStream);
                MyMemoryStream.Close();
            }

            response.End();
        }
*/
    }
}