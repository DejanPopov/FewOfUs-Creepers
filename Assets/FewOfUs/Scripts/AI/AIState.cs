using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public abstract AIStateType getStateType();
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

    public abstract AIStateType OnUpdate();

    public virtual void OnAnimatorUpdated()
    {
        if (machineM.useRootPosition)
        {
            machineM.navAgentN.velocity = machineM.animatorA.deltaPosition / Time.deltaTime;
        }

        if (machineM.useRootRotation)
        {
            machineM.transform.rotation = machineM.animatorA.rootRotation;
        }
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
