using System.IO;
using UnityEditor;
using UnityEngine;

namespace InventorySystem.UI
{
    public class FactoryUI
    {
        private const string Path = "Assets/UI/Prefabs";

        private T CreateUiPrefab<T>(Transform root, string title) where T : Component
        {
            if (!Directory.Exists(Path)) return null;
            var prefab = (GameObject)AssetDatabase
                .LoadAssetAtPath(System.IO.Path.Combine(Path, title), typeof(GameObject));
            return Object.Instantiate(prefab, root).GetComponent<T>();

        } 
        public SlotUI CreateSlotUI(Transform root)
        {
            const string title = "InventorySlotUI.prefab";
            return CreateUiPrefab<SlotUI>(root, title);
        }

        private CursorSlotUI CreateCursorSlotUI(Transform root)
        {
            const string title = "CursorSlotUI.prefab";
            return CreateUiPrefab<CursorSlotUI>(root, title);
        }
    
        public InventoryView CreateInventoryView(Transform root, bool withCursorSlot)
        {
            const string title = "InventoryView.prefab";
            var inventoryView = CreateUiPrefab<InventoryView>(root, title);

            if (!withCursorSlot) return inventoryView;
            
            var cursorSlot = CreateCursorSlotUI(root);
            cursorSlot.transform.SetParent(inventoryView.Root.parent, false);

            return inventoryView;
        }
    }
}