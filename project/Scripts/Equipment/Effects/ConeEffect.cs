using UnityEngine;
using System;
using System.Collections;

public abstract class ConeEffect : IActable {
    private Entity owner;
    private Vector3 position;
    private Vector2 direction;

    /// <summary>
    /// Cone-effect-specific value for the range of the cone.
    /// </summary>
    public abstract float Range { get; }

    /// <summary>
    /// Cone-effect-specific value for the angle of the cone in degrees. This angle is measured from one edge of the cone to the other.
    /// </summary>
    public abstract int AngleInDegrees { get; }

	public ConeEffect(Entity owner, Vector3 position, Vector2 direction) {
        this.owner = owner;
        this.position = position;
        SetDirection(direction);
    }

    /// <summary>
    /// Sets the direction of the cone based on the given vector. The new direction value is normalized.
    /// The given vector must not be the zero vector.
    /// </summary>
    /// <param name="directionOfEffect">The new direction of the cone.</param>
    public void SetDirection(Vector2 directionOfEffect) {
        if (directionOfEffect.magnitude == 0)
            throw new ArgumentException();

        direction = directionOfEffect;
        direction.Normalize();
    }

    /// <summary>
    /// Determines entities hit by the effect and returns the resulting list of entities.
    /// </summary>
    /// <returns>The list of entities hit by the action.</returns>
    public LinkedList<Entity> DetermineEntitiesHit() {
        //Grab the entities within proximity to this effect
        LinkedList<Entity> nearbyEntities = EntityCollisionHelper.AllEntitiesInCircularArea(position, Range);

        //Check each nearby entity to see if it lies within the effect's cone
        foreach (Entity entity in nearbyEntities) {
            if (!DirectionallyWithinEffect(entity.transform.position, entity.PhysicalRadius))
                nearbyEntities.Remove(entity);
        }

        return nearbyEntities;
    }

    /// <summary>
    /// Returns whether or not the entity at the provided position with the provided radius is within the cone specified by the effect's
    /// angle, range, and direction.
    /// </summary>
    /// <param name="entityPosition">The position of the entity.</param>
    /// <param name="entityRadius">The physical radius of the entity, or the radius used for collision detection.</param>
    /// <returns>True if the entity is inside the cone, false if the entity is not inside the cone.</returns>
    private bool DirectionallyWithinEffect(Vector3 entityPosition, float entityRadius) {
        int targetAngle = Mathf.Clamp(AngleInDegrees, 0, 180);
        Vector2 positionXY = position;
        Vector2 entityPositionXY = entityPosition;

        //If the effect is a full circle, angles do not need to be calculated since any angle is valid
        if (targetAngle == 0)
            return false;

        //Find the angle the left, right edges of the cone are from the direction vector of the cone
        float halfAngle = (float) targetAngle / 2;

        //Convert the given position information into a displacement, returning true if the entity and the effect have the same position
        Vector2 entityDirection = entityPositionXY - positionXY;
        if (entityDirection.Equals(Vector2.zero))
            return true;

        //CHECK 1: ENTITY CENTER TEST

        //Return true if the center of the entity (entityDirection) lies in between the edges
        float entityAngle = Mathf.Abs(TwoDimLinearHelper.AngleInDegreesBetween(direction, entityDirection.normalized));
        if (entityAngle <= halfAngle)
            return true;

        //CHECK 2: ENTITY EDGE TEST

        //Create the left, right edge vectors for the cone. We know these vectors are already normalized
        Vector2 leftEdge = TwoDimLinearHelper.RotateVectorByDegrees(direction, halfAngle);
        Vector2 rightEdge = TwoDimLinearHelper.RotateVectorByDegrees(direction, -halfAngle);

        //Get the perpendicular vectors (pointing inward) of the edges. We know these vectors are already normalized
        Vector2 perpendicularLeftEdge = new Vector2(leftEdge.y, -leftEdge.x);
        Vector2 perpendicularRightEdge = new Vector2(-rightEdge.y, rightEdge.x);

        //Find the points on the edge of the entity's radius that are closest to the cone's edges
        Vector2 entityLeftEdgeTangent = entityPositionXY + (perpendicularLeftEdge * entityRadius);
        Vector2 entityRightEdgeTangent = entityPositionXY + (perpendicularRightEdge * entityRadius);

        //Use the tangent points to generate displacement vectors
        Vector2 entityLeftTangentDirection = entityLeftEdgeTangent - positionXY;
        Vector2 entityRightTangentDirection = entityRightEdgeTangent - positionXY;

        //Find the angles between the tangent and the tangent point directions
        float entityLeftTangentRelationAngle = TwoDimLinearHelper.AngleInDegreesBetween(leftEdge, entityLeftTangentDirection);
        float entityRightTangentRelationAngle = TwoDimLinearHelper.AngleInDegreesBetween(rightEdge, entityRightTangentDirection);

        //Test the angles of the tangent point directions and return false on failure
        bool entityLeftTangentWithinLeftEdge = entityLeftTangentRelationAngle <= 0;
        bool entityRightTangentWithinRightEdge = entityRightTangentRelationAngle >= 0;
        if (!(entityLeftTangentWithinLeftEdge && entityRightTangentWithinRightEdge))
            return false;

        //Return false if the entity is actually behind the cone and is physically unable to be hit by the cone
        bool leftOutsidePerpendicular = Math.Abs(entityLeftTangentRelationAngle) > 90;
        bool rightOutsidePerpendicular = Math.Abs(entityRightTangentRelationAngle) > 90;
        if (leftOutsidePerpendicular && rightOutsidePerpendicular) {
            if (entityDirection.magnitude > entityRadius)
                return false;
        }

        //END TESTS

        //At this point, all tests have passed. Return true
        return true;
    }

    /// <summary>
    /// Cone-effect-specific procedure that processes entities hit by the action.
    /// </summary>
    /// <param name="entitiesHit">The list of entities hit by the action.</param>
    public abstract void OnEntitiesHit(LinkedList<Entity> entitiesHit);
}
