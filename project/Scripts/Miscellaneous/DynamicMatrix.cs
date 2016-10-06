using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DynamicMatrix<T> : IEnumerable<T> {
    private readonly int rows;
    private readonly int columns;

    public T[,] matrix;
    private int upperCornerRow;
    private int upperCornerColumn;

    public DynamicMatrix(T[,] matrix) {
        rows = matrix.GetLength(0);
        columns = matrix.GetLength(1);

        this.matrix = new T[rows, columns];
        for (int currentRow = 0; currentRow < rows; currentRow++) {
            for (int currentColumn = 0; currentColumn < columns; currentColumn++) {
                this.matrix[currentRow, currentColumn] = matrix[currentRow, currentColumn];
            }
        }

        upperCornerRow = 0;
        upperCornerColumn = 0;
    }

    /// <summary>
    /// Returns the matrix currently held by this object.
    /// </summary>
    /// <returns>The matrix currently held by this object.</returns>
    public T[,] GetMatrix() {
        return CreateResolvedMatrix();
    }

    /// <summary>
    /// Returns the matrix element at the specified row and column. The matrix is treated as row major.
    /// </summary>
    /// <param name="row">The index of the row of the element.</param>
    /// <param name="column">The index of the column of the element.</param>
    /// <returns>The matix element at the specified row and column.</returns>
    public T GetMatrixElement(int row, int column) {
        T[,] resolvedMatrix = CreateResolvedMatrix();
        return resolvedMatrix[row, column];
    }
    
    /// <summary>
    /// Effectively moves the matrix upward by inserting a row at the lowest index and removing the highest index row of data.
    /// </summary>
    /// <param name="row">The data for the new row.</param>
    public void InsertRowAbove(T[] row) {
        ValidateRowLength(row);

        upperCornerRow = DecrementIndexAlongColumn(upperCornerRow);
        AlterRowData(upperCornerRow, upperCornerColumn, row);
    }

    /// <summary>
    /// Effectively moves the matrix downward by inserting a row at the highest index and removing the lowest index row of data.
    /// </summary>
    /// <param name="row">The data for the new row.</param>
    public void InsertRowBelow(T[] row) {
        ValidateRowLength(row);

        AlterRowData(upperCornerRow, upperCornerColumn, row);
        upperCornerRow = IncrementIndexAlongColumn(upperCornerRow);
    }

    /// <summary>
    /// Effectively moves the matrix left by inserting a column at the lowest index and removing the highest index column of data.
    /// </summary>
    /// <param name="column">The data for the new column.</param>
    public void InsertColumnToLeft(T[] column) {
        ValidateColumnLength(column);

        upperCornerColumn = DecrementIndexAlongRow(upperCornerColumn);
        AlterColumnData(upperCornerRow, upperCornerColumn, column);
    }

    /// <summary>
    /// Effectively moves the matrix right by inserting a column at the highest index and removing the lowest index column of data.
    /// </summary>
    /// <param name="column">The data for the new column.</param>
    public void InsertColumnToRight(T[] column) {
        ValidateColumnLength(column);

        AlterColumnData(upperCornerRow, upperCornerColumn, column);
        upperCornerColumn = IncrementIndexAlongRow(upperCornerColumn);
    }

    /// <summary>
    /// Returns the string representation for this object.
    /// </summary>
    /// <returns>The string representation for this object.</returns>
    public override string ToString() {
        T[,] reference = CreateResolvedMatrix();
        string dimensions = string.Format("{0}Rx{1}C", rows, columns);
        string data = MatrixAsString(reference);
        return string.Format("DynMat({0}) Data: {1}", dimensions, data);
    }

    /// <summary>
    /// Returns the string representation of the given matrix. This representation is the same format as DynamicMatrix string representations.
    /// </summary>
    /// <param name="reference">The matrix to get the string representation for.</param>
    /// <returns>The string representation of the matrix.</returns>
    public static string MatrixAsString(T[,] reference) {
        int rows = reference.GetLength(0);
        string[] data = new string[rows];
        for (int currentRow = 0; currentRow < rows; currentRow++)
            data[currentRow] = RowAsString(reference, currentRow);

        string matrixData = string.Join(",\n", data);
        return "{ " + matrixData + " }";
    }

    /// <summary>
    /// Returns the string representation of the given array. This representation is the same format as a DynamicMatrix's string representation for a row.
    /// </summary>
    /// <param name="reference">The array to get the string representation for.</param>
    /// <returns>The string representation of the array.</returns>
    public static string ArrayAsString(T[] reference) {
        string[] data = new string[reference.Length];
        int index = 0;
        foreach (T element in reference)
            data[index++] = element != null ? element.ToString() : "null";
        return string.Join(", ", data);
    }

    #region Private Methods

    /// <summary>
    /// Returns the string represenation for the specified row in the given matrix.
    /// </summary>
    /// <param name="reference">The matrix to use.</param>
    /// <param name="rowIndex">The index of the row to get the string representation for.</param>
    /// <returns>The string representation of the row.</returns>
    private static string RowAsString(T[,] reference, int rowIndex) {
        int columns = reference.GetLength(1);
        string[] data = new string[columns];
        for (int currentColumn = 0; currentColumn < columns; currentColumn++) {
            data[currentColumn] = reference[rowIndex, currentColumn] != null ? reference[rowIndex, currentColumn].ToString() : "null"; 
        }

        string rowData = string.Join(", ", data);
        return "{ " + rowData + " }";
    }

    /// <summary>
    /// Returns the next index along the rows, wrapping if necessary.
    /// </summary>
    /// <param name="index">The current value of the index.</param>
    /// <returns>The next row index.</returns>
    private int IncrementIndexAlongRow(int index) {
        index++;
        return index % columns;
    }

    /// <summary>
    /// Returns the previous index along the rows, wrapping if necessary.
    /// </summary>
    /// <param name="index">The current value of the index.</param>
    /// <returns>The previous row index.</returns>
    private int DecrementIndexAlongRow(int index) {
        index += columns;
        index--;
        return index % columns;
    }

    /// <summary>
    /// Returns the next index along the columns, wrapping if necessary.
    /// </summary>
    /// <param name="index">The current value of the index.</param>
    /// <returns>The next column index.</returns>
    private int IncrementIndexAlongColumn(int index) {
        index++;
        return index % rows;
    }

    /// <summary>
    /// Returns the previous index along the columns, wrapping if necessary.
    /// </summary>
    /// <param name="index">The current value of the index.</param>
    /// <returns>The previous column index.</returns>
    private int DecrementIndexAlongColumn(int index) {
        index += rows;
        index--;
        return index % rows;
    }

    /// <summary>
    /// Creates the resolved (or "true") form of the matrix kept in reference by the DynamicMatrix by unwrapping the matrix in data and representing the data in order.
    /// </summary>
    /// <returns>The resolved form of the current matrix.</returns>
    private T[,] CreateResolvedMatrix() {
        T[,] resolved = new T[rows, columns];
        int currentRow = upperCornerRow;
        int currentColumn;

        for (int rowIndex = 0; rowIndex < rows; rowIndex++) {
            currentColumn = upperCornerColumn;
            for (int columnIndex = 0; columnIndex < columns; columnIndex++) {
                resolved[rowIndex, columnIndex] = matrix[currentRow, currentColumn];
                currentColumn = IncrementIndexAlongRow(currentColumn);
            }
            currentRow = IncrementIndexAlongColumn(currentRow);
        }

        return resolved;
    }

    /// <summary>
    /// Creates an immutable copy of the matrix in data.
    /// </summary>
    /// <returns>A copy of the current matrix.</returns>
    private T[,] CreateMatrixCopy() {
        T[,] copy = new T[rows, columns];

        for (int rowIndex = 0; rowIndex < rows; rowIndex++) {
            for (int columnIndex = 0; columnIndex < columns; columnIndex++)
                copy[rowIndex, columnIndex] = matrix[rowIndex, columnIndex];
        }

        return copy;
    }

    /// <summary>
    /// Ensures the length of the given row matches the number of columns expected. Throws an ArgumentException if this is not the case.
    /// </summary>
    /// <param name="row">The row of data to validate.</param>
    private void ValidateRowLength(T[] row) {
        if (row.Length != columns)
            throw new ArgumentException(string.Format("Row length of {0} does not match expected length of {1}.", row.Length, columns));
    }

    /// <summary>
    /// Ensures the length of the given column matches the number of rows expected. Throws an ArgumentException is this is not the case.
    /// </summary>
    /// <param name="column">The column of data to validate.</param>
    private void ValidateColumnLength(T[] column) {
        if (column.Length != rows)
            throw new ArgumentException(string.Format("Column length of {0} does not match expected length of {1}.", column.Length, rows));
    }

    /// <summary>
    /// Replaces the row at the given row index, starting the replacement at the specified column index.
    /// </summary>
    /// <param name="rowIndex">The (literal to data) index of the row to replace.</param>
    /// <param name="startingColumnIndex">The (literal to data) starting column index for the replacement, or the index of what will be the first index of the row.</param>
    /// <param name="rowData">The row to replace the old data with.</param>
    private void AlterRowData(int rowIndex, int startingColumnIndex, T[] rowData) {
        int currentColumn = startingColumnIndex;
        for (int rowDataIndex = 0; rowDataIndex < rowData.Length; rowDataIndex++) {
            matrix[rowIndex, currentColumn] = rowData[rowDataIndex];
            currentColumn = IncrementIndexAlongRow(currentColumn);
        }
    }

    /// <summary>
    /// Replaces the column at the given column index, starting the replacement at the specified row index.
    /// </summary>
    /// <param name="startingRowIndex">The (literal to data) starting row index for the replacement, or the index of what will be the first index of the column.</param>
    /// <param name="columnIndex">The (literal to data) index of the column to replace.</param>
    /// <param name="columnData">The colump to replace the old data with.</param>
    private void AlterColumnData(int startingRowIndex, int columnIndex, T[] columnData) {
        int currentRow = startingRowIndex;
        for (int columnDataIndex = 0; columnDataIndex < columnData.Length; columnDataIndex++) {
            matrix[currentRow, columnIndex] = columnData[columnDataIndex];
            currentRow = IncrementIndexAlongColumn(currentRow);
        }
    }
    #endregion

    /// <summary>
    /// Defines how this object is iterated over.
    /// </summary>
    /// <returns>The IEnumerator as directed by this object's iterative behavior.</returns>
    public IEnumerator<T> GetEnumerator() {
        T[,] resolvedMatrix = CreateResolvedMatrix();

        foreach (T element in resolvedMatrix)
            yield return element;
    }

    /// <summary>
    /// Returns the object's enumerator for iteration.
    /// </summary>
    /// <returns>The object's enumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
