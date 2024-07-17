using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Models.Services
{
    public static class ItemService
    {
        // Saves the list of items to a JSON file.
        public static void SaveItem(List<Item> items)
        {
            string appDataDirectory = Utils.GetAppDirectoryPath();
            string filePath = Utils.GetItemsFilePath();

            // Serialize the list of items to JSON and write it to the file.
            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(filePath, json);
        }

        // Retrieves the list of items from the JSON file.

        public static List<Item> GetItem()
        {
            string filePath = Utils.GetItemsFilePath();
            if (!File.Exists(filePath))
            {
                return new List<Item>();
            }
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Item>>(json);
        }

        public static List<Item> AddItem(string iName, float iPrice, ItemType iType)
        {
            List<Item> items = GetItem();

            // Check if an item with the same name already exists.
            bool itemExists = items.Any(x => x.itemName == iName);
            if (itemExists)
            {
                throw new Exception("This item already exists!");
            }
            items.Add(
                new Item
                {
                    itemName = iName,
                    itemPrice = iPrice,
                    itemType = iType
                }
                );
            SaveItem(items);
            return items;
        }

        public static List<Item> DeleteItem(string iName)
        {
            List<Item> items = GetItem();
            Item item = items.FirstOrDefault(x => x.itemName == iName);
            if (item == null)
            {
                throw new Exception("No item to delete!");
            }
            items.Remove(item);
            SaveItem(items);
            return items;
        }

        public static List<Item> UpdatePrice(string iName, float iPrice)
        {
            List<Item> items = GetItem();
            Item item = items.FirstOrDefault(x => x.itemName == iName);
            if (item == null)
            {
                throw new Exception("Item not found!");
            }
            item.itemPrice = iPrice;
            SaveItem(items);
            return items;
        }

    }
}
