export default [
    {
        path: "/claimapplication",
        name: "ClaimApplication",
        component: () => import("@/views/claimapplication/index.vue"),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'ClaimApplication',
                    active: true
                }
            ]
        },
    },
    {
        path: "/claimapplication/edit/id=:id",
        name: "ClaimApplicationEdit",
        component: () => import("@/views/claimapplication/edit.vue"),
        meta: {
            isAuth: true,
            hideCallCenter: true,
            breadcrumbs: [
                {
                    text: 'ClaimApplication',
                    to: 'ClaimApplication'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        },
    },
    {
        path: "/mediation",
        name: "Mediation",
        component: () => import("@/views/mediation/index.vue"),
        meta: {
            isAuth: false,
            breadcrumbs: [
                {
                    text: 'Mediation',
                    active: true
                }
            ]
        },
    },
    {
        path: "/mediationplan",
        name: "MediationPlan",
        component: () => import("@/views/mediationplan/index.vue"),
        meta: {
            isAuth: false,
            breadcrumbs: [
                {
                    text: 'MediationPlan',
                    active: true
                }
            ]
        },
    },
]