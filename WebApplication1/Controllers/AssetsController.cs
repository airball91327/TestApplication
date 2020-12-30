using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AssetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 50;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AssetsController()
        {

        }
        public AssetsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
                                ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        // GET: Assets
        public ActionResult Index()
        {
            List<SelectListItem> listItem3 = new List<SelectListItem>();
            listItem3.Add(new SelectListItem { Value = "", Text = "請選擇" });
            listItem3.Add(new SelectListItem { Value = "0", Text = "無" });
            listItem3.Add(new SelectListItem { Value = "1", Text = "I" });
            listItem3.Add(new SelectListItem { Value = "2", Text = "II" });
            listItem3.Add(new SelectListItem { Value = "3", Text = "III" });
            ViewData["RiskLvl"] = new SelectList(listItem3, "Value", "Text");

            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Value = "ToList", Text = "ToList" });
            listItem.Add(new SelectListItem { Value = "AsQueryable", Text = "AsQueryable" });
            listItem.Add(new SelectListItem { Value = "ToDictionary", Text = "ToDictionary" });
            ViewData["QryType"] = new SelectList(listItem, "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fm, int page = 1)
        {
            QryAssetViewModel qryAsset = new QryAssetViewModel();
            qryAsset.AssetName = fm["AssetName"];
            qryAsset.AssetNo = fm["AssetNo"];
            qryAsset.AccDpt = fm["AccDpt"];
            qryAsset.DelivDpt = fm["DelivDpt"];
            qryAsset.Type = fm["Type"];
            qryAsset.RiskLvl = fm["RiskLvl"];
            qryAsset.VendorId = fm["VendorId"];
            qryAsset.AssetCName2 = fm["AssetCName2"];
            qryAsset.QryType = fm["QryType"];

            TempData["qry"] = qryAsset;
            ViewData["CountAsset"] = db.Assets.Count();
            ViewData["CountDpt"] = db.Departments.Count();
            List<Asset> at = new List<Asset>();
            switch (qryAsset.QryType)
            {
                case "ToList":
                    Stopwatch timer1 = Stopwatch.StartNew();
                    var qAsset = db.Assets.ToList();
                    if (!string.IsNullOrEmpty(qryAsset.AssetNo))
                    {
                        qAsset = qAsset.Where(a => a.AssetNo == qryAsset.AssetNo).ToList();
                    }
                    if (!string.IsNullOrEmpty(qryAsset.AssetName))
                    {
                        qAsset = qAsset.Where(a => a.Cname.Contains(qryAsset.AssetName)).ToList();
                    }
                    if (!string.IsNullOrEmpty(qryAsset.AssetCName2))
                    {
                        qAsset = qAsset.Where(a => a.Cname2 != null).Where(a => a.Cname2.Contains(qryAsset.AssetCName2)).ToList();
                    }
                    if (!string.IsNullOrEmpty(qryAsset.AccDpt))
                    {
                        qAsset = qAsset.Where(a => a.AccDpt == qryAsset.AccDpt).ToList();
                    }
                    if (!string.IsNullOrEmpty(qryAsset.DelivDpt))
                    {
                        qAsset = qAsset.Where(a => a.DelivDpt == qryAsset.DelivDpt).ToList();
                    }
                    if (!string.IsNullOrEmpty(qryAsset.Type))
                    {
                        qAsset = qAsset.Where(a => a.Type == qryAsset.Type).ToList();
                    }
                    if (!string.IsNullOrEmpty(qryAsset.RiskLvl))
                    {
                        qAsset = qAsset.Where(a => a.RiskLvl == qryAsset.RiskLvl).ToList();
                    }
                    timer1.Stop();
                    TimeSpan timespan1 = timer1.Elapsed;
                    //
                    Stopwatch timer4 = Stopwatch.StartNew();
                    try
                    {
                        qAsset.GroupJoin(db.Departments, a => a.DelivDpt, d => d.DptId,
                       (a, d) => new { Asset = a, Department = d })
                       .SelectMany(p => p.Department.DefaultIfEmpty(),
                       (x, y) => new { Asset = x.Asset, Department = y })
                       .GroupJoin(db.Departments, a => a.Asset.AccDpt, d => d.DptId,
                            (a, d) => new { Asset = a.Asset, DelivDpt = a.Department, AccDpt = d })
                            .SelectMany(p => p.AccDpt.DefaultIfEmpty(),
                            (x, y) => new { Asset = x.Asset, DelivDpt = x.DelivDpt, AccDpt = y })
                            .ToList()
                       .ForEach(p =>
                       {
                           p.Asset.DelivDptName = p.DelivDpt == null ? "" : p.DelivDpt.Name_C;
                           p.Asset.AccDptName = p.AccDpt == null ? "" : p.AccDpt.Name_C;
                           at.Add(p.Asset);
                       });
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    timer4.Stop();
                    TimeSpan timespan4 = timer4.Elapsed;
                    ViewData["TestResult"] = "搜尋:" + timespan1.TotalMilliseconds + "ms" + " / Foreach:" + timespan4.TotalMilliseconds + "ms";
                    break;
                case "AsQueryable":
                    Stopwatch timer2 = Stopwatch.StartNew();
                    var qAsset2 = db.Assets.AsQueryable();
                    if (!string.IsNullOrEmpty(qryAsset.AssetNo))
                    {
                        qAsset2 = qAsset2.Where(a => a.AssetNo == qryAsset.AssetNo);
                    }
                    if (!string.IsNullOrEmpty(qryAsset.AssetName))
                    {
                        qAsset2 = qAsset2.Where(a => a.Cname.Contains(qryAsset.AssetName));
                    }
                    if (!string.IsNullOrEmpty(qryAsset.AssetCName2))
                    {
                        qAsset2 = qAsset2.Where(a => a.Cname2 != null).Where(a => a.Cname2.Contains(qryAsset.AssetCName2));
                    }
                    if (!string.IsNullOrEmpty(qryAsset.AccDpt))
                    {
                        qAsset2 = qAsset2.Where(a => a.AccDpt == qryAsset.AccDpt);
                    }
                    if (!string.IsNullOrEmpty(qryAsset.DelivDpt))
                    {
                        qAsset2 = qAsset2.Where(a => a.DelivDpt == qryAsset.DelivDpt);
                    }
                    if (!string.IsNullOrEmpty(qryAsset.Type))
                    {
                        qAsset2 = qAsset2.Where(a => a.Type == qryAsset.Type);
                    }
                    if (!string.IsNullOrEmpty(qryAsset.RiskLvl))
                    {
                        qAsset2 = qAsset2.Where(a => a.RiskLvl == qryAsset.RiskLvl);
                    }
                    var result = qAsset2.GroupJoin(db.Departments, a => a.DelivDpt, d => d.DptId,
                                        (a, d) => new { Asset = a, Department = d })
                                        .SelectMany(p => p.Department.DefaultIfEmpty(),
                                        (x, y) => new { Asset = x.Asset, Department = y })
                                        .GroupJoin(db.Departments, a => a.Asset.AccDpt, d => d.DptId,
                                        (a, d) => new { Asset = a.Asset, DelivDpt = a.Department, AccDpt = d })
                                        .SelectMany(p => p.AccDpt.DefaultIfEmpty(),
                                        (x, y) => new { Asset = x.Asset, DelivDpt = x.DelivDpt, AccDpt = y });
                    timer2.Stop();
                    TimeSpan timespan2 = timer2.Elapsed;
                    //
                    Stopwatch timer5 = Stopwatch.StartNew();
                    try
                    {
                        result.ToList()
                       .ForEach(p =>
                       {
                           p.Asset.DelivDptName = p.DelivDpt == null ? "" : p.DelivDpt.Name_C;
                           p.Asset.AccDptName = p.AccDpt == null ? "" : p.AccDpt.Name_C;
                           at.Add(p.Asset);
                       });
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    timer5.Stop();
                    TimeSpan timespan5 = timer5.Elapsed;
                    ViewData["TestResult"] = "搜尋:" + timespan2.TotalMilliseconds + "ms" + " / Foreach:" + timespan5.TotalMilliseconds + "ms";
                    break;
                case "ToDictionary":
                    Stopwatch timer3 = Stopwatch.StartNew();
                    var qAsset3 = db.Assets.ToDictionary(a => a.AssetNo);
                    List<KeyValuePair<string, Asset>> qAsset4 = new List<KeyValuePair<string, Asset>>();
                    if (!string.IsNullOrEmpty(qryAsset.AssetNo))
                    {
                        qAsset4 = qAsset3.Where(a => a.Key == qryAsset.AssetNo).ToList();             
                    }
                    else if (!string.IsNullOrEmpty(qryAsset.AssetName))
                    {
                        qAsset4 = qAsset3.Where(a => a.Value.Cname.Contains(qryAsset.AssetName)).ToList();
                    }
                    else
                    {
                        qAsset4 = qAsset3.ToList();
                    }
                    timer3.Stop();
                    TimeSpan timespan3 = timer3.Elapsed;
                    //
                    Stopwatch timer6 = Stopwatch.StartNew();
                    try
                    {
                        qAsset4.GroupJoin(db.Departments, a => a.Value.DelivDpt, d => d.DptId,
                       (a, d) => new { Asset = a, Department = d })
                       .SelectMany(p => p.Department.DefaultIfEmpty(),
                       (x, y) => new { Asset = x.Asset, Department = y })
                       .GroupJoin(db.Departments, a => a.Asset.Value.AccDpt, d => d.DptId,
                            (a, d) => new { Asset = a.Asset, DelivDpt = a.Department, AccDpt = d })
                            .SelectMany(p => p.AccDpt.DefaultIfEmpty(),
                            (x, y) => new { Asset = x.Asset, DelivDpt = x.DelivDpt, AccDpt = y })
                            .ToList()
                       .ForEach(p =>
                       {
                           p.Asset.Value.DelivDptName = p.DelivDpt == null ? "" : p.DelivDpt.Name_C;
                           p.Asset.Value.AccDptName = p.AccDpt == null ? "" : p.AccDpt.Name_C;
                           at.Add(p.Asset.Value);
                       });
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    timer6.Stop();
                    TimeSpan timespan6 = timer6.Elapsed;
                    ViewData["TestResult"] = "搜尋:" + timespan3.TotalMilliseconds + "ms" + " / Foreach:" + timespan6.TotalMilliseconds + "ms";
                    break;
            }
            if (at.ToPagedList(page, pageSize).Count <= 0)
                return PartialView("List", at.ToPagedList(1, pageSize));
            return PartialView("List", at.ToPagedList(page, pageSize));
        }
        public ActionResult Index2()
        {
            // ToList
            Stopwatch timer1 = Stopwatch.StartNew();
            var testToList = db.Assets.ToList();
            testToList = testToList.Where(a => a.AssetClass == "醫療儀器").ToList();
            testToList = testToList.Where(a => a.Cname.Contains("測試")).ToList();
            timer1.Stop();
            TimeSpan timespan1 = timer1.Elapsed;
            // ToDictionary
            Stopwatch timer2 = Stopwatch.StartNew();
            var testTodic = db.Assets.ToDictionary(a => a.AssetNo);
            var test2 = testTodic.Where(a => a.Value.AssetClass == "醫療儀器");
            var list = test2.Where(a => a.Key == "000");
            timer2.Stop();
            TimeSpan timespan2 = timer2.Elapsed;
            // AsQueryable
            Stopwatch timer3 = Stopwatch.StartNew();
            var testAsQuery = db.Assets.AsQueryable();
            testAsQuery = testAsQuery.Where(a => a.AssetClass == "醫療儀器");
            var list2 = testAsQuery.Where(a => a.AssetNo == "000");
            timer3.Stop();
            TimeSpan timespan3 = timer3.Elapsed;
            //
            return View(list2) ;
        }

        // GET: Assets/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        // GET: Assets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Assets/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetNo,BmedNo,AssetArea,AssetClass,Cname,Cname2,Ename,AccDate,BuyDate,RelDate,Brand,Standard,Type,Origin,Voltage,Current,VendorId,DisposeKind,DelivDpt,DelivUid,DelivEmp,EngId,LeaveSite,AccDpt,Cost,Shares,RiskLvl,UseLife,IpAddr,MacAddr,MakeNo,Note,WartySt,WartyEd,Docid,ResponsDpt,Rtp,Rtt")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Assets.Add(asset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(asset);
        }

        // GET: Assets/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        // POST: Assets/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetNo,BmedNo,AssetArea,AssetClass,Cname,Cname2,Ename,AccDate,BuyDate,RelDate,Brand,Standard,Type,Origin,Voltage,Current,VendorId,DisposeKind,DelivDpt,DelivUid,DelivEmp,EngId,LeaveSite,AccDpt,Cost,Shares,RiskLvl,UseLife,IpAddr,MacAddr,MakeNo,Note,WartySt,WartyEd,Docid,ResponsDpt,Rtp,Rtt")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asset);
        }

        // GET: Assets/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Asset asset = db.Assets.Find(id);
            db.Assets.Remove(asset);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
