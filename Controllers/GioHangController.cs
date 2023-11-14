using Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2.Controllers
{
    public class GioHangController : Controller
    {
        public List<MatHangMua> LayGioHang()
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;
            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }
        public ActionResult ThemSanPhamVaoGio(int MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();
            MatHangMua sanPham = gioHang.FirstOrDefault(s => s.MaSach == MaSP);
            if (sanPham == null)
            {
                sanPham = new MatHangMua(MaSP);

                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.SoLuong++;
            }
            return RedirectToAction("Details", "BookStore", new
            {
                id = MaSP
            });
        }
        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = LayGioHang();

            if (gioHang == null || gioHang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }
        public ActionResult XoaMatHang(int MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();

            var sanpham = gioHang.FirstOrDefault(s => s.MaSach == MaSP);
            if (sanpham !=null)
            {
                gioHang.RemoveAll(s => s.MaSach == MaSP);
                return RedirectToAction("HienThiGioHang");
            }
            if (gioHang.Count == 0)
                return RedirectToAction("Index", "BookStore");
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult CapNhatMatHang(int MaSP, int SoLuong) {
            List<MatHangMua> gioHang = LayGioHang();

            var sanpham = gioHang.FirstOrDefault(s => s.MaSach == MaSP);

            if (sanpham !=null)
            {
                sanpham.SoLuong = SoLuong;
            }
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null)
                return RedirectToAction("DangNhap", "NguoiDung");
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0)
                return RedirectToAction("Index", "BookStore");

            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);

        }
        QLBANSACHEntities database = new QLBANSACHEntities();
        public ActionResult DongYDatHang()
        {
            KHACHHANG khach = Session["TaiKhoan"] as KHACHHANG;
            List<MatHangMua> gioHang = LayGioHang();

            DONDATHANG DonHang = new DONDATHANG();
            DonHang.MaKH = khach.MaKH;
            DonHang.NgayDH = DateTime.Now;
            DonHang.Trigia = (decimal)TinhTongTien();
            DonHang.Dagiao = false;
            DonHang.Tennguoinhan = khach.HoTenKH;
            DonHang.Diachinhan = khach.DiachiKH;
            DonHang.Dienthoainhan = khach.DienthoaiKH;
            DonHang.HTThanhtoan = false;
            DonHang.HTGiaohang = false;
            database.DONDATHANGs.Add(DonHang);
            database.SaveChanges();


            foreach (var sanpham in gioHang)
            {
                CTDATHANG chitiet = new CTDATHANG();
                chitiet.SoDH = DonHang.SoDH;
                chitiet.Masach = sanpham.MaSach;
                chitiet.Soluong = sanpham.SoLuong;
                chitiet.Dongia = (decimal)sanpham.DonGia;
                database.CTDATHANGs.Add(chitiet);
            }
            database.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        public ActionResult HoanThanhDonHang()
        {
            return View();
        }
        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.SoLuong);
            return tongSL;
        }
        private int TinhTongTien()
        {
            int TongTien = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                TongTien = gioHang.Sum(sp => sp.SoLuong);
            return TongTien;
        }
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
    }
}