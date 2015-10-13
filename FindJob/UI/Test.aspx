<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="FindJob.UI.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript" src="../Scripts/jquery-1.4.1-vsdoc.js"></script>
<script src="../Scripts/jquery-easyui-1.4.2/jquery.easyui.min.js" type="text/javascript"></script>
<link href="../Scripts/jquery-easyui-1.4.2/themes/icon.css" rel="stylesheet" type="text/css" />
<link href="../Scripts/jquery-easyui-1.4.2/themes/metro/easyui.css" rel="stylesheet"
    type="text/css" />
<script src="../Scripts/jquery-easyui-1.4.2/jquery-easyui-datagridview/datagrid-detailview.js"
    type="text/javascript"></script>
<script src="../Scripts/jquery-easyui-1.4.2/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Title</title>
</head>
<body>
    <form id="HtmlForm">
    <table id="tt">
    </table>
    </form>
</body>
</html>
<script type="text/javascript">
    $(function () {
        $('#tt').datagrid({
            title: 'DataGrid - DetailView',
            width: 500,
            height: 250,
            remoteSort: false,
            singleSelect: true,
            nowrap: false,
            fitColumns: true,
            url: '../BAL/Handler1.ashx',
            columns: [
                [
                    { field: 'itemid', title: 'Item ID', width: 80 },
                    { field: 'productid', title: 'Product ID', width: 100, sortable: true },
                    { field: 'listprice', title: 'List Price', width: 80, align: 'right', sortable: true },
                    { field: 'unitcost', title: 'Unit Cost', width: 80, align: 'right', sortable: true },
                    { field: 'attr1', title: 'Attribute', width: 150, sortable: true },
                    { field: 'status', title: 'Status', width: 60, align: 'center' }
                ]
            ],
            view: detailview,
            detailFormatter: function (rowIndex, rowData) {
                return "<div class='ddv'></div>";
            },
            onExpandRow: function (index, row) {
                var ddv = $(this).datagrid('getRowDetail', index).find('div.ddv'); //在这一行中找到class="ddv"的div
                ddv.panel({
                    border: false,
                    cache: true,
                    href: '../BAL/Handler1.ashx', //展开行访问的路径及传递的参数
                    onLoad: function () {
                        $("#dg").datagrid('fixDetailRowHeight', index); //固定高度
                        $('#dg').datagrid('selectRow', index);
                        //                        $('#dg').datagrid('getRowDetail', index).find('form').form('load', row); //将行的数据加载，这里可能要把列名和show_form.php文件中的name对应起来
                    }
                });
                $('#db').datagrid('fixDetailRowHeight', index);
            }
        });

    });
</script>
