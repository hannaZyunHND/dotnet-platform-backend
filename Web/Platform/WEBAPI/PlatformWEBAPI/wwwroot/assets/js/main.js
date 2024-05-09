//Banner
if (window.innerWidth <= 768) {
    var menuBg = $('.home-banner img').data('mobile');
    $('.home-banner img').attr('src', menuBg);
} 

// viewerImage
const boxsViewerImage = document.querySelectorAll(".viewerImage");
if (boxsViewerImage.length > 0) {
  boxsViewerImage.forEach((box) => new Viewer(box));
}
// End viewerImage

// show-country
const showCountry = document.querySelector("[show-country]");
if (showCountry) {
    let quantityShowCountry = showCountry.getAttribute("show-country");

  quantityShowCountry = parseInt(quantityShowCountry);
    if (window.innerWidth <= 768) {
        quantityShowCountry = 10
    } else {
        quantityShowCountry = parseInt(quantityShowCountry);
    }
  const listItem = showCountry.querySelectorAll(".inner-item");

  for (let i = 0; i < quantityShowCountry; i++) {
    if (listItem[i]) {
      listItem[i].classList.remove("d-none");
    }
  }

  const buttonShowAll = document.querySelector("[button-show-all]");
  const textHidden = buttonShowAll.getAttribute("text-hidden");
  const textShow = buttonShowAll.getAttribute("text-show");
  let isShow = false;

  buttonShowAll.addEventListener("click", () => {
    isShow = !isShow;

    if (isShow) {
      for (let i = 0; i < listItem.length; i++) {
        if (listItem[i]) {
          listItem[i].classList.remove("d-none");
        }
      }
    } else {
      for (let i = quantityShowCountry; i < listItem.length; i++) {
        if (listItem[i]) {
          listItem[i].classList.add("d-none");
        }
      }
    }

    buttonShowAll.innerHTML = isShow ? textShow : textHidden;
  });
}
// End show-country

// swiperReviews
const swiperReviews = document.querySelector(".swiperReviews");
if (swiperReviews) {
  const swiper = new Swiper(".swiperReviews", {
    effect: "coverflow",
    grabCursor: true,
    centeredSlides: true,
    slidesPerView: 1,
    spaceBetween: 30,
    coverflowEffect: {
      rotate: 0,
      stretch: 0,
      depth: 0,
      scale: 0.42,
      slideShadows: false,
    },
    navigation: {
      nextEl: ".swiper-button-next",
      prevEl: ".swiper-button-prev",
    },
    pagination: {
      el: ".swiper-pagination",
      clickable: true,
    },
    breakpoints: {
      992: {
        slidesPerView: 2,
      },
    },
  });
}
// End swiperReviews

// Menu Footer
const listMenuFooter = document.querySelectorAll(
  ".footer .inner-top .inner-menu"
);
if (listMenuFooter.length > 0) {
  listMenuFooter.forEach((menu) => {
    menu.addEventListener("click", () => {
      menu.classList.toggle("show");
    });
  });
}
// End Menu Footer

// Fixed Search
const boxSearch = document.querySelector("[box-search]");
if (boxSearch) {
  var targetElementPosition = boxSearch.offsetTop;

  window.addEventListener("scroll", function () {
    const scrollPosition = window.scrollY || window.pageYOffset;

    if (scrollPosition > targetElementPosition) {
      boxSearch.classList.add("fixed");
    } else {
      boxSearch.classList.remove("fixed");
    }
  });
}
// End Fixed Search

// swiperSimAvailable
const swiperSimAvailable = document.querySelector(".swiperSimAvailable");
if(swiperSimAvailable) {
  const swiper = new Swiper(".swiperSimAvailable", {
    slidesPerView: 1,
    spaceBetween: 30,
    navigation: {
      nextEl: ".swiper-button-next",
      prevEl: ".swiper-button-prev",
    },
    pagination: {
      el: ".swiper-pagination",
      clickable: true,
    },
    breakpoints: {
      576: {
        slidesPerView: 2,
      },
    },
  });
}
// End swiperSimAvailable

// modalSearchNetwork
$('#modalSearchNetwork').on('hidden.bs.modal', function (event) {
  document.querySelector("body").classList.add("modal-open");
});
// End modalSearchNetwork

// box-input
const listBoxInput = document.querySelectorAll(".box-input");
if(listBoxInput.length > 0) {
  listBoxInput.forEach(box => {
    box.addEventListener("click", () => {
      box.classList.add("active");
    });
  });
}
// End box-input

// box-toggle
const listBoxToggle = document.querySelectorAll(".box-toggle");
if(listBoxToggle.length > 0) {
  listBoxToggle.forEach(box => {
    box.addEventListener("click", () => {
      box.classList.toggle("active");
    });
  });
}
// End box-toggle