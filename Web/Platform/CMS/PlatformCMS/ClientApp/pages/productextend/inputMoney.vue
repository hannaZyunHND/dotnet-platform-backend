<template>
    <input  type="text" v-model="displayValue" class="form-control" @blur="isInputActive = false" @focus="isInputActive = true" />
</template>
<script>
    export default {
        name: 'input_format',
        props: ['value'],
        data: function () {
            return {
                isInputActive: false
            }
        },
        computed: {
            displayValue: {
                get: function () {
                    if (this.isInputActive) {
                        // Cursor is inside the input field. unformat display value for user
                        return this.value.toString()
                    } else {
                        // User is not modifying now. Format display value for user interface
                        return "$ " + this.value.toFixed(0).replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1,")
                    }
                },
                set: function (modifiedValue) {
                    // Recalculate value after ignoring "$" and "," in user input
                    let newValue = parseFloat(modifiedValue.replace(/[^\d\.]/g, ""))
                    // Ensure that it is not NaN
                    if (isNaN(newValue)) {
                        newValue = 0
                    }
                    // Note: we cannot set this.value as it is a "prop". It needs to be passed to parent component
                    // $emit the event so that parent component gets it
                    this.$emit('input', newValue)
                }
            }
        }
    }
</script>
