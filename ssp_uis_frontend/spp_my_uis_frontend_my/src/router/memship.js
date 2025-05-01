export default [
    {
        path: '/memship',
        name: 'memship',
        component: () => import('@/views/memship/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'memship',
                    active: true
                }
            ]
        }
    },
    {
        path: "/memship/application",
        name: "MemshipApplication",
        component: () => import("@/views/memship/application/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'memship',
                    to: "memship"
                },
                {
                    text: 'MemshipApplication',
                    active: true
                }
            ]
        },
    },
    {
        path: "/memship/application/edit/id=:id",
        name: "MemshipApplicationEdit",
        component: () => import("@/views/memship/application/edit.vue"),
        meta: {
            isAuth: true,
            hideCallCenter: true,
            breadcrumbs: [
                {
                    text: 'memship',
                    to: "memship"
                },
                {
                    text: 'MemshipApplication',
                    to: 'MemshipApplication'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        },
    },
    {
        path: "/memship/contract",
        name: "MemshipContract",
        component: () => import("@/views/memship/contract/List.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'memship',
                    to: "memship"
                },
                {
                    text: 'MemshipContract',
                    active: true
                }
            ]
        },
    },
    {
        path: "/memship/contract/edit/id=:id",
        name: "MemshipContractEdit",
        component: () => import("@/views/memship/contract/view.vue"),
        meta: {
            isAuth: true,
            hideCallCenter: true,
            breadcrumbs: [
                {
                    text: 'memship',
                    to: "memship"
                },
                {
                    text: 'MemshipContract',
                    to: 'MemshipContract'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        },
    },
    {
        path: "/memship/certificate",
        name: "MemshipCertificate",
        component: () => import("@/views/memship/certificate/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'memship',
                    to: "memship"
                },
                {
                    text: 'memshipcertificate',
                    to: 'MemshipCertificate'
                }
            ]
        },
    },
    {
        path: "/memship/certificate/edit/id=:id",
        name: "MemshipCertificateEdit",
        component: () => import("@/views/memship/certificate/edit.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'memship',
                    to: "memship"
                },
                {
                    text: 'memshipcertificate',
                    to: 'MemshipCertificate'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        },
    }
]