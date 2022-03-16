using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tuan4_HuynhKimSang.Models;

namespace Tuan4_HuynhKimSang.Models
{
    public class GioHang
    {
        MyDataDataContext db = new MyDataDataContext();

        public int masach { get; set; }

        [Display(Name = "Tên sách")]
        public string tensach { get; set; }

        [Display(Name = "Ảnh bìa")]
        public string hinh { get; set; }


        [Display(Name = "Giá bán")]
        public Double giaban { get; set; }

        [Display(Name = "Số lượng")]
        public int iSoLuong { get; set; }

        [Display(Name = "Thành Tiền")]
        public double dThanhTien {

            get { return iSoLuong * giaban; } }

        public GioHang(int id)
        {
            masach = id;
            Sach sach = db.Saches.FirstOrDefault(p => p.masach == id);
            tensach = sach.tensach;
            hinh = sach.hinh;
            giaban = double.Parse(sach.giaban.ToString());
            iSoLuong = 1;

        }
    }

}