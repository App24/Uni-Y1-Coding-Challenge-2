using System.Collections.Generic;

namespace ChallengeWeek8
{
    internal class Player
    {
        public float Money { get; set; }

        private List<Item> boughtItems = new List<Item>();

        public static Player Instance { get; private set; }

        public Player(float startingMoney)
        {
            Money = startingMoney;
            if (Instance == null)
                Instance = this;
        }

        /// <summary>
        /// Add a certain <paramref name="amount"/> of <paramref name="itemToAdd"/> to the boughtItems list
        /// </summary>
        /// <param name="itemToAdd">The item to add</param>
        /// <param name="amount">The amount of <paramref name="itemToAdd"/> to add</param>
        public void AddItem(Item itemToAdd, int amount)
        {
            if (amount <= 0) return;

            int index = boughtItems.FindIndex(i => i.Equals(itemToAdd) && i.Amount < Item.MAX_STACK);
            Item item = null;

            if (index >= 0) item = boughtItems[index];

            if (item == null)
            {
                item = itemToAdd.Clone();
                item.Amount = 0;
            }
            item.Amount += amount;

            // Get the remaining amount to add to the bought items list
            amount = item.Amount - Item.MAX_STACK;

            // Clamp item.Amount to Item.MAX_STACK
            if (item.Amount > Item.MAX_STACK)
            {
                item.Amount = Item.MAX_STACK;
            }

            if (index >= 0)
            {
                boughtItems.RemoveAt(index);
                boughtItems.Insert(index, item);
            }
            else
            {
                boughtItems.Add(item);
            }
            // Call itself to add the remaining amount
            AddItem(itemToAdd, amount);
        }

        /// <summary>
        /// Remove a certain <paramref name="amount"/> of <paramref name="itemToRemove"/> from the boughtItems list
        /// </summary>
        /// <param name="itemToRemove">The item to add</param>
        /// <param name="amount">The amount of <paramref name="itemToRemove"/> to remove</param>
        public void RemoveItem(Item itemToRemove, int amount)
        {
            if (amount <= 0) return;

            int index = boughtItems.FindLastIndex(i => i.Equals(itemToRemove));
            if (index < 0) return;

            Item item = boughtItems[index];
            item.Amount -= amount;
            amount = -item.Amount;
            boughtItems.RemoveAt(index);
            if (item.Amount > 0)
            {
                boughtItems.Insert(index, item);
            }

            // Call itself to remove remaining amount
            RemoveItem(itemToRemove, amount);
        }

        #region Gets
        public string GetBoughtItemsString()
        {
            if (boughtItems.Count <= 0) return $"[{ColorConstants.BAD_COLOR}]No Items Bought!";

            List<string> text = new List<string>();

            foreach (Item item in boughtItems)
            {
                text.Add($"[{ColorConstants.ITEM_COLOR}]{item.Name}\nResale Value: [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", Item.GetResalePrice(item, 1))}[/]\nAmount: [{ColorConstants.AMOUNT_COLOR}]{item.Amount}[/]");
            }

            return string.Join("\n\n", text);
        }

        public List<Item> GetBoughtItems()
        {
            // Flatten boughtItems list into a list that has one instance of each item, instead of multiple instances of one item
            // due to stacking
            List<Item> toReturn = new List<Item>();

            foreach (Item item in boughtItems)
            {
                Item toReturnItem = toReturn.Find(i => i.Equals(item));
                if (toReturnItem == null)
                {
                    toReturnItem = item.Clone();
                    toReturnItem.Amount = 0;
                }
                toReturnItem.Amount += item.Amount;
                toReturn.AddOrReplace(toReturnItem);
            }

            return toReturn;
        }
        #endregion
    }
}
