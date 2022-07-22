using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public static class DataBase
    {
        private static readonly Dictionary<string, Item> Items = Resources.LoadAll<Item>(path)
            .ToDictionary(item => item.Id);
    
        private const string path = "Items";
    
        public static Item GetItemById(string id)
        {
            return Items.ContainsKey(id) ? Items[id] : null;
        }
    
        //For tests
        internal static Item GetItemByName(string name)
        {
            return (from item in Items where item.Value.name.Equals(name) select item.Value).FirstOrDefault();
        }

        internal static List<string> GetAllItemNames()
        {
            return Items.Select(x => x.Value.name).ToList();
        }
        
        public static int Count()
        {
            return Items.Count();
        }
    }
}