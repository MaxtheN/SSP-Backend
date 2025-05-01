namespace SspUis.BizLogicLayer.MfyServices
{
    public class CreateMfyExternalDto
    {
        public long ExternalId { get; set; }
        public string RegionSoatoCode { get; set; }
        public string DistrictSoatoCode { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }
}
