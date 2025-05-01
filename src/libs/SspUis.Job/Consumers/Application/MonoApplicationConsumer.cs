using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.BizLogicLayer.MonoApplicationServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Application.Messages;
using SspUis.RabbitMQ.Doc.Exceptions;
using SspUis.RabbitMQ.Models;
using StatusGeneric;

namespace SspUis.RabbitMQ.Application.Consumers
{
    public class MonoApplicationConsumerConfig : IConsumerConfig<MonoApplicationMessage>
    {
        public string Name => "MonoApplicationSync";
        public RabbitQueue Queue => Queues.Mono.MonoApplication;
        public ushort PrefetchCount { get; set; } = 1;
        public int WorkerCount { get; set; } = 1;
        public bool RequeueOnFailed { get; set; }

    }

    public class MonoApplicationConsumer : StatusGenericHandler, IConsumer<MonoApplicationMessage>
    {
        private readonly IMonoApplicationService _monoapplicationService;
        private readonly IDocumentJobHistoryService _documentJobHistoryService;
        private readonly IAuthService _authService;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContext _dbContext;

        public MonoApplicationConsumer(IMonoApplicationService monoapplicationService,
            IDocumentJobHistoryService documentJobHistoryService,
            IAuthService authService,
            ILogger<MonoApplicationConsumer> logger,
            IServiceProvider serviceProvider,
            DbContext dbContext)
        {
            _monoapplicationService = monoapplicationService;
            _documentJobHistoryService = documentJobHistoryService;
            _authService = authService;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
        }

        public async Task ConsumeAsync(MonoApplicationMessage message, CancellationToken cancellationToken)
        {
            var jobHistory = _documentJobHistoryService.Start(message.JobHistoryId);
            CombineStatuses(_documentJobHistoryService);
            if (HasErrors)
                throw new Exception(GetAllErrors());

            var processName = "";
            var table = _dbContext.Set<Table>().FromSqlRaw($"SELECT * FROM sys_table WHERE ID={message.TableId}")?.FirstOrDefault();

            if (table == null)
                throw new Exception($"SYS_TABLE не найдена по ID='{message.TableId}'");

            try
            {
                //_authService.ResetUserName(message.UserName, message.OrganizationId);
                _authService.ResetUserName("webaseadmin");

                // update doc status as EXECUTING and clear messasge
                _dbContext.Database.ExecuteSqlRaw($"UPDATE {table.DbSchemaName}.{table.DbTableName} SET STATUS_ID={StatusIdConst.SENDING}, MESSAGE='' WHERE ID={message.DocId}");

                await _monoapplicationService.SentForReview(message.DocId, message.UserIp, message.UserAgent);
                CombineStatuses(_monoapplicationService);

                if (IsValid)
                    _documentJobHistoryService.End(jobHistory.Id, succeed: true);
                else
                {
                    _documentJobHistoryService.End(jobHistory.Id, succeed: false);
                    throw new Exception(GetAllErrors());
                }
            }
            catch (CheckStatusdException ex)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var documentJobHistoryService = scope.ServiceProvider.GetRequiredService<IDocumentJobHistoryService>();
                    documentJobHistoryService.End(jobHistory.Id, succeed: true, errorMessage: $"Успешно завершен процесс «{processName}». Но не удалось проверить статус после завершения процесса.");
                }
            }
            catch (InvalidStatusChangedException ex)
            {
                _logger.LogInformation($"Завершен процесс '{processName}' с неожиданным результатом. {ex.Message}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
                    var documentJobHistoryService = scope.ServiceProvider.GetRequiredService<IDocumentJobHistoryService>();
                    using (var command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // set doc message
                        dbContext.Database.ExecuteSqlRaw($"UPDATE {table.DbSchemaName}.{table.DbTableName} SET MESSAGE='{ex.Message.SafeSubstring(800)}' WHERE ID={message.DocId}");
                        documentJobHistoryService.End(jobHistory.Id, succeed: false, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Не удалось «{processName}». Ошибка: {ex.Message}".SafeSubstring(800);
                _logger.LogInformation(errorMessage);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
                    var jobHistoryService = scope.ServiceProvider.GetRequiredService<IDocumentJobHistoryService>();
                    using (var command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // reset doc previous status 
                        string sql = $"UPDATE {table.DbSchemaName}.{table.DbTableName} SET STATUS_ID=PREV_STATUS_ID, PREV_STATUS_ID=NULL, MESSAGE=:documentMessage WHERE ID={message.DocId}";
                        dbContext.Database.ExecuteSqlRaw(sql, new NpgsqlParameter("documentMessage", errorMessage));
                        jobHistoryService.End(jobHistory.Id, succeed: false, ex.Message);
                    }
                }
            }

            await Task.CompletedTask;

        }
    }
}
