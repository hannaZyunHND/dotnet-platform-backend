<template>
    <AppHeaderDropdown right no-caret>
        <template slot="header">
            <img id="accImg" :src="accAvatar"
                 class="img-avatar"
                 alt="user" />
        </template>

        <template slot="dropdown">
            <b-dropdown-item>
                <i class="fa fa-user" />
                <a @click="profile">Hồ sơ người dùng</a>
            </b-dropdown-item>
            <b-dropdown-divider />
            <b-dropdown-item>
                '<i class="fa fa-lock" />
                <a @click="logout">Đăng xuất</a>
            </b-dropdown-item>
        </template>
    </AppHeaderDropdown>
</template>

<script>
    import { HeaderDropdown as AppHeaderDropdown } from '@coreui/vue'
    import { authenticationRepository } from "../repository/authentication/authenticationRepository";
    import { router } from "../router/index";
    export default {
        name: 'DefaultHeaderDropdownAccnt',
        components: {
            AppHeaderDropdown
        },
        data: () => {
            return {
                itemsCount: 42,
                accAvatar: '',
                objRequest: {}
            }
        },
        methods: {
            async GetByIdAvata() {
                this.objRequest = await authenticationRepository.getCurrentUser();
                if (this.objRequest.avatar) {
                    this.accAvatar = "/uploads" + this.objRequest.avatar;
                }
                else {
                    this.accAvatar = 'https://i.ibb.co/z6PN2kf/Joy-time-logo-07.png';
                }

                this.isLoading = false;
            },
            logout() {
                authenticationRepository.logout();
                router.push('/admin/login');
            },
            profile() {
                router.push('/admin/profile');
            }
        },
        created() {
            this.GetByIdAvata();
        }
    }
</script>
