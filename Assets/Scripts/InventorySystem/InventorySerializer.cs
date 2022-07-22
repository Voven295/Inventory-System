using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InventorySystem
{
    public class InventorySerializer
    {
        private const string FolderName = "SerializedData"; 
        private const string FileName = "InventoryData.json"; 
        private readonly string path;

        public InventorySerializer(string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(Application.dataPath, FolderName);
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                path = Path.Combine(path, FileName);
            }

            this.path = path;
        }

        public void Save(Inventory inventory)
        {
            var slotData = inventory.Slots.Select(s => s.ConvertToSlotData()).ToArray();
            
            var inventoryData = new InventoryData
            {
                AmountSlots = inventory.MaxAmountSlots,
                SlotData = slotData                        
            };
            
            var json = JsonUtility.ToJson(inventoryData);
            File.WriteAllText(path, json);
        }

        public InventoryData Load()
        {
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path, Encoding.UTF8);
            return JsonUtility.FromJson<InventoryData>(json);
        }
    }
}