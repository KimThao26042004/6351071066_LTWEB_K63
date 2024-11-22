using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TH_Project.Data;

namespace TH_Project.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<XEGANMAY> XeGanMay { get; set; }
        public IEnumerable<LOAIXE> LoaiXe { get; set; }
        public IEnumerable<NHAPHANPHOI> NhaPhanPhoi { get; set; }


    }
}