using SceneUI;
using UnityEngine;

namespace InventorySystem
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private AuxiliaryUI ui;
        
        private InventorySerializer inventorySerializer;
        private Inventory inventory;
        private const int MinAmountSlots = 10;

        private void Awake()
        {
            inventorySerializer = new InventorySerializer();
            var inventoryData = inventorySerializer.Load();
            inventory = inventoryData == null ? new Inventory(MinAmountSlots) : new Inventory(inventoryData.AmountSlots);
            var controller = new InventoryController(inventoryData, inventory, player);
            
            //Scene UI
            ui.AddButton.onClick.AddListener(() =>
            {
                var id = DataBase.GetItemByName(ui.CurrentName).Id;
                inventory.AddItem(id, ui.AmountItems);
            });
            ui.RemoveButton.onClick.AddListener(() =>
            {
                var item = DataBase.GetItemByName(ui.CurrentName);
                var id = item.Id;
                inventory.RemoveItem(id, ui.AmountItems);
            });
            ui.ClearButton.onClick.AddListener(() => inventory.Clear());
            ui.ItemNamesDropDown.AddOptions(DataBase.GetAllItemNames());
        }
    
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) inventorySerializer.Save(inventory);
        }
    }
}

//TODO: mb fix item null in slot 