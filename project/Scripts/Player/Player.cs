using UnityEngine;
using System.Collections;
using System;

public class Player : Entity {
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
            return 0.35f;
        }
    }

    /// <summary>
    /// Entity-specific value for the maximum health possible.
    /// </summary>
    public override int MaxHealth {
        get {
            return 100;
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
