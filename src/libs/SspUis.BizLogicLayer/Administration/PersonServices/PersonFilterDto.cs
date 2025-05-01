using System;

namespace SspUis.BizLogicLayer.PersonServices;
public class PersonFilterDto
{
    public string Seria { get; set; }
    public string Number { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int? documentTypeId { get; set; }
}

