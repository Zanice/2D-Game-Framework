using UnityEngine;
using System;
using System.Collections;

public class TileObstacleBuffer {
    private Vector2 position;
    private Vector2 buffer;

    public TileObstacleBuffer(Vector2 localPosition, Vector2 buffer) {
        ValidatePosition(localPosition);

        position = localPosition;
        this.buffer = buffer;
    }

    public Vector2 LocalPosition {
        get {
            return position;
        }
        set {
            position = value;
        }
    }

    public Vector2 Buffer {
        get {
            return buffer;
        }
        set {
            ValidatePosition(value);

            buffer = value;
        }
    }

    public bool IsDeadBuffer() {
        return buffer.Equals(Vector2.zero);
    }

    public static TileObstacleBuffer DEAD_BUFFER {
        get {
            return new TileObstacleBuffer(Vector2.zero, Vector2.zero);
        }
    }

    private void ValidatePosition(Vector2 pos) {
        bool validPositionX = 0 <= pos.x && pos.x <= .5f;
        bool validPositionY = 0 <= pos.y && pos.y <= .5f;

        if (!(validPositionX && validPositionY))
            throw new ArgumentException("Invalid position given.");
    }
}
