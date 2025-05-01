export default {
	methods: {
		/**
		 * It takes a number or a string, converts it to a number, rounds it to 2 decimal places, adds a
		 * space between every 3 digits, and returns the result as a string.
		 * @param {number | string} [num=0] - number | string = 0
		 * @returns A function that takes a number or string and returns a string.
		 */
		formatCurrency(num = 0) {
			return Number(num)
				.toFixed(2)
				.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1 ");
		},
	},
};
