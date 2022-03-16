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
            GioHang sanpham = lstGioHang.Find(p => p.masach == id);
            if(sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
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
            GioHang sanpham = lstGioHang.SingleOrDefault(p => p.masach == id);
            if(sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(collection["txtSoLg"].ToString().Trim());

            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Xoatatcagiohang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }

        public ActionResult Dathang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            if(lstGioHang.Count() != 0)
            {
                DialogResult result = MessageBox.Show("bạn muốn đặt hàng", "Hỏi", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    //create invoice
                    Invoice invoice = new Invoice();
                    invoice.Invoice_DateCreate = DateTime.Now;
                    db.Invoices.InsertOnSubmit(invoice);
                    db.SubmitChanges();
                    int invoide_id = db.Invoices.OrderByDescending(p => p.Invoice_ID).Select(p => p.Invoice_ID).FirstOrDefault();
                    //add invoice's detail
                    Invoice_Detail idetail;
                    foreach (var ele in lstGioHang)
                    {
                        idetail = new Invoice_Detail();
                        idetail.masach = ele.masach;
                        idetail.Invoice_ID = invoide_id;
                        idetail.giamua = ele.giaban;
                        idetail.soluongmua = ele.iSoLuong;
                        db.Invoice_Details.InsertOnSubmit(idetail);
                    }


                    db.SubmitChanges();
                    MessageBox.Show("Đặt hàng thành công!");
                    return View("Index", "Home");
                }
            }
            else
            {
                MessageBox.Show("Giỏ hàng trống");
            }
            return RedirectToAction("Home");

        }


        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
//test
    }
}