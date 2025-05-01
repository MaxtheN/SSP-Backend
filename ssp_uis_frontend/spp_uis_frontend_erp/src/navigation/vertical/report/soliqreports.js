import { SoliqReportVisibles } from "./data";

export default [
    {
       title: 'SoliqReports',
       icon: 'ClipboardIcon',
       visible: SoliqReportVisibles,
       children: [
         {
            title: 'Soliq',
            route: 'soliq',
            visible: 'ReportPrtnApplicationByContractTypeInfoView'
         },
         {
            title: 'GetTaxCreditReportShort',
            route: 'GetTaxCreditReport',
            visible: 'ReportPrtnApplicationByContractTypeInfoView'
         },
         {
            title: 'GetSoliqReportByContractor',
            route: 'GetSoliqReportByContractor',
            visible: 'ReportSoliqByContractorView'
         },
       ]
    }
 ];
 