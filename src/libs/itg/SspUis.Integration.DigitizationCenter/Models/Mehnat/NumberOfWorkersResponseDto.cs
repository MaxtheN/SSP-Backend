namespace SspUis.Integration.DigitizationCenter.Models.Mehnat
{
    public class NumberOfWorkersResponseDto
    {
        public object error { get; set; }
        public int id { get; set; }
        public string jsonrpc { get; set; }
        public Result result { get; set; }
    }
    public class Result
    {
        public int code { get; set; }
        public NumberOfWorkersData data { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
    public class NumberOfWorkersData
    {
        public int employees_count { get; set; }
        public int positions_count { get; set; }
        public string tin { get; set; }
    }
}
