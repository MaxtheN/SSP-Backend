export default [
  {
    path: "/dashboard",
    name: "Dashboard",
    component: () => import("@/views/dashboard.vue"),
    meta: {
      // resource: 'Auth',
      // redirectIfLoggedIn: true,
      pageTitle: "Dashboard",
      breadcrumb: [
        {
          text: "dashboard",
        },
        {
          text: "Dashboard",
          active: true,
        },
      ],
    },
  }
]