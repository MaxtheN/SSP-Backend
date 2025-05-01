using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models;


public class SoliqAosAylanmaResponseDto
{
public bool Success { get; set; }
public string Reason { get; set; }
public SoliqAosAylanmaData Data { get; set; }

}

public class SoliqAosAylanmaData
{
public long Tin { get; set; }
public string Name { get; set; }
public int Year { get; set; }
public decimal NetIncome { get; set; }
}
