using UnityEngine;
using System;
using System.Collections;

public class MapRepository : Singleton<MapRepository> {
    private string currentFile;
    private string data;
    private string line;
    private int[,] mapData;

    public MapRepository() {
        mapData = new int[,] {};
    }

    /// <summary>
    /// The integer matrix representing the map data currently loaded.
    /// </summary>
    public int[,] MapData {
        get {
            return mapData;
        }
    }

    /// <summary>
    /// Loads the file from the specified 
    /// </summary>
    /// <param name="fileDirectory">The directory the file is located in.</param>
    /// <param name="fileName">The name of the file without the file type tag.</param>
    /// <returns></returns>
    public bool LoadMapData(string fileDirectory, string fileName) {
        int expectedWidth = GridRepository.Instance.GridWidth;
        int expectedHeight = GridRepository.Instance.GridHeight;

        //Skip this file load if the specified file is the currently-loaded file.
        if (fileName.Equals(currentFile))
            return true;

        currentFile = fileName;
        IntegerMatrixFileReader reader = new IntegerMatrixFileReader(fileDirectory, fileName, expectedWidth, expectedHeight);
        mapData = reader.ReadFromFile();
        return true;
    }
}
