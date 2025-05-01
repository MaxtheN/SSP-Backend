using Microsoft.Extensions.Logging;
using StatusGeneric;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Models;
using SspUis.RabbitMQ.Application.Messages;
using SspUis.BizLogicLayer.ApplicationServices;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.Core.Security;
using WEBASE;
using Npgsql;
using Microsoft.Extensions.DependencyInjection;
using SspUis.RabbitMQ.Doc.Exceptions;

namespace SspUis.RabbitMQ.Application.Consumers
{
    public class StateAssetApplicationConsumerConfig : IConsumerConfig<StateAssetApplicationMessage>
    {
        public string Name => "StateAssetApplication";
        public RabbitQueue Queue => Queues.StateAsset.StateAssetApplication;
        public ushort PrefetchCount { get; set; } = 1;
        public int WorkerCount { get; set; } = 1;
        public bool RequeueOnFailed { get; set; }

    }

    public class StateAssetApplicationConsumer : StatusGenericHandler, IConsumer<StateAssetApplicationMessage>
    {
        private readonly IStateAssetApplicationService _applicationService;
        private readonly IDocumentJobHistoryService _documentJobHistoryService;
        private readonly IAuthService _authService;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContext _dbContext;

        public StateAssetApplicationConsumer(IStateAssetApplicationService applicationService,
            IDocumentJobHistoryService documentJobHistoryService,
            IAuthService authService,
            ILogger<StateAssetApplicationConsumer> logger,
            IServiceProvider serviceProvider,
            DbContext dbContext)
        {
            _applicationService = applicationService;
            _documentJobHistoryService = documentJobHistoryService;
            _authService = authService;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
        }

        public async Task ConsumeAsync(StateAssetApplicationMessage message, CancellationToken cancellationToken)
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
                //_authService.ResetUserName(message.UserName, message.OrganizationId);1
                _authService.ResetUserName("davakiv");

                // update doc status as EXECUTING and clear messasge
                _dbContext.Database.ExecuteSqlRaw($"UPDATE {table.DbSchemaName}.{table.DbTableName} SET STATUS_ID={StatusIdConst.SENDING}, MESSAGE='' WHERE ID={message.DocId}");

                await _applicationService.Send(new SendStatusStateAssetApplicationDto
                {
                    Id = message.DocId,
                    UserIp = message.UserIp,
                    UserAgent = message.UserAgent,
                });

                CombineStatuses(_applicationService);

                if (IsValid)
                {
                    try
                    {
                        using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                        {
                            if (command.Connection?.State != System.Data.ConnectionState.Open)
                                command.Connection?.Open();

                            command.CommandText = $"SELECT STATUS_ID FROM {table.DbSchemaName}.{table.DbTableName} WHERE ID={message.DocId}";
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
