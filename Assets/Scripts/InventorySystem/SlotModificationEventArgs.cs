namespace InventorySystem
{
    public class SlotModificationEventArgs : System.EventArgs
    {
        public Item SlotItem { get; }
        public int SlotIndex { get; }
        public int AmountOfItems { get; }
        public SlotModificationEventArgs(Item item, int index, int amountOfItems)
        {
            SlotItem = item;
            SlotIndex = index;
            AmountOfItems = amountOfItems;
        }
    }
}