using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    private Vector3 curPlayerPortPos;
    public Camera mainCam;

    public float smoothSpeed = 0.125f;

    [HideInInspector]
    public bool locked;
    // Start is called before the first frame update
    void Start()
    {
        locked = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        curPlayerPortPos = mainCam.WorldToViewportPoint(player.transform.position);
        //Debug.Log(curPlayerPortPos);
        if((curPlayerPortPos.x >= .5f) && !locked)
        {
            Vector3 desiredPos = new Vector3(player.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
            //mainCam.transform.position = new Vector3(player.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
            //Debug.Log("on other half");
        }
    }
}
