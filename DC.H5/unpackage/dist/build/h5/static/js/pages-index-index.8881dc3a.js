(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["pages-index-index"],{"0c05":function(t,e,a){"use strict";var n;a.d(e,"b",(function(){return u})),a.d(e,"c",(function(){return r})),a.d(e,"a",(function(){return n}));var u=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("v-uni-view",["home"==t.PageCur?a("home"):t._e(),"product"==t.PageCur?a("product"):t._e(),"user"==t.PageCur?a("user"):t._e(),a("v-uni-view",{staticClass:"cu-bar tabbar bg-white shadow foot"},[a("v-uni-view",{staticClass:"action",attrs:{"data-cur":"home"},on:{click:function(e){arguments[0]=e=t.$handleEvent(e),t.NavChange.apply(void 0,arguments)}}},[a("v-uni-view",{staticClass:"cuIcon-cu-image"},[a("v-uni-image",{attrs:{src:"/static/tabbar/home"+["home"==t.PageCur?"_active":""]+".png"}})],1),a("v-uni-view",{class:"home"==t.PageCur?"text-green":"text-gray"},[t._v("首页")])],1),a("v-uni-view",{staticClass:"action",attrs:{"data-cur":"product"},on:{click:function(e){arguments[0]=e=t.$handleEvent(e),t.NavChange.apply(void 0,arguments)}}},[a("v-uni-view",{staticClass:"cuIcon-cu-image"},[a("v-uni-image",{attrs:{src:"/static/tabbar/product"+["product"==t.PageCur?"_active":""]+".png"}})],1),a("v-uni-view",{class:"product"==t.PageCur?"text-green":"text-gray"},[t._v("产品列表")])],1),a("v-uni-view",{staticClass:"action",attrs:{"data-cur":"user"},on:{click:function(e){arguments[0]=e=t.$handleEvent(e),t.NavChange.apply(void 0,arguments)}}},[a("v-uni-view",{staticClass:"cuIcon-cu-image"},[a("v-uni-image",{attrs:{src:"/static/tabbar/user"+["user"==t.PageCur?"_active":""]+".png"}})],1),a("v-uni-view",{class:"user"==t.PageCur?"text-green":"text-gray"},[t._v("个人中心")])],1)],1)],1)},r=[]},"12be":function(t,e,a){"use strict";a.r(e);var n=a("b770"),u=a.n(n);for(var r in n)"default"!==r&&function(t){a.d(e,t,(function(){return n[t]}))}(r);e["default"]=u.a},6368:function(t,e,a){"use strict";a.r(e);var n=a("0c05"),u=a("12be");for(var r in u)"default"!==r&&function(t){a.d(e,t,(function(){return u[t]}))}(r);var i,c=a("f0c5"),s=Object(c["a"])(u["default"],n["b"],n["c"],!1,null,"5cef797c",null,!1,n["a"],i);e["default"]=s.exports},b770:function(t,e,a){"use strict";Object.defineProperty(e,"__esModule",{value:!0}),e.default=void 0;var n={data:function(){return{PageCur:"product"}},methods:{NavChange:function(t){this.PageCur=t.currentTarget.dataset.cur}}};e.default=n}}]);