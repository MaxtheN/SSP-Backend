import { BankReportVisibles } from "./data";

export default [
   {
      title: 'BankReports',
      icon: 'ClipboardIcon',
      visible: BankReportVisibles,
      children: [
         {
            title: 'GetPrtnCreditDemandInfo',
            route: 'GetPrtnCreditDemandInfo',
            visible: 'ReportPrtnCreditDemandInfoView'
         },
         {
            title: 'GetPrtnCreditDemandInfoByBank',
            route: 'GetPrtnCreditDemandInfoByBank',
            visible: 'ReportPrtnCreditDemandInfoByBankView'
         },
         {
            title: 'BankReport',
            route: 'BankReport',
            visible: 'ReportBankCreditView'
         },
         {
            title: 'BankReportSecond2',
            route: 'BankReportSecond',
            visible: 'ReportBankCreditView'
         },
         {
            title: 'BankReportThird3',
            route: 'BankReportThird',
            visible: 'ReportBankCreditView'
         },
         {
            title: 'GetBankCreditApplicationReportByRegionAndDistrict',
            route: 'GetBankCreditApplicationReportByRegionAndDistrict',
            visible: 'ReportBankCreditView'
         },
         {
            title: 'GetBankCreditApplicationReportByRegionAndDistrict2',
            route: 'GetBankCreditApplicationReportByRegionAndDistrict2',
            visible: 'ReportBankCreditView'
         }
         // {
         //    title: 'GetBusinessActivityTypeReport',
         //    route: 'GetBusinessActivityTypeReport',
         //    visible: true
         // },
         // {
         //    title: 'GetBusinessActivityTypeReportByRegion',
         //    route: 'GetBusinessActivityTypeReportByRegion',
         //    visible: true
         // }
      ]
   }
];
