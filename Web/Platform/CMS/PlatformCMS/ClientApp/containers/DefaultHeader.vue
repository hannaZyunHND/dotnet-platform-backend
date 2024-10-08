<template>
  <AppHeader fixed>
    <SidebarToggler class="d-lg-none" display="md" mobile />

    <b-link class="navbar-brand" to="#">
      <img class="navbar-brand-full" src="./../assets/img/Joy-time-logo-07.png" width="89" height="25" alt="Logo">
      <img class="navbar-brand-minimized" src="./../assets/img/Joy-time-logo-07.png" width="30" height="30" alt="Logo">
    </b-link>
    <SidebarToggler class="d-md-down-none" display="lg" :defaultOpen=true />
    <div class="language-select d-flex">
      <div @click="onChangeLanguage('vi')">
        <img src="https://cdn.save.moe/b/6QziFKt7.png" alt="vietnam" border="0" style="width: 30px;">
      </div>
      <div @click="onChangeLanguage('zh')">
        <img src="https://cdn.save.moe/b/3abfy0.png" alt="china" border="0" style="width: 30px;margin-left:10px;">
      </div>
      <div @click="onChangeLanguage('en')">
        <img src="https://cdn.save.moe/b/JAc5Rez.png" alt="united kingdom" border="0"
          style="width: 30px;margin-left:10px" >
      </div>
    </div>
    <b-navbar-nav class="ml-auto">

      <DefaultHeaderDropdownAccnt />
    </b-navbar-nav>
    <AsideToggler class="d-none d-lg-block" />
    <!--<AsideToggler class="d-lg-none" mobile />-->
  </AppHeader>
</template>
<script>
import { Header as AppHeader, SidebarToggler, AsideToggler } from '@coreui/vue'
import DefaultHeaderDropdownAccnt from './DefaultHeaderDropdownAccnt'

export default {
  name: 'DefaultHeader',
  components: {
    AsideToggler,
    AppHeader,
    DefaultHeaderDropdownAccnt,
    SidebarToggler
  },
  methods: {
    onChangeLanguage(lang) {
      // alert(lang)
      this.$i18n.locale = lang; // Thay đổi ngôn ngữ trong i18n
      localStorage.setItem('language', lang); // Lưu ngôn ngữ vào localStorage
      //w
      window.location.reload();
    },
  }
}
</script>



// WEBPACK FOOTER //
// DefaultHeader.vue?18197f82