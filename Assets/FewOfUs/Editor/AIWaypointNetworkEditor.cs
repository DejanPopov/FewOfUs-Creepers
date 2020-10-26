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




        if (network.DisplayMode == PathDisplayMode.Connections)
        {
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

                if (network.waypoints[index] != null)
                {
                    //If it is not null reference we will get index in linePoints
                    linePoints[i] = network.waypoints[index].position;
                }
                else
                {
                    //If it is null this code will make sure that the lines will go to
                    //infinity and wee will be abale to see that graphicaly
                    linePoints[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
                }

                //Lines colour
                Handles.color = Color.cyan;

                //This will draw the lines between waypoints
                Handles.DrawPolyLine(linePoints);
            }
        }
        else
        {
            if (network.DisplayMode == PathDisplayMode.Paths)
            {
                //CODE
            }

        }
    }
}
 