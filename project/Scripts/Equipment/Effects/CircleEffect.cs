using UnityEngine;
using System;
using System.Collections;

public abstract class CircleEffect : IActable {
    private Entity owner;
    private Vector3 position;

    /// <summary>
    /// Circle-effect-specific value for the range, or radius, of the effect.
    /// </summary>
    public abstract float Range { get; }

    public CircleEffect(Entity owner, Vector3 position) {
        this.owner = owner;
        this.position = position;
    }

    /// <summary>
    /// Determines entities hit by the effect and returns the resulting list of entities.
    /// </summary>
    /// <returns>The list of entities hit by the action.</returns>
    public LinkedList<Entity> DetermineEntitiesHit() {
        //Grab the entities within proximity to this effect
        LinkedList<Entity> nearbyEntities = EntityCollisionHelper.AllEntitiesInCircularArea(position, Range);
        return nearbyEntities;
    }

    /// <summary>
    /// Circle-effect-specific procedure that processes entities hit by the action.
    /// </summary>
    /// <param name="entitiesHit">The list of entities hit by the action.</param>
    public abstract void OnEntitiesHit(LinkedList<Entity> entitiesHit);
}
