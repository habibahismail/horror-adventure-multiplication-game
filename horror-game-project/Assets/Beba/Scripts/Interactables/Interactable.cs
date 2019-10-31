using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] protected float interactableDistance = 2.5f;
        [SerializeField] protected string actionKeyText;
        [SerializeField] protected string actionKeyDesc;
        
        protected string _actionKeyText;
        protected string _actionKeyDesc;
        protected DialogSystem dialogSystem;
        protected BoxCollider boxCollider;
        protected UIManager ui;

        private float playerDistance;
        private string objectHitName;

        protected virtual void Start()
        {
            ui = GameObject.Find("UIManager").GetComponent<UIManager>();
            dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();
            boxCollider = GetComponent<BoxCollider>();

            _actionKeyText = actionKeyText;
            _actionKeyDesc = actionKeyDesc;
        }
        

        protected void FixedUpdate()
        {
            playerDistance = PlayerRaycast.distanceFromTarget;
            objectHitName = PlayerRaycast.objectHit;
        }

        protected virtual void Update()
        {
            if(GameManager.Instance.isInteracting == false)
            {
                boxCollider.enabled = true;
            }
        }

        protected void OnMouseOver()
        {
            if (playerDistance <= interactableDistance && objectHitName == gameObject.name && GameManager.Instance.isBattling == false)
            {
                SetMouseOverText();

                actionKeyText = _actionKeyText;
                actionKeyDesc = _actionKeyDesc;
                
                if (Input.GetButtonDown("Action"))
                {
                    DoAction();
                    GameManager.Instance.isInteracting = true;
                }
            }
            else
            {
                actionKeyText = "";
                actionKeyDesc = "";
            }
           
            ui.SetActionKeyUI(actionKeyText, actionKeyDesc);
        }

        protected void OnMouseExit()
        {
            ClearActionText();
        }
        
        protected void ClearActionText()
        {
            actionKeyText = "";
            actionKeyDesc = "";

            ui.SetActionKeyUI(actionKeyText, actionKeyDesc);
        }
        
        protected virtual void DoAction() { }
        protected virtual void SetMouseOverText() { }
    }
}
