import mainreport from './mainreport';
import soliqreports from './soliqreports';
import bankreports from './bankreports';
import bojxonareports from './bojxonareports';
import davaktivreports from './davaktivreports';
import tadbirkorjamgarmareport from './tadbirkorjamgarmareport';

// visibles
import {
   BankReportVisibles,
   BojxonaReportVisibles,
   DavAktivreportVisibles,
   MainVisibles,
   SoliqReportVisibles,
   TadbirkorReportVisibles
} from './data';

export default [
   {
      title: 'Reports',
      icon: 'ClipboardIcon',
      visible: [
         ...BankReportVisibles,
         ...BojxonaReportVisibles,
         ...DavAktivreportVisibles,
         ...MainVisibles,
         ...SoliqReportVisibles,
         ...TadbirkorReportVisibles,
         'ExecutionApplicationView'
      ],
      children: [
         ...mainreport,
         ...soliqreports,
         ...bankreports,
         ...tadbirkorjamgarmareport,
         ...bojxonareports,
         ...davaktivreports,
         {
            title: 'ExecutionApplication',
            route: 'ExecutionApplication',
            visible: 'ExecutionApplicationView'
         },
         {
            title: 'ReportOnProjectImplement',
            route: 'ReportOnProjectImplement',
            visible: true
         },
         {
            title: 'MonoApplicationReport',
            route: 'MonoApplicationReport',
            visible: 'MonoReportView'
         },
         {
            title: 'GetExpiredContractorsReport',
            route: 'GetExpiredContractorsReport',
            visible: 'ExpiredReportView'
         },
         {
            title: 'PrtnEmploymentGraphNewReport',
            route: 'PrtnEmploymentGraphNewReport',
            visible: true
         },
         {
            title: 'newreport',
            route: 'newreport',
            visible: true
         }
      ]
   }
];
