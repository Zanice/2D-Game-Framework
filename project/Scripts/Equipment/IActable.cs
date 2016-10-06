using UnityEngine;
using System.Collections;

public interface IActable {
    /// <summary>
    /// Actable-specific procedure for determining entities hit by the action.
    /// </summary>
    /// <returns>The list of entities hit by the action</returns>
    LinkedList<Entity> DetermineEntitiesHit();

    /// <summary>
    /// Actable-specific procedure that processes entities hit by the action.
    /// </summary>
    /// <param name="entitiesHit">The list of entities hit by the action.</param>
    void OnEntitiesHit(LinkedList<Entity> entitiesHit);
}
