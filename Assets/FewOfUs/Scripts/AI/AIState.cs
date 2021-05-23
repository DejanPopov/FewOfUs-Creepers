using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
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

        radius = Mathf.Max(radius, col.radius * col.transform.lossyScale.z);
    }
    public static float FindSignedAngle(Vector3 fromVector, Vector3 toVector)
    {
        if(fromVector == toVector)
        {
            return 0.0f;
        }

        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 cross = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(cross.y);
        return angle;
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
