using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using System.Collections.Generic;

//AI states enumeration
public enum AIStateType
{
    None,
    Idle,
    Alerted,
    Partol,
    Attack,
    Feeding,
    Pursuit,
    Dead
}
public abstract class AIStateMachine : MonoBehaviour
{
    //Dictionary for state types
    //When zombie enters some state we will store it in Dictionary thus knowing its state
    private Dictionary<AIStateType, AIState> states = new Dictionary<AIStateType, AIState>();
}
 