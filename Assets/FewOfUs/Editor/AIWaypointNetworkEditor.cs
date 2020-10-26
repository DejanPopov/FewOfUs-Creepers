using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Editor class
using UnityEditor;

//Editor class thats type of script that has list of waypoints
[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor
{
    //This will show in Unity name of waypoints and flags
    private void OnSceneGUI()
    {
        //Casting (Unity reference documentation)
        AIWaypointNetwork network = (AIWaypointNetwork)target;

        //Looping through waypoints
        for (int i = 0; i < network.waypoints.Count; i++)
        {
            //Label needs 2 parameters - Vector and sttring
            Handles.Label(network.waypoints[i].position, "Waypoint" + i.ToString());
        }
    }
}
