// isMobileMixin.js
export default {
    data() {
        return {
            isMobile: window.innerWidth < 768, // Boshlang'ich qiymat
        };
    },
    methods: {
        handleResize() {
            this.isMobile = window.innerWidth < 768;
        },
    },
    mounted() {
        this.handleResize(); // Boshlang'ich holatini o'rnatish
        window.addEventListener("resize", this.handleResize);
    },
    beforeDestroy() {
        window.removeEventListener("resize", this.handleResize);
    },
};
