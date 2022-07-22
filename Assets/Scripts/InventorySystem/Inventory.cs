using System.Collections.Generic;
using System.Linq;

namespace InventorySystem
{
    public sealed class Inventory
    {
        /// <summary>
        /// Key = item id, Value = slots where the item is located
        /// </summary>
        private readonly Dictionary<string, HashSet<Slot>> itemSlots;
        private readonly Slot[] slots;
        private int amountFreeSlots { get; set; }
    
        public readonly int MaxAmountSlots;

        public IEnumerable<Slot> Slots => slots;
        public Slot this[int index] => slots[index];
        public event System.EventHandler<SlotModificationEventArgs> SlotModified;

        public Inventory(int amountSlots)
        {
            slots = new Slot[amountSlots];
        
            for (var i = 0; i < amountSlots; i++)
            {
                slots[i] = new Slot(i);
            }
            itemSlots = new Dictionary<string, HashSet<Slot>>();
            amountFreeSlots = MaxAmountSlots = amountSlots;
        }
    
        /// <summary>
        /// Finds the nearest empty slot with the same id or an empty slot and fills it
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void AddItem(string id, int amount = 1)
        {
            var item = DataBase.GetItemById(id);
        
            if (!item) return;
       
            if (!itemSlots.ContainsKey(id)) itemSlots.Add(id, new HashSet<Slot>());
 
            //fill existing slots
            foreach (var slot in itemSlots[id].OrderBy(s=>s.Index).Where(slot => !slot.IsFull))
            {
                amount = slot.AddItem(amount);
            
                SlotModified?.Invoke(this, new SlotModificationEventArgs(slot.Item, slot.Index, slot.AmountItems));
            
                if (amount == 0) return;
            }
            
            //fill empty slots
            while (amount != 0 && amountFreeSlots > 0)
            {
                var closestFreeSlot = GetClosestFreeSlot();

                closestFreeSlot.Item = item;
                amount = closestFreeSlot.AddItem(amount);
                amountFreeSlots--;
            
                itemSlots[item.Id].Add(closestFreeSlot);
                SlotModified?.Invoke(this, 
                    new SlotModificationEventArgs(closestFreeSlot.Item, closestFreeSlot.Index, closestFreeSlot.AmountItems));
            }
        }
    
        /// <summary>
        /// Removes items from slots from the end, if any exist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void RemoveItem(string id, int amount = 1)
        {
            if (!itemSlots.ContainsKey(id) || itemSlots[id].Count == 0) return;

            foreach (var slot in itemSlots[id].OrderByDescending(s => s.Index))
            {
                if (amount == 0) break;

                amount = slot.RemoveItem(amount);
                
                if (slot.IsEmpty)
                {
                    itemSlots[id].Remove(slot);
                    amountFreeSlots++;
                }

                SlotModified?.Invoke(this, new SlotModificationEventArgs(slot.Item, slot.Index, slot.AmountItems));
            }
        }
    
        /// <summary>
        /// Adds items to the slot by index
        /// </summary>
        /// <param name="id"></param>
        /// <param name="slotIndex"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public int AddItem(string id, int slotIndex, int amount = 1)
        {
            if (amount < 1) return 0;
        
            var item = DataBase.GetItemById(id);
            var slot = slots[slotIndex];
            int remainder = 0;

            if (slot.IsEmpty)
            {
                slot.Item = item;
                slot.AddItem(amount);
                amountFreeSlots--;

                if (!itemSlots.ContainsKey(id)) itemSlots.Add(id, new HashSet<Slot>());

                itemSlots[item.Id].Add(slot);
            }
            else remainder = slot.AddItem(amount);

            SlotModified?.Invoke(this, new SlotModificationEventArgs(slot.Item, slotIndex, slot.AmountItems));
            return remainder;
        }
    
        /// <summary>
        /// Remove items from slot by index
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="amount"></param>
        public void RemoveItem(int slotIndex, int amount = 1)
        {
            if (slotIndex > MaxAmountSlots) return;

            var currentSlot = slots[slotIndex];
            var id = currentSlot.Item.Id;
    
            currentSlot.RemoveItem(amount);

            if (currentSlot.IsEmpty)
            {
                itemSlots[id].Remove(currentSlot);
                amountFreeSlots++;
            }
        
            SlotModified?.Invoke(this, 
                new SlotModificationEventArgs(currentSlot.Item, currentSlot.Index, currentSlot.AmountItems));
        }

        public void Clear()
        {
            itemSlots.Clear();
            amountFreeSlots = MaxAmountSlots;
        
            foreach (var slot in slots)
            {
                slot.Clear();   
                SlotModified?.Invoke(this, new SlotModificationEventArgs(slot.Item, slot.Index, slot.AmountItems));
            }
        }

        private Slot GetClosestFreeSlot()
        {
            return slots.FirstOrDefault(s => s.IsEmpty);
        }
        
        public void SetData(IEnumerable<SlotData> slotData)
        {
            foreach (var data in slotData)
            {
                AddItem(data.ItemId, data.Index, data.Amount);
            }
        }
    }
}