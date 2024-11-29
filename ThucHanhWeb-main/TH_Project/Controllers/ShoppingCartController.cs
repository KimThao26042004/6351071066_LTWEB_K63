using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TH_Project.Data;
using TH_Project.Model;

namespace TH_Project.Controllers
{
    public class GioHangController : Controller
    {
        private readonly QLBanXeGanMayEntities1 _qlbanMayEntities1;
        public GioHangController(QLBanXeGanMayEntities1 db)
        {
            _qlbanMayEntities1 = db;
        }
        public GioHangController() : this(new QLBanXeGanMayEntities1())
        {
        }

        public List<ShoppingCartVM> LayGioHang()
        {
            var list = Session["GioHang"] as List<ShoppingCartVM>;
            if (list == null)
            {
                list = new List<ShoppingCartVM>();
                Session["GioHang"] = list;

            }
            return list;
        }
        // GET: GioHang
        public ActionResult Index()
        {
            var list = LayGioHang();
            if (list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            ViewData["LOAIXE"] = _qlbanMayEntities1.LOAIXE.ToList();
            ViewData["NPP"] = _qlbanMayEntities1.NHAPHANPHOI.ToList();
            return View(list);
        }

        public ActionResult ThemGioHang(int MaXe, string strURL)
        {

            var list = LayGioHang();
            var sanPham = list.Find(n => n.iMaXe == MaXe);

            if (sanPham == null)
            {
                sanPham = new ShoppingCartVM(MaXe);
                list.Add(sanPham);
                return Redirect(strURL);
            }
            else
            {
                sanPham.SoLuong++;
                return Redirect(strURL);
            }
        }

        public ActionResult XoaGioHang(int iMaSP)
        {
            List<ShoppingCartVM> lstGiohang = LayGioHang();
            ShoppingCartVM sanpham = lstGiohang.SingleOrDefault(n => n.iMaXe == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaXe == iMaSP);
                return RedirectToAction("Index");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["UserName"] == null || string.IsNullOrEmpty(Session["UserName"].ToString()) ||
     Session["TaiKhoan"] == null || string.IsNullOrEmpty(Session["TaiKhoan"].ToString()))
            {
                return RedirectToAction("Index", "User");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<ShoppingCartVM> lstGiohang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewData["LOAIXE"] = _qlbanMayEntities1.LOAIXE.ToList();
            ViewData["NPP"] = _qlbanMayEntities1.NHAPHANPHOI.ToList();
            return View(lstGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("DangNhap", "User");
            }

            List<ShoppingCartVM> gh = LayGioHang();
            if (gh == null || gh.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;

            var ngaygiao = collection["Ngaygiao"];
            if (DateTime.TryParse(ngaygiao, out DateTime ngayGiaoParsed))
            {
                ddh.Ngaygiao = ngayGiaoParsed;
            }
            else
            {

                ddh.Ngaygiao = DateTime.Now.AddDays(1);
            }

            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;

            _qlbanMayEntities1.DONDATHANG.Add(ddh);
            _qlbanMayEntities1.SaveChanges();

            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaXe = item.iMaXe;
                ctdh.Soluong = item.SoLuong;
                ctdh.Dongia = (decimal)item.GiaBan;

                _qlbanMayEntities1.CHITIETDONTHANG.Add(ctdh);
            }

            _qlbanMayEntities1.SaveChanges();

            Session["GioHang"] = null;

            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            ViewData["LOAIXE"] = _qlbanMayEntities1.LOAIXE.ToList();
            ViewData["NPP"] = _qlbanMayEntities1.NHAPHANPHOI.ToList();
            return View();
        }

        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<ShoppingCartVM> lstGiohang = LayGioHang();

            ShoppingCartVM sanpham = lstGiohang.SingleOrDefault(n => n.iMaXe == iMaSP);

            if (sanpham != null)
            {
                sanpham.SoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Index");
        }


        public ActionResult XoaTatcaGiohang()
        {
            // Lấy giỏ hàng từ Session
            List<ShoppingCartVM> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }


        public int TongSoLuong()
        {
            int count = 0;
            var list = Session["GioHang"] as List<ShoppingCartVM>;
            if (list != null)
            {
                count = list.Sum(n => n.SoLuong);
            }

            return count;
        }

        public Double TongTien()
        {
            double sum = 0;
            var list = Session["GioHang"] as List<ShoppingCartVM>;
            if (list != null)
            {
                sum = list.Sum(n => n.ThanhTien);
            }
            return sum;
        }
    }
}