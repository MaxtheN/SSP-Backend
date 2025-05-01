const visibles = ['VideoCategoryView', 'VideoLessonView', 'NewsView', 'NewsTagView'];
export default [
   {
      header: 'WebSettings',
      visible: visibles
   },
   {
      title: 'WebSettings',
      icon: 'SettingsIcon',
      visible: visibles,
      children: [
        
         {
            title: 'VideoCategory',
            route: 'VideoCategory',
            visible: 'VideoCategoryView'
         },
         {
            title: 'VideoLesson',
            route: 'VideoLesson',
            visible: 'VideoLessonView'
         },
         {
            title: 'News',
            route: 'News',
            visible: 'NewsView'
         },
         {
            title: 'NewsTag',
            route: 'NewsTag',
            visible: 'NewsTagView'
         }
      ]
   }
];
