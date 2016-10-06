using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileType {
    public enum TileType_Code {
        EMPTY = 0,
        FLOOR = 1,
        WALL = 2,
        RUBBLE = 3
    }

    public enum TileType_Obstacle {
        FLOOR = 0,
        HOLE = 1,
        WALL = 2
    }

    private static readonly Dictionary<TileType_Code, TileType_Obstacle> obstacleDictionary = new Dictionary<TileType_Code, TileType_Obstacle>() {
        { TileType_Code.EMPTY, TileType_Obstacle.HOLE },
        { TileType_Code.FLOOR, TileType_Obstacle.FLOOR },
        { TileType_Code.WALL, TileType_Obstacle.WALL },
        { TileType_Code.RUBBLE, TileType_Obstacle.HOLE }
    };

    private static readonly Dictionary<TileType_Obstacle, TileObstacleBuffer> bufferDictionary = new Dictionary<TileType_Obstacle, TileObstacleBuffer> {
        { TileType_Obstacle.FLOOR, TileObstacleBuffer.DEAD_BUFFER },
        { TileType_Obstacle.HOLE, new TileObstacleBuffer(Vector2.zero, new Vector2(.5f, .5f)) },
        { TileType_Obstacle.WALL, new TileObstacleBuffer(Vector2.zero, new Vector2(.5f, .5f)) }
    };

    private TileType_Code code;
    private TileType_Obstacle obstacle;
    private TileObstacleBuffer buffer;

    public TileType(int code) : this((TileType_Code) code) {}
    public TileType(TileType_Code tileCode) {
        Code = tileCode;
    }

    public TileType_Code Code {
        get {
            return code;
        }
        set {
            code = value;
            obstacle = ObstaclePropertyOf(code);
            buffer = BufferOf(obstacle);
        }
    }

    public TileType_Obstacle Obstacle {
        get {
            return obstacle;
        }
    }

    public TileObstacleBuffer Buffer {
        get {
            return buffer;
        }
    }

    public override string ToString() {
        return code.ToString();
    }

    public static TileType_Obstacle ObstaclePropertyOf(TileType_Code code) {
        TileType_Obstacle output;
        if (obstacleDictionary.TryGetValue(code, out output))
            return output;
        else
            throw new ArgumentException("Entered key does not exist in dictionary.");
    }

    public static TileObstacleBuffer BufferOf(TileType_Obstacle obstacle) {
        TileObstacleBuffer output;
        if (bufferDictionary.TryGetValue(obstacle, out output))
            return output;
        else
            throw new ArgumentException("Entered key does not exist in dictionary.");
    }
}
