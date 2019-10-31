using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace gameBeba
{
    public class Inspect : Interactable
    {
        [SerializeField] private string[] dialogLines;

        protected override void DoAction()
        {
            boxCollider.enabled = false;
            dialogSystem.StartDialog(dialogLines);
        }
    }
}
