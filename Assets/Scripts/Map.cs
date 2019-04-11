using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public float mapLength;
    public Camera mainCam;
    public float[] combatPositions;
    private int currentCombatNum;
    // Start is called before the first frame update
    void Start()
    {
        currentCombatNum = 0;
        transform.localScale = new Vector3(mapLength, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
       if(mainCam.transform.position.x >= combatPositions[currentCombatNum])
        {
            mainCam.GetComponent<CameraScript>().locked = true;
            combatEvent();
            currentCombatNum += 1;
        }
    }

    public void combatEvent()
    {
        //definition for what happens in this event enemy spawn ect...
    }
}
