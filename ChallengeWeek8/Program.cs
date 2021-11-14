using System;
using System.Collections.Generic;

namespace ChallengeWeek8
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Player player = new Player(200);
            Shop shop = new Shop();

            bool running = true;
            while (running)
            {
                Console.Clear();
                Utils.WriteColor("What do you want to do?");
                int selection = Utils.GetSelection(
                    $"[{ColorConstants.COMMAND_COLOR}]Check Balance", // 0
                    $"[{ColorConstants.COMMAND_COLOR}]Add Item to Basket", // 1 
                    $"[{ColorConstants.COMMAND_COLOR}]Remove Item from Basket", // 2 
                    $"[{ColorConstants.COMMAND_COLOR}]List Items in Basket", // 3
                    $"[{ColorConstants.COMMAND_COLOR}]Buy Basket", // 4
                    $"[{ColorConstants.COMMAND_COLOR}]Sell Item to Shop", // 5
                    $"[{ColorConstants.COMMAND_COLOR}]List Items in Shop", // 6
                    $"[{ColorConstants.COMMAND_COLOR}]List Bought Items", // 7
                    $"[{ColorConstants.COMMAND_COLOR}]Exit" // 8
                    );
                Console.WriteLine();

                switch (selection)
                {
                    // Check Balance
                    case 0:
                        {
                            Utils.WriteColor($"Your balance is: [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", player.Money)}");
                        }
                        break;
                    // Add Item to basket
                    case 1:
                        {
                            List<Item> stockItems = shop.GetStockItems();

                            if (stockItems.Count <= 0)
                            {
                                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]There is no items in stock");
                                break;
                            }

                            List<string> itemNames = new List<string>();
                            stockItems.ForEach(i => itemNames.Add($"[{ColorConstants.ITEM_COLOR}]{i.Name}"));
                            itemNames.Add("Back");
                            Utils.WriteColor("Select item to add to basket:");
                            selection = Utils.GetSelection(itemNames.ToArray());

                            if (selection == itemNames.Count - 1)
                            {
                                break;
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
                                break;
                            }

                            if (Utils.GetConfirmation($"You sure you want to add [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] to basket? It will cost [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", item.Value * amount)}"))
                            {
                                shop.AddToBasket(item, amount);

                                Utils.WriteColor($"Added [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] to your basket");
                            }
                        }
                        break;
                    // Remove item from basket
                    case 2:
                        {
                            List<Item> basketItems = shop.GetBasketItems();

                            if (basketItems.Count <= 0)
                            {
                                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]Basket is empty");
                                break;
                            }

                            List<string> itemNames = new List<string>();
                            basketItems.ForEach(i => itemNames.Add($"[{ColorConstants.ITEM_COLOR}]{i.Name}"));
                            itemNames.Add("Back");
                            Utils.WriteColor("Select item to remove:");
                            selection = Utils.GetSelection(itemNames.ToArray());

                            if (selection == itemNames.Count - 1)
                            {
                                break;
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
                                break;
                            }

                            if (Utils.GetConfirmation($"You sure you want to remove [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] from your basket?"))
                            {
                                shop.RemoveFromBasket(item, amount);

                                Utils.WriteColor($"Removed [{ColorConstants.AMOUNT_COLOR}]{amount}[/] [{ColorConstants.ITEM_COLOR}]{item.Name}(s)[/] from your basket");
                            }
                        }
                        break;
                    // Show basket
                    case 3:
                        {
                            Utils.WriteColor("Items in basket:");
                            Utils.WriteColor(shop.GetBasketString());
                        }
                        break;
                    // Buy basket
                    case 4:
                        {
                            if (shop.GetBasketItems().Count <= 0)
                            {
                                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]Basket is empty");
                                break;
                            }
                            if (Utils.GetConfirmation($"You sure you want to buy the items in your basket? It will cost [{ColorConstants.MONEY_COLOR}]£{string.Format("{0:0.00}", shop.GetBasketPrice())}"))
                            {
                                shop.BuyBasket();
                                Utils.WriteColor($"[{ColorConstants.GOOD_COLOR}]Basket Bought!");
                            }
                        }
                        break;
                    // Sell Item
                    case 5:
                        {
                            List<Item> boughtItems = player.GetBoughtItems();

                            if (boughtItems.Count <= 0)
                            {
                                Utils.WriteColor($"[{ColorConstants.BAD_COLOR}]You do not have any bought items!");
                                break;
                            }

                            List<string> itemNames = new List<string>();
                            boughtItems.ForEach(i => itemNames.Add($"[{ColorConstants.ITEM_COLOR}]{i.Name}"));
                            itemNames.Add("Back");
                            Utils.WriteColor("Select item to sell:");
                            selection = Utils.GetSelection(itemNames.ToArray());

                            if (selection == itemNames.Count - 1)
                            {
                                break;
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
                                break;
                            }

                            if (Utils.GetConfirmation($"You sure you want to sell [cyan]{amount}[/] [green]{item.Name}(s)[/]? You will earn [magenta]£{string.Format("{0:0.00}", Item.GetResalePrice(item, amount))}"))
                            {
                                shop.SellToShop(item, amount);

                                Utils.WriteColor($"Sold [cyan]{amount}[/] [green]{item.Name}(s)[/] back to shop");
                            }
                        }
                        break;
                    // Show stock items
                    case 6:
                        {
                            Utils.WriteColor("Items in shop:");
                            Utils.WriteColor(shop.GetStockItemsString());
                        }
                        break;
                    // Show Bought items
                    case 7:
                        {
                            Utils.WriteColor("Bought Items:");
                            Utils.WriteColor(player.GetBoughtItemsString());
                        }
                        break;
                    // Exit
                    case 8:
                        {
                            running = false;
                        }
                        break;
                }
                Utils.WriteColor("Press enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
