import { TadbirkorReportVisibles } from "./data";

export default [
   {
      title: 'Tadbirkor jamg`armasi',
      icon: 'ClipboardIcon',
      visible: TadbirkorReportVisibles,
      children: [
         {
            title: 'GetBusinessActivityTypeReport',
            route: 'GetBusinessActivityTypeReport',
            visible: true
         },
         {
            title: 'GetBusinessActivityTypeReportByRegion',
            route: 'GetBusinessActivityTypeReportByRegion',
            visible: true
         }
      ]
   }
];
