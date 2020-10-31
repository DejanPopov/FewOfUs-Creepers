using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RootMotionConfigurator : AIStateMachineLink
{
    [SerializeField]
    private int rootPosition = 0;

    [SerializeField]
    private int rootRotation = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateMachineS)
        {
            Debug.Log(stateMachineS.GetType().ToString());
            stateMachineS.AddRootMotionRequest(rootPosition, rootRotation);
        }
       // base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateMachineS)
        {
            stateMachineS.AddRootMotionRequest(- rootPosition, - rootRotation);
        }

        //base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
