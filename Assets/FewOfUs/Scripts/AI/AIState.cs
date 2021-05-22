using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public abstract AIStateType getStateType();

    protected AIStateMachine machineM;

    public virtual void SetStateMachine (AIStateMachine stateMachine)
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

    public static void ConvertSphereColliderToWorldSpace (SphereCollider col, out Vector3 pos, out float radius)
    {
        pos = Vector3.zero;
        radius = 0.0f;

        if (col == null)
        {
            return;
        }
        //Calcualte world space position of sphere center
        pos = col.transform.position;
        pos.x += col.center.x * col.transform.lossyScale.x;
        pos.y += col.center.y * col.transform.lossyScale.y;
        pos.z += col.center.z * col.transform.lossyScale.z;

        // Calculate world space radius of sphere
        radius = Mathf.Max(col.radius * col.transform.lossyScale.x,
                col.radius * col.transform.lossyScale.y);

        Mathf.Max(radius, col.radius * col.transform.lossyScale.z);
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
