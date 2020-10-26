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
            //Skip waypoint that is null
            if (network.waypoints[i] != null)
            {
                //Label needs 2 parameters - Vector and sttring
                Handles.Label(network.waypoints[i].position, "Waypoint " + i.ToString());
            }
        }

        //DrawPolyLine - takes Array of Vector3 points
        //We pass numbers of waypoints + 1 because last will be duplicate of first waypoint
        //(network.waypoints.Count + 1)
        //and that will add aditional line to connect everything
        Vector3[] linePoints = new Vector3[network.waypoints.Count + 1];

        for (int i = 0; i < network.waypoints.Count; i++)
        {
            // when i = 6 (the num,ber of waypoints) is not valid because in the Array we have
            //from 0 to 5, so when i = 6 we want that to be the first index (0,1,2,3,4,5,1)
            int index = i != network.waypoints.Count ? i : 0;
        }
    }
}
