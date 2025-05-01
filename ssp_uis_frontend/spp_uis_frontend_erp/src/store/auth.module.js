const state = {
	authenticating: false,
	accessToken: '',
	rules: [],
	authenticationErrorCode: 0,
	authenticationError: '',
	refreshTokenPromise: null, // Holds the promise of the refresh token
	user: {},
	organizationtype : localStorage.getItem('user_info') ? JSON.parse(localStorage.getItem('user_info'))?.organizationtypeid : 0
};

const getters = {
	loggedIn: (state) => {
		return !!state.accessToken
	},

	authenticationErrorCode: (state) => {
		return state.authenticationErrorCode
	},

	authenticationError: (state) => {
		return state.authenticationError
	},

	authenticating: (state) => {
		return state.authenticating
	}
};

const actions = {
	async signIn({commit},data) {
		try {
			commit('loginSuccess', data)
			router.push('/');
			return true
		} catch (e) {
			(e.message);
			if (e instanceof AuthenticationError) {
				commit('loginError', {errorCode: e.errorCode, errorMessage: e.message})
			}

			return false
		}
	},
	setOrganizationtype({commit},data){
		commit('organizationtype',data)
	},
	logout({commit}) {
		UserService.logout()
		commit('logoutSuccess')
		router.push('/user/login')
	},
    login({commit},data){
        commit('loginSuccess',data)
    }
}

const mutations = {
	organizationtype(state,data){
		state.organizationtype = data
	},
	loginRequest(state) {
		state.authenticating = true;
		state.authenticationError = ''
		state.authenticationErrorCode = 0
	},
	setUserInfo(state, data) {
		state.user = data;
	},

	loginSuccess(state, data) {
		state.accessToken = data.token;
		state.permissions = data.user.modules;
		let formattedRules = [];
		if (data.user.modules.length > 0) {
			formattedRules = data.user.modules.map(perm => {
				const formattedObj = {};
				formattedObj.actions = perm;
				formattedObj.subject = 'permissions'
				return formattedObj;
			})
		}
		state.rules = formattedRules;
		state.authenticating = false;
	},
	logoutSuccess(state) {
		state.accessToken = '';
		state.user = {}
	},
}

export const auth = {
	namespaced: true,
	state,
	getters,
	actions,
	mutations
}
