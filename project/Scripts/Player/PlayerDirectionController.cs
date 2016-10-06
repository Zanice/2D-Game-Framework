using UnityEngine;
using System.Collections;

public class PlayerDirectionController : MonoBehaviour {
    private Camera gameCamera;

    /// <summary>
    /// MONOBEHAVIOUR Start
    /// </summary>
    void Start() {
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    /// <summary>
    /// MONOBEHAVIOUR Update
    /// </summary>
	void Update () {
        LookAtMousePoint();
	}

    /// <summary>
    /// Rotates the transform according to the direction the mouse pointer is in.
    /// </summary>
    private void LookAtMousePoint() {
        Vector3 mousePositionOnScreen = Input.mousePosition;
        Vector3 mousePositionInWorld = gameCamera.ScreenToWorldPoint(mousePositionOnScreen);
        Vector3 direction = mousePositionInWorld - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
