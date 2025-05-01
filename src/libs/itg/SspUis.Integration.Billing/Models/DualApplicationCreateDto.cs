namespace SspUis.Integration.Billing.Models;

public class DualApplicationCreateDto
{
    public int EduTypeId { get; set; }
    public long ExternalApplicationId { get; set; }
    public int EmptyPositionsCount { get; set; }
    public int OrganizationId { get; set; }
    public int EduSpecialityId { get; set; }
    public string? Message { get; set; }
    public string ContractorInn { get; set; }
}

public class DualApplicationCreateDto1
{
    public int OrganizationId { get; set; }
    public string ContractorInn { get; set; }
    public long ExternalApplicationId { get; set; }
    public List<SpecialtyApplication> Tables { get; set; } = new List<SpecialtyApplication>();
}

public class SpecialtyApplication
{
    public int EduTypeId { get; set; }
    public int EmptyPositionsCount { get; set; }
    public int EduSpecialityId { get; set; }
    public string? Message { get; set; } = String.Empty;
}