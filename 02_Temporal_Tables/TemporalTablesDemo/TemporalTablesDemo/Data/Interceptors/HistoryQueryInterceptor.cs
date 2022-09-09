using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace TemporalTablesDemo.Data.Interceptors
{
    public class HistoryQueryInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            ManipulateCommand(command);
            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            ManipulateCommand(command);
            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }

        private void ManipulateCommand(DbCommand command)
        {
            if (command.CommandText.StartsWith($"-- {nameof(HistoryQueryInterceptor)}");
            {
                //[c].[Id], [c].[Changes], [c].[Name], [c].[PeriodEnd], 
                //  [c].[PeriodStart], [e].[Id], [e].[Changes], [e].[CompanyId], 
                //  [e].[Department], [e].[FirstName], [e].[LastName], 
                //  [e].[PeriodEnd], 
                //  [e].[PeriodStart], [e].[Test]
                //FROM[Companies] FOR SYSTEM_TIME ALL AS[c]
                //LEFT JOIN[Employees] FOR SYSTEM_TIME ALL AS[e] ON[c].[Id] = [e].[CompanyId]
                //WHERE[c].[Id] = 2
                //ORDER BY[c].[Id]
                Console.WriteLine(command.CommandText);
                command.CommandText = command.CommandText
                 .Replace("[Companies]", "[Companies] FOR SYSTEM_TIME ALL ")
                 .Replace("[Employees]", "[Employees] FOR SYSTEM_TIME ALL ");
            }
        }
    }
}
