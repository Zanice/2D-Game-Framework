using UnityEngine;
using System.Collections;

public class Tile {
    private PrefabRepository _prefabRepository;

    //World variables
    private CoordinatePair gridCoordinates;
    private Vector3 position;
    private GameObject objRepresentation;
    //Tile information variables
    private TileType type;
    private TileElement element;
    private TileObstacleBuffer buffer;
    //Entity variables
    private LinkedList<Entity> entities;

    public Tile(PrefabRepository prefabRepo, int xCoord, int yCoord, int tileCode, int elementCode) {
        _prefabRepository = prefabRepo;

        //Determine tile information and buffer in use
        type = new TileType(tileCode);
        element = new TileElement(elementCode);
        if (!type.Buffer.IsDeadBuffer())
            buffer = type.Buffer;
        else
            buffer = element.Buffer;
        entities = new LinkedList<Entity>();

        //Create world representation of tile
        gridCoordinates = new CoordinatePair(xCoord, yCoord);
        position = GridRepository.Instance.PositionFromGridCoordinates(gridCoordinates);
        CreateGameObjectRepresentation();
    }

    public CoordinatePair GridCoordinates {
        get {
            return gridCoordinates;
        }
    }

    public Vector3 WorldPosition {
        get {
            return position;
        }
    }

    public TileType Type {
        get {
            return type;
        }
    }

    public TileElement Element {
        get {
            return element;
        }
    }

    public Vector3 BufferWorldPosition {
        get {
            return new Vector3(position.x - buffer.LocalPosition.x, position.y - buffer.LocalPosition.y, position.z);
        }
    }

    public TileObstacleBuffer Buffer {
        get {
            return buffer;
        }
    }

    public LinkedList<Entity> Entities {
        get {
            return entities;
        }
    }

    public void CreateGameObjectRepresentation() {
        DestroyGameObjectRepresentation();
        GameObject prefab = _prefabRepository.PrefabOf(type);
        if (null != prefab)
            objRepresentation = GameObject.Instantiate(prefab, position, Quaternion.identity) as GameObject;
    }

    public void DestroyGameObjectRepresentation() {
        if (null != objRepresentation) {
            GameObject.Destroy(objRepresentation);
            objRepresentation = null;
        }
    }

    public override bool Equals(object obj) {
        if (obj is Tile)
            return ((Tile) obj).WorldPosition.Equals(position);
        return false;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public override string ToString() {
        return type.ToString();
    }
}
