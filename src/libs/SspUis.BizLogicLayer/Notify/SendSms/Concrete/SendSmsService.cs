using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Notify.Sms;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsService : StatusGenericHandler, ISendSmsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmsService _smsService;
        private readonly ISendSmsLogService _sendSmsLogService;
        public SendSmsService(
            IUnitOfWork unitOfWork,
            ISmsService smsService,
            ISendSmsLogService sendSmsLogService)
        {
            _unitOfWork = unitOfWork;
            _smsService = smsService;
            _sendSmsLogService = sendSmsLogService;
        }

        public async Task SendSms(int tableId, int? fromStatusId, int? toStatusId, string phoneNumber)
        {
            var config = CheckConfig(tableId, fromStatusId, toStatusId);

            if (config is null) return;

            string errorText = string.Empty;
            try
            {
                await Task.Run(() => _smsService.Send(phoneNumber, config.SmsText));
            }
            catch (Exception ex)
            {
                errorText += ($"{ex.Message} // {ex.InnerException}");
                return;
            }
            finally
            {
                var logId = _sendSmsLogService.Create(new CreateSendSmsLogDlDto
                {
                    PhoneNumer = phoneNumber,
                    SmsText = config.SmsText,
                    ErrorText = string.IsNullOrEmpty(errorText) ? errorText : null,
                    TableId = tableId,
                    DocId = tableId,
                    FromStatusId = fromStatusId,
                    ToStatusId = toStatusId
                });
            }
        }
        private SendSmsConfig CheckConfig(int tableId, int? fromStatusId, int? toStatusId)
        {
            var config = _unitOfWork.Context
                .Set<SendSmsConfig>()
                .FirstOrDefault(a => a.TableId == tableId 
                                  && a.FromStatusId == fromStatusId 
                                  && a.ToStatusId == toStatusId
                                  && a.StateId != StateIdConst.PASSIVE
                 );

            return config;
        }
    }
}
