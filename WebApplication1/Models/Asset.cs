using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    [Table("Assets")]
    public class Asset
    {
        [Key]
        [Required]
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "醫工碼")]
        public string BmedNo { get; set; }
        [Display(Name = "院區")]
        public string AssetArea { get; set; }
        [Required]
        [Display(Name = "財產種類")]
        public string AssetClass { get; set; }
        [Required]
        [Display(Name = "中文名稱")]
        public string Cname { get; set; }
        [Display(Name = "別名")]
        public string Cname2 { get; set; }
        [Display(Name = "英文名稱")]
        public string Ename { get; set; }
        [Display(Name = "立帳日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? AccDate { get; set; }
        [Display(Name = "購入日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BuyDate { get; set; }
        [Display(Name = "出廠日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? RelDate { get; set; }
        [Display(Name = "廠牌")]
        public string Brand { get; set; }
        [Display(Name = "規格")]
        public string Standard { get; set; }
        [Display(Name = "型號")]
        public string Type { get; set; }
        [Display(Name = "產地")]
        public string Origin { get; set; }
        [Display(Name = "電壓")]
        public string Voltage { get; set; }
        [Display(Name = "電流")]
        public string Current { get; set; }
        [Display(Name = "代理商")]
        public int? VendorId { get; set; }
        [NotMapped]
        [Display(Name = "廠商名稱")]
        public string VendorName { get; set; }
        [Required]
        [Display(Name = "財產狀況")]
        public string DisposeKind { get; set; }
        [Required]
        [Display(Name = "保管部門")]
        public string DelivDpt { get; set; }
        [NotMapped]
        [Display(Name = "保管部門名稱")]
        public string DelivDptName { get; set; }
        [Required]
        [Display(Name = "保管人代號")]
        public int? DelivUid { get; set; }
        [Display(Name = "保管人姓名")]
        public string DelivEmp { get; set; }
        [Required]
        [Display(Name = "設備工程師")]
        public int EngId { get; set; }
        [NotMapped]
        public string EngName { get; set; }
        [NotMapped]
        [Display(Name = "工程師Email")]
        public string EngEmail { get; set; }
        [NotMapped]
        [Display(Name = "工程師電話")]
        public string EngTel { get; set; }
        [Display(Name = "存放地點")]
        public string LeaveSite { get; set; }
        [Required]
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [NotMapped]
        [Display(Name = "成本中心名稱")]
        public string AccDptName { get; set; }
        [Display(Name = "成本")]
        public Nullable<decimal> Cost { get; set; }
        [Display(Name = "計算基數")]
        public Nullable<decimal> Shares { get; set; }
        [Display(Name = "風險等級")]
        public string RiskLvl { get; set; }
        [Display(Name = "使用年限")]
        public int? UseLife { get; set; }
        [Display(Name = "固定IP位址")]
        public string IpAddr { get; set; }
        [Display(Name = "MAC位址")]
        public string MacAddr { get; set; }
        [Display(Name = "製造號碼")]
        public string MakeNo { get; set; }
        [Display(Name = "備註")]
        public string Note { get; set; }
        [Display(Name = "保固起始日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? WartySt { get; set; }
        [Display(Name = "保固終止日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? WartyEd { get; set; }
        [Display(Name = "採購評估單號")]
        public string Docid { get; set; } //採購評估單號
        [Display(Name = "專責單位")]
        public string ResponsDpt { get; set; }

        [Display(Name = "異動人員")]
        public int? Rtp { get; set; }

        [Display(Name = "異動時間")]
        public DateTime? Rtt { get; set; }
        [NotMapped]
        public string upload { get; set; }

        [NotMapped]
        [Display(Name = "*保養週期(月)")]
        public int? Cycle { get; set; }
        [NotMapped]
        [Display(Name = "*保養起始年月(民國年+月份=>10512)")]
        public int? KeepYm { get; set; }
    }
}