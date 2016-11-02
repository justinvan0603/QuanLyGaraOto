using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyGaraOto.Reports
{
    public partial class BaoCaoTonKho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RenderReport();
            }
        }
        private void RenderReport()
        {

            TonKhoReportViewer.Reset();
            TonKhoReportViewer.LocalReport.EnableExternalImages = true;
            TonKhoReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/BaoCaoTonKho.rdlc");

            string ngaylap = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            ReportParameter param = new ReportParameter("NgayLap", ngaylap);
            GARADBDataSet ds = new GARADBDataSet();
            GARADBDataSetTableAdapters.PT_BAOCAOTONKHO_StoreTableAdapter adapter = new GARADBDataSetTableAdapters.PT_BAOCAOTONKHO_StoreTableAdapter();
            adapter.Fill(ds.PT_BAOCAOTONKHO_Store);
            TonKhoReportViewer.LocalReport.SetParameters(param);

            DataTable tb = ds.PT_BAOCAOTONKHO_Store;
            TonKhoReportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("GaraDBDataSet", tb));

        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            TonKhoReportViewer.Reset();
            TonKhoReportViewer.LocalReport.EnableExternalImages = true;
            TonKhoReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/BaoCaoTonKho.rdlc");

            string ngaylap = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            ReportParameter param = new ReportParameter("NgayLap", ngaylap);
            GARADBDataSet ds = new GARADBDataSet();
            GARADBDataSetTableAdapters.PT_BAOCAOTONKHO_StoreTableAdapter adapter = new GARADBDataSetTableAdapters.PT_BAOCAOTONKHO_StoreTableAdapter();
            adapter.Fill(ds.PT_BAOCAOTONKHO_Store);
            TonKhoReportViewer.LocalReport.SetParameters(param);

            DataTable tb = ds.PT_BAOCAOTONKHO_Store;
            TonKhoReportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("GaraDBDataSet", tb));
            string deviceInfo =
          "<DeviceInfo>" +
          "  <OutputFormat>EMF</OutputFormat>" +
          "  <PageWidth>29.7cm</PageWidth>" +
          "  <PageHeight>21cm</PageHeight>" +

          "</DeviceInfo>";

            byte[] bytes = TonKhoReportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);



            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + "BaoCaoTonKho" + "." + "pdf");
            //Response.TransmitFile("BaoCaoCongNo");
            Response.BinaryWrite(bytes);
            Response.End();
            //  Response.Flush(); 

        }
    }
}