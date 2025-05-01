import claim from './claim';
import dual from './dual';
import memship from './memship';
import srv from './srv';
import mono from './mono';
import arbitrationcourtapplication from './arbitrationcourtapplication';
import account from './account';
import terms from './terms';
import contractorsurvey from './contractorsurvey';
import appeal from './appeal';
import partnership from './partnership';

export default [
    {
        path: '/',
        name: 'Home',
        component: () => import('@/views/home.vue'),
        meta: {
            isAuth: false
        }
    },
    {
        path: '/register',
        name: 'Register',
        component: () => import('@/views/register.vue'),
        meta: {
            isAuth: false
        }
    },
    {
        path: '/login',
        name: 'Login',
        component: () => import('@/views/login.vue'),
        meta: {
            isAuth: false
        }
    },
    {
        path: '/settings',
        name: 'Settings',
        component: () => import('@/views/settings.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'settings',
                    active: true
                }
            ]
        }
    },
    {
        path: '/mycabinet',
        name: 'MyCabinet',
        component: () => import('@/views/cabinet/main.vue'),
        meta: {
            isAuth: true
        }
    },

    {
        path: '/oferta',
        name: 'Oferta',
        component: () => import('@/views/oferta.vue'),
        meta: {
            isAuth: false
        }
    },
    {
        path: '/myinfo',
        name: 'info',
        component: () => import('@/views/cabinet/myinfo1.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'myinfo',
                    active: true
                }
            ]
        }
    },
    {
        path: '/claim_application',
        name: 'claim_application',
        component: () => import('@/views/cabinet/claim_application.vue'),
        meta: {
            isAuth: true
        }
    },
    {
        path: '/joinanticorruptionapplication',
        name: 'JoinAntiCorruptionApplication',
        component: () => import('@/views/joinanticorruptionapplication/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'JoinAntiCorruptionApplication',
                    active: true
                }
            ]
        }
    },
    {
        path: '/joinanticorruptionapplication/edit/id=:id',
        name: 'JoinAntiCorruptionApplicationEdit',
        component: () => import('@/views/joinanticorruptionapplication/edit.vue'),
        meta: {
            isAuth: true,
            hideCallCenter: true,
            breadcrumbs: [
                {
                    text: 'JoinAntiCorruptionApplication',
                    to: 'JoinAntiCorruptionApplication'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        }
    },
    {
        path: '/sspcertificate',
        name: 'sspcertificate',
        component: () => import('@/views/sspcertificate/index.vue'),
        meta: {
            isAuth: true
        }
    },
    {
        path: '/sspcertificate/edit/id=:id',
        name: 'SSPCertificateEdit',
        component: () => import('@/views/sspcertificate/edit.vue'),
        meta: {
            isAuth: true
        }
    },
    {
        path: '/taklif2023',
        name: 'Proposal',
        component: () => import('@/views/proposal.vue'),
        meta: {
            isAuth: false
        }
    },

    {
        path: '/videolesson',
        name: 'videolesson',
        component: () => import('@/views/videolesson/index.vue'),
        meta: {
            isAuth: true
        }
    },
    {
        path: '/news',
        name: 'News',
        component: () => import('@/views/news/index.vue'),
        meta: {
            isAuth: false
        }
    },
    {
        path: '/services',
        name: 'NeedChamberService',
        component: () => import('@/views/needchamberservice/index.vue'),
        meta: {
            isAuth: false,
            breadcrumbs: [
                {
                    text: 'NeedChamberService',
                    active: true
                }
            ]
        }
    },
    {
        path: '/applicationforcourt',
        name: 'ApplicationForCourt',
        component: () => import('@/views/applicationforcourt/index.vue'),
        meta: {
            isAuth: false
        }
    },
    {
        path: '/additionalagreement/view/id=:id',
        name: 'AdditionalAgreementView',
        component: () => import('@/views/additionalagreement/view.vue'),
        meta: {
            isAuth: true
        }
    },

    ...partnership,
    ...memship,
    ...dual,
    ...srv,
    ...claim,
    ...mono,
    ...arbitrationcourtapplication,
    ...account,
    ...terms,
    ...contractorsurvey,
    ...appeal
];
