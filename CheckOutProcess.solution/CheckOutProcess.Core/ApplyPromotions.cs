using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CheckOutProcess.Core
{
    public static class ApplyPromotions
    {
        public static int ApplyPromotion(Cart cart, IEnumerable<Promotions> promotions)
        {
            int finalPrice = 0;
            var items = cart.GetAllItems();
            List<char> itemList = new List<char>();
            List<char> promoApplied = new List<char>();
            
            foreach(var item in items)
            {
                itemList.Add(item.SKUId);
            }

            //Apply Single Promotions
            int priceAfterSinglePromotion = 0;
            foreach (var item in items)
            {
                var itemPromotion = promotions.FirstOrDefault(p => p.PrimarySKUId == item.SKUId && p.PromotionType=="Single");
                if (itemPromotion != null && item.Quantity>=itemPromotion.PromotionQty)
                {
                    int promotionQty = itemPromotion.PromotionQty;
                    int promotionPrice = itemPromotion.PromotionPrice;
                    int itemInPromotion = item.Quantity/promotionQty;
                    int itemsRemaing = item.Quantity % promotionQty;
                    priceAfterSinglePromotion += (itemInPromotion * promotionPrice + itemsRemaing * item.Price);
                    promoApplied.Add(item.SKUId);
                }
            }

            //Apply paired promotions
            int priceAfterPairedPromotion = 0;
            foreach(var item in items)
            {
                var itemPromotion = promotions.FirstOrDefault(p => p.PrimarySKUId == item.SKUId && p.PromotionType == "Pair" && p.SecondarySKUId != '\0');
                if (itemPromotion != null)
                {
                    var secondarySKUID = promotions.FirstOrDefault(p => p.PrimarySKUId == item.SKUId && p.PromotionType == "Pair").SecondarySKUId;
                    int primaryQty = item.Quantity;
                    int primaryPrice = item.Price;
                    int secondaryQty = items.FirstOrDefault(i => i.SKUId == secondarySKUID).Quantity;
                    int secondaryPrice = items.FirstOrDefault(i => i.SKUId == secondarySKUID).Price;
                    if (secondaryQty != 0)
                    {
                        if (primaryQty == secondaryQty)
                        {
                            int totalQty = (primaryQty + secondaryQty) / 2;
                            priceAfterPairedPromotion += totalQty * itemPromotion.PromotionPrice;
                        }
                        else if (primaryQty > secondaryQty)
                        {
                            int normalQty = primaryQty - secondaryQty;
                            int promoQty = (primaryQty + secondaryQty + -normalQty) / 2;
                            priceAfterPairedPromotion += (promoQty * itemPromotion.PromotionPrice) + (normalQty * primaryPrice);
                        }
                        else if (primaryQty < secondaryQty)
                        {
                            int normalQty = secondaryQty - primaryQty;
                            int promoQty = (primaryQty + secondaryQty + -normalQty) / 2;
                            priceAfterPairedPromotion += (promoQty * itemPromotion.PromotionPrice) + (normalQty * secondaryPrice);
                        }
                        promoApplied.Add(item.SKUId);
                        promoApplied.Add(secondarySKUID);
                    }
                }
            }

            //Price Without Promotion
            int priceWithoutPromotion = 0;
            
            var remainingItems= (from item in itemList
                                 where !promoApplied.Contains(item)
                                 select item).ToList();
            foreach (var remain in remainingItems)
            {
                var item = items.FirstOrDefault(i => i.SKUId == remain);
                priceWithoutPromotion += item.Quantity * item.Price;
            }

            finalPrice = priceAfterSinglePromotion + priceAfterPairedPromotion + priceWithoutPromotion;
            cart.IsPromotionApplied = true;
            return finalPrice;
        }

        public static bool CheckPromotionStatus(Cart cart)
        {
            return cart.IsPromotionApplied;
        }
    }
}
