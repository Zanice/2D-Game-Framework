using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour, IActable {
    /// <summary>
    /// Constant used to throttle the speed calculation of the projectile.
    /// </summary>
    public readonly float SPEED_CONSTANT = .05f;

    private Entity ownerOfProjectile;
    private Vector3 velocity;
    private bool inFlight;

    /// <summary>
    /// Projectile-specific value for the range at which entity detection occurs. A value of 0f signifies direct contact between projectile
    /// and entity, while a larger value adds non-contact range to the projectile.
    /// </summary>
    public abstract float Range { get; }

    /// <summary>
    /// Projectile-specific value for the speed at which the projectile travels.
    /// </summary>
    public abstract float Speed { get; }

    /// <summary>
    /// MONOBEHAVIOUR Start
    /// </summary>
    void Start () {
        ;
	}

    /// <summary>
    /// MONOBEHAVIOUR Update
    /// </summary>
    void Update () {
        //Run the flight simulation if applicable
	    if (inFlight) {
            Travel();

            //Destroy the projectile if it has escaped the map or is colliding with an obstacle
            if (CheckOutOfBounds() || CheckObstacle()) {
                GameObject.Destroy(gameObject);
            }

            ActDuringFlight();
        }
	}

    /// <summary>
    /// Sets the flight path for the projectile and begins the flight simulation based on the owning Entity and directional information provided.
    /// </summary>
    /// <param name="owner">The Entity instance that owns this projectile.</param>
    /// <param name="direction">The direction in which to launch this projectile.</param>
    public void StartFlight(Entity owner, Vector2 direction) {
        ownerOfProjectile = owner;

        //Calculate the velocity of the projectile
        Vector2 velocityTemp = new Vector2(direction.x, direction.y);
        velocityTemp.Normalize();
        velocityTemp *= Speed * SPEED_CONSTANT;
        velocity = new Vector3(velocityTemp.x, velocityTemp.y, 0);
        
        //Rotate the projectile object so it faces its trajectory
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        inFlight = true;
    }

    /// <summary>
    /// Determines entities hit by the effect and returns the resulting list of entities.
    /// </summary>
    /// <returns>The list of entities hit by the action.</returns>
    public LinkedList<Entity> DetermineEntitiesHit() {
        //Grab the entities within proximity to this effect
        LinkedList<Entity> nearbyEntities = EntityCollisionHelper.AllEntitiesInCircularArea(transform.position, Range);
        return nearbyEntities;
    }

    /// <summary>
    /// Perform the simulated travel of the projectile.
    /// </summary>
    private void Travel() {
        transform.position += velocity;
    }

    /// <summary>
    /// Determines if the projectile has traveled off the map.
    /// </summary>
    /// <returns>True if the projectile is off the map, false if the projectile is not off the map.</returns>
    private bool CheckOutOfBounds() {
        float minX = GridRepository.Instance.GridCornerX;
        float minY = GridRepository.Instance.GridCornerY;
        float maxX = minX + GridRepository.Instance.GridWidth;
        float maxY = minY + GridRepository.Instance.GridHeight;

        bool outOfBoundsX = transform.position.x < minX || maxX < transform.position.x;
        bool outOfBoundsY = transform.position.y < minY || maxY < transform.position.y;

        return outOfBoundsX || outOfBoundsY;
    }

    /// <summary>
    /// Projectile-specific procedure that determines if the projectile has collided with an obstacle that will destroy the projectile.
    /// </summary>
    /// <returns></returns>
    public abstract bool CheckObstacle();

    /// <summary>
    /// Projectile-specific procedure that runs during each flight update.
    /// </summary>
    public abstract void ActDuringFlight();

    /// <summary>
    /// Projectile-specific procedure that processes entities hit by the action.
    /// </summary>
    /// <param name="entitiesHit">The list of entities hit by the action.</param>
    public abstract void OnEntitiesHit(LinkedList<Entity> entitiesHit);
}
