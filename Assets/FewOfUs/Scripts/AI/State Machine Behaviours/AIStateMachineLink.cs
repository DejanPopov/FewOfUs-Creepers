using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachineLink : StateMachineBehaviour
{
    private AIStateMachine stateMachineS;
    public AIStateMachine stateMachine { set { stateMachineS = value; } }
}
