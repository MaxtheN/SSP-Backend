export default [
    {
        path: "/appeal",
        name: "Appeal",
        component: () => import("@/views/appeal/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Appeal',
                    active: true
                }
            ]
        },
    },
    {
        path: "/appeal/AppealApplication/edit/id=:id",
        name: "AppealApplicationEdit",
        component: () => import("@/views/appeal/AppealApplication/edit.vue"),
        meta: {
            isAuth: true,
            hideCallCenter: true,
            breadcrumbs: [
                {
                    text: 'Appeal',
                    to: 'Appeal'
                },
                {
                    text: 'appealSend',
                    active: true
                }
            ]
        },
    },
    {
        path: "/appeal/AppealApplication",
        name: "AppealApplication",
        component: () => import("@/views/appeal/AppealApplication/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Appeal',
                    to: 'Appeal'
                },
                {
                    text: 'appealSend',
                    active: true
                }
            ]
        },
    },
]