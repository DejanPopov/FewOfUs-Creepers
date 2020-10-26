using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentExample : MonoBehaviour
{
    private NavMeshAgent navAgent = null;

    // Start is called before the first frame update
    void Start()
    {
        //NavMesh agent reference
        navAgent = GetComponent<NavMeshAgent>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
