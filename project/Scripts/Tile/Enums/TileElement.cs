using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileElement {
    public enum TileElement_Code {
        EMPTY = 0,
        PILLAR = 1
    }

    private static readonly Dictionary<TileElement_Code, TileObstacleBuffer> bufferDictionary = new Dictionary<TileElement_Code, TileObstacleBuffer>() {
        { TileElement_Code.EMPTY, TileObstacleBuffer.DEAD_BUFFER },
        { TileElement_Code.PILLAR, new TileObstacleBuffer(Vector2.zero, new Vector2(.2f, .2f)) }
    };

    private TileElement_Code code;
    private TileObstacleBuffer buffer;

    public TileElement(int code) : this((TileElement_Code) code) { }
    public TileElement(TileElement_Code tileCode) {
        Code = tileCode;
    }

    public TileElement_Code Code {
        get {
            return code;
        }
        set {
            code = value;
            buffer = BufferOf(code);
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

    public static TileObstacleBuffer BufferOf(TileElement_Code code) {
        TileObstacleBuffer output;
        if (bufferDictionary.TryGetValue(code, out output))
            return output;
        else
            throw new ArgumentException("Entered key does not exist in dictionary.");
    }
}
