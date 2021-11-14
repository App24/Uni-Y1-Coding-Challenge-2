using System.Collections.Generic;

namespace ChallengeWeek8
{
    internal class Shop
    {
        private List<Item> stockItems = new List<Item>();
        private List<Item> playerBasket = new List<Item>();

        public const float RESALE_FACTOR = 0.8f;

        public Shop()
        {
            InitialiseItems();
        }

        private void InitialiseItems()
        {
            AddItemToStock(Item.HydrogenItem, 4000);
            AddItemToStock(Item.HydrogenItem, 2000);

            AddItemToStock(Item.HeliumItem, 4000);

            AddItemToStock(Item.LithiumItem, 3000);

            AddItemToStock(Item.BerylliumAtom, 2500);

            AddItemToStock(Item.BoronAtom, 750);
        }

        #region Gets
        public string GetStockItemsString()
        {
            if (stockItems.Count <= 0) return $"[{ColorConstants.BAD_COLOR}]There are no items in stock!";

            List<string> text = new List<string>();

            foreach (Item item in stockItems)
            {
                text.Add($"[{ColorConstants.ITEM_COLOR}]{item.Name}[/]\n{item.Description}\nCost: [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", item.Value)}[/]\nAmount: [{ColorConstants.AMOUNT_COLOR}]{item.Amount}[/]");
            }

            return string.Join("\n\n", text);
        }

        public List<Item> GetStockItems()
        {
            return stockItems;
        }

        public string GetBasketString()
        {
            if (playerBasket.Count <= 0) return $"[{ColorConstants.BAD_COLOR}]Basket is empty!";

            List<string> text = new List<string>();

            foreach (Item item in playerBasket)
            {
                text.Add($"[{ColorConstants.ITEM_COLOR}]{item.Name}[/]\nTotal Cost: [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", item.Amount * item.Value)}[/]\nAmount: [{ColorConstants.AMOUNT_COLOR}]{item.Amount}[/]");
            }

            return string.Join("\n\n", text);
        }

        public List<Item> GetBasketItems()
        {
            return playerBasket;
        }

        public float GetBasketPrice()
        {
            float value = 0;
            playerBasket.ForEach(i => value += (i.Value * i.Amount));
            return value;
        }
        #endregion

        #region Stock Logic
        private void AddItemToStock(Item itemToAdd, int amount)
        {
            Item item = stockItems.Find(i => i.Equals(itemToAdd));
            if (item == null)
            {
                item = itemToAdd.Clone();
                item.Amount = 0;
            }
            item.Amount += amount;
            stockItems.AddOrReplace(item);
        }

        private void RemoveItemFromStock(Item itemToRemove, int amount)
        {
            Item item = stockItems.Find(i => i.Equals(itemToRemove));
            if (item == null) return;
            item.Amount -= amount;
            if (item.Amount > 0)
            {
                stockItems.AddOrReplace(item);
            }
            else
            {
                stockItems.Remove(item);
            }
        }
        #endregion

        #region Basket Logic
        public void AddToBasket(Item itemToAdd, int amount)
        {
            Item stockItem = stockItems.Find(i => i.Equals(itemToAdd));
            if (stockItem == null || stockItem.Amount <= 0)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]That item is not in stock!");
                return;
            }
            if (stockItem.Amount < amount)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]There is not enought stock to buy [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{itemToAdd.Name}(s)");
                return;
            }
            Item item = playerBasket.Find(i => i.Equals(itemToAdd));
            if (item == null)
            {
                item = itemToAdd.Clone();
                item.Amount = 0;
            }
            item.Amount += amount;
            playerBasket.AddOrReplace(item);
        }

        public void RemoveFromBasket(Item itemToRemove, int amount)
        {
            Item item = playerBasket.Find(i => i.Equals(itemToRemove));
            if (item == null) return;
            item.Amount -= amount;
            if (item.Amount > 0)
            {
                playerBasket.AddOrReplace(item);
            }
            else
            {
                playerBasket.Remove(item);
            }
        }

        public void BuyBasket()
        {
            float cost = GetBasketPrice();
            if (Player.Instance.Money < cost)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]You do not have enough money. You are missing [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", cost - Player.Instance.Money)}[/]");
                if (Utils.GetConfirmation("Do you want to clear your basket?"))
                {
                    ResetBasket();
                    Utils.WriteColor($"[{ColorConstants.GOOD_COLOR}]Basket Cleared!");
                }
                return;
            }

            Player.Instance.Money -= cost;
            playerBasket.ForEach(i =>
            {
                int amount = i.Amount;
                RemoveItemFromStock(i, amount);
                Player.Instance.AddItem(i, amount);
            });

            ResetBasket();
        }

        public void ResetBasket()
        {
            playerBasket.Clear();
        }
        #endregion

        #region Sell Logic
        public void SellToShop(Item item, int amount)
        {
            float value = Item.GetResalePrice(item, amount);

            Player.Instance.RemoveItem(item, amount);
            Player.Instance.Money += value;
            AddItemToStock(item, amount);
        }
        #endregion
    }
}
