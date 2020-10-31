using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor : MonoBehaviour
{
    private AIStateMachine parentStateMachine = null;
    public AIStateMachine parentStateMachines { set { parentStateMachine = value; } }

    private void OnTriggerEnter(Collider col)
    {
        if (parentStateMachine != null)
        {
            parentStateMachine.OnTriggerEvent(AITriggerEventType.Enter, col);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (parentStateMachine != null)
        {
            parentStateMachine.OnTriggerEvent(AITriggerEventType.Stay, col);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (parentStateMachine != null)
        {
            parentStateMachine.OnTriggerEvent(AITriggerEventType.Exit, col);
        }
    }
}
