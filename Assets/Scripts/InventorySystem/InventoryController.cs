using InventorySystem.UI;
using UnityEngine;

namespace InventorySystem
{
    //Connects logic and UI
    public class InventoryController
    {
        private readonly InventoryView inventoryView;
        private readonly Inventory inventory;
        private readonly CursorSlotUI cursorSlotUI;

        private string lastUsedItemId;
        public InventoryController(InventoryData inventoryData, Inventory inventory, Transform root)
        {
            var factoryUI = new FactoryUI();
            inventoryView = factoryUI.CreateInventoryView(root, true);

            this.inventory = inventory;
            this.inventory.SlotModified += inventoryView.UpdateUI;

            cursorSlotUI = inventoryView.GetComponentInChildren<CursorSlotUI>();
            cursorSlotUI.CursorSlotUiChanged += ChangeSlotData;
        
            var amountSlots = inventory.MaxAmountSlots;

            var slotsUI = new SlotUI[amountSlots];

            for (var i = 0; i < amountSlots; i++)
            {
                slotsUI[i] = factoryUI.CreateSlotUI(inventoryView.Root);
                slotsUI[i].SlotClicked += cursorSlotUI.ChangeUiData;
            }

            inventoryView.Init(slotsUI);
            this.inventory.SetData(inventoryData?.SlotData);
        }

        /// <summary>
        /// Changes the slot data depending on the slotUI change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ChangeSlotData(object sender, CursorSlotEventArgs args)
        {
            var slot = inventory[args.Index];

            switch (args.OperationType)
            {
                case OperationType.Add:
                    inventory.AddItem(lastUsedItemId, args.Index, args.Amount);
                    break;
                case OperationType.Remove:
                    lastUsedItemId = slot.Item.Id;
                    inventory.RemoveItem(args.Index, args.Amount);
                    return;
                case OperationType.Increase:
                    var remainder = inventory.AddItem(lastUsedItemId, args.Index, args.Amount);
                    cursorSlotUI.AmountOfItemsUI = remainder.ToString();
                    break;
                case OperationType.Replace:
                    var itemId = slot.Item.Id;
                    inventory.RemoveItem(args.Index, slot.AmountItems);
                    inventory.AddItem(lastUsedItemId, args.Index, args.Amount);
                    lastUsedItemId = itemId;
                    break;
                default: Debug.Log($"Unknown operation: {args.OperationType}");
                    break;
            }
        }
    }
}