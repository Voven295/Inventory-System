using System;
using UnityEngine;

namespace InventorySystem
{
    public abstract class Item : ScriptableObject, IEquatable<Item>
    {
        [SerializeField]
        private Sprite icon;

        public Sprite Icon => icon;
    
        [SerializeField]
        private string id;
        public string Id => id;
    
        [SerializeField]
        private int cost;

        public int Cost => cost;

        [SerializeField]
        private int maxStack;

        public int MaxStack => maxStack;

        //Generate GUID (id) for item and save him in PlayerPrefs
        private void OnValidate()
        {
            if (!PlayerPrefs.HasKey($"{this.name}Id"))
            {
                id = UnityEditor.GUID.Generate().ToString();
                PlayerPrefs.SetString($"{this.name}Id", id);
            }
            else
            {
                id = PlayerPrefs.GetString($"{this.name}Id");
            }
        }

        public bool Equals(Item otherItem) => otherItem != null && Id.Equals(otherItem.Id);

        public override int GetHashCode() => base.GetHashCode();
    }
}