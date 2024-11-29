using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TH_Project.Data;

namespace TH_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly QLBanXeGanMayEntities1 _qlbanMayEntities1;
        public UserController(QLBanXeGanMayEntities1 db)
        {
            _qlbanMayEntities1 = db;
        }
        public UserController() : this(new QLBanXeGanMayEntities1())
        {
        }
        // GET: User
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ViewData["XEGANMAY"] = await _qlbanMayEntities1.LOAIXE.ToListAsync();
            ViewData["NPP"] = await _qlbanMayEntities1.NHAPHANPHOI.ToListAsync();
            return View(new KHACHHANG());
        }

        [HttpPost]
        public async Task<ActionResult> DangNhap(string TaiKhoan, string MatKhau)
        {
            var user = await _qlbanMayEntities1.KHACHHANG.FirstOrDefaultAsync(u => u.Taikhoan == TaiKhoan && u.Matkhau == MatKhau);

            if (user == null)
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                return View("Index");
            }


            Session["TaiKhoan"] = user;
            Session["UserName"] = user.HoTen;

            ViewData["LOAIXE"] = await _qlbanMayEntities1.LOAIXE.ToListAsync();
            ViewData["NPP"] = await _qlbanMayEntities1.NHAPHANPHOI.ToListAsync();

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<ActionResult> DangKy()
        {
            ViewData["LOAIXE"] = await _qlbanMayEntities1.LOAIXE.ToListAsync();
            ViewData["NPP"] = await _qlbanMayEntities1.NHAPHANPHOI.ToListAsync();
            return View(new KHACHHANG());
        }

        [HttpPost]
        public async Task<ActionResult> DangKy(KHACHHANG model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = _qlbanMayEntities1.KHACHHANG.FirstOrDefault(u => u.Taikhoan == model.Taikhoan);
            if (existingUser != null)
            {
                ModelState.AddModelError("TaiKhoan", "Tài khoản đã tồn tại.");
                return View(model);
            }

            var existingEmail = _qlbanMayEntities1.KHACHHANG.FirstOrDefault(u => u.Email == model.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng.");
                return View(model);
            }

            var user = new KHACHHANG()
            {
                HoTen = model.HoTen,
                Taikhoan = model.Taikhoan,
                Matkhau = model.Matkhau,
                Email = model.Email,
                DiachiKH = model.DiachiKH,
                DienthoaiKH = model.DienthoaiKH,
                Ngaysinh = model.Ngaysinh
            };

            try
            {
                _qlbanMayEntities1.KHACHHANG.Add(user);
                await _qlbanMayEntities1.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã có lỗi xảy ra, vui lòng thử lại.");
                return View(model);
            }
        }
    }
}