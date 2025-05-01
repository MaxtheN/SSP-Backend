const visibles1 = ['AppealApplicationView'];

const visibles3 = ['AppealDescriptionView'];

export default [
   {
      header: 'Appeal',
      visible: [...visibles1, ...visibles3]
   },

   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles1,
      children: [
         {
            title: 'AppealApplication',
            route: 'AppealApplication',
            visible: 'AppealApplicationView'
         }
      ]
   },
   {
      title: 'Report',
      route: 'AppealReport',
      icon: 'ClipboardIcon',
      visible: visibles3,
      children: [
         {
            title: 'GetReportByAppealType',
            route: 'GetReportByAppealType',
            visible: true
         },
         {
            title: 'AppealApplicationReport',
            route: 'AppealApplicationReport',
            visible: true
         }
      ]
   }
];
