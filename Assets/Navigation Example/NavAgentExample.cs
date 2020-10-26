using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
{
    //Option that we can assign Waypoint Network
    public AIWaypointNetwork WaypointNetwork = null;

    //Waypoint number
    public int CurrentIndex = 0;

    private NavMeshAgent navAgent = null;

    // Start is called before the first frame update
    void Start()
    {
        //NavMesh agent reference
        navAgent = GetComponent<NavMeshAgent>();

        if (WaypointNetwork == null)
        {
            return;
        }

        if (WaypointNetwork.waypoints[CurrentIndex] != null)
        {
            navAgent.destination = WaypointNetwork.waypoints[CurrentIndex].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
