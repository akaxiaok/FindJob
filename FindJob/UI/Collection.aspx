<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Header.Master" AutoEventWireup="true"
    CodeBehind="Collection.aspx.cs" Inherits="FindJob.UI.Collection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <body class="easyui-layout">
        <div data-options="region:'north', split:false,border:false" style="height: 20%;">
            1
        </div>
        <div data-options="region:'center', split:false,border:false" style="height: 20%;">
            <div class="easyui-layout" fit="true">
                <div data-options="region:'west', split:false,border:false" style="width: 15%;">
                    <div id="acc" class="easyui-accordion" fit="true">
                        <div title="城市" style="padding: 10px; color: #0099FF;">
                            <p>
                                Accordion is a part of easyui framework for jQuery. It lets you define your accordion
                                component on web page more easily.</p>
                        </div>
                        <div title="来源" style="padding: 10px; color: #0099FF;">
                            content2
                        </div>
                        <div title="投递" style="padding: 10px; color: #0099FF;">
                            content3
                        </div>
                    </div>
                </div>
                <div data-options="region:'east', split:false,border:false" style="width: 80%;">
                    2
                </div>
            </div>
        </div>
        <div data-options="region:'south', split:false,border:false" style="height: 10%;">
            1
        </div>
    </body>
</asp:Content>
