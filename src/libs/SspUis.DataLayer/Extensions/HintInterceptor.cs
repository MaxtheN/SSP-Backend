using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SspUis.DataLayer
{
    public class HintInterceptor : DbCommandInterceptor
    {
        private static readonly Regex _tableAliasRegex = new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(*HINT*\)))", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        [ThreadStatic]
        public static string HintValue;

        //private static string Replace(string input)
        //{
        //    if (!String.IsNullOrWhiteSpace(HintValue))
        //    {
        //        if (!_tableAliasRegex.IsMatch(input))
        //        {
        //            throw new InvalidProgramException("Could not identify a table to be marked for update!", new Exception(input));
        //        }
        //        input = _tableAliasRegex.Replace(input, "${tableAlias} WITH (*HINT*)");
        //        input = input.Replace("*HINT*", HintValue);
        //    }
        //    HintValue = String.Empty;
        //    return input;
        //}

        private static string Replace(string input)
        {
            if (!String.IsNullOrWhiteSpace(HintValue))
            {
                input = String.Concat(input, " ", HintValue);
            }
            HintValue = String.Empty;
            return input;
        }

        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            command.CommandText = Replace(command.CommandText);
            return base.ScalarExecuting(command, eventData, result);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            command.CommandText = Replace(command.CommandText);
            return base.ReaderExecuting(command, eventData, result);
        }

    }
}
