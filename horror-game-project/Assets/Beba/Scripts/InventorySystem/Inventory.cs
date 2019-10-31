using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<Item> itemsList;
        [SerializeField] private List<Item> startingItemList;
        [SerializeField] private ItemsDatabase itemsDatabase;

        private List<SaveGameItem> saveGameItems = new List<SaveGameItem>();
        private int maxStackPerSlot = 99;
        private UIManager ui;
        private bool isUsed = false;
        
        public List<Item> ItemList { get { return itemsList; } }

        private void Awake()
        {
            ui = GameObject.Find("UIManager").GetComponent<UIManager>();

            if (SaveLoad.SaveExist("Inventory"))
            {
                Load();
            }else
            {
                itemsList = new List<Item>();

                foreach (var item in startingItemList)
                {
                    AddItem(item);
                }
            }
        }

        private void Start()
        {
            GameEvents.SaveInitiated += Save;
            GameEvents.UseItem += UseItem;
        }

        private bool IsFull()
        {
            int invSlot = ui.InventorySlotCount();

            return itemsList.Count >= invSlot;
        }

        private void UseItem(Item item)
        {
            if (item.isUsable == true)
            {
                Debug.Log(item.name + " is used!");
                RemoveItem(item);
            }
            else
            {
                Debug.Log(item.name + " cannot be used.");
            }
            
        }
        
        public void RefreshInventory()
        {
            ui.UpdateInventorySlot(itemsList);
        }

        private void Save()
        {
            foreach (Item item in itemsList)
            {
                saveGameItems.Add(new SaveGameItem(item));
            }

            SaveLoad.Save<List<SaveGameItem>>(saveGameItems, "Inventory");
            
            Debug.Log("Inventory saved!");
        }

        private void Load()
        {
            if (SaveLoad.SaveExist("Inventory"))
            {
                List<SaveGameItem> reloadedItems = SaveLoad.Load<List<SaveGameItem>>("Inventory");

                itemsList = new List<Item>();

                foreach ( SaveGameItem SavedItem in reloadedItems)
                {
                    Item LoadedItem = itemsDatabase.GetItemReference(SavedItem._ID);
                    LoadedItem.description = SavedItem._description;
                    LoadedItem.quantity = SavedItem._quantity;

                    itemsList.Add(LoadedItem);
                }
            }
        }

        public bool AddItem(Item item)
        {
            if (IsFull())
            {
                ui.SetActionKeyUI("", "You cannot carry any more items.");
                GameManager.Instance.ClearNotification();
                return false;
            }

            if (itemsList.Contains(item))
            {
                for (int i = 0; i < itemsList.Count; i++)
                {
                    if (itemsList[i].ID == item.ID) {

                        if(itemsList[i].quantity < maxStackPerSlot)
                        {
                            itemsList[i].quantity++;
                            return true;
                        }
                    }
                }
            }

            item.quantity++;
            itemsList.Add(item);
            return true;
        }

        public void RemoveItem(Item item)
        {
            if (itemsList.Contains(item))
            {
                for (int i = 0; i < itemsList.Count; i++)
                {
                    if (itemsList[i].ID == item.ID)
                    {

                        if (itemsList[i].quantity == 1)
                        {
                            itemsList[i].quantity--;
                            itemsList.Remove(itemsList[i]);

                        }else
                        {
                            itemsList[i].quantity--;
                        }
                        
                    }
                }

                RefreshInventory();
            }
        }

        public void UpdateItemDescription(Item item, string desc)
        {
            if (itemsList.Contains(item))
            {
                for (int i = 0; i < itemsList.Count; i++)
                {
                    if (itemsList[i].ID == item.ID)
                    {
                        ItemList[i].description = desc;
                        Debug.Log(desc);
                    }
                }
            }
        }
    }
}