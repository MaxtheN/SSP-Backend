using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer
{
    public class RestrictionSendingAppDto : UpdateRestrictionSendingAppDlDto , ILinkToEntity<RestrictionOfSendingApplication>
    {
        public string Table { get; set; }
        public string ApplicationModelCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
