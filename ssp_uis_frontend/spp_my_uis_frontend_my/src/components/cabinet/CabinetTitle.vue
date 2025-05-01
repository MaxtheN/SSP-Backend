<template>
    <div class="px-4 py-3 border-bottom bg-white d-flex justify-content-between align-content-center">
        <div>
            <div class=" text-black h4 mb-0">{{ title }}</div>
            <!-- <div class="text-grey2 text-body1 ">An ERP software system helps automate and streamline core business processes.</div> -->
        </div>

        <slot name="actions"></slot>
    </div>
</template>

<script>
export default {
    props: {
        title: String
    }
};
</script>

<style lang="scss"></style>
