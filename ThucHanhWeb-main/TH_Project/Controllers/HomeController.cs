using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH_Project.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using TH_Project.ViewModel;
using PagedList;
using PagedList.Mvc;

namespace TH_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly QLBanXeGanMayEntities1 _qlbanMayEntities1;
      
        public HomeController(QLBanXeGanMayEntities1 db)
        {
            _qlbanMayEntities1 = db;
        }
        public HomeController() : this(new QLBanXeGanMayEntities1())
        {
        }
        [HttpGet]
        public async  Task<ActionResult> Index(int? page)
        {
            int pageSize = 15; // Số mục mỗi trang
            int pageNumber = (page ?? 1); // Nếu không có số trang, mặc định là trang 1

            // Lấy tất cả dữ liệu (sử dụng async)
            var xeGanMayList = await _qlbanMayEntities1.XEGANMAY.ToListAsync();
            var loaiXeList = await _qlbanMayEntities1.LOAIXE.ToListAsync();
            var nhaPhanPhoiList = await _qlbanMayEntities1.NHAPHANPHOI.ToListAsync();

            // Chuyển xeGanMayList thành IPagedList để phân trang (Không cần await vì ToPagedList() là đồng bộ)
            var xeGanMayPaged = xeGanMayList.ToPagedList(pageNumber, pageSize);

            // Tạo ViewModel
            HomeVM homeVM = new HomeVM()
            {
                XeGanMay = xeGanMayPaged, // Sử dụng IPagedList để phân trang
                LoaiXe = loaiXeList,
                NhaPhanPhoi = nhaPhanPhoiList
            };

            // Truyền dữ liệu cho View
            ViewData["LOAIXE"] = loaiXeList;
            ViewData["NPP"] = nhaPhanPhoiList;

            return View(homeVM);
        }


    }
}