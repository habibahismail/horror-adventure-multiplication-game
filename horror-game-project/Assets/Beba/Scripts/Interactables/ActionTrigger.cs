using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    public class ActionTrigger : Interactable
    {

        protected override void DoAction()
        {
            Debug.Log("trigger some action.yehaa!");
        }
    }
}
