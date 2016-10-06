using UnityEngine;
using System.Collections;

public sealed class DEVGameManager : MonoBehaviour {
    //Editor-viewable map settings
    public string mapFileDirectory = "Maps/";
    public string mapFileName = "DEV_TEST";
    public int mapWidth = 7;
    public int mapHeight = 7;
    public float mapCornerX = 0;
    public float mapCornerY = 0;

    /// <summary>
    /// MONOBEHAVIOUR Start
    /// </summary>
	void Start () {
        //Get the reference to the used prefab repository and initialize it
        PrefabRepository prefabRepository = GetComponent<PrefabRepository>();
        prefabRepository.InitializeDictionaries();

        //Setup the grid and load the map
        GridRepository gridRepository = GridRepository.Instance;
        gridRepository.LinkPrefabRepository(prefabRepository);
        LoadMap();

        //<TEMPORARY> Spawn a test player
        GameObject playerPrefab = prefabRepository.PrefabOfPlayer();
        GameObject player = GameObject.Instantiate(playerPrefab, new Vector3(1.5f, 1.5f, 0), Quaternion.identity) as GameObject;
        player.GetComponent<PlayerActionController>().LinkPrefabRepository(prefabRepository);

        //<TEMPORARY> Spawn a test player
        GameObject bystanderPrefab = prefabRepository.PrefabOfBystander();
        GameObject bystander = GameObject.Instantiate(bystanderPrefab, new Vector3(1.5f, 2.5f, 0), Quaternion.identity) as GameObject;
    }

    /// <summary>
    /// MONOBEHAVIOUR Update
    /// </summary>
	void Update () {
        ;
	}

    /// <summary>
    /// Use the editor-viewable variables provided for this manager to configure the GridRepository singleton and load the map data.
    /// </summary>
    private void LoadMap() {
        GridRepository gridRepository = GridRepository.Instance;
        gridRepository.SetGridParameters(mapWidth, mapHeight, mapCornerX, mapCornerY);
        gridRepository.CreateGridFromMap(mapFileDirectory, mapFileName);
    }
}
