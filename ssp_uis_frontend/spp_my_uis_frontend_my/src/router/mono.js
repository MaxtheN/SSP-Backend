const defBread = [
    {
        text: 'yigirmaming_tadbirkor',
        to: 'Partnership'
    },
]
export default [
    {
        path: "/mono",
        name: "Mono",
        component: () => import("@/views/mono/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'MonoApplication',
                    active: true
                }
            ]
        },
    },
    {
        path: "/monoapplication",
        name: "MonoApplication",
        component: () => import("@/views/mono/monoapplication/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'MonoApplication',
                    active: true
                }
            ]
        },
    },
    {
        path: "/monoapplication/edit/id=:id",
        name: "MonoApplicationEdit",
        component: () => import("@/views/mono/monoapplication/edit.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'MonoApplication',
                    to: 'MonoApplication'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        },
    },
]