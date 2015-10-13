var text;
var expandRowIndex = -1;
var fromWhere;
$(function () {
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
    //    $("#from").combotree({
    //        data: [{
    //            text: "智联招聘"
    //        }, {
    //            text: "猎聘网"
    //        }],
    //        checkbox: true,
    //        multiple: true
    //    });
    document.onkeydown = function (e) {
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
    };


});
function setWidth(percent) {
    return $(this).width() * percent;
}
function search() {
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
            return "<div id='" + index + "'></div></br>" + "<a href=" + row.InfoUrl + " target='_blank'>访问原页</a>";
        },
        onLoadSuccess: function (data) {
            $("#dg").datagrid("scrollTo", "0");
        },
        rowStyler: function (index, row) {

            return "height:" + setWidth(5);

        },
        onExpandRow: function (index, row) {
            $("#" + index).panel({
                href: "../BAL/Content.ashx?url=" + row.InfoUrl,
                border: true,
                cache: false,
                onLoad: function () {
                    $("#dg").datagrid("fixDetailRowHeight", index);
                }
            });
            $("#dg").datagrid("fixDetailRowHeight", index);
        },
        onClickRow: function (index, row) {

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


 