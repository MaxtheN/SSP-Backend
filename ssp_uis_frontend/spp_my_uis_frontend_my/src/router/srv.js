export default [
    {
        path: "/srv",
        name: "Srv",
        component: () => import("@/views/srv/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Srv',
                    active: true
                }
            ]
        },
    },
    {
        path: "/srv/serviceapplication/edit/id=:id",
        name: "ServiceApplicationEdit",
        component: () => import("@/views/srv/serviceapplication/edit.vue"),
        meta: {
            isAuth: true,
            hideCallCenter: true,
            breadcrumbs: [
                {
                    text: 'Srv',
                    to: 'Srv'
                },
                {
                    text: 'ServiceApplication',
                    to: 'ServiceApplication'
                },
                {
                    text: 'View',
                    active: true
                },
            ]
        },
    },
    {
        path: "/srv/serviceapplication",
        name: "ServiceApplication",
        component: () => import("@/views/srv/serviceapplication/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Srv',
                    to: 'Srv'
                },
                {
                    text: 'ServiceApplication',
                    active: true
                },
            ]
        },
    },
    {
        path: "/srv/servicecontract/edit/id=:id",
        name: "ServiceContractEdit",
        component: () => import("@/views/srv/servicecontract/view.vue"),
        meta: {
            isAuth: true,
            hideCallCenter: true,
            breadcrumbs: [
                {
                    text: 'Srv',
                    to: 'Srv'
                },
                {
                    text: 'ServiceContract',
                    to: 'ServiceContract'
                },
                {
                    text: 'View',
                    active: true
                },
            ]
        },
    },
    {
        path: "/srv/servicecontract",
        name: "ServiceContract",
        component: () => import("@/views/srv/servicecontract/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Srv',
                    to: 'Srv'
                },
                {
                    text: 'ServiceContract',
                    active: true
                },
            ]
        },
    },
    {
        path: "/srv/servicedeed/edit/id=:id",
        name: "ServiceDeedEdit",
        component: () => import("@/views/srv/servicedeed/view.vue"),
        meta: {
            isAuth: true,
            hideCallCenter: true,
            breadcrumbs: [
                {
                    text: 'Srv',
                    to: 'Srv'
                },
                {
                    text: 'servicedeed',
                    to: 'ServiceDeed'
                },
                {
                    text: 'View',
                    active: true
                },
            ]
        },
    },
    {
        path: "/srv/servicedeed",
        name: "ServiceDeed",
        component: () => import("@/views/srv/servicedeed/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Srv',
                    to: 'Srv'
                },
                {
                    text: 'servicedeed',
                    active: true
                }
            ]
        },
    },
    {
        path: "/srv/list",
        name: "SrvList",
        component: () => import("@/views/srv/List.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Srv',
                    to: 'Srv'
                },
                {
                    text: 'ServiceApplication',
                    active: true
                },
            ]
        },
    },
]