using System;
using System.Collections.Generic;
using System.Text;

namespace CheckOutProcess.Core
{
    public class Promotions
    {
        public int PromotionId { get; set; }
        public string PromotionType { get; set; }
        public string PromotionDetails { get; set; }
        public int PromotionPrice { get; set; }
        public int PromotionQty { get; set; }
        public char PrimarySKUId { get; set; }
        public char SecondarySKUId { get; set; }
    }
}
