using UnityEngine;
using System.Collections;

public abstract class RectangleEffect : IActable {
    private Entity owner;
    private Vector2 position;
    private Vector2 direction;

    /// <summary>
    /// Rectangle-effect-specific value for the width of the rectangle. If the direction is positive-y, this value determines the x-axis distance between
    /// the left and right sides of the rectangle.
    /// </summary>
    public abstract float Width { get; }

    /// <summary>
    /// Rectangle-effect-specific value for the height of the rectangle. If the direction is positive-y, this value determines the y-axis distance between
    /// the top and bottom sides of the rectangle.
    /// </summary>
    public abstract float Height { get; }

    /// <summary>
    /// Value representing the width as the distance between the left/right sides of the rectangle and the center axis of the rectangle.
    /// </summary>
    public float WidthFromCenter {
        get {
            return Width / 2;
        }
    }

    /// <summary>
    /// Value representing the height has the distance between the top/bottom sides of the rectangle and the center axis of the rectangle.
    /// </summary>
    public float HeightFromCenter {
        get {
            return Height / 2;
        }
    }

	public RectangleEffect(Entity owner, Vector2 position) {
        this.owner = owner;
        this.position = position;
    }

    /// <summary>
    /// Determines entities hit by the effect and returns the resulting list of entities.
    /// </summary>
    /// <returns>The list of entities hit by the action.</returns>
    public LinkedList<Entity> DetermineEntitiesHit() {
        //Calculate the range by finding the diagonal of the rectangle
        float range = TwoDimLinearHelper.PythagoreanLength(WidthFromCenter, HeightFromCenter);

        //Grab the entities within proximity to this effect
        LinkedList<Entity> nearbyEntities = EntityCollisionHelper.AllEntitiesInCircularArea(position, range);

        //Check each nearby entity to see if it lies within the effect's cone
        foreach (Entity entity in nearbyEntities) {
            if (!WithinRectangle(entity.transform.position, entity.PhysicalRadius))
                nearbyEntities.Remove(entity);
        }

        return nearbyEntities;
    }

    /// <summary>
    /// Determines whether or not the entity with the specified position and radius is within the rectangle of the effect.
    /// </summary>
    /// <param name="entityPosition">The position of the entity.</param>
    /// <param name="entityRadius">The physical radius of the entity, or the radius used for collision detection.</param>
    /// <returns>True if the entity is within the effect's rectangle, false if the entity is not within the effect's rectangle.</returns>
    private bool WithinRectangle(Vector2 entityPosition, float entityRadius) {
        //Calculate the entity's displacement vector
        Vector2 entityDisplacement = entityPosition - position;

        //CHECK 1: EFFECT CENTER TEST

        //Find the proximity of the entity's edge to the center of the effect
        float entityEdgeDistance = entityDisplacement.magnitude - entityRadius;

        //Assuming a positive radius, return true if the entity covers the center of the effect
        if (entityEdgeDistance <= 0)
            return true;

        //CHECK 2: SMALLEST RECTANGLE TEST

        //Return true if the distance to the entity's edge is smaller than the width and height
        float minDimension = Mathf.Min(Width, Height);
        if (entityEdgeDistance <= minDimension)
            return true;

        //CHECK 3: TANGENT POINT TEST

        //Verify the direction is not the zero vector
        if (direction.magnitude == 0)
            return false;

        //Effectively rotate the entity according to the rectangle's rotation, so that we can perform 
        //simple collision logic with a rectangle along the x/y axes
        float rectangleRotationInDegrees = TwoDimLinearHelper.AngleInDegreesBetween(Vector2.up, direction);
        Vector2 rotatedEntityDisplacement = TwoDimLinearHelper.RotateVectorByDegrees(entityDisplacement, rectangleRotationInDegrees);

        //Create the four tangent points on the edge of the entity
        Vector2[] tangentPoints = new Vector2[] {
            new Vector2(rotatedEntityDisplacement.x + entityRadius, rotatedEntityDisplacement.y),
            new Vector2(rotatedEntityDisplacement.x - entityRadius, rotatedEntityDisplacement.y),
            new Vector2(rotatedEntityDisplacement.x, rotatedEntityDisplacement.y + entityRadius),
            new Vector2(rotatedEntityDisplacement.x, rotatedEntityDisplacement.y - entityRadius)
        };

        //Keep the half-values of the rectangle parameters on reference t avoid recalculations
        float halfWidth = WidthFromCenter;
        float halfHeight = HeightFromCenter;

        //For each tangent point, return true if the point lies inside the rectangle
        bool toRightOfLeftEdge;
        bool toLeftOfRightEdge;
        bool aboveLowerEdge;
        bool belowUpperEdge;
        foreach (Vector2 tangentPoint in tangentPoints) {
            toRightOfLeftEdge = -halfWidth <= tangentPoint.x;
            toLeftOfRightEdge = tangentPoint.x <= halfWidth;
            aboveLowerEdge = -halfHeight <= tangentPoint.y;
            belowUpperEdge = tangentPoint.y <= halfHeight;

            if (toRightOfLeftEdge && toLeftOfRightEdge && aboveLowerEdge && belowUpperEdge)
                return true;
        }

        //CHECK 4: RECTANGLE CORNER TEST

        //Find the corners of the rectangle
        Vector2[] rectangleCorners = new Vector2[] {
            new Vector2(-halfWidth, -halfHeight),
            new Vector2(halfWidth, -halfHeight),
            new Vector2(-halfWidth, halfHeight),
            new Vector2(halfWidth, halfHeight)
        };

        //Since no tangent points are inside the rectangle, return true if any corner is within the entity's radius
        Vector2 rotatedEntityDisplacementFromCorner;
        foreach (Vector2 rectangleCorner in rectangleCorners) {
            rotatedEntityDisplacementFromCorner = rotatedEntityDisplacement - rectangleCorner;
            if (rotatedEntityDisplacementFromCorner.magnitude <= entityRadius)
                return true;
        }

        //END TESTS

        //At this point, all tests have failed and the entity is not overlapping with the rectangle. Return false
        return false;
    }

    /// <summary>
    /// Circle-effect-specific procedure that processes entities hit by the action.
    /// </summary>
    /// <param name="entitiesHit">The list of entities hit by the action.</param>
    public abstract void OnEntitiesHit(LinkedList<Entity> entitiesHit);
}
