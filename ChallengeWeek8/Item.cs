namespace ChallengeWeek8
{
    internal class Item
    {
        public string Name { get; }
        public string Description { get; }
        public float Value { get; }
        public int Amount { get; set; }

        public const int MAX_STACK = 50;

        #region Item Initialisation
        public static Item HydrogenItem { get; } = new Item("Hydrogen Atom", "One Proton and One Electron", 0.05f);
        public static Item HeliumItem { get; } = new Item("Helium Atom", "One Proton and Two Electrons", 0.1f);
        public static Item LithiumItem { get; } = new Item("Lithium Atom", "Two Protons and One Electron", 0.25f);
        public static Item BerylliumAtom { get; } = new Item("Beryllium Atom", "Two Protons and Two Electrons", 0.37f);
        public static Item BoronAtom { get; } = new Item("Boron Atom", "Two Protons and Three Electrons", 0.5f);
        #endregion

        public Item(string name, string description, float value)
        {
            Name = name;
            Description = description;
            Value = value;
        }

        // Allows for easy comparison of objects to know if these are the same
        public override bool Equals(object obj)
        {
            if (obj is Item item)
            {
                return item.Name == Name;
            }
            return false;
        }

        /// <summary>
        /// Clone this instance of the object with exact same values
        /// </summary>
        /// <returns>A clone of this instance of item</returns>
        public Item Clone()
        {
            Item item = new Item(Name, Description, Value);
            item.Amount = Amount;
            return item;
        }

        /// <summary>
        /// Get the resale value of an item depending on <paramref name="amount"/>
        /// </summary>
        /// <param name="item">The item to calculate the resale price</param>
        /// <param name="amount">The amount of <paramref name="item"/></param>
        /// <returns>A float that is the resale price of the item</returns>
        public static float GetResalePrice(Item item, int amount)
        {
            return amount * item.Value * Shop.RESALE_FACTOR;
        }
    }
}
