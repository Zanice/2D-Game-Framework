using UnityEngine;
using System.Collections;
using System;

public class GridRepository : Singleton<GridRepository> {
    //Static z-value of grid position
    private static readonly float POSITION_Z_VALUE = 1f;

    //Prefab repository reference
    private PrefabRepository prefabRepository;

    //Map parameters
    private int gridWidth;
    private int gridHeight;
    private float gridCornerX;
    private float gridCornerY;

    //Loaded and created map data
    private int[,] mapData;
    private Tile[,] grid;

    public GridRepository() {
        gridWidth = 0;
        gridHeight = 0;
        gridCornerX = 0;
        gridCornerY = 0;
    }

    /// <summary>
    /// Keeps the given PrefabRepository on reference, using it whenever a prefabrication is needed.
    /// </summary>
    /// <param name="prefabRepo">The PrefabRepository to use for this grid.</param>
    public void LinkPrefabRepository(PrefabRepository prefabRepo) {
        prefabRepository = prefabRepo;
    }

    /// <summary>
    /// Value representing the number of tiles wide the grid is.
    /// </summary>
    public int GridWidth {
        get {
            return gridWidth;
        }
    }

    /// <summary>
    /// Value representing the number of tiles high the grid is.
    /// </summary>
    public int GridHeight {
        get {
            return gridHeight;
        }
    }

    /// <summary>
    /// The X component of the grid's lower left corner.
    /// </summary>
    public float GridCornerX {
        get {
            return gridCornerX;
        }
    }

    /// <summary>
    /// The Y component of the grid's lower left corner.
    /// </summary>
    public float GridCornerY {
        get {
            return gridCornerY;
        }
    }

    /// <summary>
    /// Alters the grid's parameter values based on the given values.
    /// </summary>
    /// <param name="width">The new width, in tiles, of the grid.</param>
    /// <param name="height">The new height, in tiles, of the grid.</param>
    /// <param name="cornerX">The new X location of the grid's lower left corner.</param>
    /// <param name="cornerY">The new Y location of the grid's lower left corner.</param>
    public void SetGridParameters(int width, int height, float cornerX, float cornerY) {
        gridWidth = width;
        gridHeight = height;
        gridCornerX = cornerX;
        gridCornerY = cornerY;
    }
    
    /// <summary>
    /// Returns the integer matrix representation of the map data currently loaded into the grid.
    /// </summary>
    /// <returns>The integer matrix representation of the map data.</returns>
    public int[,] GetMapData() {
        return MapRepository.Instance.MapData;
    }

    /// <summary>
    /// Destroys the current grid and its game object representations, reinstantiating the grid matrix with the current width, height values.
    /// </summary>
    public void ResetGrid() {
        if (null != grid) {
            for (int row = 0; row < gridWidth; row++) {
                for (int column = 0; column < gridHeight; column++) {
                    if (grid[row, column] != null)
                        grid[row, column].DestroyGameObjectRepresentation();
                }
            }
        }
        grid = new Tile[gridWidth, gridHeight];
    }

    /// <summary>
    /// Attempts to load the map data from the corresponding file information. Upon success, constructs the grid based on the map data.
    /// </summary>
    /// <param name="fileDirectory">The directory the file is located in.</param>
    /// <param name="fileName">The name of the file without the file type tag.</param>
    public void CreateGridFromMap(string fileDirectory, string fileName) {
        MapRepository.Instance.LoadMapData(fileDirectory, fileName);
        int[,] data = MapRepository.Instance.MapData;

        ResetGrid();
        for (int row = 0; row < gridWidth; row++) {
            for (int column = 0; column < gridHeight; column++) {
                grid[row, column] = new Tile(prefabRepository, row, column, data[row, column], 0);
            }
        }
    }

    /// <summary>
    /// Returns the tile at the specified grid coordinate.
    /// </summary>
    /// <param name="x">The grid-based X component of the tile.</param>
    /// <param name="y">The grid-based Y component of the tile.</param>
    /// <returns>The tile at this grid-based coordinate location.</returns>
    public Tile GetTileAt(int x, int y) {
        ValidateXYCoordinates(x, y);

        return grid[x, y];
    }

    /// <summary>
    /// Using the given X, Y values as the lower left corner of the sample area, returns a sample area of the grid as a square of 'width' tiles by 'height' tiles.
    /// This method throws an ArgumentOutOfRangeException if a non-existing tile out of range of the sample is attempted.
    /// </summary>
    /// <param name="x">The X component of the tile acting as the lower left corner of the sample area.</param>
    /// <param name="y">The Y component of the tile acting as the lower left corner of the sample area.</param>
    /// <param name="width">The number of tiles wide the sample will be.</param>
    /// <param name="height">The number of tiles high the sample will be.</param>
    /// <returns>A Tile matrix representing the sampled grid tiles.</returns>
    public Tile[,] GetInBoundsGridSample(int x, int y, int width, int height) {
        ValidateXYCoordinates(x, y);

        bool xInBounds = x + width < gridWidth;
        bool yInBounds = y + height < gridHeight;

        if (!(xInBounds && yInBounds))
            throw new ArgumentOutOfRangeException("Sample with corner at {0}, {1} with width {2} and height {3} extends beyond bounds of grid.");

        return GetUnboundedGridSample(x, y, width, height);
    }

    /// <summary>
    /// Using the given X, Y values as the lower left corner of the sample area, returns a sample area of the grid as a square of 'width' tiles by 'height' tiles.
    /// This method allows sampling of non-existing tiles out of range of the sample; the result is a 'null' value in the returned matrix.
    /// </summary>
    /// <param name="x">The X component of the tile acting as the lower left corner of the sample area.</param>
    /// <param name="y">The Y component of the tile acting as the lower left corner of the sample area.</param>
    /// <param name="width">The number of tiles wide the sample will be.</param>
    /// <param name="height">The number of tiles high the sample will be.</param>
    /// <returns>A Tile matrix representing the sampled grid tiles.</returns>
    public Tile[,] GetUnboundedGridSample(int x, int y, int width, int height) {
        Tile[,] sample = new Tile[width, height];
        for (int xIndex = 0; xIndex < width; xIndex++) {
            for (int yIndex = 0; yIndex < height; yIndex++) {
                try {
                    sample[xIndex, yIndex] = grid[x + xIndex, y + yIndex];
                } catch (IndexOutOfRangeException) {
                    sample[xIndex, yIndex] = null;
                }
            }
        }

        return sample;
    }

    /// <summary>
    /// Samples all tiles within a square area - defined by the given location and range - on the grid, using the tracking lists of the tiles to
    /// find all entities existing within this sampled area.
    /// </summary>
    /// <param name="location">The location of the query.</param>
    /// <param name="range">The range of the query.</param>
    /// <returns>A list - sorted by range from the query location - of all entities within range of the location.</returns>
    public SortedList EntitiesInArea(Vector3 location, float range) {
        Vector3 bottomCorner = new Vector3(location.x - range, location.y - range, location.z);
        Vector3 upperCorner = new Vector3(location.x + range, location.y + range, location.z);

        CoordinatePair bottomCornerCoordinates = GridCoordinatesFromPosition(bottomCorner);
        CoordinatePair upperCornerCoordinates = GridCoordinatesFromPosition(upperCorner);

        int minX = Math.Max(0, bottomCornerCoordinates.x);
        int maxX = Math.Min(gridWidth - 1, upperCornerCoordinates.x);
        int minY = Math.Max(0, bottomCornerCoordinates.y);
        int maxY = Math.Min(gridHeight - 1, upperCornerCoordinates.y);

        SortedList entities = new SortedList();
        Entity[] tileEntityList;
        Entity currentEntity;
        Vector2 relativeVector;
        float distance;
        for (int xIndex = minX; xIndex <= maxX; xIndex++) {
            for (int yIndex = minY; yIndex <= maxY; yIndex++) {
                tileEntityList = grid[xIndex, yIndex].Entities.AsArray();
                for (int listIndex = 0; listIndex < tileEntityList.Length; listIndex++) {
                    currentEntity = tileEntityList[listIndex];
                    if (currentEntity != null && !entities.ContainsValue(currentEntity)) {
                        relativeVector = new Vector2(currentEntity.transform.position.x - location.x, currentEntity.transform.position.y - location.y);
                        distance = relativeVector.magnitude;
                        entities.Add(distance, currentEntity);
                    }
                }
            }
        }

        return entities;
    }

    /// <summary>
    /// Returns a CoordinatePair object representing the integer coordinates on the grid that the given vector resides on.
    /// </summary>
    /// <param name="position">The position in the world, represented as a vector.</param>
    /// <returns>The same position represented as integer coordinates for the grid.</returns>
    public CoordinatePair GridCoordinatesFromPosition(Vector3 position) {
        float realPositionX = (float) position.x - gridCornerX;
        float realPositionY = (float) position.y - gridCornerY;

        if (realPositionX < 0)
            realPositionX -= 1;
        if (realPositionY < 0)
            realPositionY -= 1;

        int gridX = (int) realPositionX;
        int gridY = (int) realPositionY;

        return new CoordinatePair(gridX, gridY);
    }

    /// <summary>
    /// Returns a Vector3 object representing the world representation of a integer coordinate location on the grid.
    /// </summary>
    /// <param name="gridCoordinates">The integer coordinates on the grid.</param>
    /// <returns>The world coordinates of the given grid coordinates.</returns>
    public Vector3 PositionFromGridCoordinates(CoordinatePair gridCoordinates) {
        float positionX = (float) gridCoordinates.x + gridCornerX + .5f;
        float positionY = (float) gridCoordinates.y + gridCornerY + .5f;
        return new Vector3(positionX, positionY, POSITION_Z_VALUE);
    }

    /// <summary>
    /// Ensures the given X, Y coordinates exist on the grid (correspond to a tile) and throws an ArgumentOutOfRangeException if this is not the case.
    /// </summary>
    /// <param name="x">The X component of the coordinates to validate.</param>
    /// <param name="y">The Y component of the coordinates to validate.</param>
    private void ValidateXYCoordinates(int x, int y) {
        bool invalidX = x < 0 || gridWidth <= x;
        bool invalidY = y < 0 || gridHeight <= y;

        if (invalidX || invalidY) {
            string message;
            if (invalidX && invalidY)
                message = string.Format("The given x ({0}) and y ({1}) are out of the bounds of the grid.", x, y);
            else if (invalidX)
                message = string.Format("The given x ({0}) is out of the bounds of the grid.", x);
            else
                message = string.Format("The given y ({0}) is out of the bounds of the grid.", y);

            throw new ArgumentOutOfRangeException(message);
        }
    }
}
