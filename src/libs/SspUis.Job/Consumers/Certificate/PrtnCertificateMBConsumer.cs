using Microsoft.Extensions.Logging;
using StatusGeneric;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Models;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.Core.Security;
using Npgsql;
using Microsoft.Extensions.DependencyInjection;
using SspUis.RabbitMQ.Doc.Exceptions;
using SspUis.RabbitMQ.Certificate.Messages;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.Certificate.Services;

namespace SspUis.RabbitMQ.Certificate.Consumers
{
    public class PrtnCertificateMBConsumerConfig : IConsumerConfig<PrtnCertificateMBMessage>
    {
        public string Name => "PrtnCertificateMBSync";
        public RabbitQueue Queue => Queues.Partner.PrtnCertificateMB;
        public ushort PrefetchCount { get; set; } = 1;
        public int WorkerCount { get; set; } = 1;
        public bool RequeueOnFailed { get; set; }

    }

    public class PrtnCertificateMBConsumer : StatusGenericHandler, IConsumer<PrtnCertificateMBMessage>
    {
        private readonly IPrtnCertificateService _prtnCertificateService;
        private readonly IDocumentJobHistoryService _documentJobHistoryService;
        private readonly IAuthService _authService;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContext _dbContext;

        public PrtnCertificateMBConsumer(IPrtnCertificateService prtnCertificateService,
            IDocumentJobHistoryService documentJobHistoryService,
            IAuthService authService,
            ILogger<PrtnCertificateMBConsumer> logger,
            IServiceProvider serviceProvider,
            DbContext dbContext)
        {
            _prtnCertificateService = prtnCertificateService;
            _documentJobHistoryService = documentJobHistoryService;
            _authService = authService;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
        }

        public async Task ConsumeAsync(PrtnCertificateMBMessage message, CancellationToken cancellationToken)
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
                //authService.ResetUserName(message.UserName, message.OrganizationId);
                _authService.ResetUserName("certificate");
                //_authService.ResetUserName("webaseadmin"); 

                await _prtnCertificateService.SentForOtherServiceFormedCertificate(message.DocId, EnumTypeOrganization.MarkaziyBank);
                CombineStatuses(_prtnCertificateService);

                if (IsValid)
                {
                    try
                    {
                        using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                        {
                            if (command.Connection?.State != System.Data.ConnectionState.Open)
                                command.Connection?.Open();
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
