using System;
using System.Collections.Generic;

namespace ChallengeWeek8
{
    internal class Program
    {
        private static Player player;
        private static Shop shop;
        private static bool running;

        private static void Main(string[] args)
        {
            player = new Player(200);
            shop = new Shop();

            running = true;
            while (running)
            {
                Console.Clear();
                Utils.WriteColor("What do you want to do?");
                int selection = Utils.GetSelection(
                    $"[{ColorConstants.COMMAND_COLOR}]Check Balance",
                    $"[{ColorConstants.COMMAND_COLOR}]Add Item to Basket",
                    $"[{ColorConstants.COMMAND_COLOR}]Remove Item from Basket",
                    $"[{ColorConstants.COMMAND_COLOR}]List Items in Basket",
                    $"[{ColorConstants.COMMAND_COLOR}]Buy Basket",
                    $"[{ColorConstants.COMMAND_COLOR}]Sell Item to Shop",
                    $"[{ColorConstants.COMMAND_COLOR}]List Items in Shop",
                    $"[{ColorConstants.COMMAND_COLOR}]List Bought Items",
                    $"[{ColorConstants.COMMAND_COLOR}]Exit"
                    );
                Console.WriteLine();

                List<Action> actions = new List<Action>() {
                    ShowBalance,
                    AddItemToBasket,
                    RemoveItemFromBasket,
                    ShowBasket,
                    BuyBasket,
                    SellItem,
                    ShowStockItems,
                    ShowBoughtItems,
                    ExitProgram
                };

                actions[selection]();

                Utils.WriteColor("Press enter to continue...");
                Console.ReadLine();
            }
        }

        private static void ShowBalance()
        {
            Utils.WriteColor($"Your balance is: [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", player.Money)}");
        }

        private static void AddItemToBasket()
        {
            List<Item> stockItems = shop.GetStockItems();

            if (stockItems.Count <= 0)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]There is no items in stock");
                return;
            }

            List<string> itemNames = new List<string>();
            stockItems.ForEach(i => itemNames.Add($"[{ColorConstants.ITEM_COLOR}]{i.Name}"));
            itemNames.Add("Back");
            Utils.WriteColor("Select item to add to basket:");
            int selection = Utils.GetSelection(itemNames.ToArray());

            if (selection == itemNames.Count - 1)
            {
                return;
            }

            Item item = stockItems[selection];

            Utils.WriteColor($"There are [{ColorConstants.AMOUNT_COLOR}]{item.Amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] in stock");

            Utils.WriteColor("Enter amount to add to basket (Press enter for 1)");
            string input = Console.ReadLine().ToLower().Trim();
            if (!int.TryParse(input, out int amount))
            {
                amount = 1;
            }

            if (item.Amount < amount)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]There is not enought stock to buy [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)");
                return;
            }

            if (Utils.GetConfirmation($"You sure you want to add [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] to basket? It will cost [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", item.Value * amount)}"))
            {
                shop.AddToBasket(item, amount);

                Utils.WriteColor($"Added [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] to your basket");
            }
        }

        private static void RemoveItemFromBasket()
        {
            List<Item> basketItems = shop.GetBasketItems();

            if (basketItems.Count <= 0)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]Basket is empty");
                return;
            }

            List<string> itemNames = new List<string>();
            basketItems.ForEach(i => itemNames.Add($"[{ColorConstants.ITEM_COLOR}]{i.Name}"));
            itemNames.Add("Back");
            Utils.WriteColor("Select item to remove:");
            int selection = Utils.GetSelection(itemNames.ToArray());

            if (selection == itemNames.Count - 1)
            {
                return;
            }

            Item item = basketItems[selection];

            Utils.WriteColor($"You have [{ColorConstants.AMOUNT_COLOR}]{item.Amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] in your basket");

            Utils.WriteColor("Enter amount to remove (Press enter for 1)");
            string input = Console.ReadLine().ToLower().Trim();
            if (!int.TryParse(input, out int amount))
            {
                amount = 1;
            }

            if (item.Amount < amount)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]You do not have that many items in your basket!");
                return;
            }

            if (Utils.GetConfirmation($"You sure you want to remove [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] from your basket?"))
            {
                shop.RemoveFromBasket(item, amount);

                Utils.WriteColor($"Removed [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] from your basket");
            }
        }

        private static void ShowBasket()
        {
            Utils.WriteColor("Items in basket:");
            Utils.WriteColor(shop.GetBasketString());
        }

        private static void BuyBasket()
        {
            if (shop.GetBasketItems().Count <= 0)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]Basket is empty");
                return;
            }
            if (Utils.GetConfirmation($"You sure you want to buy the items in your basket? It will cost [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", shop.GetBasketPrice())}"))
            {
                shop.BuyBasket();
                Utils.WriteColor($"[{ColorConstants.GOOD_COLOR}]Basket Bought!");
            }
        }

        private static void SellItem()
        {
            List<Item> boughtItems = player.GetBoughtItems();

            if (boughtItems.Count <= 0)
            {
                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]You do not have any bought items!");
                return;
            }

            List<string> itemNames = new List<string>();
            boughtItems.ForEach(i => itemNames.Add($"[{ColorConstants.ITEM_COLOR}]{i.Name}"));
            itemNames.Add("Back");
            Utils.WriteColor("Select item to sell:");
            int selection = Utils.GetSelection(itemNames.ToArray());

            if (selection == itemNames.Count - 1)
            {
                return;
            }

            Item item = boughtItems[selection];

            Utils.WriteColor($"You have [{ColorConstants.AMOUNT_COLOR}]{item.Amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] to sell");

            Utils.WriteColor("Enter amount to sell (Press enter for 1)");
            string input = Console.ReadLine().ToLower().Trim();
            if (!int.TryParse(input, out int amount))
            {
                amount = 1;
            }

            if (item.Amount < amount)
            {
                Utils.WriteColor("[red]You do not have that many items bought!");
                return;
            }

            if (Utils.GetConfirmation($"You sure you want to sell [cyan]{amount}[/] [green]{item.Name}(s)[/]? You will earn [magenta]£{string.Format("{0:0.00}", Item.GetResalePrice(item, amount))}"))
            {
                shop.SellToShop(item, amount);

                Utils.WriteColor($"Sold [cyan]{amount}[/] [green]{item.Name}(s)[/] back to shop");
            }
        }

        private static void ShowStockItems()
        {
            Utils.WriteColor("Items in shop:");
            Utils.WriteColor(shop.GetStockItemsString());
        }

        private static void ShowBoughtItems()
        {
            Utils.WriteColor("Bought Items:");
            Utils.WriteColor(player.GetBoughtItemsString());
        }

        private static void ExitProgram()
        {
            running = false;
        }
    }
}
