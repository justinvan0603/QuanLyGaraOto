﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaoCaoCongNo.aspx.cs" Inherits="QuanLyGaraOto.Reports.BaoCaoCongNo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Báo cáo công nợ</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnExport" runat="server" Text="Xuất file PDF" OnClick="btnExport_Click" />
        <asp:scriptmanager runat="server"></asp:scriptmanager>
        <rsweb:ReportViewer ID="CongNoReportViewer" runat="server" ZoomMode="PageWidth" BackColor="White"
             AsyncRendering="false" BorderColor="White" ShowExportControls="false" Height="600" Width="900" ShowPrintButton="true">

        </rsweb:ReportViewer>

    </div>
    </form>
</body>
</html>
