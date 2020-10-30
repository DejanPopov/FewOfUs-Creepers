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

    //Default handlers
    public virtual void OnEnterState()
    {

    }

    public virtual void OnExitState()
    {

    }

    public abstract AIStateType OnUpdate()
    {

    }

    public virtual void OnAnimatorUpdated()
    {

    }

    public virtual void OnAnimatorIKUpdated()
    {

    }

    public virtual void OnTriggerEvent(AITriggerEventType eventType, Collider other)
    {

    }

    public virtual void OnDestinationReached(bool isReached)
    {

    }
}
