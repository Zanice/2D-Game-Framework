using UnityEngine;
using System.Collections;

public class MovementCollisionManager : MonoBehaviour {
    private CoordinatePair coordinates;
    private Tile[,] detectionSample;
    private bool[,] obstacleSample;

    /// <summary>
    /// MONOBEHAVIOUR Start
    /// </summary>
    void Start () {
        GridRepository gridRepository = GridRepository.Instance;
        coordinates = gridRepository.GridCoordinatesFromPosition(transform.position);
        detectionSample = gridRepository.GetUnboundedGridSample(coordinates.x - 1, coordinates.y - 1, 3, 3);
        obstacleSample = new bool[3, 3];
        UpdateObstacleSample();
	}

    /// <summary>
    /// The buffer for the entity, or the radius from the center of the entity for which the entity is physical.
    /// </summary>
    public float EntityBuffer {
        get {
            return GetComponent<Entity>().PhysicalRadius;
        }
    }

    /// <summary>
    /// The current collision detection sample for the entity.
    /// </summary>
    public Tile[,] DetectionSample {
        get {
            return detectionSample;
        }
    }

    /// <summary>
    /// The current obstacle sample, derived from the collision detection sample, for the entity.
    /// </summary>
    public bool[,] ObstacleSample {
        get {
            return obstacleSample;
        }
    }

    /// <summary>
    /// MONOBEHAVIOUR Update
    /// </summary>
    void Update () {
        UpdateCoordinates();
    }

    /// <summary>
    /// Updates the coordinates of the entity, also updating collision detection information if necessary.
    /// </summary>
    private void UpdateCoordinates() {
        GridRepository gridRepository = GridRepository.Instance;

        Vector3 position = transform.position;
        CoordinatePair adjustedCoordinates = gridRepository.GridCoordinatesFromPosition(position);

        if (!coordinates.Equals(adjustedCoordinates)) {
            coordinates = adjustedCoordinates;
            UpdateCollisionInformation();
        }
    }

    /// <summary>
    /// Grabs a new collision detection sample based on the entity's current coordinates, and updates the obstacle sample accordingly.
    /// </summary>
    private void UpdateCollisionInformation() {
        GridRepository gridRepository = GridRepository.Instance;

        detectionSample = gridRepository.GetUnboundedGridSample(coordinates.x - 1, coordinates.y - 1, 3, 3);
        UpdateObstacleSample();
    }

    /// <summary>
    /// Uses the collision detection sample to update the current obstacle sample.
    /// </summary>
    private void UpdateObstacleSample() {
        Tile current;
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                current = detectionSample[x, y];
                obstacleSample[x, y] = null == current || current.Type.Obstacle == TileType.TileType_Obstacle.WALL;
            }
        }
    }
}
