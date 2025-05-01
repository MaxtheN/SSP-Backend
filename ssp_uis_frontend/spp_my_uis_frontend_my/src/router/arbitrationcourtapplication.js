export default [
  {
    path: "/hakamliksudi",
    name: "HakamlikSudi",
    component: () => import("@/views/cabinet/hakamliksudi.vue"),
    meta: {
      isAuth: true,
      breadcrumbs: [
        {
          text: 'HakamlikSudi',
          active: true
        }
      ]
    },
  },
  {
    path: "/arbitrationcourtapplication",
    name: "ArbitrationCourtApplication",
    component: () => import("@/views/ArbitrationCourtApplication/index.vue"),
    meta: {
      isAuth: true,
      breadcrumbs: [
        {
          text: 'HakamlikSudi',
          to: 'HakamlikSudi'
        },
        {
          text: 'ArbitrationCourtApplication',
          active: true
        }
      ]
    },
  },
  {
    path: "/applicationprocess",
    name: "ApplicationProcess",
    component: () => import("@/views/ArbitrationCourtApplication/process.vue"),
    meta: {
      isAuth: true,
      breadcrumbs: [
        {
          text: 'HakamlikSudi',
          to: 'HakamlikSudi'
        },
        {
          text: 'ApplicationProcess',
          active: true
        }
      ]
    },
  },
  {
    path: "/arbitrationcourtapplication/edit/id=:id",
    name: "ArbitrationCourtApplicationEdit",
    component: () => import("@/views/ArbitrationCourtApplication/edit.vue"),
    meta: {
      isAuth: true,
      breadcrumbs: [
        {
          text: 'HakamlikSudi',
          to: 'HakamlikSudi'
        },
        {
          text: 'ArbitrationCourtApplication',
          to: 'ArbitrationCourtApplication'
        },
        {
          text: 'View',
          active: true
        }
      ]
    },
  },
  {
    path: "/arbitrationcourtapplication/selectjudge/id=:id",
    name: "SelectJudge",
    component: () =>
      import("@/views/ArbitrationCourtApplication/selectjudge.vue"),
    meta: {
      isAuth: true,
      breadcrumbs: [
        {
          text: 'HakamlikSudi',
          to: 'HakamlikSudi'
        },
        {
          text: 'ArbitrationCourtApplication',
          to: 'ArbitrationCourtApplication'
        },
        {
          text: 'SelectJudge',
          active: true
        }
      ]
    },
  },
];
