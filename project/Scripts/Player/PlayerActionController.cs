using UnityEngine;
using System.Collections;

public class PlayerActionController : MonoBehaviour {
    private readonly int LEFT_CLICK_CODE = 0;
    private readonly int RIGHT_CLICK_CODE = 1;
    private readonly KeyCode Q_KEY_CODE = KeyCode.Q;

    private Camera gameCamera;
    private PrefabRepository prefabRepository;

    private EquipmentController currentEquipment;

    /// <summary>
    /// MONOBEHAVIOUR Start
    /// </summary>
    void Start() {
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        currentEquipment = new TazerController(prefabRepository);
        //currentEquipment = new KnifeController(prefabRepository);
    }

    /// <summary>
    /// Keeps the provided PrefabRepository as reference, using it when necessary.
    /// </summary>
    /// <param name="prefabRepo">The repository to reference.</param>
    public void LinkPrefabRepository(PrefabRepository prefabRepo) {
        prefabRepository = prefabRepo;
    }

    /// <summary>
    /// MONOBEHAVIOUR Update
    /// </summary>
    void Update () {
        bool downAction;
        Vector3 targetLocation = GetTargetLocationFromMouse();

	    if (Input.GetMouseButton(LEFT_CLICK_CODE)) {
            downAction = Input.GetMouseButtonDown(LEFT_CLICK_CODE);
            currentEquipment.OnPrimaryAction(GetComponent<Entity>(), targetLocation, downAction);
        }
        if (Input.GetMouseButton(RIGHT_CLICK_CODE)) {
            downAction = Input.GetMouseButtonDown(RIGHT_CLICK_CODE);
            currentEquipment.OnSecondaryAction(GetComponent<Entity>(), targetLocation, downAction);
        }
        if (Input.GetKey(Q_KEY_CODE)) {
            downAction = Input.GetKeyDown(Q_KEY_CODE);
            currentEquipment.OnRemoteAction(GetComponent<Entity>(), targetLocation, downAction);
        }
    }

    /// <summary>
    /// Converts the mouse location on-screen to the corresponding world location and returns this world location.
    /// </summary>
    /// <returns>The world location of the mouse pointer.</returns>
    private Vector3 GetTargetLocationFromMouse() {
        Vector3 mousePositionOnScreen = Input.mousePosition;
        Vector3 mousePositionInWorld = gameCamera.ScreenToWorldPoint(mousePositionOnScreen);
        return mousePositionInWorld;
    }
}
