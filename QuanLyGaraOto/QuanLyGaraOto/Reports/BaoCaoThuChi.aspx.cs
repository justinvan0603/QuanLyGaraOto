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
    public partial class BaoCaoThuChi : System.Web.UI.Page
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
            string fromdate = Request["tungay"];
            DateTime tungay = DateTime.Parse(fromdate);
            string todate = Request["denngay"];
            DateTime denngay = DateTime.Parse(todate);
            ThuChiReportViewer.Reset();
            ThuChiReportViewer.LocalReport.EnableExternalImages = true;
            ThuChiReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/BaoCaoThuChi.rdlc");
            List<ReportParameter> ListParam = new List<ReportParameter>();
            
            string ngaylap = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            string TuNgay = tungay.Day + "/" + tungay.Month + "/" + tungay.Year;
            string DenNgay = denngay.Day + "/" + denngay.Month + "/" + denngay.Year;
            ReportParameter param = new ReportParameter("NgayLap", ngaylap);
            ReportParameter param1 = new ReportParameter("TuNgay", TuNgay);
            ReportParameter param2 = new ReportParameter("DenNgay", DenNgay);
            ListParam.Add(param);
            ListParam.Add(param1);
            ListParam.Add(param2);
            GARADBDataSet ds = new GARADBDataSet();
            GARADBDataSetTableAdapters.TC_BAOCAOTHUCHI_StoreTableAdapter adapter = new GARADBDataSetTableAdapters.TC_BAOCAOTHUCHI_StoreTableAdapter();
            adapter.Fill(ds.TC_BAOCAOTHUCHI_Store, tungay, denngay);
            ThuChiReportViewer.LocalReport.SetParameters(ListParam);
            
            DataTable tb = ds.TC_BAOCAOTHUCHI_Store;
            ThuChiReportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("GaraDBDataSet", tb));

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string fromdate = Request["tungay"];
            DateTime tungay = DateTime.Parse(fromdate);
            string todate = Request["denngay"];
            DateTime denngay = DateTime.Parse(todate);
            ThuChiReportViewer.Reset();
            ThuChiReportViewer.LocalReport.EnableExternalImages = true;
            ThuChiReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/BaoCaoThuChi.rdlc");
            List<ReportParameter> ListParam = new List<ReportParameter>();

            string ngaylap = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            string TuNgay = tungay.Day + "/" + tungay.Month + "/" + tungay.Year;
            string DenNgay = denngay.Day + "/" + denngay.Month + "/" + denngay.Year;
            ReportParameter param = new ReportParameter("NgayLap", ngaylap);
            ReportParameter param1 = new ReportParameter("TuNgay", TuNgay);
            ReportParameter param2 = new ReportParameter("DenNgay", DenNgay);
            ListParam.Add(param);
            ListParam.Add(param1);
            ListParam.Add(param2);
            GARADBDataSet ds = new GARADBDataSet();
            GARADBDataSetTableAdapters.TC_BAOCAOTHUCHI_StoreTableAdapter adapter = new GARADBDataSetTableAdapters.TC_BAOCAOTHUCHI_StoreTableAdapter();
            adapter.Fill(ds.TC_BAOCAOTHUCHI_Store, tungay, denngay);
            ThuChiReportViewer.LocalReport.SetParameters(ListParam);

            DataTable tb = ds.TC_BAOCAOTHUCHI_Store;
            ThuChiReportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("GaraDBDataSet", tb));
            string deviceInfo =
          "<DeviceInfo>" +
          "  <OutputFormat>EMF</OutputFormat>" +
          "  <PageWidth>8.5in</PageWidth>" +
          "  <PageHeight>11in</PageHeight>" +
          "<MarginTop>0.25in</MarginTop>"+
                "<MarginLeft>0.25in</MarginLeft>"+
                "<MarginRight>0.25in</MarginRight>"+
                "<MarginBottom>0.25in</MarginBottom>"+
          "</DeviceInfo>";
            //DataTable tb = ds.KH_BAOCAOCONGNO_Store;
            //ThuChiReportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("GaraDBDataSet", tb));
            byte[] bytes = ThuChiReportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);



            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + "BaoCaoThuChi" + "." + "pdf");
            //Response.TransmitFile("BaoCaoCongNo");
            Response.BinaryWrite(bytes);
            Response.End();
            //  Response.Flush(); 
        }
    }
}