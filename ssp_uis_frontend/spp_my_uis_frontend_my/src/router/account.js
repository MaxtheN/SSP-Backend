export default [
    {
        path: "/account/addneworganization",
        name: "AddNewOrganization",
        component: () => import("@/views/account/NewOrganization.vue"),
        meta: {
            isAuth: false,
        },
    }
]