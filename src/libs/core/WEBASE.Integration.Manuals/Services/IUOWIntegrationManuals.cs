namespace WEBASE.Integration.Manuals.Services
{
    public interface IUOWIntegrationManuals
    {
        ICitizenshipService Citizenship { get; }
        ICountryService Country { get; }
        IDistrictService District { get; }
        IGenderService Gender { get; }
        INationalityService Nationality { get; }
        IRegionService Region { get; }
    }
}