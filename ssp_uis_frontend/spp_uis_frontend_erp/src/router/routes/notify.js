export default [
    {
      path: '/notify/sendsmsconfig',
      name: 'SendSmsConfig',
      component: () => import('@/views/notify/sendsmsconfig/index.vue'),
      meta: {
        pageTitle: "SendSmsConfig",
        breadcrumb: [
          {
            text: "notify",
          },
          {
            text: "SendSmsConfig",
            active: true,
          },
        ],
      },
    },
    {
      path: "/notify/sendsmsconfig/edit/id=:id",
      name: "EditSendSmsConfig",
      component: () => import("@/views/notify/sendsmsconfig/edit.vue"),
      meta: {
        pageTitle: "SendSmsConfig",
        navActiveLink: 'SendSmsConfig',
        breadcrumb: [
          {
            text: "notify",
          },
          {
            text: "SendSmsConfig",
          },
          {
            text: "create",
            active: true,
          },
        ],
      },
    }
  ]