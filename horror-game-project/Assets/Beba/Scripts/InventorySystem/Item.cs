using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gameBeba
{
    [CreateAssetMenu(fileName ="New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private string id;
        public string ID { get { return id;  } }

        public new string name = "New Item";
        public Sprite image = null;

        [TextArea(5,10)]
        public string description;
        public int quantity = 0;
        public bool isUsable = false;
        public bool isBattleItem = false;

    #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            string path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(path);
        }
        
#endif
    }

    [System.Serializable]
    public class SaveGameItem
    {
        public string _ID;
        public string _description;
        public int _quantity;

        public SaveGameItem(Item item)
        {
            _ID = item.ID;
            _description = item.description;
            _quantity = item.quantity;
        }
    }

}

