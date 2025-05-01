import { MainVisibles } from "./data";

export default [
   {
      title: 'contractorReport',
      icon: 'ClipboardIcon',
      visible: MainVisibles,
      children: [
         {
            title: 'prtnapplicationandcontractinfo',
            route: 'PrtnApplicationAndContractInfo',
            visible: 'ReportPrtnApplicationAndContractInfoView'
         },
         {
            title: 'prtnapplicationandcontractInfo2',
            route: 'PrtnApplicationAndContractInfo2',
            visible: true
         },
         {
            title: 'GetPrtnApplicationAndContractInfoByRegion',
            route: 'GetPrtnApplicationAndContractInfoByRegion',
            visible: 'ReportPrtnApplicationAndContractInfoByRegionView'
         },
         {
            title: 'getprtnapplicationbycontracttype',
            route: 'GetPrtnApplicationByContractType',
            visible: 'ReportPrtnApplicationByContractTypeInfoView'
         },
         {
            title: 'davaktiv',
            route: 'davaktiv',
            visible: 'ReportPrtnApplicationByContractTypeInfoView'
         },
         {
            title: 'jamgarma',
            route: 'jamgarma',
            visible: 'ReportPrtnApplicationByContractTypeInfoView'
         },
         {
            title: 'BusinessActivityType',
            route: 'BusinessActivityType',
            visible: 'BusinessActivityTypeView'
         },
         {
            title: 'GetOffertaCalculate',
            route: 'GetOffertaCalculate',
            visible: 'ReportPrtnApplicationAndContractInfoView'
         },

         {
            title: 'GetFreeAreaFromBandlik',
            route: 'GetFreeAreaFromBandlik',
            visible: 'ReportPrtnApplicationAndContractInfoView'
         },
         {
            title: 'PrtnEmploymentGraphReport',
            route: 'PrtnEmploymentGraphReport',
            visible: 'ReportPrtnApplicationAndContractInfoView'
         }
      ]
   }
];
