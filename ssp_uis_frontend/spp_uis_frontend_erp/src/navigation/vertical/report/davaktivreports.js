import { DavAktivreportVisibles } from "./data";

export default [
   {
      title: 'DavAktivreports',
      icon: 'ClipboardIcon',
      visible: DavAktivreportVisibles,
      children: [
         {
            title: 'GetStateAssetApplicationReport',
            route: 'GetStateAssetApplicationReport',
            visible: 'ReportStateAssetApplicationView'
         }
      ]
   }
];
