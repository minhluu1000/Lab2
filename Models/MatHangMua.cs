using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Lab2.Models
{
    public class MatHangMua
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string AnhBia { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }

        public double ThanhTien()
        {
            return SoLuong * DonGia;
        }
        public MatHangMua(int MaSach)
        {
            this.MaSach = MaSach;

            var sach = db.SACHes.Single(s => s.Masach == this.MaSach);
            this.TenSach = sach.Tensach;
            this.AnhBia = sach.Hinhminhhoa;
            this.DonGia = double.Parse(sach.Dongia.ToString());
            this.SoLuong = 1;
        }
    }
}