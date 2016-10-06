using UnityEngine;
using System;
using System.Collections;

public class IntegerMatrixFileReader {
    private readonly string directory;
    private readonly string file;
    private readonly int width;
    private readonly int height;

    private string textData;
    private string currentLine;

    /// <summary>
    /// Create a file reader object for the file with the specified directory and name. The file reader object treats the file as defining a matrix
    /// of integers, expecting to load a matrix of a certain width and height and returning errors if these expectations are not met.
    /// </summary>
    /// <param name="fileDirectory">The directory the file is located in.</param>
    /// <param name="fileName">The name of the file to try to load from.</param>
    /// <param name="expectedWidth">The width, or number of columns, expected to read from the file.</param>
    /// <param name="expectedHeight">The height, or number of rows, expected to read from the file.</param>
    public IntegerMatrixFileReader(string fileDirectory, string fileName, int expectedWidth, int expectedHeight) {
        directory = fileDirectory;
        file = fileName;
        width = expectedWidth;
        height = expectedHeight;
    }

    /// <summary>
    /// Reads from the linked file in the linked directory, returning the read integer matrix if successful (in part, reliant on if the
    /// expected width and height of the matrix were met.
    /// </summary>
    /// <returns>The read integer matrix.</returns>
    public int[,] ReadFromFile() {
        //Create the matrix and grab the file data
        int[,] matrixData = new int[width, height];
        TextAsset fileData = (TextAsset) Resources.Load(directory + file);
        textData = fileData.text;

        string idString = null;
        int idInt;

        //Parse the data for each row for the expected number of rows
        for (int yIndex = 0; yIndex < height; yIndex++) {
            try {
                currentLine = NextLine();
            } catch (ArgumentException ae) {
                ThrowExceptionForInvalidRowCount(file, height, ae);
            }

            //Parse the current line for each integer for the expected number of columns
            for (int xIndex = 0; xIndex < width; xIndex++) {
                try {
                    idString = NextIntegerFromCurrentLine();
                } catch (ArgumentException ae) {
                    ThrowExceptionForInvalidRowData(file, yIndex, width, ae);
                }

                idInt = int.Parse(idString);
                matrixData[xIndex, height - 1 - yIndex] = idInt;
            }

            //Make sure the row does not have any more integers
            try {
                NextIntegerFromCurrentLine();
                throw new ArgumentException("Row has too many inputs.");
            } catch (ArgumentException ae) {
                if (ae.Message.Equals("Row has too many inputs."))
                    ThrowExceptionForInvalidRowData(file, yIndex, width, ae);
            }
        }

        return matrixData;
    }

    /// <summary>
    /// Read the data from the next, returning the string representation of the line. Throws an ArgumentException if the line is too short to
    /// parse correctly or if no right-side delimiter exists to end the line parse.
    /// </summary>
    /// <returns>The parsed line.</returns>
    private string NextLine() {
        if (2 > textData.Length)
            throw new ArgumentException("Data is too short to parse.");

        //Find the line based on specified start/end conditions.
        int leftIndex = 0;
        int rightIndex;
        while (IsNotLineLeftBound(leftIndex))
            leftIndex++;
        rightIndex = leftIndex + 1;
        while (IsNotLineRightBound(rightIndex)) {
            rightIndex++;
            if (textData.Length < rightIndex)
                throw new ArgumentException("End of line reached; no right-side delimiter encountered.");
        }

        //Return the line and trim the data accordingly
        string line = textData.Substring(leftIndex + 1, rightIndex - leftIndex - 1);
        textData = textData.Substring(rightIndex + 1);
        return line;
    }

    /// <summary>
    /// Returns true if the given index of 'textData' does not represent the left-side delimiter of a line.
    /// </summary>
    /// <param name="index">The index to check.</param>
    /// <returns>True if the index does not represent the delimiter.</returns>
    private bool IsNotLineLeftBound(int index) {
        return char.Parse(textData.Substring(index, 1)) != '{';
    }

    /// <summary>
    /// Returns true if the given index of 'textData' does not represent the right-side delimiter of a line.
    /// </summary>
    /// <param name="index">The index to check.</param>
    /// <returns>True if the index does not represent the delimiter.</returns>
    private bool IsNotLineRightBound(int index) {
        return char.Parse(textData.Substring(index, 1)) != '}';
    }

    /// <summary>
    /// Parses 'currentLine' for the next integer entry, throwing an ArgumentException if the line is too shart to parse correctly or if
    /// no right-side delimiter exists to end the integer parse.
    /// </summary>
    /// <returns>The parsed integer in string form.</returns>
    private string NextIntegerFromCurrentLine() {
        if (0 == currentLine.Length)
            throw new ArgumentException("Line is too short to parse.");

        //Find the id value based on specified start/end conditions.
        int leftIndex = 0;
        int rightIndex;
        while (IsNotIntegerLeftBound(leftIndex) && IsNotLetter(leftIndex))
            leftIndex++;
        rightIndex = leftIndex + 1;
        while (IsNotIntegerRightBound(rightIndex) && IsNotLetter(rightIndex)) {
            rightIndex++;
            if (currentLine.Length < rightIndex)
                throw new ArgumentException("End of line reached; no right-side delimiter encountered");
        }

        //Return the value and trim the line accordingly.
        string tileId = currentLine.Substring(leftIndex, rightIndex - leftIndex);
        currentLine = currentLine.Substring(rightIndex + 1);
        return tileId;
    }

    /// <summary>
    /// Returns true if the given index of 'currentLine' does not represent the left-side delimiter of an integer entry.
    /// </summary>
    /// <param name="index">The index to check.</param>
    /// <returns>True if the index does not represent the delimiter.</returns>
    private bool IsNotIntegerLeftBound(int index) {
        return (char.Parse(currentLine.Substring(index, 1)) < 48) || (57 < char.Parse(currentLine.Substring(index, 1)));
    }

    /// <summary>
    /// Returns true if the given index of 'currentLine' does not represent the right-side delimiter of an integer entry.
    /// </summary>
    /// <param name="index">The index to check.</param>
    /// <returns>True if the index does not represent the delimiter.</returns>
    private bool IsNotIntegerRightBound(int index) {
        return (48 <= char.Parse(currentLine.Substring(index, 1))) && (char.Parse(currentLine.Substring(index, 1)) <= 57);
    }

    /// <summary>
    /// Returns true if the given index of 'currentLine' does not represent a letter character.
    /// </summary>
    /// <param name="index">The index to check.</param>
    /// <returns>True if the index does not represent a letter character.</returns>
    private bool IsNotLetter(int index) {
        char current = char.Parse(currentLine.Substring(index, 1));
        bool isUpperCaseLetter = (65 <= current) && (current <= 90);
        bool isLowerCaseLetter = (97 <= current) && (current <= 122);

        if (isUpperCaseLetter || isLowerCaseLetter)
            throw new ArgumentException("Invalid input encountered.");
        return true;
    }

    /// <summary>
    /// Throws an ArgumentException representing the event of an invalid number of columns or type of data in a row.
    /// </summary>
    /// <param name="filename">The name of the file.</param>
    /// <param name="row">The row index at which this occurred.</param>
    /// <param name="expectedWidth">The width expected for the matrix.</param>
    /// <param name="ae">The source ArgumentException.</param>
    private void ThrowExceptionForInvalidRowData(string filename, int row, int expectedWidth, ArgumentException ae) {
        string error = string.Format("MAPLOAD<{0}>: Problem @ row index {1} - Row is not expected length of {2} or contains invalid input -> {3} {4}", filename, row, expectedWidth, ae.Message, ae.StackTrace);
        throw new ArgumentException(error);
    }

    /// <summary>
    /// Throws an ArgumentException representing the event of an invalid number of rows in the matrix.
    /// </summary>
    /// <param name="filename">The name of the file.</param>
    /// <param name="expectedHeight">The height expected for the matrix.</param>
    /// <param name="ae">The source ArgumentException.</param>
    private void ThrowExceptionForInvalidRowCount(string filename, int expectedHeight, ArgumentException ae) {
        string error = string.Format("MAPLOAD<{0}>: Row count does not match expected count of {1} -> {2} {3}", filename, expectedHeight, ae.Message, ae.StackTrace);
        throw new ArgumentException(error);
    }

    /// <summary>
    /// Throws an ArgumentException representing the event of an invalid integer parse.
    /// </summary>
    /// <param name="filename">The name of the file.</param>
    /// <param name="currentRow">The row index at which this occurred.</param>
    /// <param name="currentColumn">The height index at which this occurred.</param>
    /// <param name="ae">The source ArgumentException.</param>
    private void ThrowExceptionForInvalidInput(string filename, int currentRow, int currentColumn, ArgumentException ae) {
        string error = string.Format("MAPLOAD<{0}>: Invalid input for tile ({1}, {2}) is not an integer -> {3} {4}", filename, currentRow, currentColumn, ae.Message, ae.StackTrace);
        throw new ArgumentException(error);
    }
}
