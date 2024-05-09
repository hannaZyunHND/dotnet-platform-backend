<template>
    <button :class="['dropdown-item',{'active':item[0].active}]" @click="changeValueRight(index)" v-for="(item,index) in ListValue">
        <div style="display:flex;width:100%">
            <div class="col-md-">
                {{item.ten}}
            </div>
            <div class="col-md-3">
                <input style="height:20px" @change="changeValue(item)" min="0" type="number" class="form-control" v-model="item.price" />
            </div>
            <div class="col-md-2">
                <input style="height:20px" @change="changeValue(item)" min="0" max="100" type="number" class="form-control" v-model="item.discountPercent" />
            </div>
            <div class="col-md-3">
                <input style="height:20px" min="0" type="number" class="form-control" v-model="item.salePrice" />
            </div>
        </div>
    </button>
</template>

<script>
    export default {
        name: "LocationApp",
        props: {
            ListLocation: {
                type: Object
            }
        },
        data() {
            return {
                ListValue: this.ListLocation
            }
        },
        computed: {

        },
        methods: {
        }

    }
</script>
