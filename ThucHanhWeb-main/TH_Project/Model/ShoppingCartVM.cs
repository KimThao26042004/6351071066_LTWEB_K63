using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TH_Project.Data;

namespace TH_Project.Model
{
    public class ShoppingCartVM
    {
        QLBanXeGanMayEntities1 db = new QLBanXeGanMayEntities1();
        public int iMaXe { get; set; }
        public string TenXe { get; set; }
        public string AnhBia { get; set; }
        public Double GiaBan { get; set; }
        public int SoLuong { get; set; }
        public Double ThanhTien
        {
            get { return GiaBan * SoLuong; }
        }

        public ShoppingCartVM(int MaXe)
        {
            iMaXe = MaXe;
            XEGANMAY xe = db.XEGANMAY.Single(s => s.MaXe == MaXe);
            TenXe = xe.TenXe;
            AnhBia = xe.Anhbia;
            GiaBan = double.Parse(xe.Giaban.ToString());
            SoLuong = 1;
        }
    }
}