using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners
{
    public abstract class CustomJobRunner<TActionInputData, TActionInputDataSource, TActionExecuter> : ICustomJobRunner
        where TActionExecuter : ICustomJobActionExecuter<TActionInputData>
        where TActionInputDataSource : ICustomJobActionInputDataSource<TActionInputData>
    {
        private readonly IServiceProvider _serviceProvider;
        private int _successCount = 0;
        private int _errorCount = 0;
        private int _cacheCount = 0;
        private DateTime _lastSyncAt = DateTime.MinValue;
        private TimeSpan _syncInterval = TimeSpan.FromSeconds(10);
        private object _syncLockObj = new object();

        public CustomJobRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public abstract int JobTypeId { get; }

        public async Task Run(long jobId, CancellationToken cancellationToken, string? backgroundJobId = null)
        {
            var runContext = _serviceProvider.GetRequiredService<DbContext>();
            var runAuthService = _serviceProvider.GetRequiredService<IAuthService>();
            var actionInputDataSource = _serviceProvider.GetService<TActionInputDataSource>();
            var entity = runContext.Set<CustomJob>().Include(a => a.JobType).FirstOrDefault(a => a.Id == jobId);
            var source = await actionInputDataSource.GetSource(entity);

            if (entity.JobTypeId != JobTypeId)
                throw new Exception($"{GetType().Name} and {entity.JobType.FullName} not compatible");

            UpdateEntity(runContext, entity, e =>
            {
                entity.BackGoundJobId = backgroundJobId;
                entity.StartAt = DateTime.Now;
                entity.TotalCount = source.Length;
                entity.Error = null;
                entity.ErrorCount = 0;
                entity.SuccesCount = 0;
                entity.CacheCount = 0;
            });

            try
            {
                await Parallel.ForEachAsync(source, new ParallelOptions() { MaxDegreeOfParallelism = entity.JobType.ConcurrentActionCount }, async (actionInputData, ct) =>
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    using var scope = _serviceProvider.CreateScope();
                    var scopeContext = scope.ServiceProvider.GetService<DbContext>();
                    CustomJobAction actionEntity = null;

                    try
                    {
                        var scopeAuthservice = scope.ServiceProvider.GetService<IAuthService>();
                        scopeAuthservice.ResetUserName(runAuthService.UserName);
                        var executer = scope.ServiceProvider.GetService<TActionExecuter>();

                        actionEntity = AddActionEntity(scopeContext, entity.Id, actionInputData);
                        var result = await executer.Execute(entity, actionInputData);
                        if (executer.IsValid)
                        {
                            UpdateActionEntity(scopeContext, actionEntity, ae =>
                            {
                                ae.EndAt = DateTime.Now;
                                ae.IsSuccess = true;
                                ae.ReturnData = result?.ReturnData;
                                ae.UserMessage = result?.UserMessage;
                                ae.FromCache = (result?.FromCache).GetValueOrDefault();
                            });
                            Interlocked.Increment(ref _successCount);
                            if (actionEntity.FromCache)
                                Interlocked.Increment(ref _cacheCount);
                        }
                        else
                        {
                            UpdateActionEntity(scopeContext, actionEntity, ae =>
                            {
                                ae.EndAt = DateTime.Now;
                                ae.Error = executer.GetAllErrors();
                            });
                            Interlocked.Increment(ref _errorCount);
                        }

                        SyncEntity(runContext, entity);
                    }
                    catch (Exception ex)
                    {
                        if (actionEntity != null)
                        {
                            UpdateActionEntity(scopeContext, actionEntity, ae =>
                            {
                                ae.EndAt = DateTime.Now;
                                ae.HasException = true;
                                ae.Error = ex.ToString();
                            });
                        }
                        Interlocked.Increment(ref _errorCount);
                        SyncEntity(runContext, entity);
                    }
                });

                SyncEntity(runContext, entity, true);
            }
            catch (Exception ex)
            {
                UpdateEntity(runContext, entity, e =>
                {
                    e.EndAt = DateTime.Now;
                    e.Error = ex.ToString();
                });
                SyncEntity(runContext, entity);
            }
        }

        private void SyncEntity(DbContext context, CustomJob entity, bool force = false)
        {
            if (!force && (DateTime.Now - _lastSyncAt < _syncInterval))
                return;

            lock (_syncLockObj)
            {
                DateTime now = DateTime.Now;
                if (!force && (now - _lastSyncAt < _syncInterval))
                    return;
                _lastSyncAt = now;

                UpdateEntity(context, entity, e =>
                {
                    e.SuccesCount = _successCount;
                    e.ErrorCount = _errorCount;
                    e.CacheCount = _cacheCount;
                });
            }
        }

        private CustomJobAction AddActionEntity(DbContext context, long ownerId, TActionInputData sourceItem)
        {
            var actionEntity = new CustomJobAction
            {
                StartAt = DateTime.Now,
                InputData = JsonConvert.SerializeObject(sourceItem),
                OwnerId = ownerId
            };
            actionEntity = context.Add(actionEntity).Entity;
            context.SaveChanges();
            return actionEntity;
        }

        private void UpdateActionEntity(DbContext context, CustomJobAction actionEntity, Action<CustomJobAction> action)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                action(actionEntity);
                context.SaveChanges();
                transaction.Commit();
            }
        }

        private void UpdateEntity(DbContext context, CustomJob entity, Action<CustomJob> action)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                context.Set<CustomJob>().Lock(entity.Id);
                action(entity);
                context.SaveChanges();
                transaction.Commit();
            }
        }

    }
}
