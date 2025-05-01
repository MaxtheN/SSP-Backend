/* eslint-disable func-names */
/**
 * debounce function
 *
 * @param {Function} func The function to debounce
 * @param {number} delay The number of milliseconds
 * @return {Function} 
 */
// eslint-disable-next-line import/prefer-default-export
export const debounce = function (func, delay) {
    let debounceTimer;
    return function () {
        const context = this;
        // eslint-disable-next-line prefer-rest-params
        const args = arguments;
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => func.apply(context, args), delay);
    };
};