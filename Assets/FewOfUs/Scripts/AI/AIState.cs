using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public abstract AIStateType GetStateType();
    protected AIStateMachine machineM;

    public void SetStateMachine (AIStateMachine stateMachine)
    {
        machineM = stateMachine;
    }
}
