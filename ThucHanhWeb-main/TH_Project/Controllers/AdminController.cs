using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TH_Project.Data;

namespace TH_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly QLBanXeGanMayEntities1 _qlbanMayEntities1;
        public AdminController(QLBanXeGanMayEntities1 db)
        {
            _qlbanMayEntities1 = db;
        }
        public AdminController() : this(new QLBanXeGanMayEntities1())
        {
        }
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Xe(int? page)
        {
            int pageSize = 9; // Số lượng sách trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là trang 1

            var books = await _qlbanMayEntities1.XEGANMAY.ToListAsync(); // Lấy tất cả sách từ cơ sở dữ liệu
            var pagedBooks = books.ToPagedList(pageNumber, pageSize); // Phân trang danh sách sách

            return View(pagedBooks); // Trả về view với danh sách đã phân trang
        }


        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Lấy dữ liệu từ FormCollection
            var tendn = collection["username"];
            var matkhau = collection["password"];

            // Kiểm tra dữ liệu nhập vào
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }

            // Kiểm tra nếu không có lỗi
            if (string.IsNullOrEmpty(ViewData["Loi1"]?.ToString()) && string.IsNullOrEmpty(ViewData["Loi2"]?.ToString()))
            {
                // Kiểm tra tên đăng nhập và mật khẩu
                Admin ad = _qlbanMayEntities1.Admin.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    // Lưu thông tin vào session và chuyển hướng
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewData["Thongbao"] = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            // Trả về view nếu có lỗi
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> Create()
        {

            ViewBag.MaLX = new SelectList(_qlbanMayEntities1.LOAIXE.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(_qlbanMayEntities1.NHAPHANPHOI.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]

        public async Task<ActionResult> Create(XEGANMAY xeGanMay, HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName); // Lưu đường dẫn của file
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName); // Kiểm tra hình ảnh tồn tại chưa?

                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    // Lưu hình ảnh vào đường dẫn
                    fileUpload.SaveAs(path);
                    xeGanMay.Anhbia = "/Content/images/" + fileName; // Lưu đường dẫn hình ảnh vào đối tượng sach
                }
            }
            else
            {
                ViewBag.Thongbao = "Vui lòng chọn hình ảnh để tải lên."; // Thông báo nếu không có tệp nào được tải lên
            }

            // Gửi dữ liệu cho View
            ViewBag.MaLX = new SelectList(_qlbanMayEntities1.LOAIXE.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(_qlbanMayEntities1.NHAPHANPHOI.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");

            // Lưu thông tin sách vào cơ sở dữ liệu
            _qlbanMayEntities1.XEGANMAY.Add(xeGanMay);
            await _qlbanMayEntities1.SaveChangesAsync();

            // Trả về View
            return RedirectToAction("Xe");
        }


        public async Task UploadImageAsync(XEGANMAY product, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                // Tạo tên file duy nhất để tránh trùng lặp
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Content/images/"), uniqueFileName);

                // Kiểm tra xem thư mục đã tồn tại chưa, nếu chưa thì tạo mới
                var directoryPath = Server.MapPath("~/Content/images/");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath); // Tạo thư mục nếu chưa có
                }

                // Lưu tệp vào thư mục
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.InputStream.CopyToAsync(fileStream);
                }

                // Cập nhật đường dẫn hình ảnh vào đối tượng product
                product.Anhbia = "/Content/images/" + uniqueFileName;
            }
            else
            {
                throw new InvalidOperationException("Không có tệp nào được tải lên.");
            }
        }


        public async Task<ActionResult> Details(int id)
        {
            XEGANMAY xeGanMay = await _qlbanMayEntities1.XEGANMAY.FindAsync(id);
            ViewBag.MaXe = xeGanMay.MaXe;
            if (xeGanMay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xeGanMay);

        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        { // lấy ra đối tượng xe cần xóa theo mã
            XEGANMAY xeGanMay = await _qlbanMayEntities1.XEGANMAY.SingleOrDefaultAsync(n => n.MaXe == id);
            ViewBag.MaXe = xeGanMay;
            if (xeGanMay == null) { Response.StatusCode = 404; return null; }
            return View(xeGanMay);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            var xeGanMay = await _qlbanMayEntities1.XEGANMAY.FindAsync(id);

            if (xeGanMay == null)
            {
                return HttpNotFound();
            }

            // Xóa các bản ghi tham chiếu trong bảng SANXUATXE
            var sanXuatXeRecords = _qlbanMayEntities1.SANXUATXE.Where(s => s.MaXe == id).ToList();
            foreach (var record in sanXuatXeRecords)
            {
                _qlbanMayEntities1.SANXUATXE.Remove(record);
            }

            // Sau khi xóa các bản ghi tham chiếu, xóa bản ghi trong bảng XEGANMAY
            _qlbanMayEntities1.XEGANMAY.Remove(xeGanMay);

            await _qlbanMayEntities1.SaveChangesAsync();

            return RedirectToAction("Xe");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Lấy đối tượng xe theo id
            XEGANMAY xeGanMay = _qlbanMayEntities1.XEGANMAY.SingleOrDefault(n => n.MaXe == id);

            if (xeGanMay == null)
            {
                Response.StatusCode = 404;
                return HttpNotFound("Không tìm thấy xe.");
            }

            // Lấy danh sách loại xe từ cơ sở dữ liệu
            var loaiXeList = _qlbanMayEntities1.LOAIXE.OrderBy(n => n.TenLoaiXe).ToList();

            // Gán SelectList vào ViewBag.MaLX
            // Chuyển đổi danh sách loại xe thành SelectList, với giá trị mặc định là MaLX của xeGanMay
            ViewBag.MaLX = new SelectList(loaiXeList, "MaLX", "TenLoaiXe", xeGanMay.MaLX);

            // Lấy danh sách nhà phân phối và gán vào ViewBag.MaNPP
            ViewBag.MaNPP = new SelectList(_qlbanMayEntities1.NHAPHANPHOI.OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", xeGanMay.MaNPP);

            // Trả về view với đối tượng xe
            return View(xeGanMay);
        }


        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(XEGANMAY xeGanMay, HttpPostedFileBase fileUpload)
        {
            // Kiểm tra xem model có hợp lệ không
            if (xeGanMay == null)
            {
                ViewBag.Thongbao = "Thông tin sản phẩm không hợp lệ.";
                return View(xeGanMay);
            }

            if (xeGanMay.MaXe == 0)
            {
                ViewBag.Thongbao = "ID sản phẩm không hợp lệ.";
                return View(xeGanMay);
            }

            // Xử lý upload file
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                // Kiểm tra nếu file đã tồn tại
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại.";
                }
                else
                {
                    // Lưu file vào server
                    fileUpload.SaveAs(path);
                    xeGanMay.Anhbia = "/Content/images/" + fileName;
                }
            }
            else if (string.IsNullOrEmpty(xeGanMay.Anhbia))
            {
                ViewBag.Thongbao = "Vui lòng chọn hình ảnh.";
                return View(xeGanMay);
            }

            try
            {
                // Tìm sách trong database
                var model = await _qlbanMayEntities1.XEGANMAY.FirstOrDefaultAsync(n => n.MaXe == xeGanMay.MaXe);

                if (model != null)
                {
                    // Cập nhật thông tin sách
                    model.TenXe = xeGanMay.TenXe;
                    model.Mota = xeGanMay.Mota;
                    model.MaLX = xeGanMay.MaLX;
                    model.MaNPP = xeGanMay.MaNPP;
                    model.Giaban = xeGanMay.Giaban;
                    model.Soluongton = xeGanMay.Soluongton;
                    model.Anhbia = xeGanMay.Anhbia;
                    model.Ngaycapnhat = DateTime.Now;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _qlbanMayEntities1.SaveChangesAsync();

                    ViewBag.Thongbao = "Cập nhật sách thành công!";
                    return RedirectToAction("Xe");
                }
                else
                {
                    ViewBag.Thongbao = "Không tìm thấy sản phẩm cần sửa.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Thongbao = "Đã xảy ra lỗi: " + ex.Message;
            }

            // Tạo lại SelectList cho dropdown
            ViewBag.MaLX = new SelectList(_qlbanMayEntities1.LOAIXE.OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe", xeGanMay.MaLX);
            ViewBag.MaNPP = new SelectList(_qlbanMayEntities1.NHAPHANPHOI.OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", xeGanMay.MaNPP);

            // Trả về view với dữ liệu model
            return View(xeGanMay);
        }


        public ActionResult ThongKeXe()
        {
            var chartData = _qlbanMayEntities1.XEGANMAY
                               .GroupBy(s => s.MaLX)
                               .Select(g => new
                               {
                                   TenLX = g.FirstOrDefault().LOAIXE.TenLoaiXe,
                                   SoLuong = g.Count()
                               })
                               .ToList();

            return View(chartData);
        }

    }
}