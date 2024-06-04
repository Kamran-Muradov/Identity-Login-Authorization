"use strict";

let navHeaders = document.querySelectorAll(".navbar-collapse .nav-item");
let navHeaderList = document.querySelectorAll(
  ".navbar-collapse .nav-item .nav-link"
);
let mobileNavBtn = document.querySelector(".header-right .navbar button i");
let categoryElem = document.querySelector(".categories");
let categorySubmenu = document.querySelector(
  ".categories .category-submenu"
);
let headerLogo = document.querySelector(".header-left img");
let navSubmenus = document.querySelectorAll(".submenu");
let headerElem = document.querySelector("header");
let sticky = headerElem.offsetTop;
let searchInput = document.querySelector(".search-area input");
let sidebar = document.querySelector("header .sidebar-mobile");
let sidebarOpenBtn = document.querySelector(
  "header .header-right .navbar button"
);
let sidebarCloseBtn = document.querySelector(
  "header .sidebar-mobile .close button"
);
let sidebarMenus = document.querySelectorAll(
  "header .sidebar-mobile .sidebar-content .menu"
);
let sidebarSubmenus = document.querySelectorAll(
  "header .sidebar-mobile .sidebar-content .mob-submenu"
);

sidebarOpenBtn.addEventListener("click", function () {
  sidebar.classList.add("sidebar-open");
});

sidebarCloseBtn.addEventListener("click", function () {
  sidebar.classList.remove("sidebar-open");
});

sidebarMenus.forEach(menu => {
  menu.addEventListener("click", function () {
    for (const item of sidebarSubmenus) {
      if (item.getAttribute("data-id") == menu.getAttribute("data-id")) {
        item.classList.toggle("d-none");
      }
    }
  });
});

function changeHeaderStyle() {
  if (window.scrollY > sticky) {
    headerElem.style.backgroundColor = "white";
    searchInput.style.backgroundColor = "#edeef3";
    navHeaderList.forEach(item => {
      item.classList.remove("nav-white");
      item.classList.add("nav-black");
    });
    categoryElem.classList.remove("category-white");
    categoryElem.classList.add("category-black");
    headerLogo.setAttribute("src", "./assets/images/logo.png");
  } else {
    headerElem.style.backgroundColor = "transparent";
    searchInput.style.backgroundColor = "white";
    navHeaderList.forEach(item => {
      item.classList.remove("nav-black");
      item.classList.add("nav-white");
    });
    mobileNavBtn.style.color = "#2b4eff";
    categoryElem.classList.remove("category-black");
    categoryElem.classList.add("category-white");
    headerLogo.setAttribute("src", "./assets/images/logo-2.png");
  }
}

categoryElem.addEventListener("mouseover", function () {
  categorySubmenu.classList.add("active-submenu");
});

categoryElem.addEventListener("mouseleave", function () {
  categorySubmenu.classList.remove("active-submenu");
});

navHeaders.forEach(navHeader => {
  navHeader.addEventListener("mouseover", function () {
    for (const item of navSubmenus) {
      if (item.getAttribute("data-id") == navHeader.getAttribute("data-id")) {
        item.classList.add("active-submenu");
      }
    }
  });
});

navHeaders.forEach(navHeader => {
  navHeader.addEventListener("mouseleave", function () {
    for (const item of navSubmenus) {
      if (item.getAttribute("data-id") == navHeader.getAttribute("data-id")) {
        item.classList.remove("active-submenu");
        item.addEventListener("mouseover", function () {
          item.classList.remove("active-submenu");
        });
      }
    }
  });
});

window.onscroll = function () {
  changeHeaderStyle();
};

let courseTabHeaders = document.querySelectorAll(
  "#all-courses .tab-area .tab-headers i"
);
let courseTabContents = document.querySelectorAll(
  "#all-courses .tab-body .content"
);

courseTabHeaders.forEach(header => {
  header.addEventListener("click", function () {
    document.querySelector(".active-tab").classList.remove("active-tab");
    this.classList.add("active-tab");
    for (const item of courseTabContents) {
      if (item.getAttribute("data-id") == this.getAttribute("data-id")) {
        item.classList.remove("d-none");
      } else {
        item.classList.add("d-none");
      }
    }
  });
});

let selectionBtn = document.querySelector("#all-courses .sort-area .default");
let selectionMenu = document.querySelector("#all-courses .sort-area ul");

selectionBtn.addEventListener("click", e => {
  selectionMenu.classList.toggle("d-none");
  e.stopPropagation();
});

document.addEventListener("click", e => {
  if (e.target.closest(".list")) {
    return;
  }
  selectionMenu.classList.add("d-none");
});
