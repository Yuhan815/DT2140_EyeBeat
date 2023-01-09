using UnityEngine;
using Tobii.Gaming;


/// <summary>
/// Changes the color of the game object's material, when the the game object 
/// is in focus of the user's eye-gaze.
/// </summary>
/// <remarks>
/// Referenced by the Target game objects in the Simple Gaze Selection example scene.
/// </remarks>

public class GazeDetect : MonoBehaviour
{
    public bool isGazed;

    private GazeAware _gazeAwareComponent;


    /// <summary>
    /// Set the lerp color
    /// </summary>
    void Start()
    {
        _gazeAwareComponent = GetComponent<GazeAware>();
    }

    /// <summary>
    /// Lerping the color
    /// </summary>
    void Update()
    {

        // Change the color of the cube
        if (_gazeAwareComponent.HasGazeFocus)
        {
            isGazed = true;
        }
        else
        {
            isGazed = false;
        }
    }

    /// <summary>
    /// Update the color, which should used for the lerping
    /// </summary>
    /// <param name="lerpColor"></param>
}