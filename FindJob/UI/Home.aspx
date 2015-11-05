<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--<script type="text/javascript" src="../Scripts/jquery-1.4.1-vsdoc.js"></script>--%>
<script src="../Scripts/jquery-easyui-1.4.2/jquery.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-easyui-1.4.2/jquery.easyui.min.js" type="text/javascript"></script>
<link href="../Scripts/jquery-easyui-1.4.2/themes/icon.css" rel="stylesheet" type="text/css" />
<link href="../Scripts/jquery-easyui-1.4.2/themes/metro/easyui.css" rel="stylesheet"
    type="text/css" />
<script src="../Scripts/jquery-easyui-1.4.2/jquery-easyui-datagridview/datagrid-detailview.js"
    type="text/javascript"></script>
<script src="../Scripts/jquery-easyui-1.4.2/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
<script type="text/javascript" src="../Scripts/UIJS.js"></script>
<script src="../Scripts/CryptoJS/components/core-min.js" type="text/javascript"></script>
<script src="../Scripts/CryptoJS/rollups/sha1.js" type="text/javascript"></script>
 
<link href="../CSS/MyStyle.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Title</title>
</head>
<body class="easyui-layout">
    <div data-options="region:'north', split:false,border:false" style="height: 20%;">
        <div id="header">
            <a id="btnLogin" href="#" onclick="login()" class="easyui-linkbutton" data-options="">登陆</a> <a id="btnRegist"
                href="#" onclick="regist()" class="easyui-linkbutton" data-options="">注册</a>
        </div>
        <div id="search">
            <input id="addr" />
            <input id="searchBox" class="searchBox" type="text" />
            <div id="checkboxs">
                <input id="zlzp" checked="checked" type="checkbox">智联招聘<input id="qcwy" checked="checked"
                    type="checkbox">前程无忧<input id="lpw" checked="checked" type="checkbox">猎聘网</div>
            <%--<div><input id="from"/></div>--%>
        </div>
    </div>
    <div data-options="region:'center', split:true,border:false">
        <table id="dg">
        </table>
    </div>
</body>
<div id="registWin" style="display: none">
    <span>
        <input id="userName" type="text" /></span><br />
    <span>
        <input id="password" type="password" /></span><br />
    <span>
        <input id="rePassword" type="password" /></span><br />
    <span>
        <input id="email" type="text" /></span><br />
        <div id="btns">
         <a id="regist" href="#" onclick="ensure()"  class="easyui-linkbutton">注册</a> 
    <a id="cacel" href="#" onclick="cacel()" class="easyui-linkbutton">关闭</a>
</div>
   <div id="loginWin" style="display: none">
       <span><input id="loginName" type="text"/> </span><br />
       <span><input id="loginPwd" type="password" /></span><br />
       <a id="loginEnsure" href="#" onclick="loginEnsure()"  class="easyui-linkbutton">登陆</a> 
        <a id="loginCacel" href="#" onclick="loginCacel()" class="easyui-linkbutton">关闭</a>
   </div>
</div>
</html>
