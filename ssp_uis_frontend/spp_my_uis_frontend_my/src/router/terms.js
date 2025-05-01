export default [
    {
        path: "/terms/:lang/main",
        name: "termsMain",
        component: () => import("@/views/terms/main.vue"),
        meta: {
            isAuth: false,
            layout: 'full'
        },
    },
    {
        path: "/terms/privacy_policy",
        name: "termsPrivacyPolicy",
        component: () => import("@/views/terms/PrivacyPolicy.vue"),
        meta: {
            isAuth: false,
            layout: 'full'
        },
    },
]