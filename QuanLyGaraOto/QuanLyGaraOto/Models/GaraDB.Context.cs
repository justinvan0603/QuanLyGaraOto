﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyGaraOto.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GARADBEntities : DbContext
    {
        public GARADBEntities()
            : base("name=GARADBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BANGTHAMSO> BANGTHAMSOes { get; set; }
        public virtual DbSet<HIEUXE> HIEUXEs { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANGs { get; set; }
        public virtual DbSet<NHOMNGUOIDUNG> NHOMNGUOIDUNGs { get; set; }
        public virtual DbSet<PHIEU_BAOHANH> PHIEU_BAOHANH { get; set; }
        public virtual DbSet<PHUTUNG> PHUTUNGs { get; set; }
        public virtual DbSet<THO> THOes { get; set; }
        public virtual DbSet<TIENCONG> TIENCONGs { get; set; }
        public virtual DbSet<XE> XEs { get; set; }
        public virtual DbSet<PHIEU_TIEPNHAN> PHIEU_TIEPNHAN { get; set; }
        public virtual DbSet<CHITIET_PHIEUBH> CHITIET_PHIEUBH { get; set; }
        public virtual DbSet<CHITIET_PHIEUDV> CHITIET_PHIEUDV { get; set; }
        public virtual DbSet<CHITIET_PHIEUNH> CHITIET_PHIEUNH { get; set; }
        public virtual DbSet<PHIEU_DICHVU> PHIEU_DICHVU { get; set; }
        public virtual DbSet<PHIEU_NHAPHANG> PHIEU_NHAPHANG { get; set; }
        public virtual DbSet<PHIEU_THUTIEN> PHIEU_THUTIEN { get; set; }
        public virtual DbSet<PHIEUCHI> PHIEUCHIs { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIENs { get; set; }
    }
}
