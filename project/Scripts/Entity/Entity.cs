using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour {
    private DynamicMatrix<Tile> trackingTiles;
    private CoordinatePair previousTrackingLocation;

    private int health;
    private float speed;

    //MONOBEHAVIOUR Start
    void Start () {
        trackingTiles = null;
    }

    /// <summary>
    /// Represents the current health of the entity. The entity is considered dead at 0 health.
    /// </summary>
    public int Health {
        get {
            return health;
        }
        set {
            health = Mathf.Clamp(value, 0, MaxHealth);
            if (health == 0)
                Kill();
        }
    }

    /// <summary>
    /// Represents the current speed proportion of the entity. At 1f, the entity moves according to its base speed.
    /// </summary>
    public float Speed {
        get {
            return speed;
        }
        set {
            speed = speed > 0 ? 0 : speed;
        }
    }

    /// <summary>
    /// Entity-specific value for team ownership.
    /// </summary>
    public abstract Allegiance Allegiance { get; }

    /// <summary>
    /// Entity-specific value for the radial width of the entity.
    /// </summary>
    public abstract float PhysicalRadius { get; }

    /// <summary>
    /// Entity-specific value for the maximum health possible.
    /// </summary>
    public abstract int MaxHealth { get; }

    /// <summary>
    /// Entity-specific value for the base movement speed of the entity.
    /// </summary>
    public abstract int BaseSpeed { get; }
	
    //MONOBEHAVIOUR Update
	void Update () {
        UpdateTileTracking();
    }

    public void TakeDamage(int amount) {
        Health -= amount;
    }

    public void Heal(int amount) {
        Health += amount;
    }

    /// <summary>
    /// Kill command for the entity, causing it to be removed from current play.
    /// </summary>
    public void Kill() {
        if (trackingTiles != null) {
            foreach (Tile tile in trackingTiles) {
                tile.Entities.Remove(this);
            }
        }

        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// Updates the GridRepository's records of the entity's position on the grid, making changes as the entity's location changes.
    /// Entity tracking is primarily used for collision detection with abilities.
    /// </summary>
    private void UpdateTileTracking() {
        //Calculate the tile buffer parameters needed for the current physical radius of the entity
        int tileOffset = Mathf.FloorToInt(PhysicalRadius) + 1;
        int tileRange = (2 * tileOffset) + 1;

        //Grab the tiles currently within the buffer around the entity
        CoordinatePair coordinates = GridRepository.Instance.GridCoordinatesFromPosition(transform.position);
        Tile[,] currentTiles = GridRepository.Instance.GetUnboundedGridSample(coordinates.x - tileOffset, coordinates.y - tileOffset, tileRange, tileRange);

        //Determine if the entity is not currently being tracked or is now within a different buffer than the one recorded in
        bool noTrackedTiles = null == trackingTiles;
        bool differentTiles = true;
        if (!noTrackedTiles)
            differentTiles = !coordinates.Equals(previousTrackingLocation);

        //If new tracking information needs to be recorded, adjust the tracking record for the tiles involved
        if (noTrackedTiles || differentTiles) {
            //if (!noTrackedTile) {
            //    foreach (Tile tile in trackingTiles)
            //        tile.Entities.Remove(gameObject);
            //}

            //TEMPORARY: Remove all tiles' tracking record for this entity.
            for (int i = 0; i < GridRepository.Instance.GridWidth; i++) {
                for (int j = 0; j < GridRepository.Instance.GridHeight; j++) {
                    GridRepository.Instance.GetTileAt(i, j).Entities.Remove(this);
                    //Debug.Log("REMOVING FROM " + GridRepository.Instance.GetTileAt(i, j).GridCoordinates + " for size of " + GridRepository.Instance.GetTileAt(i, j).Entities.Size + ": " + GridRepository.Instance.GetTileAt(i, j).Entities.ToString());
                }
            }

            //TEMPORARY: Add the tracking record for this entity to all tiles within the buffer
            foreach (Tile tile in currentTiles) {
                tile.Entities.AddUnique(this);
                //Debug.Log("Adding to " + tile.GridCoordinates + " for size of " + tile.Entities.Size + ": " + tile.Entities.ToString());
            }

            //TEMPORARY: Store the tiles tracking this entity as an instantiation of DynamicMatrix
            trackingTiles = new DynamicMatrix<Tile>(currentTiles);
        }

        //Record the coordinates for which this tracking information will remain valid
        previousTrackingLocation = coordinates;
    }
}
