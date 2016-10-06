using UnityEngine;
using System;
using System.Collections.Generic;

public class PrefabRepository : MonoBehaviour {
    //Editor-viewable prefab references
    public GameObject Prefab_Player;
    public GameObject Prefab_Tile_Floor;
    public GameObject Prefab_Tile_Wall;
    public GameObject Prefab_Tile_Rubble;
    public GameObject Prefab_Tazer_Bolt;
    public GameObject Prefab_Bystander;

    //Dictionary variables
    private Dictionary<TileType.TileType_Code, GameObject> tileDictionary;

    /// <summary>
    /// Creates dictionaries from the editor-viewable prefab references. These dictionaries are used by the other 
    /// methods of the PrefabRepository.
    /// </summary>
    public void InitializeDictionaries() {
        tileDictionary = new Dictionary<TileType.TileType_Code, GameObject>() {
            { TileType.TileType_Code.EMPTY, null },
            { TileType.TileType_Code.FLOOR, Prefab_Tile_Floor },
            { TileType.TileType_Code.WALL, Prefab_Tile_Wall },
            { TileType.TileType_Code.RUBBLE, Prefab_Tile_Rubble }
        };
    }

    /// <summary>
    /// Returns the default player prefab.
    /// </summary>
    /// <returns>A GameObject reference to the default player prefab.</returns>
    public GameObject PrefabOfPlayer() {
        return Prefab_Player;
    }

    /// <summary>
    /// Returns the default bystander prefab.
    /// </summary>
    /// <returns>A GameObject reference to the default bystander prefab.</returns>
    public GameObject PrefabOfBystander() {
        return Prefab_Bystander;
    }

    /// <summary>
    /// Returns the tazer bolt prefab.
    /// </summary>
    /// <returns>A GameObject reference to the tazer bolt prefab.</returns>
    public GameObject PrefabOfTazerBolt() {
        return Prefab_Tazer_Bolt;
    }

    /// <summary>
    /// Queries the repository's dictionaries for the prefab corresponding to the provided tile type and returns the reference
    /// to this prefab.
    /// </summary>
    /// <param name="tileType">The tile information for which a prefab is being queried.</param>
    /// <returns>A GameObject reference to the tile prefab corresponding to the given type.</returns>
    public GameObject PrefabOf(TileType tileType) {
        GameObject output;
        if (tileDictionary.TryGetValue(tileType.Code, out output))
            return output;
        else
            throw new ArgumentException("Entered key does not exist in dictionary.");
    }
}
