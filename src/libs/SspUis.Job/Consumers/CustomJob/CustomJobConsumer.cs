using Microsoft.Extensions.Logging;
using StatusGeneric;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Models;
using SspUis.RabbitMQ.CustomJob.Messages;
using SspUis.BizLogicLayer.CustomJobServices;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.Core.Security;
using WEBASE;
using Npgsql;
using Microsoft.Extensions.DependencyInjection;
using SspUis.RabbitMQ.Doc.Exceptions;
using SspUis.RabbitMQ.Messages;
using SspUis.RabbitMQ.CustomJob.Messages;
using SspUis.BizLogicLayer.CustomJobServices;
using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners;

namespace SspUis.RabbitMQ.customJob.Consumers
{
    public class CustomJobConsumerConfig : IConsumerConfig<CustomJobMessage>
    {
        public string Name => "CustomJobSync";
        public RabbitQueue Queue => Queues.Job.CustomJob;
        public ushort PrefetchCount { get; set; } = 1;
        public int WorkerCount { get; set; } = 1;
        public bool RequeueOnFailed { get; set; }

    }

    public class CustomJobConsumer : StatusGenericHandler, IConsumer<CustomJobMessage>
    {
        private readonly ICustomJobService _customJobService;
        private readonly ICustomJobRunnerFactory _customJobRunnerFactory;
        private readonly IDocumentJobHistoryService _documentJobHistoryService;
        private readonly IAuthService _authService;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContext _dbContext;

        public CustomJobConsumer(ICustomJobService customJobService,
            IDocumentJobHistoryService documentJobHistoryService,
            IAuthService authService,
            ILogger<CustomJobConsumer> logger,
            IServiceProvider serviceProvider,
            ICustomJobRunnerFactory customJobRunnerFactory,
            DbContext dbContext)
        {
            _customJobService = customJobService;
            _documentJobHistoryService = documentJobHistoryService;
            _authService = authService;
            _logger = logger;
            _customJobRunnerFactory = customJobRunnerFactory;
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
        }

        public async Task ConsumeAsync(CustomJobMessage message, CancellationToken cancellationToken)
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
                _authService.ResetUserName(message.UserName);
                //_authService.ResetUserName("mahalla");

                // update doc status as EXECUTING and clear messasge
                _dbContext.Database.ExecuteSqlRaw($"UPDATE {table.DbSchemaName}.{table.DbTableName} SET STATUS_ID={StatusIdConst.EXECUTING}, MESSAGE='' WHERE ID={message.DocId}");

                var entity = _customJobService.Get(message.DocId);

                ICustomJobRunner jobRunner = _customJobRunnerFactory.GetJobRunner(entity.JobTypeId);

                await jobRunner.Run(entity.Id, CancellationToken.None);
                CombineStatuses(_customJobService);

                if (IsValid)
                {
                    try
                    {
                        using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                        {
                            if (command.Connection?.State != System.Data.ConnectionState.Open)
                                command.Connection?.Open();

                            command.CommandText = $"UPDATE {table.DbSchemaName}.{table.DbTableName} SET STATUS_ID = 17 WHERE ID={message.DocId}";
                            var statusId = command.ExecuteScalar().AsString();

                            if (statusId.AsInt() != message.ToStatusId)
                                throw new InvalidStatusChangedException($"Статус документа изменился на другой статус, чем ожидалось: изменен на «{statusId}», но ожидался {message.ToStatusId}");
                        }

                        _documentJobHistoryService.End(jobHistory.Id, succeed: true);
                    }
                    catch (Exception ex)
                    {
                        throw new CheckStatusdException(ex.Message);
                    }
                }
                else
                {
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
