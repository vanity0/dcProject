// 屏蔽F12查看元素
document.onkeydown = function () {
    if (window.event && window.event.keyCode == 123) {
        window.location = "about:blank"; //将当前窗口跳转置空白页
        event.keyCode = 0;
        event.returnValue = false;
    }
    if (window.event && window.event.keyCode == 13) {
        window.event.keyCode = 505;
    }
    if (window.event && window.event.keyCode == 8) {
        alert("\n请使用Del键进行字符的删除操作！");
        window.event.returnValue = false;
    }
}

// 屏蔽右键菜单
document.oncontextmenu = function (event) {
    if (window.event) {
        event = window.event;
    } try {
        var the = event.srcElement;
        if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")) {
            return false;
        }
        return true;
    } catch (e) {
        return false;
    }
}

// 屏蔽粘贴
document.onpaste = function (event) {
    if (window.event) {
        event = window.event;
    } try {
        var the = event.srcElement;
        if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")) {
            return false;
        }
        return true;
    } catch (e) {
        return false;
    }
}

// 屏蔽复制
document.oncopy = function (event) {
    if (window.event) {
        event = window.event;
    } try {
        var the = event.srcElement;
        if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")) {
            return false;
        }
        return true;
    } catch (e) {
        return false;
    }
}

// 屏蔽剪切
document.oncut = function (event) {
    if (window.event) {
        event = window.event;
    } try {
        var the = event.srcElement;
        if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")) {
            return false;
        }
        return true;
    } catch (e) {
        return false;
    }
}

// 屏蔽选中
document.onselectstart = function (event) {
    if (window.event) {
        event = window.event;
    } try {
        var the = event.srcElement;
        if (!((the.tagName == "INPUT" && the.type.toLowerCase() == "text") || the.tagName == "TEXTAREA")) {
            return false;
        }
        return true;
    } catch (e) {
        return false;
    }
}
 