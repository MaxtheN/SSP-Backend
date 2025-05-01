namespace SspUis.Integration.Billing;

public class SpecialityListDto
{
    public int count { get; set; }
    public List<SpecialityBilling> specialities { get; set; }
}

public class SpecialityBilling
{
    public int id { get; set; }
    public string code { get; set; }
    public string shortName { get; set; }
    public string fullName { get; set; }
    public int organizationId { get; set; }
    public int facultyId { get; set; }
    public int eduAreaId { get; set; }
    public int eduSpecialityClassifierId { get; set; }
    public int eduTypeId { get; set; }
    public int eduPeriod { get; set; }
    public int stateId { get; set; }
    public string eduAreName { get; set; }
    public string organization { get; set; }
    public string faculty { get; set; }
    public string eduSpecialityClassifier { get; set; }
    public string eduType { get; set; }
    public string eduForm { get; set; }
    public string state { get; set; }
}