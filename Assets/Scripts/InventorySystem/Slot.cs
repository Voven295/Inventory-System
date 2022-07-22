namespace InventorySystem
{
    public class Slot
    {
        public Slot(int index)
        {
            Index = index;
        }
    
        private int MaxCapacity => Item == null ? 0 : Item.MaxStack;
        
        public Item Item;
        public int Index { get; }
        public bool IsFull => AmountItems == MaxCapacity;
        public bool IsEmpty => AmountItems == 0;
        public int AmountItems { get; private set; }
    
        public int AddItem(int amount)
        {
            var remainder = MaxCapacity - AmountItems;

            if (amount > remainder)
            {
                AmountItems += remainder;
                return amount - remainder;
            }

            AmountItems += amount;
            return 0;
        }

        public int RemoveItem(int amount)
        {
            if (amount < AmountItems)
            {
                AmountItems -= amount;
                return 0;
            }

            var remainder = amount - AmountItems; 
            Clear();
            return remainder;
        }

        public void Clear()
        {
            Item = null;
            AmountItems = 0;
        }

        public override string ToString()
        {
            return $"Slot[{Index}], Item: {Item}, Amount of items {AmountItems}";
        }

        public SlotData ConvertToSlotData()
        {
            return new SlotData() {Amount = AmountItems, Index = Index, ItemId = Item == null ? "" : Item.Id};
        }
    }
}