<template>
    <div>
        <svg v-if="!active" id="Account_Icon" data-name="Account Icon" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
            <g id="Account_Icon" data-name="Account Icon">
                <rect id="Frame" width="32" height="32" fill="rgba(255,255,255,0)" />
                <path
                    id="Path_207"
                    data-name="Path 207"
                    d="M11782.5,19720a2.5,2.5,0,0,1,0-5h15a2.5,2.5,0,1,1,0,5Zm-1.662-2.5a1.67,1.67,0,0,0,1.662,1.67h15a1.666,1.666,0,1,0,0-3.332h-15A1.666,1.666,0,0,0,11780.834,19717.5Zm2.5-10.832a6.665,6.665,0,1,1,6.666,6.67A6.672,6.672,0,0,1,11783.334,19706.666Zm.834,0a5.832,5.832,0,1,0,5.832-5.828A5.841,5.841,0,0,0,11784.168,19706.666Z"
                    transform="translate(-11774.001 -19694)"
                    fill="#002429"
                    stroke="#002429"
                    stroke-width="0.6"
                />
            </g>
        </svg>
        <svg v-show="active" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
            <g id="Account_Icon" data-name="Account Icon" transform="translate(8)">
                <rect id="Frame" width="32" height="32" transform="translate(-8)" fill="rgba(255,255,255,0)" />
                <path
                    id="Path_207"
                    data-name="Path 207"
                    d="M11782.5,19720a2.5,2.5,0,0,1,0-5h15a2.5,2.5,0,1,1,0,5Zm.838-13.336a6.665,6.665,0,1,1,6.666,6.67A6.672,6.672,0,0,1,11783.334,19706.666Z"
                    transform="translate(-11782.001 -19694)"
                    fill="#00b894"
                    stroke="#00b894"
                    stroke-width="0.6"
                />
            </g>
        </svg>
    </div>
</template>

<script>
export default {
    props: {
        active: {
            type: Boolean,
            default: false
        }
    }
};
</script>
