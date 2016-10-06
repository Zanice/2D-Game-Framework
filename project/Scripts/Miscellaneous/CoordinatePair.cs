using UnityEngine;
using System.Collections;

public class CoordinatePair {
    public int x { get; set; }
    public int y { get; set; }
    
    public CoordinatePair(int coordX, int coordY) {
        x = coordX;
        y = coordY;
    }

    /// <summary>
    /// Returns the hash code for this object.
    /// </summary>
    /// <returns>The hash code for this object.</returns>
    public override int GetHashCode() {
        return base.GetHashCode();
    }

    /// <summary>
    /// Returns true if the two objects have the same values.
    /// </summary>
    /// <param name="obj">The object to compare this object to.</param>
    /// <returns>True if this object and the compared object have the same values, false otherwise.</returns>
    public override bool Equals(object obj) {
        if (obj is CoordinatePair) {
            CoordinatePair other = (CoordinatePair) obj;

            bool sameX = x == other.x;
            bool sameY = y == other.y;

            return sameX && sameY;
        }
        return false;
    }

    /// <summary>
    /// Returns the string representation of this object.
    /// </summary>
    /// <returns>The string representation of this object.</returns>
    public override string ToString() {
        return string.Format("{0}, {1}", x, y);
    }
}
