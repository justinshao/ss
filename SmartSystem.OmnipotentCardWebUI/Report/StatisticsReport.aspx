<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatisticsReport.aspx.cs" Inherits="SmartSystem.OmnipotentCardWebUI.Report.StatisticsReport" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="../Content/crystalreportviewers13/js/crviewer/crv.js" type="text/javascript"></script>
<link href="../Content/crystalreportviewers13/js/crviewer/images/style.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>  
   <form id="form1" runat="server">
    <div>    
        <CR:CrystalReportViewer ID="StatisticsReportViewer1" runat="server" 
            AutoDataBind="true" HasToggleGroupTreeButton="False" BackColor="White" 
            HasToggleParameterPanelButton="False" ToolPanelView="None"/>    
    </div>
    </form>    
</body>
</html>
