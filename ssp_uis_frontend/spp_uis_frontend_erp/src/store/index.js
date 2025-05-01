import Vue from 'vue';
import Vuex from 'vuex';

// Modules
import app from './app';
import appConfig from './app-config';
import verticalMenu from './vertical-menu';
import { auth } from './auth.module';
import { staffingStore } from '@/views/hrm/document/staffing/store/staffingStore';
import status from './status';

import { abilityPlugin, ability as appAbility } from './ability';
import storage from './storage';
export const ability = appAbility;
Vue.use(Vuex);

export default new Vuex.Store({
   plugins: [
      storage({
         storedKeys: ['rules'],
         destroyOn: ['auth/logoutSuccess']
      }),
      abilityPlugin
   ],
   modules: {
      auth,
      app,
      appConfig,
      verticalMenu,
      staffingStore,
      status
   },
});
