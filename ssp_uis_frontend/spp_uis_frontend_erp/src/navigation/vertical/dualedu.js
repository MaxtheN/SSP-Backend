const visibles = ['SpecialtyView', 'InstituteView', 'DualEducationTypeView'];
const visibles1 = ['DualApplicationView', 'SubsidyRequestView'];

export default [
   {
      header: 'dualedu',
      visible: [...visibles1, ...visibles]
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles,
      children: [
         {
            title: 'Specialty',
            route: 'Specialty',
            visible: 'SpecialtyView'
         },
         {
            title: 'Institutes',
            route: 'Institute',
            visible: 'InstituteView'
         },
         {
            title: 'DualEducationType',
            route: 'DualEducationType',
            visible: 'DualEducationTypeView'
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles1,
      children: [
         {
            title: 'DualApplication',
            route: 'DualApplication',
            visible: 'DualApplicationView'
         },
         {
            title: 'DualContract',
            route: 'DualContract',
            visible: true
         },
         {
            title: 'SubsidyRequest',
            route: 'SubsidyRequest',
            visible: 'SubsidyRequestView'
         }
      ]
   }
];
