/********************************************************************
created:	2015/10/15
created:	15:10:2015   16:22
filename: 	D:\visual studio 2010\Projects\FindJob\FindJob\Scripts\UIJS.js
file path:	D:\visual studio 2010\Projects\FindJob\FindJob\Scripts
file base:	UIJS
file ext:	js
author:		Kimi
	
purpose:	
*********************************************************************/



var text;
var expandRowIndex = -1;
var fromWhere;
var cookie;
$(function () {


    /// <summary>
    /// 搜索框
    /// </summary>
    $("#searchBox").textbox({
        iconAlign: "right",
        width: setWidth(0.4),
        height: setWidth(0.03),
        buttonIcon: "icon-search",
        prompt: "请输入关键字进行搜索",
        onClickButton: function () {
            if ($("#zlzp").is(":checked")) {
                fromWhere = "zlzp";
            }
            if ($("#qcwy").is(":checked")) {
                fromWhere += ",qcwy";
            }
            if ($("#lpw").is(":checked")) {
                fromWhere += ",lpw";
            }
            search();
        }
    });

    /// <summary>
    /// 显示城市选择列表
    /// </summary>
    $("#addr").combobox({
        valueField: "id",
        textField: "value",
        width: setWidth(0.08),
        height: setWidth(0.03),
        data: [{
            id: "01",
            value: "北京",
            "selected": true
        }, {
            id: "02",
            value: "上海"
        }, {
            id: "03",
            value: "广州"
        }, {
            id: "04",
            value: "深圳"
        }, {
            id: "05",
            value: "杭州"
        }, {
            id: "06",
            value: "成都"
        }, {
            id: "07",
            value: "天津"
        }, {
            id: "08",
            value: "苏州"
        }, {
            id: "09",
            value: "重庆"
        }, {
            id: "10",
            value: "长沙"
        }, {
            id: "11",
            value: "南京"
        }, {
            id: "12",
            value: "大连"
        }, {
            id: "13",
            value: "武汉"
        }, {
            id: "14",
            value: "沈阳"
        }
        ]
    });


    /// <summary>
    /// 回车搜索
    /// </summary>

    $(document).keypress(function (e) {
        if (e.keyCode == 13) {
            if ($("#zlzp").is(":checked")) {
                fromWhere = "zlzp";
            }
            if ($("#qcwy").is(":checked")) {
                fromWhere += ",qcwy";
            }
            if ($("#lpw").is(":checked")) {
                fromWhere += ",lpw";
            }
            search();
        }
    });


    /// <summary>
    /// Cookie免登陆
    /// </summary>
    var c = GetCookie("Id");
    if (c !== 0) {
        $.post("/BAL/IsOutData.ashx", { cookie: c }, function (data) {
            if (data == "outdata") {
                return;
            }
        });
        $("#header").hide();
        $("#headAfterLogin").show();
    }
});

/// <summary>
/// 设置宽或高
/// </summary>
function setWidth(percent) {
    return $(this).width() * percent;
}

/// <summary>
/// 显示搜索结果
/// </summary>
/// 
var selceted;
function search() {
    $("#collection").datagrid();
    $("#collection").datagrid("getPanel").panel("minimize");
    $("#dg").datagrid({
        url: "../BAL/Handler.ashx",
        queryParams: {
            addr: $("#addr").combobox("getText"),
            key: $("#searchBox").val(),
            from: fromWhere
        },
        selectOnCheck: true,
        fit: true,
        fitColumns: true,
        singleSelect: true,
        rownumbers: false,
        remoteSort: false,
        pagination: true,
        columns: [
            [
                { field: "TitleName", title: "职位", width: setWidth(20) },
                { field: "Company", title: "公司", width: setWidth(20) },
                { field: "Salary", title: "薪资", width: setWidth(5), sortable: true },
                { field: "Date", title: "发布时间", width: setWidth(5), sortable: true },
                { field: "City", title: "工作地点", width: setWidth(5) },
                { field: "Source", title: "来自", width: setWidth(5), sortable: true }
            ]
        ],
        view: detailview,
        detailFormatter: function (index, row) {
            return "<div  id='" + index + "'></div></br>" + "<a class='oriUrl' href=" + row.InfoUrl + " target='_blank'>访问原页</a>" + "</br><a class='colUrl' href='#' onclick='addToCollect()'>加入收藏</a>";
        },
        onLoadSuccess: function (data) {
            $("#dg").datagrid("scrollTo", "0");
        },
        rowStyler: function (index, row) {

            return "height:" + setWidth(5);

        },
        onExpandRow: function (index, row) {//显示详细信息
            $("#" + index).panel({
                href: "../BAL/Content.ashx?url=" + row.InfoUrl,
                border: true,
                cache: false,
                onLoad: function () {
                    $("#dg").datagrid("fixDetailRowHeight", index);
                }
            });
            $("#dg").datagrid("fixDetailRowHeight", index);
            selceted = row;
        },
        onClickRow: function (index, row) {//点击行展开或折叠

            if (index == expandRowIndex) {
                $("#dg").datagrid("collapseRow", expandRowIndex);
                expandRowIndex = -1;
            } else {
                $("#dg").datagrid("collapseRow", expandRowIndex);
                $("#dg").datagrid("expandRow", index);
                expandRowIndex = index;
            }

            $("#dg").datagrid("scrollTo", index);

        }
    });



    var pager = $("#dg").datagrid("getPager");    // 得到datagrid的pager对象  
    pager.pagination({
        layout: ["prev", "next"],
        displayMsg: ""
    }
    );

}

/// <summary>
/// 显示登陆框
/// </summary>
function login() {
    $("#loginWin").window({
        collapsible: false,
        top: setWidth(0.1),
        minimizable: false,
        width: setWidth(0.4),
        height: setWidth(0.2),
        modal: true,
        title: "登陆"
    }).show();
    $("#loginName").textbox({
        required: true,
        prompt: "用户名",
        iconCls: "icon-man",
        height: setWidth(0.03),
        width: setWidth(0.25)
    });

    $("#loginPwd").textbox({
        required: true,
        prompt: "密码  ",
        iconCls: "icon-lock",
        height: setWidth(0.03),
        width: setWidth(0.25)
    });
}


/// <summary>
/// 确定登陆
/// </summary>
function loginEnsure() {
    var user = new Object();
    user.name = $("#loginName").val();
    user.pwd = CryptoJS.SHA1($("#loginPwd").val()).toString();
    if ($("#loginWin").form("validate")) {
        $.post("/BAL/Login.ashx", user, function (data) {
            if (data === "True") {
                $.messager.alert("", "登陆成功!", "info", function () {
                    $("#loginWin").window("close");
                    //                    $("#btnLogin").hide();
                    $("#loginPwd").textbox("clear");
                    $("#header").hide();
                    $("#headAfterLogin").show();

                });
            } else {
                $.messager.alert("", "登陆失败!请输入正确的用户名/密码", "info", function () {

                    $("#loginPwd").textbox("clear");
                });
            }
        }
        );
    }
}




/// <summary>
/// 取消登陆
/// </summary>
function loginCacel() {
    $("#loginWin").window("close");
    $("#loginPwd").textbox("clear");
}


/// <summary>
/// 显示注册框
/// </summary>
function regist() {
    $("#registWin").window({
        collapsible: false,
        top: setWidth(0.1),
        minimizable: false,
        width: setWidth(0.4),
        height: setWidth(0.2),
        modal: true,
        title: "注册"
    }).show();

    //    $("registWin").window("vcenter");

    $("#userName").textbox({
        required: true,
        prompt: "用户名",
        iconCls: "icon-man",
        height: setWidth(0.03),
        width: setWidth(0.25),
        validType: ["length[6, 20]"]

    });

    $("#password").textbox({
        required: true,
        prompt: "密码  ",
        iconCls: "icon-lock",
        height: setWidth(0.03),
        width: setWidth(0.25),
        validType: ["length[6, 20]"]
    });

    $("#rePassword").textbox({
        required: true,
        prompt: "确认密码",
        iconCls: "icon-lock",
        height: setWidth(0.03),
        width: setWidth(0.25),
        validType: ["length[6, 20]"]
    });
    $("#email").textbox({
        required: true,
        validType: "email",
        prompt: "邮箱",
        iconCls: "icon-sum",
        height: setWidth(0.03),
        width: setWidth(0.25)
    });

}

/// <summary>
/// 提交注册信息
/// </summary>
function ensure() {
    var user = new Object();
    user.name = $("#userName").val();
    user.pwd = CryptoJS.SHA1($("#password").val()).toString();
    user.rePwd = CryptoJS.SHA1($("#rePassword").val()).toString();
    user.email = $("#email").val();
    if ($("#registWin").form("validate")) {
        var result;
        $.post("/BAL/Regist.ashx", user, function (data) {

            switch (data) {
                case "Success":
                    result = "注册成功";
                    break;
                case "Error":
                    result = "未知错误,请联系管理员";
                    break;
                case "NameExist":
                    result = "用户名已存在";
                    break;
                case "EmailExist":
                    result = "邮箱已注册，请更换邮箱";
                    break;
                case "SamePwd":
                    result = "两次密码不同";
                    break;
                default:
                    break;
            }
            $.messager.alert("", result, "info", function () {
                if (data == "Success") {
                    $("#registWin").window("close");
                    $("#header").hide();
                    $("#password").textbox("clear");
                    $("#rePassword").textbox("clear");
                    $("#headAfterLogin").show();
                }
            });
        });
    }
}


/// <summary>
/// 取消注册
/// </summary>
function cacel() {
    $("#registWin").window("close");
    $("#password").textbox("clear");
    $("#rePassword").textbox("clear");
}

/// <summary>
/// 检索cookie
/// </summary>
function GetCookie(sName) {
    var aCookie = document.cookie.split(";");
    for (var i = 0; i < aCookie.length; i++) {
        var aCrumb = aCookie[i].split("=");
        if (sName == aCrumb[0] || aCrumb[0] == " " + sName)
            return unescape(aCrumb[1]);
    }
    return 0;
}

/// <summary>
/// 添加至收藏夹
/// </summary>
function addToCollect() {
    var postData = { data: JSON.stringify(selceted) };
    $.post("/BAL/Collect.ashx", postData, function (data) {
        if (data == "true") {
            $.messager.alert("成功", "已添加至收藏夹", "info", function () {
            });
        } else if (data == "exist") {
            $.messager.alert("失败", "已收藏过该职位", "warning", function () {
            });
        } else if (data == "login") {
            $.messager.alert("失败", "添加失败，请登陆后再试", "error", function () {
            });
        }
        else {
            $.messager.alert("失败", "添加失败", "error", function () {
            });
        }

    });

}
/// <summary>
/// 点击退出
/// </summary>
function logoutClick() {
    $.messager.confirm("确定", "确认退出吗？", function (r) {
        if (r) {
            logout();
        }
    });
}

/// <summary>
/// 退出
/// </summary>
function logout() {

    $.post("/BAL/logout.ashx", { logout: true }, function (data) {
        if (data == "Success")
            $("#headAfterLogin").hide();
        $("#header").show(); ///
    });
}


function collection() {

    $("#dg").datagrid();
    $("#dg").datagrid("getPanel").panel("minimize");
    //   dg.pan
    //    $("#collection").show();
    $("#collection").datagrid({
        url: "../BAL/Collection.ashx",
        selectOnCheck: true,
        fit: true,
        fitColumns: true,
        singleSelect: true,
        rownumbers: false,
        remoteSort: false,
        pagination: true,
        columns: [
            [
                { field: "TitleName", title: "职位", width: setWidth(20) },
                { field: "Company", title: "公司", width: setWidth(20) },
                { field: "Salary", title: "薪资", width: setWidth(5), sortable: true },
                { field: "Date", title: "发布时间", width: setWidth(5), sortable: true },
                { field: "City", title: "工作地点", width: setWidth(5) },
                { field: "Source", title: "来自", width: setWidth(5), sortable: true }
            ]
        ],
        view: detailview,
        detailFormatter: function (index, row) {
            return "<div  id='" + index + "'></div></br>" + "<a class='oriUrl' href=" + row.InfoUrl + " target='_blank'>访问原页</a>";
        },
        onLoadSuccess: function (data) {
            $("#collection").datagrid("scrollTo", "0");
        },
        rowStyler: function (index, row) {

            return "height:" + setWidth(5);

        },
        onExpandRow: function (index, row) {//显示详细信息
            $("#" + index).panel({
                href: "../BAL/Content.ashx?url=" + row.InfoUrl,
                border: true,
                cache: false,
                onLoad: function () {
                    $("#collection").datagrid("fixDetailRowHeight", index);
                }
            });
            $("#collection").datagrid("fixDetailRowHeight", index);
            selceted = row;
        },
        onClickRow: function (index, row) {//点击行展开或折叠

            if (index == expandRowIndex) {
                $("#collection").datagrid("collapseRow", expandRowIndex);
                expandRowIndex = -1;
            } else {
                $("#collection").datagrid("collapseRow", expandRowIndex);
                $("#collection").datagrid("expandRow", index);
                expandRowIndex = index;
            }

            $("#collection").datagrid("scrollTo", index);
        }
    });
    //    var pager = $("#collection").datagrid("getPager");    // 得到datagrid的pager对象  
    //    pager.pagination({
    //        layout: ["prev", "next"],
    //        displayMsg: ""
    //    }
    //    );
}



$.extend($.fn.validatebox.defaults.rules, {
    minLength: {
        validator: function (value, param) {
            return value.length >= param[0];
        },
        message: "Please enter at least {0} characters."
    }
});  