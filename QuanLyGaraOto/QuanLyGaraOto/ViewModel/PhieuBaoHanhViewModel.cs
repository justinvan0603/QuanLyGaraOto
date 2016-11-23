using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using QuanLyGaraOto.Models;
using System.Web.Mvc;

namespace QuanLyGaraOto.ViewModel
{
    public class PhieuBaoHanhViewModel
    {
        // danh sach phieu chi trong database
        public IPagedList<PHIEU_BAOHANH> danhSachPhieuBaoHanh { get; set; }

        // danh sach chi tiet
        public List<CHITIET_PHIEUBH> danhSachChiTietBaoHanh { get; set; }        

        // phieu bao hanh hien tai ma nguoi dung chon
        public PHIEU_BAOHANH selectedItem { get; set; }

        // danh sach options de loc du lieu
        public List<SelectListItem> listSearchOption = new List<SelectListItem>() { new SelectListItem(){Text = "", Value="0"},
                                                                                    new SelectListItem(){Text = "Ngày lập", Value="1"},
                                                                                    new SelectListItem(){Text = "Ngảy hẹn trả" , Value = "2"}};


        // danh sach phu tung
        public List<PHUTUNG> danhSachPhuTung { get; set; }

        /**
         *  Model cho việc lập phiếu bảo hành từ phiếu dịch vụ
         * */
        public PHIEU_DICHVU phieuDichVu { get; set; }

        public List<CHITIET_PHIEUDV> danhSachChiTietDichVu { get; set; }

        /**
         * database service
         * */

        public GARADBEntities service = new GARADBEntities();
    }
}