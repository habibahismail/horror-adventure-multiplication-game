using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gameBeba
{
    [CreateAssetMenu(fileName = "New Items Database", menuName = "Inventory/Items Database")]
    public class ItemsDatabase : ScriptableObject
    {
        [SerializeField] private Item[] items;

        public Item GetItemReference(string itemID)
        {
            foreach (Item item in items)
            {
                if (item.ID == itemID)
                {
                    return item;
                }
            }
            return null;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            LoadItems();
        }

        private void OnEnable()
        {
            EditorApplication.projectChanged -= LoadItems;
            EditorApplication.projectChanged += LoadItems;
        }

        private void OnDisable()
        {
            EditorApplication.projectChanged -= LoadItems;
        }

        private void LoadItems()
        {
            items = FindAssetsByType<Item>("Assets/Beba/Scripts/InventorySystem/InventoryItemSO");
        }

        // Slightly modified version of this answer: http://answers.unity.com/answers/1216386/view.html
        public static T[] FindAssetsByType<T>(params string[] folders) where T : Object
        {
            string type = typeof(T).Name;

            string[] guids;
            if (folders == null || folders.Length == 0)
            {
                guids = AssetDatabase.FindAssets("t:" + type);
            }
            else
            {
                guids = AssetDatabase.FindAssets("t:" + type, folders);
            }

            T[] assets = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }
            return assets;
        }
#endif
    }

}
