using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraMount : MonoBehaviour
{
    public Transform Mount = null;
    //Camera speed
    public float Speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Find position of the camera
        transform.position = Vector3.Lerp(transform.position, Mount.position, Time.deltaTime * Speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Mount.rotation, Time.deltaTime * Speed);
    }
}
