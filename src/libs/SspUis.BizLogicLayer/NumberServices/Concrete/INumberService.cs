namespace SspUis.ServiceLayer.NumberServices
{
    public interface INumberService
    {
        (int, string) GetNext(string document);
        (int, string) GetNext(string document, int organizationId, int? externalSourceTypeId = null);
        (int, string) GetNext(string templateDocument, string numberDocument, int organizationId, int? externalSourceTypeId, int regionId = 1, int districtId = 1);
        (int, string) GetNext(string document, int organizationId, int regionId, int districtId, int? externalSourceTypeId = null);
    }
}