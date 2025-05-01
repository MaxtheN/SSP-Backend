namespace SspUis.Job.WebApi.Configs
{
    public class HangFireConfig
    {
        public bool Enabled { get; set; } = true;
        public SecurityInfo Security { get; set; } = new SecurityInfo();
        public StorageInfo Storage { get; set; } = new StorageInfo();
        public List<JobServersInfo> JobServers { get; set; }
        public string RunCustomJobCron { get; set; } = "";

        public class SecurityInfo
        {
            public string UserName { get; set; } = "developer";
            public string Password { get; set; } = "d9JAIp^lc%4rg24t9Q$%eJHqo0p$f33@KI#%VjeKDm3FycAkXy";
        }
        public class StorageInfo
        {
            public PostgreSqlDb PostgreSql { get; set; } = new PostgreSqlDb();
            public class PostgreSqlDb
            {
                public string ConnectionString { get; set; } = null;
            }
        }
        public class JobServersInfo
        {
            public string ServerName { get; set; }
            public int WorkerCount { get; set; }
            public List<QueueInfo> Queues { get; set; }
            public class QueueInfo
            {
                public string QueueName { get; set; }
            }
        }
    }
}
