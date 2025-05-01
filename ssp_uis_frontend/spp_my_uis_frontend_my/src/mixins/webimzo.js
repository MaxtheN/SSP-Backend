export default {
    data() {
        return {
            redirect: window.location.href // Current page URL for redirection
        };
    },
    methods: {
        /**
         * Redirects to a Webimzo URL with a redirect parameter appended.
         * @param {string} link - The base URL to redirect to.
         * @param {'_blank'|'_self'} target - The target window for the redirect.
         */
        redirectWebimzo(link, target = "_blank") {
            if (link && typeof link === 'string') {
                const separator = link.includes('?') ? '&' : '?';
                const fullUrl = `${link}${separator}redirect=${encodeURIComponent(this.redirect)}`;
                window.open(fullUrl, target);
            } else {
                alert("SecretKey yoki RequestId mavjud emas!"); // Alert for missing link
            }
        }
    }
};
