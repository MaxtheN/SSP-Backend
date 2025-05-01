export default [
    {
        path: '/contractorsurvey',
        name: 'Questionnarie',
        component: () => import('@/views/contractorsurvey/index.vue'),
        meta: {
            isAuth: true
        }
    },
    {
        path: '/contractorsurvey/view/id=:id',
        name: 'QuestionnarieView',
        component: () => import('@/views/contractorsurvey/view.vue'),
        meta: {
            isAuth: true
        }
    }
];
