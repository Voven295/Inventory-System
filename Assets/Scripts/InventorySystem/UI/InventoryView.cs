using UnityEngine;

namespace InventorySystem.UI
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Transform root;
    
        private SlotUI[] slotsUI;
        public Transform Root => root;
    
        public void Init(SlotUI[] slots)
        {
            slotsUI = slots;
        }

        public void UpdateUI(object sender, SlotModificationEventArgs e)
        {
            var currentSlotUI = slotsUI[e.SlotIndex];

            currentSlotUI.ItemIcon = e.SlotItem == null ? null : e.SlotItem.Icon;
            currentSlotUI.AmountOfItemsUI = e.AmountOfItems.ToString();
        }
    }
}