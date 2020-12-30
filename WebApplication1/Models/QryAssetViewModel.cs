using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class QryAssetViewModel
    {
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "保管部門")]
        public string DelivDpt { get; set; }
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱")]
        public string AssetName { get; set; }
        [Display(Name = "型號")]
        public string Type { get; set; }
        [Display(Name = "廠商")]
        public string VendorId { get; set; }
        [Display(Name = "廠商")]
        public string VendorName { get; set; }
        [Display(Name = "風險等級")]
        public string RiskLvl { get; set; }
        [Display(Name = "購置日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? BuyDate1 { get; set; }
        public DateTime? BuyDate2 { get; set; }
        [Display(Name = "醫療儀器")]
        public string AssetClass1 { get; set; }
        [Display(Name = "工程設施")]
        public string AssetClass2 { get; set; }
        [Display(Name = "立帳日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? AccDate1 { get; set; }
        public DateTime? AccDate2 { get; set; }
        [Display(Name = "設備別名")]
        public string AssetCName2 { get; set; }

        [Display(Name = "查詢方式")]
        public string QryType { get; set; }
    }
}