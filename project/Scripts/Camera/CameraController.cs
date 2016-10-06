using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    /// <summary>
    /// Value signifying the screen "edge", or at what value and corresponding reflected value the screen will start to pan.
    /// Can only range from 0.5f to 1f.
    /// </summary>
    private readonly float MOVE_THRESHOLD = .9f;

    /// <summary>
    /// Value expressing the speed at which the camera will pan.
    /// </summary>
    private readonly float CAMERA_SPEED = .25f;

    private Camera thisCamera;
    private Vector3 DELTA_X;
    private Vector3 DELTA_Y;

    //MONOBEHAVIOUR Start
    void Start () {
        thisCamera = gameObject.GetComponent<Camera>();

        //Set up the vectors that will be used to pan the camera
        DELTA_X = new Vector3(CAMERA_SPEED, 0, 0);
        DELTA_Y = new Vector3(0, CAMERA_SPEED, 0);
    }

    //MONOBEHAVIOUR Update
    void Update () {
        //Get the mouse location on the screen as floats from 0f to 1f.
        Vector3 mousePositionOnScreen = Input.mousePosition;
        Vector3 screenPositionDiff = thisCamera.ScreenToViewportPoint(mousePositionOnScreen);

        //If mouse falls beyond reflected threshold from center of horizontal axis, pan the camera left
        if (screenPositionDiff.x < 1 - MOVE_THRESHOLD)
            transform.position -= DELTA_X;
        //Else, if mouse falls beyond threshold from center of horizontal axis, pan the camera right
        else if (screenPositionDiff.x > MOVE_THRESHOLD)
            transform.position += DELTA_X;

        //If mouse falls beyond reflected threshold from center of vertical axis, pan the camera down
        if (screenPositionDiff.y < 1 - MOVE_THRESHOLD)
            transform.position -= DELTA_Y;
        //Else, if mouse falls beyond threshold from center of vertical axis, pan the camera up
        else if (screenPositionDiff.y > MOVE_THRESHOLD)
            transform.position += DELTA_Y;
    }
}
