namespace WEBASE.Integration.Manuals.Services
{
    public interface IDistrictService
    {
        string MapGNKToWbCode(string gnkId);
        string MapGSPToWbCode(int gspId);
        string MapWbCodeToGNK(string wbCode);
        int? MapWbCodeToGSP(string wbCode);
    }
}