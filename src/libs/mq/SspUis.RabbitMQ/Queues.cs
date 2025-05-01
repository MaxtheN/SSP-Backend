using RabbitMQ.Client;
using SspUis.RabbitMQ.Models;

namespace SspUis.RabbitMQ
{
    public static class Queues
    {
        /// <summary>
        /// Producer -> any services
        /// Consumer -> audit-service
        /// </summary>
        public static RabbitQueue UserAction = new RabbitQueue
        {
            Name = "sys-user-action-queue",
            Exchange = "sys-user-action-exchange",
            ExchangeType = ExchangeType.Direct,
            RoutingKey = "sys-user-action-route",
            Producer = "*",
            Consumer = "audit-service"
        };

        /// <summary>
        /// Producer -> any services
        /// Consumer -> audit-service
        /// </summary>
        public static RabbitQueue DocumentHistory = new RabbitQueue
        {
            Name = "doc-history-queue",
            Exchange = "doc-history-exchange",
            ExchangeType = ExchangeType.Direct,
            RoutingKey = "doc-history-route",
            Producer = "*",
            Consumer = "audit-service"
        };

        /// <summary>
        /// Producer -> any services
        /// Consumer -> audit-service
        /// </summary>
        public static RabbitQueue Error = new RabbitQueue
        {
            Name = "error-queue",
            Exchange = "error-exchange",
            ExchangeType = ExchangeType.Direct,
            RoutingKey = "error-route",
            Producer = "*",
            Consumer = "audit-service"
        };

        public static class Partner
        {
            /// <summary>
            /// Producer -> any services
            /// Consumer -> job-service
            /// </summary>
            public static RabbitQueue PrtnApplication = new RabbitQueue
            {
                Name = "doc-prtn-application-queue",
                Exchange = "doc-prtn-application-exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "doc-prtn-application-route",
                Producer = "application-service",
                Consumer = "application-job-service"
            };

            /// <summary>
            /// Producer -> any services
            /// Consumer -> job-service
            /// </summary>
            public static RabbitQueue PrtnCertificate = new RabbitQueue
            {
                Name = "doc-prtn-certificate-queue",
                Exchange = "doc-prtn-certificate-exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "doc-prtn-certificate-route",
                Producer = "certificate-service",
                Consumer = "certificate-job-service"
            };

            /// <summary>
            /// Producer -> any services
            /// Consumer -> job-service
            /// </summary>
            public static RabbitQueue PrtnCertificateMB = new RabbitQueue
            {
                Name = "doc-prtn-certificate-mb-queue",
                Exchange = "doc-prtn-certificate-mb-exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "doc-prtn-certificate-mb-route",
                Producer = "certificate-service",
                Consumer = "certificate-job-service"
            };

            /// <summary>
            /// Producer -> any services
            /// Consumer -> job-service
            /// </summary>
            public static RabbitQueue PrtnCertificateBojxona = new RabbitQueue
            {
                Name = "doc-prtn-certificate-bojxona-queue",
                Exchange = "doc-prtn-certificate-bojxona-exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "doc-prtn-certificate-bojxona-route",
                Producer = "certificate-service",
                Consumer = "certificate-job-service"
            };
            public static RabbitQueue PrtnCertificateMoliya = new RabbitQueue
            {
                Name = "doc-prtn-certificate-moliya-queue",
                Exchange = "doc-prtn-certificate-moliya-exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "doc-prtn-certificate-moliya-route",
                Producer = "certificate-service",
                Consumer = "certificate-job-service"
            };
        }
        public static class Mono
        {
            /// <summary>
            /// Producer -> any services
            /// Consumer -> job-service
            /// </summary>
            public static RabbitQueue MonoApplication = new RabbitQueue
            {
                Name = "doc-mono-application-queue",
                Exchange = "doc-mono-application-exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "doc-mono-application-route",
                Producer = "application-service",
                Consumer = "application-job-service"
            };
        }
        public static class StateAsset
        {
            /// <summary>
            /// Producer -> any services
            /// Consumer -> job-service
            /// </summary>
            public static RabbitQueue StateAssetApplication = new RabbitQueue
            {
                Name = "doc-state-asset-application-queue",
                Exchange = "doc-state-asset-application-exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "doc-state-asset-application-route",
                Producer = "application-service",
                Consumer = "application-job-service"
            };
        }
        public static class Job
        {
            /// <summary>
            /// Producer -> any services
            /// Consumer -> job-service
            /// </summary>
            public static RabbitQueue CustomJob = new RabbitQueue
            {
                Name = "doc-custom-job-queue",
                Exchange = "doc-custom-job-exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = "doc-custom-job-route",
                Producer = "custom-service",
                Consumer = "custom-job-service"
            };
        }
    }
}