export default {
    methods: {
        isPinfl(item) {
            let info = {};
            if (item && !!item.data.alias) {
                const arr = [];
                item.data.alias.split(',').forEach((e) => {
                    arr.push(e.split('='));
                });
                const entries = new Map(arr);
                info = Object.fromEntries(entries);
            }

            return !info['1.2.860.3.16.1.1'];
        }
    }
}