using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameSceneManager is to detect all colliders
public class GameSceneManager : MonoBehaviour
{
    private Dictionary<int, AIStateMachine> stateMachines = new Dictionary<int, AIStateMachine>();

    //This is for AIState machine to register its colliders
    public void RegisterAIStateMachine(int key, AIStateMachine stateMachine)
    {
        if (! stateMachines.ContainsKey(key))
        {
            stateMachines[key] = stateMachine;
        }
    }

    //For objests in level
    public AIStateMachine GetAIStateMachine(int key)
    {
        AIStateMachine machine = null;

        if (stateMachines.TryGetValue(key, out machine))
        {
            return machine;
        }
        return null;
    }
}
