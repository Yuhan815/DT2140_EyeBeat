using UnityEngine;
using Tobii.Gaming;

public class Circle : MonoBehaviour
{
    public GameObject CircleAccuracy, CircleBody, CircleBackGround, CircleCombo;
    private SpriteRenderer approachcircle, hitcircle, hitcircleoverlay, circlecombo;
    private GameObject CircleObject, GameControl;
    private Vector3 AccuracyChange;
    private Color CircleColor;
    private GameHandler gameHandler;
    private float targetScale = 0.85f;
    private bool ScaleAccuracy;

    public bool isGazed;

    private GazeAware _gazeAwareComponent;

    private void Awake() {
        approachcircle = CircleAccuracy.GetComponent<SpriteRenderer>();
        hitcircle = CircleBody.GetComponent<SpriteRenderer>();
        hitcircleoverlay = CircleBackGround.GetComponent<SpriteRenderer>();
        circlecombo = CircleCombo.GetComponent<SpriteRenderer>();
        AccuracyChange = new Vector3(3f, 3f, 3f);
        GameControl = GameObject.Find("GameControl");
        gameHandler = GameControl.GetComponent<GameHandler>();
        _gazeAwareComponent = GetComponent<GazeAware>();
    }

    // Function spawn object from GameHandler
    public void Spawn(GameObject Circle){
        CircleObject = Circle;
        this.enabled = true;
        ScaleAccuracy = true;
        CircleColor = approachcircle.color;
    }

    void Update()
    {
        if (_gazeAwareComponent.HasGazeFocus)
        {
            isGazed = true;
        }
        else
        {
            isGazed = false;
        }
        // Animation object and delete;
        if (ScaleAccuracy){
            CircleAccuracy.transform.localScale -= AccuracyChange * Time.deltaTime;
            if(CircleAccuracy.transform.localScale.x < targetScale){
                ScaleAccuracy = false;
                gameHandler.NeedCircle++;
                this.enabled = false;
                Destroy(CircleObject);
            }
        }

        CircleColor.a += 3f * Time.deltaTime;
        approachcircle.color = CircleColor;
        hitcircle.color = CircleColor;
        hitcircleoverlay.color = CircleColor;
        circlecombo.color = CircleColor;
    }
}
