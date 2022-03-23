using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Tuan4_HuynhKimSang.Models;

namespace Tuan4_HuynhKimSang.Controllers
{
    public class GioHangController : Controller
    {

        MyDataDataContext db = new MyDataDataContext();

        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if(lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["Giohang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int id,string strURL)
        {
            List<GioHang> lstGioHang = Laygiohang();
            var sach = db.Saches.FirstOrDefault(p => p.masach == id);
            GioHang sanpham = lstGioHang.Find(p => p.masach == id);
            if(sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                if (sanpham.iSoLuong < sach.soluongton)
                {
                    sanpham.iSoLuong++;
                    return Redirect(strURL);
                }
                else
                {
                    MessageBox.Show("Không có đủ sách đẻ bán");
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.iSoLuong);
            }
            return tsl;
        }

        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                tsl = lstGioHang.Count();
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                tt = lstGioHang.Sum(n => n.dThanhTien);
            }
            return tt;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();

            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult Xoagiohang(int id)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.masach == id);
            if(sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult Capnhatgiohang(int id, System.Web.Mvc.FormCollection collection) 
        {
            List<GioHang> lstGioHang = Laygiohang();
            var sach = db.Saches.FirstOrDefault(p => p.masach == id);
            GioHang sanpham = lstGioHang.SingleOrDefault(p => p.masach == id);
            if(sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(collection["txtSoLg"].ToString().Trim());
                if(sanpham.iSoLuong > sach.soluongton)
                {
                    MessageBox.Show("Không còn đủ sách để bán");
                    sanpham.iSoLuong = 1;
                }

            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Xoatatcagiohang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }

        //public ActionResult Dathang()
        //{
        //    List<GioHang> lstGioHang = Laygiohang();
        //    if (lstGioHang.Count() != 0)
        //    {
        //        DialogResult result = MessageBox.Show("bạn muốn đặt hàng", "Hỏi", MessageBoxButtons.OKCancel);
        //        if (result == DialogResult.OK)
        //        {
        //            create invoice
        //            DonHang invoice = new DonHang();
        //            invoice.ngaydat = DateTime.Now;
        //            db.DonHangs.InsertOnSubmit(invoice);
        //            db.SubmitChanges();
        //            int invoide_id = db.DonHangs.OrderByDescending(p => p.madon).Select(p => p.madon).FirstOrDefault();
        //            add invoice's detail
        //            ChiTietDonHang idetail;
        //            foreach (var ele in lstGioHang)
        //            {
        //                idetail = new ChiTietDonHang();
        //                idetail.masach = ele.masach;
        //                idetail.madon = invoide_id;
        //                idetail.gia = (decimal?)ele.giaban;
        //                idetail.soluong = ele.iSoLuong;
        //                db.ChiTietDonHangs.InsertOnSubmit(idetail);
        //                var book = db.Saches.FirstOrDefault(p => p.masach == ele.masach);
        //                book.soluongton -= idetail.soluong;
        //                UpdateModel(book);
        //            }
        //            db.SubmitChanges();
        //            string str = "";
        //            int i = 1;
        //            foreach (var ele in lstGioHang)
        //            {
        //                str += i + " - " + ele.tensach + "\n";
        //                i++;
        //            }
        //            MessageBox.Show("Đặt hàng thành công!\n" + "---------------\n" + "Danh sách đặt hàng\n" + str);
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Giỏ hàng trống");
        //    }
        //    return RedirectToAction("Index", "Home");

        //}


        [HttpGet]
        public ActionResult DatHang()
        {
            if(Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap","NguoiDung");
            }
            if(Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult DatHang(System.Web.Mvc.FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();

            List<GioHang> gh = Laygiohang();
            var ngaygiao = string.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;

            db.DonHangs.InsertOnSubmit(dh);
            db.SubmitChanges();
            foreach(var ele in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = ele.masach;
                ctdh.soluong = ele.iSoLuong;
                ctdh.gia = (decimal)ele.giaban;
                s = db.Saches.Single(n => n.masach == ele.masach);
                s.soluongton -= ctdh.soluong;
                db.SubmitChanges();
                db.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
//test
    }
}