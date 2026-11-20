using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Creation References")]
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private Transform visualParent;

    [Header("Difficulty Settings")]
    [Range(30, 50)]
    [SerializeField] private int minEmptyCells;
    [Range(30, 50)]
    [SerializeField] private int maxEmptyCells;

    // Variables Needed
    public static GridManager Instance;
    private GridCell[,] cells = new GridCell[9, 9];
    private GridCell currentCell;
    private bool gridInitialized;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        gridInitialized = false;
        GenerateSudokuBoard();
    }

    #region Creating Sudoku Board
    private void GenerateSudokuBoard()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                GameObject cellObject = Instantiate(gridCellPrefab, visualParent);
                GridCell cell = cellObject.GetComponent<GridCell>();
                cell.SetRowColumn(row, col);
                cells[row, col] = cell;
            }
        }
        GenerateNumbers();
        EmptyCells(Random.Range(minEmptyCells, maxEmptyCells+1));
        gridInitialized = true;
    }  
    private bool GenerateNumbers()
    {
        // Find Empty Cell
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (cells[row, col].GetCurrentNumber() == 0)
                {
                    // Create Numbers 1-9
                    // ------------------
                    List<int> numbers = new();
                    for (int i = 1; i <= 9; i++) { numbers.Add(i); }

                    // Shuffle Numbers
                    // ---------------
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        int randomIndex = Random.Range(i, numbers.Count);
                        (numbers[i], numbers[randomIndex]) =
                            (numbers[randomIndex], numbers[i]);
                    }

                    // Try Number
                    // ----------
                    foreach (int number in numbers)
                    {
                        if (ValidateNumber(row, col, number))
                        {
                            cells[row, col].SetNumber(number);

                            // Keep Placement
                            if (GenerateNumbers())
                            {
                                return true;
                            }

                            // Undo Placement
                            cells[row, col].SetNumber(0);
                        }
                    }

                    // No valid number found
                    return false;
                }
            }
        }

        // No empty cells remain
        return true;
    } 
    private void EmptyCells(int amount)
    {
        int removed = 0;
        while (removed < amount)
        {
            int row = Random.Range(0, 9);
            int col = Random.Range(0, 9);
            
            if (cells[row, col].GetCurrentNumber() != 0)
            {
                cells[row, col].SetEmpty();
                removed++;
            }
        }
    }
    #endregion

    #region Getting Cell References
    private List<GridCell> GetCellsInRow(int row)
    {
        List<GridCell> result = new();

        for (int col = 0; col < 9; col++)
        {
            result.Add(cells[row, col]);
        }

        return result;
    }
    private List<GridCell> GetCellsInColumn(int col)
    {
        List<GridCell> result = new();

        for (int row = 0; row < 9; row++)
        {
            result.Add(cells[row, col]);
        }

        return result;
    }
    private List<GridCell> GetCellsInSubgrid(int row, int col)
    {
        List<GridCell> result = new();

        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;

        for (int r = startRow; r < startRow + 3; r++)
        {
            for (int c = startCol; c < startCol + 3; c++)
            {
                result.Add(cells[r, c]);
            }
        }

        return result;
    }
    private HashSet<GridCell> GetAllRelatedCells(GridCell target)
    {
        HashSet<GridCell> result = new();

        int row = target.rowID;
        int col = target.colID;

        foreach (GridCell cell in GetCellsInRow(row))
            result.Add(cell);

        foreach (GridCell cell in GetCellsInColumn(col))
            result.Add(cell);

        foreach (GridCell cell in GetCellsInSubgrid(row, col))
            result.Add(cell);

        result.Remove(target);
        return result;
    }
    #endregion

    #region Number Validation
    private bool ValidateNumber(int row, int col, int number)
    {
        // Check Row
        // ---------
        for (int c = 0; c < 9; c++)
        {
            if (c == col)
                continue;
            if (cells[row, c].GetCurrentNumber() == number)
                return false;
        }

        // Check Column
        // ------------
        for (int r = 0; r < 9; r++)
        {
            if (r == row)
                continue;
            if (cells[r, col].GetCurrentNumber() == number)
                return false;
        }

        // Check 3x3 Subgrid
        // -----------------
        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;
        for (int r = startRow; r < startRow + 3; r++)
        {
            for (int c = startCol; c < startCol + 3; c++)
            {
                if (r == row && c == col)
                    continue;
                if (cells[r, c].GetCurrentNumber() == number)
                    return false;
            }
        }

        // No Duplicates, All Good
        return true;
    }
    public void EvaluateCell(GridCell cell)
    {
        cell.UpdateTextColor(ValidateNumber(cell.rowID, cell.colID, cell.GetCurrentNumber()));

        foreach (GridCell gridCell in GetAllRelatedCells(cell))
        {
            gridCell.UpdateTextColor(ValidateNumber
                (gridCell.rowID, gridCell.colID, gridCell.GetCurrentNumber()));
        }

        EvaluateBoard();
    }
    public void EvaluateBoard()
    {
        if (!gridInitialized) return;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int number = cells[row, col].GetCurrentNumber();

                if (number == 0)
                    return;
                if (!ValidateNumber(row, col, number))
                    return;
            }
        }

        // If all valid, Sudoku is solved
        GameManager.Instance.DisplayVictory();
    }
    #endregion

    #region Player Input
    public void HighlightCells(GridCell cell)
    {
        cell.HighlightCell(true);
        foreach (GridCell gridCell in GetAllRelatedCells(cell))
        {
            gridCell.HighlightCell(false);
        }
        currentCell = cell;
    }
    public void UnhighlightCells()
    {
        if (currentCell == null) return;

        currentCell.UnhighlightCell();
        foreach (GridCell gridCell in GetAllRelatedCells(currentCell))
        {
            gridCell.UnhighlightCell();
        }
        currentCell = null;
    }

    public void FillCell(int number)
    {
        if (currentCell == null) return;
        currentCell.SetNumber(number);
    }
    public void ClearBoard()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (cells[row, col].IsOriginallyEmpty())
                    cells[row, col].SetNumber(0);
            }
        }
    }
    #endregion
}