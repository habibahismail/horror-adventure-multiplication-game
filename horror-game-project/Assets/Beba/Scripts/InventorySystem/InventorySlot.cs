using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace gameBeba
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject mouseCursorObject;

        private UIManager ui;
        private MouseCursor mouseCursor;

        private Item _item;
        public Item item
        {
            get { return _item;  }
            set
            {
                _item = value;

                if(item == null)
                {
                    image.enabled = false;
                }
                else
                {
                    image.sprite = _item.image;
                    image.enabled = true;
                }
            }
        }

        private void Start()
        {
            ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            mouseCursor = mouseCursorObject.GetComponent<MouseCursor>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData != null && eventData.button == PointerEventData.InputButton.Right)
            {
                if(item != null)
                {
                    GameEvents.OnUseItemInitiated(item);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ui.ShowItemDetails(item);

            if (item.isUsable)
            {
                mouseCursor.ShowRightMouseClickIcon();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui.HideItemDetails();
            mouseCursor.HideRightMouseClickIcon();
        }
        


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
        }
#endif

    }

}
