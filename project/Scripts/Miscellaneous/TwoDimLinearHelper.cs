using UnityEngine;
using System.Collections;

public class TwoDimLinearHelper {
    /// <summary>
    /// Returns the hypotenuse length derived from the two lengths provided.
    /// </summary>
    /// <param name="a">The fist length to use.</param>
    /// <param name="b">The second length to use.</param>
    /// <returns>The square root of the first length squared plus the second length squared.</returns>
    public static float PythagoreanLength(float a, float b) {
        float aSquared = a * a;
        float bSquared = b * b;
        float cSquared = aSquared + bSquared;
        return Mathf.Sqrt(cSquared);
    }

    /// <summary>
    /// Returns the length of the side that forms the given hypotenuse length from the known side length.
    /// </summary>
    /// <param name="c">The length of the hypotenuse.</param>
    /// <param name="oneSide">The known side length that contributes to the hypotenuse.</param>
    /// <returns>The unknown side length that contributes to the hypotenuse.</returns>
    public static float PythagoreanComponent(float c, float oneSide) {
        float cSquared = c * c;
        float oneSideSquared = oneSide * oneSide;
        float otherSideSquared = cSquared - oneSideSquared;
        return Mathf.Sqrt(otherSideSquared);
    }

    /// <summary>
    /// Returns the rotated version of the original 2D vector, rotated around the origin by the specified angle in degrees.
    /// A positive angle corresponds to counter-clockwise rotation while a negative angle corresponds to clockwise rotation.
    /// </summary>
    /// <param name="original">The original form of the 2D vector.</param>
    /// <param name="angleInDegrees">The angle to rotate the vector by.</param>
    /// <returns>The original vector rotated around the origin.</returns>
    public static Vector2 RotateVectorByDegrees(Vector2 original, float angleInDegrees) {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(angleInRadians);
        float sinAngle = Mathf.Sin(angleInRadians);

        Vector2 result;
        result.x = (original.x * cosAngle) - (original.y * sinAngle);
        result.y = (original.x * sinAngle) + (original.y * cosAngle);

        return result;
    }

    /// <summary>
    /// Returns the angle in degrees between the two given vectors. The sign of the angle returned corresponds to the relation between the two vectors.
    /// IF the angle is positive, the second vector is on the counter-clockwise side of the first vector.
    /// If the angle is negative, the second vector is on the clockwise side of the first vector.
    /// </summary>
    /// <param name="v1">The first vector; the vector to be compared against.</param>
    /// <param name="v2">The second vector; the vector being compared.</param>
    /// <returns>The angle between the two vectors.</returns>
    public static float AngleInDegreesBetween(Vector2 v1, Vector2 v2) {
        float angleInRadians = Mathf.Atan2(v2.y, v2.x) - Mathf.Atan2(v1.y, v1.x);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        if (angleInDegrees > 180)
            angleInDegrees -= 360;
        else if (angleInDegrees < -180)
            angleInDegrees += 360;

        return angleInDegrees;
    }
}
