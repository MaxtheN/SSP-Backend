using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Core
{
    public class NewReportConst
    {
        /// <summary>
        /// Mahalla rad etishi yoki qabul qilishi uchun berilgan vaqt
        /// </summary>
        public const int ExpiredMahallaDate = 5;


        /// <summary>
        /// Expertizadan o'tish uchun muddat
        /// </summary>
        public const int ExpiredExpertizeDate = 3;

        /// <summary>
        /// Expertizadan o'tmagan shartnomani qaytda expirtizaga yuborish uchun muddat
        /// </summary>
        public const int ExpiredRecentExpertizeDate = 1;

        /// <summary>
        /// Shartnoma turi 51-100, 101-200 bolgan ekpertizadan otgan imzolanmoqda statuslar
        /// </summary>
        public const int ExpiredSendToSignDate = 5;

        /// <summary>
        /// Hokimiyatlar uchun: shartnoma yuri 51-100, 101-200 bolgan shartnomalar ekspertizadan otdi statusdan imzolandi statusga otishi uchun 5 ish kuni
        /// </summary>
        public const int ExpiredPassToSignDate = 5;

        /// <summary>
        ///kambag'allikni qisqartirish uchun: 200+ shartnomalar ekspertizadan otdi statusdan Imzolanmoqda statusga otadi hamda 3 ish kuni ichida kambagallik vazirligi imzolashi kk
        /// </summary>
        public const int ExpiredPassToSigningDate = 3;

        /// <summary>
        ///Moliya vazirligi uchun: status imzolanmoqta bolib qolaveradi lekin kambagallik imzogandan 2 ish kuni ichida imzolandi statusga otishi kk
        /// </summary>
        public const int ExpiredToSignDate = 2;

        /// <summary>
        ///savdo-sanoat palatasi uchun: Imzolandi statusdagi shartnoma uchun sertifikat shakllantirish muddati 1 ish kuni
        /// </summary>
        public const int ExpireToGenerateCertificate = 1;


    }
}
