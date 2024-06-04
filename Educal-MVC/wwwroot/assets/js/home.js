"use strict";

let navHeaders = document.querySelectorAll(".navbar-collapse .nav-item");
let navSubmenus = document.querySelectorAll(".submenu");
let mobileNavBtn = document.querySelector(".header-right .navbar button i");
let categoryElem = document.querySelector(".categories");
let categorySubmenu = document.querySelector(
    ".categories .category-submenu"
);
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
    } else {
        headerElem.style.backgroundColor = "#edeef3";
        searchInput.style.backgroundColor = "white";
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



let courseTabHeaders = document.querySelectorAll("#all-courses .tab-headers ul li");
let courseTabContents = document.querySelectorAll("#all-courses .tab-body .content");

console.log(courseTabContents);


courseTabHeaders.forEach(header => {
    header.addEventListener("click", function () {
        document.querySelector(".active-tab").classList.remove("active-tab");
        this.classList.add("active-tab");
        if (this.getAttribute("data-id") == 0) {
            for (const item of courseTabContents) {
                item.classList.remove("d-none");
            }
        } else {
            for (const item of courseTabContents) {
                if (item.getAttribute("data-id") == this.getAttribute("data-id")) {
                    item.classList.remove("d-none");
                } else {
                    item.classList.add("d-none");
                }
            }
        }
    });
});

let priceTabHeaders = document.querySelectorAll("#prices .tab-headers span");
let priceTabContents = document.querySelectorAll("#prices .tab-body .content");

priceTabHeaders.forEach(header => {
    header.addEventListener("click", function () {
        document.querySelector(".active-price-tab").classList.remove("active-price-tab");
        this.classList.add("active-price-tab");
        for (const item of priceTabContents) {
            if (item.getAttribute("data-id") == this.getAttribute("data-id")) {
                item.classList.remove("d-none");
            } else {
                item.classList.add("d-none");
            }
        }
    });
});

