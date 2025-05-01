using System.Linq;

namespace SspUis.Core
{
    public class StepIdConst
    {
        #region   ARBITRATION
        public const int CLAIMED = 1;
        public const int NOTIFIED = 2;
        public const int NEED_DISCUSSION = 3;
        public const int DELAYED = 4;
        public const int COURT_DECISION = 5;
        #endregion

        /// <summary>
        /// Tasdiqlangan
        /// </summary>
        public const int ACCEPT = 9;

        #region CLAIM
        /// <summary>
        /// YANGI HALI KO"RILMAGAN (DAVO)
        /// </summary>
        public const int NEW_NOT_SEEN = 6;

        /// <summary>
        /// JARAYONDA
        /// </summary>
        public const int EXECUTING = 7;

        /// <summary>
        /// Xodim Tasdiqlagan
        /// </summary>
        public const int ACCEPT_THAT_EMPLOYEE = 8;

        /// <summary>
        /// Медиация Режаси Яратилди
        /// </summary>
        public const int MEDIATION_PLAN_CREATE = 10;

        /// <summary>
        /// Медияция баённома яратилди
        /// </summary>
        public const int MEDIATION_CREATE = 11;

        /// <summary>
        /// Судга ариза яратилди
        /// </summary>
        public const int APPLICATION_FOR_COURT_CREATE = 12;

        /// <summary>
        /// Заявка отменена
        /// </summary>
        public const int CLAIM_APPLICATION_CANCEL = 13;

        /// <summary>
        /// CLAIM_APPLICATION_REJECTED
        /// </summary>
        public const int CLAIM_APPLICATION_REJECTED = 21;

        /// <summary>
        /// План посредничества отменен
        /// </summary>
        public const int MEDIATION_PLAN_CANCEL = 14;

        /// <summary>
        /// Посредничество протокол отменен
        /// </summary>
        public const int MEDIATION_CANCEL = 15;
        #endregion

        #region CORRUPTION
        /// <summary>
        /// Ombudsmanga yuborildi
        /// </summary>
        public const int SENT_TO_OMBUDSMAN = 16;

        /// <summary>
        /// Korrupsiyaga Qarshi Kurash Agentligiga yuborildi
        /// </summary>
        public const int SENT_TO_ANTI_CORRUPTION_AGENCY = 17;

        // <summary>
        /// Ombudsman bekor qildi
        /// </summary>
        public const int REJECTED_OMBUDSMAN = 18;

        // <summary>
        /// Korrupsiyaga Qarshi Kurash Agentligi  bekor qildi
        /// </summary>
        public const int REJECTED_ANTI_CORRUPTION_AGENCY = 19;

        // <summary>
        /// SSP bekor qildi
        /// </summary>
        public const int REJECTED_SSP = 20;
        #endregion


        public static bool CanArbitrationCourtApplicationApplyStep(int currentStepId, int newStepId)
        {
            return newStepId switch
            {
                NOTIFIED => new int[] { CLAIMED }.Contains(currentStepId),
                NEED_DISCUSSION => new int[] { NOTIFIED }.Contains(currentStepId),
                DELAYED => new int[] { NOTIFIED ,NEED_DISCUSSION}.Contains(currentStepId),
                COURT_DECISION => new int[] { DELAYED, NEED_DISCUSSION }.Contains(currentStepId),
                _ => false
            };
        }
    }
}
