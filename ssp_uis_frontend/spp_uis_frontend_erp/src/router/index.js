import Vue from 'vue';
import VueRouter from 'vue-router';

import pages from './routes/pages';
import info from './routes/info';
import managment from './routes/managment';
import document from './routes/document';
import report from './routes/report';
import soliqreports from './routes/soliqreports';
import bankreports from './routes/bankreports';
import bojxonareports from './routes/bojxonareports';
import davaktivreports from './routes/davaktivreports';
import proposal from './routes/proposal';
import hrm from './routes/hrm/index';
import salary from './routes/salary';
import memship from './routes/memship';
import corruption from './routes/corruption';
import arbitrationcourt from './routes/arbitrationcourt';
import srv from './routes/srv';
import notify from './routes/notify';
import claim from './routes/claim';
import dualedu from './routes/dualedu/index';
import appeal from './routes/appeal/index';
import kpi from './routes/kpi/index';
import financeIntegration from './routes/financeIntegration';

const Dashboard = () => import('@/views/dashboard.vue');
const FormPersonSearch = () => import('@/components/forms/PersonSearch/form-person-search.vue');
Vue.use(VueRouter);

const router = new VueRouter({
   mode: 'history',
   scrollBehavior() {
      return { x: 0, y: 0 };
   },
   routes: [
      ...info,
      ...managment,
      ...document,
      ...report,
      ...soliqreports,
      ...bankreports,
      ...bojxonareports,
      ...davaktivreports,
      ...proposal,
      ...hrm,
      ...salary,
      ...corruption,
      ...memship,
      ...notify,
      ...arbitrationcourt,
      ...srv,
      ...claim,
      ...dualedu,
      ...appeal,
      ...kpi,
      ...financeIntegration,
      { path: '/', redirect: { name: 'dashboard' } },
      { path: '/dashboard', name: 'dashboard', component: Dashboard },
      { path: '/person-search', name: 'person-search', component: FormPersonSearch },
      ...pages,
      {
         path: '*',
         redirect: 'error-404'
      }
   ]
});

router.beforeEach((to, from, next) => {
   // return next()
   if (to.path !== '/login' && !to.meta.redirectIfLoggedIn) {
      if (localStorage.getItem('user_info')) {
         next();
      } else {
         next('login');
      }
   } else {
      next();
   }
});

export default router;
