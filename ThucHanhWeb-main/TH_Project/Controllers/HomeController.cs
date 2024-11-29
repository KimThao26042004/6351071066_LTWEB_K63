using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH_Project.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using TH_Project.ViewModel;

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
        public async  Task<ActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                XeGanMay = await _qlbanMayEntities1.XEGANMAY.ToListAsync(),
                LoaiXe =await _qlbanMayEntities1.LOAIXE.ToListAsync(),
                NhaPhanPhoi = await _qlbanMayEntities1.NHAPHANPHOI.ToListAsync()
            };
          
            return View(homeVM);
        }


    }
}