using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    //public float xmin, xmax;
    //public float ymin, ymax;
    public ChangeColor theCC;

    public GameObject hitEffect, goodEffect, perfectEffect,missEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 gazePosition = TobiiAPI.GetGazePoint().Screen;
        //if (gazePosition.x > xmin && gazePosition.x < xmax && gazePosition.y > ymin && gazePosition.y < ymax)
        if(theCC.isGazed == true)
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);

                //GameManager.instance.NoteHit();

                if(transform.position.y > 0.4)
                {
                    Debug.Log("Perfect");
                    GameManager.instance.PerfectHit();
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);

                }
                else if(transform.position.y > 0)
                {
                    Debug.Log("Good");
                    GameManager.instance.GoodHit();
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }
                else
                {
                    Debug.Log("Hit");
                    GameManager.instance.NormalHit();
                    Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = false;

            GameManager.instance.NoteMissed();
            Instantiate(missEffect, transform.position, missEffect.transform.rotation);
        }
    }
}
