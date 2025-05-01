import Vue from 'vue';
import Router from 'vue-router';
import routes from './routes';

Vue.use(Router);

const router = new Router({
    mode: 'history', // https://router.vuejs.org/api/#mode
    linkActiveClass: 'active',
    base: import.meta.env.BASE_URL,
    scrollBehavior: () => ({
        y: 0,
        x: 0
    }),
    routes: routes
});
router.beforeEach((to, from, next) => {
    var auth = localStorage.getItem('user_info');
    if (to.meta.isAuth === true) {
        if (!!auth) {
            next();
        } else {
            next('/');
        }
    } else {
        next();
    }
});
export default router;
