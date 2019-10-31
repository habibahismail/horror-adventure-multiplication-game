using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    public class PickUp : Interactable
    {
        [SerializeField] private Item item;
        [SerializeField] private string[] dialogLines;

        private Inventory playerInventory;

        protected override void Start()
        {
            base.Start();
            playerInventory = GameObject.Find("FPSController").GetComponent<Inventory>();
        }
        
        protected override void DoAction()
        {
            ClearActionText();
            playerInventory.AddItem(item);

            
            if(dialogLines.Length > 0)
            {
                dialogSystem.StartDialog(dialogLines);
            }
            else
            {
                actionKeyDesc = item.name + " has been added to Inventory.";
                GameManager.Instance.ClearNotification();
                GameManager.Instance.isInteracting = false;
            }
        
            Destroy(gameObject);    
        }

        protected override void SetMouseOverText()
        {
            _actionKeyText = "[E]";
            _actionKeyDesc = "_Pick Up " + item.name + "_";
        }
        
    }
}
