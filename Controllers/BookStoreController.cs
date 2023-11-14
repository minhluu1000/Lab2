using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Lab2.Models;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Web.UI;

namespace Lab2.Controllers
{
    public class BookStoreController : Controller
    {
        // Use DbContext to manage database
        QLBANSACHEntities database = new QLBANSACHEntities();
        private List<SACH> LaySachMoi(int soluong)
        {
            // Sắp xếp sách theo ngày cập nhật giảm dần, lấy đúng số lượng sách cần
            // Chuyển qua dạng danh sách kết quả đạt được
            return database.SACHes.OrderByDescending(sach =>
            sach.Ngaycapnhat).Take(soluong).ToList();
        }
        // GET: BookStore
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = page ?? 1;

            var dsSachMoi = LaySachMoi(15);
            return View(dsSachMoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult LayChuDe()
        {
            var dsChuDe = database.CHUDEs.ToList();
            return PartialView(dsChuDe);
        }
        public ActionResult LayNhaXuatBan()
        {
            var dsNhaXB = database.NHAXUATBANs.ToList();
            return PartialView(dsNhaXB);
        }
        public ActionResult SPTheoChuDe(int id, int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var dsSachTheoChuDe = database.SACHes.Where(sach => sach.MaCD == id).ToList();

            return View("Index", dsSachTheoChuDe.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SPTheoNXBC(int id, int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var dsSachNXB = database.SACHes.Where(sach => sach.MaNXB == id).ToList();

            return View("Index", dsSachNXB.ToPagedList(pageNum, pageSize));

        }
        public ActionResult Details(int id)
        {
            //Läy såch c6 mä tddng tng
            var sach = database.SACHes.FirstOrDefault(s => s.Masach == id);
            return View(sach);
        }
    }
}