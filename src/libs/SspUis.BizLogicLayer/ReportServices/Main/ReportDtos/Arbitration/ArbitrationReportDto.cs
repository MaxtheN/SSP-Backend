namespace SspUis.BizLogicLayer.ReportServices;

public class ArbitrationReportDto
{
    public int ClaimAmount { get; set; }
    public ArbitrationPaymentAmout arbitrationPayment { get; set; }
    public ArbitrationPaymentAmout ArbitrationFeeCharged { get; set; }
    public ArbitrationPaymentAmout DeferredArbitrationFee { get; set; }
    public NumberOfArbitrationDecisions NumberOfArbitrationDecisions { get; set; }
    public AllAmount AllAmount { get; set; }
}
public class ArbitrationPaymentAmout
{
    public decimal Dollar { get; set; }
    public decimal Rubles { get; set; }
    public decimal Som { get; set; }

}
public class NumberOfArbitrationDecisions
{
    public int Satisfied { get; set; }
    public int PartiallySatisfied { get; set; }
    public int WasRejected { get; set; }

}

public class AllAmount
{
    public ArbitrationPaymentAmout SatisfiedAmount { get; set; }
    public ArbitrationPaymentAmout PartiallySatisfiedAmount { get; set; }
    public ArbitrationPaymentAmout WasRejectedAmount { get; set; }
}

