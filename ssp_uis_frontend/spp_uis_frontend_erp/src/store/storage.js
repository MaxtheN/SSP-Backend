export default (options) => (store) => {
	if (localStorage.state) {
		const storedState = JSON.parse(localStorage.state)
		Object.assign(store.state.auth.rules, storedState.rules);
		store.replaceState(store.state)
	}
	return store.subscribe((mutation, state) => {

		if (options.destroyOn && options.destroyOn.indexOf(mutation.type) !== -1) {
			return localStorage.removeItem('state')
		}

		const newState = options.storedKeys.reduce((map, key) => {
			map[key] = state.auth[key]
			return map
		}, {});
		localStorage.state = JSON.stringify(newState)
	})
}
