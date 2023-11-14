using Lab2.Models;
using PagedList;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Net;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        // Use DbContext to manage database
        QLBANSACHEntities database = new QLBANSACHEntities();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login");
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(ADMIN admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.UserAdmin))
                    ModelState.AddModelError(string.Empty, "User name không được để trống");
                if (string.IsNullOrEmpty(admin.PassAdmin))
                    ModelState.AddModelError(string.Empty, "Password không được để trống");
                //Kiểm tra có admin này hay chưa
                var adminDB = database.ADMINs.FirstOrDefault(ad => ad.UserAdmin ==
                admin.UserAdmin && ad.PassAdmin == admin.PassAdmin);
                if (adminDB == null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                else
                {
                    Session["Admin"] = adminDB;

                    ViewBag.ThongBao = "Đăng nhập admin thành công";
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();

        }
        public ActionResult Sach(int? page)
        {
            var dsSach = database.SACHes.ToList();
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 7;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            return View(dsSach.OrderBy(sach => sach.Masach).ToPagedList(pageNum,
            pageSize));
        }
        public ActionResult SuaSach(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sach = database.SACHes.FirstOrDefault(s => s.Masach == id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCD = new SelectList(database.CHUDEs.ToList(), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs.ToList(), "MaNXB", "TenNXB");
            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSach([Bind(Include = "Masach,TenSach,Donvitinh,Dongia,Mota,Hinhminhhoa,MaCD,MaNXB,Ngaycapnhat,Soluongban,solanxem")] SACH sach, HttpPostedFileBase Hinhminhhoa)
        {
            if (ModelState.IsValid)
            {
                if (Hinhminhhoa != null)
                {
                    var fileName = Path.GetFileName(Hinhminhhoa.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    sach.Hinhminhhoa = fileName;
                    Hinhminhhoa.SaveAs(path);
                }

                database.Entry(sach).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Sach");
            }
            ViewBag.MaCD = new SelectList(database.CHUDEs.ToList(), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs.ToList(), "MaNXB", "TenNXB");
            return View(sach);
        }

        [HttpGet]
        public ActionResult ThemSach()
        {
            ViewBag.MaCD = new SelectList(database.CHUDEs.ToList(), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs.ToList(), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        public ActionResult ThemSach(SACH Sach, HttpPostedFileBase Hinhminhhoa)
        {
            ViewBag.MaCD = new SelectList(database.CHUDEs.ToList(), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs.ToList(), "MaNXB", "TenNXB");
            if (Hinhminhhoa == null)
                return View();
            else
                if (ModelState.IsValid)
            {
                var filename = Path.GetFileName(Hinhminhhoa.FileName);

                var path = Path.Combine(Server.MapPath("~/Images"), filename);

                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hinh da ton tai";
                }
                else
                {
                    Hinhminhhoa.SaveAs(path);
                }
            }
            return RedirectToAction("Sach");
        }
        public ActionResult ChiTietSach(int id)
        {
            var sach = database.SACHes.FirstOrDefault(s => s.Masach == id);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
    
    }
}