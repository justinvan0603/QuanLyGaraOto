using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuanLyGaraOto;
using System.Data;
namespace QuanLyGaraOto.Reports
{
    public partial class BaoCaoCongNo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                RenderReport();
            }
        }
        private void RenderReport()
        {
            
            CongNoReportViewer.Reset();
            CongNoReportViewer.LocalReport.EnableExternalImages = true;
            CongNoReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/BaoCaoCongNo.rdlc");

            string ngaylap = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            ReportParameter param = new ReportParameter("NgayLap", ngaylap);
            GARADBDataSet ds = new GARADBDataSet();
            GARADBDataSetTableAdapters.KH_BAOCAOCONGNO_StoreTableAdapter adapter = new GARADBDataSetTableAdapters.KH_BAOCAOCONGNO_StoreTableAdapter();
            adapter.Fill(ds.KH_BAOCAOCONGNO_Store);
            CongNoReportViewer.LocalReport.SetParameters(param);

            DataTable tb = ds.KH_BAOCAOCONGNO_Store;
            CongNoReportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("GaraDBDataSet", tb));
  
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            CongNoReportViewer.Reset();
            CongNoReportViewer.LocalReport.EnableExternalImages = true;
            CongNoReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/BaoCaoCongNo.rdlc");

            string ngaylap = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            ReportParameter param = new ReportParameter("NgayLap", ngaylap);
            GARADBDataSet ds = new GARADBDataSet();
            GARADBDataSetTableAdapters.KH_BAOCAOCONGNO_StoreTableAdapter adapter = new GARADBDataSetTableAdapters.KH_BAOCAOCONGNO_StoreTableAdapter();
            adapter.Fill(ds.KH_BAOCAOCONGNO_Store);
            CongNoReportViewer.LocalReport.SetParameters(param);
            string deviceInfo =
          "<DeviceInfo>" +
          "  <OutputFormat>EMF</OutputFormat>" +
          "  <PageWidth>29.7cm</PageWidth>" +
          "  <PageHeight>21cm</PageHeight>" +
   
          "</DeviceInfo>";
            DataTable tb = ds.KH_BAOCAOCONGNO_Store;
            CongNoReportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("GaraDBDataSet", tb));
            byte[] bytes = CongNoReportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
  

  
    Response.Buffer = true;
    Response.Clear();
    Response.ContentType = mimeType;
    Response.AddHeader("content-disposition", "attachment; filename=" + "BaoCaoCongNo" + "." + "pdf");
    //Response.TransmitFile("BaoCaoCongNo");
    Response.BinaryWrite(bytes);
    Response.End();
  //  Response.Flush(); 

        }
    }
}