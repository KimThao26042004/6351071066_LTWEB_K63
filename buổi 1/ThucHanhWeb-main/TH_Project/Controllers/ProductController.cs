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
    public class ProductController : Controller
    {
        private readonly QLBanXeGanMayEntities1 _qlbanMayEntities1;
        public ProductController(QLBanXeGanMayEntities1 db)
        {
            _qlbanMayEntities1 = db;
        }
        public ProductController() : this(new QLBanXeGanMayEntities1())
        {
        }
        // GET: Product
        [HttpGet]
        public async Task<ActionResult> Index(int? idNXB, int? idCD)
        {
            var products = new ProductVM() { 
                LoaiXe = await _qlbanMayEntities1.LOAIXE.ToListAsync(),
                NhaPhanPhoi = await _qlbanMayEntities1.NHAPHANPHOI.ToListAsync()
            };

            if (idNXB.HasValue)
                products.XeGanMay  = await _qlbanMayEntities1.XEGANMAY.Where(s => s.MaNPP == idNXB.Value).ToListAsync();
            else if (idCD.HasValue)
                products.XeGanMay = await _qlbanMayEntities1.XEGANMAY.Where(s => s.MaLX == idCD.Value).ToListAsync();
            else
                products.XeGanMay = await _qlbanMayEntities1.XEGANMAY.ToListAsync(); 

            return View(products); 
        }

        [HttpGet]
        public async Task<ActionResult> ProductDetail(int id)
        {

            var product = await _qlbanMayEntities1.XEGANMAY.FindAsync(id);
            var productVM = new DetailProductVM()
            {
                LoaiXe = await _qlbanMayEntities1.LOAIXE.ToListAsync(),
                NhaPhanPhoi = await _qlbanMayEntities1.NHAPHANPHOI.ToListAsync(),
                XEGANMAY = product
            };

            if (product == null)
            {
                return HttpNotFound(); 
            }

            return View(productVM);
        }


    }
}