<template>
    <div :class="`${active ? 'child-icon' : ''} ${variant}`">
         <svg xmlns="http://www.w3.org/2000/svg" :width="width" :height="width" viewBox="0 0 32 32">
        <g id="Icon" transform="translate(-0.655)">
            <rect id="Frame" width="32" height="32" transform="translate(0.655)" fill="rgba(255,255,255,0)"/>
            <path id="Bag_Icon" data-name="Bag Icon" d="M16011.068,19392.1a3.239,3.239,0,0,1-1.25-.166,2.568,2.568,0,0,1-1.086-.5,4.2,4.2,0,0,1-1.831-3.432v-5.861a8.3,8.3,0,0,1,2.334-5.775v-2.93a1.332,1.332,0,1,1,2.664,0v1.084a8.238,8.238,0,0,1,6.5,0v-1.084a1.335,1.335,0,0,1,2.67,0v2.928a8.742,8.742,0,0,1,2.5,5.777V19388a4.075,4.075,0,0,1-2.918,3.936,6.66,6.66,0,0,1-1.249.166Zm-1.5-14.064a6.675,6.675,0,0,0-1.416,4.188v5.857a2.818,2.818,0,0,0,2.831,2.848h8.333a2.82,2.82,0,0,0,2.836-2.848v-5.857a7.14,7.14,0,0,0-1.334-4.187,7.91,7.91,0,0,0-2.419-2.006,6.232,6.232,0,0,0-3.249-.842,7.2,7.2,0,0,0-3.25.842c-.05.021-.1.045-.151.072a7.828,7.828,0,0,0-.972.654A8.92,8.92,0,0,0,16009.567,19378.037Zm3.084,9.877a.665.665,0,1,1,0-1.33h5a.717.717,0,0,1,.67.664.657.657,0,0,1-.67.666Z" transform="translate(-15998.248 -19366.102)" fill="#002429" stroke="#011422" stroke-width="0.2"/>
        </g>
        </svg>  
    </div>
</template>

<script>
export default {
    props : {
        active : {
            type : Boolean,
            default : false
        },
        variant : {
            type : String,
            default : 'success'
        },
        width : {
            type : String,
            default : '32px'
        }
    }
}
</script>

<style lang="scss" scope>
@import '../../assets/styles/variables.scss';
.child-icon.success-light{
    svg{
        path{
            fill: $success-light;
            stroke : $success-light;
        }
    }
}
.child-icon.success{
    svg{
        path{
            fill: $success;
            stroke : $success;
        }
    }
}
</style>