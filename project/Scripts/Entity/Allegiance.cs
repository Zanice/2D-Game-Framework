using UnityEngine;
using System.Collections;

public class Allegiance {
    /// <summary>
    /// ID value representing the identity of the team.
    /// </summary>
    public enum Allegiance_Id {
        NEUTRAL = 0,
        COP = 1,
        ROBBER = 2
    }

    private Allegiance_Id id;

    public Allegiance(int id) : this((Allegiance_Id) id) {}
    public Allegiance(Allegiance_Id id) {
        this.id = id;
    }

    /// <summary>
    /// The current allegiance ID.
    /// </summary>
    public Allegiance_Id Id {
        get {
            return id;
        }
        set {
            id = value;
        }
    }

    /// <summary>
    /// Returns true if the current allegiance and the given allegiance and partners.
    /// </summary>
    /// <param name="otherAllegiance">The allegiance to compare against.</param>
    /// <returns>True if the current allegiance and other allegiance are partners.</returns>
    public bool IsAlliedWith(Allegiance otherAllegiance) {
        return AreAllied(Id, otherAllegiance.Id);
    }

    /// <summary>
    /// Compares two allegiances and returns true if they are partners.
    /// </summary>
    /// <param name="id1">The first allegiance to compare.</param>
    /// <param name="id2">The second allegiance to compare.</param>
    /// <returns>True if the two given allegiances are partners.</returns>
    public static bool AreAllied(Allegiance_Id id1, Allegiance_Id id2) {
        return id1 == id2;
    }

    public override string ToString() {
        return id.ToString();
    }
}
