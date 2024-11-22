using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using TH_Project.Data;

namespace TH_Project.ViewModel
{
    public class DetailProductVM
    {
        
        public XEGANMAY XEGANMAY { get; set; }
        public IEnumerable<LOAIXE> LoaiXe { get; set; }
        public IEnumerable<NHAPHANPHOI> NhaPhanPhoi { get; set; }
    }
}