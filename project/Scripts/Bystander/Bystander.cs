using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Behaviour class for a bystander, an AI dummy currently in use for testing.
/// </summary>
public class Bystander : Entity {
    private Allegiance allegiance = new Allegiance(Allegiance.Allegiance_Id.NEUTRAL);

    /// <summary>
    /// Entity-specific value for team ownership.
    /// </summary>
    public override Allegiance Allegiance {
        get {
            return allegiance;
        }
    }

    /// <summary>
    /// Entity-specific value for the radial width of the entity.
    /// </summary>
    public override float PhysicalRadius {
        get {
            return 0.5f;
        }
    }

    /// <summary>
    /// Entity-specific value for the maximum health possible.
    /// </summary>
    public override int MaxHealth {
        get {
            return 1;
        }
    }

    /// <summary>
    /// Entity-specific value for the base movement speed of the entity.
    /// </summary>
    public override int BaseSpeed {
        get {
            return 100;
        }
    }
}
