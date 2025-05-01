using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.Core;
using SspUis.DataLayer.Repositories;
using WEBASE.Notify.Sms;
using System.Collections.Concurrent;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class SmsController : WebaseController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IBusinessmanAccountRepository _businessmanAccountRepository;
        private readonly ISmsService _smsService;
        private readonly IAuthService _authService;
        private static int ALL_PHONES_COUNT = 0;
        private static int SENT_PHONES_COUNT = 0;
        private ConcurrentDictionary<string, string?> SENT_PHONES_EXCEPTIONS = new ConcurrentDictionary<string, string?>();

        public SmsController(IUnitOfWork unitOfWork, IBusinessmanAccountRepository businessmanAccountRepository, ISmsService smsService, IAuthService authService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _unitOfWork = unitOfWork;
            _businessmanAccountRepository = businessmanAccountRepository;
            _smsService = smsService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> GetSendSmsToAllContractorStatus()
        {
            return Ok(new { ALL_PHONES_COUNT, SENT_PHONES_COUNT, SENT_PHONES_EXCEPTIONS });
        }

        [HttpPost]        
        public async Task<ActionResult> SendSmsToAllContractor()
        {
            if (_authService.UserName != "webaseadmin")
                return BadRequest("...");
            if (ALL_PHONES_COUNT > 0)
                return BadRequest("ALL_PHONES_COUNT");

            var phoneNumbers = _unitOfWork.PrtnCertificateRepository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.FORMED)
                                                                               .SelectMany(a => a.Contractor.BusinessmanUserInContractors.Where(a => a.StateId == 1).Select(a => a.BusinessmanUser.UserName))
                                                                               .Distinct()
                                                                               .ToArray();
            ALL_PHONES_COUNT = phoneNumbers.Length;

            foreach (var phoneNumber in phoneNumbers)
            {
                SendSms(phoneNumber, @"Hurmatli tadbirkor!
Siz, “20 ming tadbirkor – 500 ming malakali mutaxassis” dasturi doirasida imtiyozli kredit olish istagingiz boʼlsa, my.chamber.uz platformasidagi shaxsiy kabinetingiz orqali kreditga boʼlgan ehtiyojingiz toʼgʼrisidagi shaklni yuborishingiz soʼraladi.");
            }

            return Ok("ok");
        }


        private void SendSms(string phoneNumber, string message)
        {
            _ = Task.Run(() => _smsService
                .Send(phoneNumber, message)
                .ContinueWith(t => t.IsCompletedSuccessfully 
                    ? Interlocked.Increment(ref SENT_PHONES_COUNT) > 0 
                    : SENT_PHONES_EXCEPTIONS.TryAdd(phoneNumber, t.Exception?.ToString()))
                );
        }

    }
}
