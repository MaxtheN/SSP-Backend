using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class TaxBenefitDto : UpdateTaxBenefitDlDto, ILinkToEntity<TaxBenefit>, IDocument
{
    public string Status { get; set; }
    public string Organization { get; set; }
    public string TaxBenefitType { get; set; }
    public int StatusId { get; set; }

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
