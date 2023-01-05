using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite defaultImage;
    public Sprite pressedImage;
    private GazeAware _gazeAwareComponent;
    //public bool isGazed;

    void Start()
    {
        _gazeAwareComponent = GetComponent<GazeAware>();
        theSR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_gazeAwareComponent.HasGazeFocus)
        {
            theSR.sprite = pressedImage;
            //isGazed = true;
        }
        else
        {
            theSR.sprite = defaultImage;
            //isGazed = false;
        }
    }
}
