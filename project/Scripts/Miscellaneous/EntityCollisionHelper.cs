using UnityEngine;
using System.Collections;

public class EntityCollisionHelper {
    /// <summary>
    /// Returns all entities within range of a specified position in the world. An entity is in range if its edge is within distance of specified range value.
    /// </summary>
    /// <param name="position">The position to survey the circular area around.</param>
    /// <param name="range">The maximum distance from the position an entity must be within for inclusion.</param>
    /// <returns>A LinkedList of entities within the circular area, ordered by increasing distance from the given position.</returns>
    public static LinkedList<Entity> AllEntitiesInCircularArea(Vector3 position, float range) {
        //Grab the entities within the specified area
        SortedList nearbyEntities = GridRepository.Instance.EntitiesInArea(position, range);

        //Set up vectors used for hit determination
        Vector2 entityPosition = new Vector2();
        Vector2 positionXY = position;

        //For each entity, in order of closest to this position, determine if it is within the range of this effect
        LinkedList<Entity> entitiesInArea = new LinkedList<Entity>();
        Entity entity;
        for (int index = 0; index < nearbyEntities.Count; index++) {
            entity = (Entity) nearbyEntities.GetByIndex(index);
            entityPosition.x = entity.transform.position.x;
            entityPosition.y = entity.transform.position.y;

            //If within range, add (in order) to the final list of entities hit
            if ((entityPosition - positionXY).magnitude <= entity.PhysicalRadius + range)
                entitiesInArea.Add(entity);
        }

        return entitiesInArea;
    }
}
