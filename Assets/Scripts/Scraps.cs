using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scraps : MonoBehaviour
{
    private int scraps;
    private float currentTime;
    private float lifeTime;
    private float rotate;
    // Start is called before the first frame update
    void Start()
    {
        rotate = 0;
        System.Random rnd = new System.Random();
        scraps = rnd.Next(1, 20);
        currentTime = 0.0f;
        lifeTime = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, rotate, 0);
        rotate += 1f;
        if (currentTime < lifeTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.tag);
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<playerNew>().increaseEnergy(scraps);
            Destroy(this.gameObject);
        }
    }
}
