using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridCell : MonoBehaviour
{
    [Header("Visual Components")]
    [SerializeField] private TextMeshProUGUI textNumber;
    [SerializeField] private Image bkgdImage;
    [SerializeField] private Color bkgdColorSelected;
    [SerializeField] private Color bkgdColorRelated;

    // Variables Needed
    private int currentNumber;
    private int correctNumber;
    private bool emptyCell;
    [HideInInspector] public int rowID;
    [HideInInspector] public int colID;

    private void Start()
    {
        currentNumber = 0;
    }
    public void SetRowColumn(int row, int column)
    {
        rowID = row;
        colID = column;
    }

    public void SetNumber(int number)
    {
        currentNumber = number;
        textNumber.text = "" + number;
        if (number == 0) textNumber.text = "";
    }
    public void CheckNumber()
    {
        // if GridManager Evaluate Cell
        textNumber.color = Color.white;
    }
    public int GetCurrentNumber()
    {
        return currentNumber;
    }
    public int GetCorrectNumber()
    {
        return correctNumber;
    }

    public void SetEmpty()
    {
        emptyCell = true;
        correctNumber = currentNumber;
        SetNumber(0);
    }
    public bool IsOriginallyEmpty()
    {
        return emptyCell;
    }

    public void HighlightCell(bool playerSelected)
    {
        if (playerSelected) bkgdImage.color = bkgdColorSelected;
        else bkgdImage.color = bkgdColorRelated;
    }
    public void UnhighlightCell()
    {
        bkgdImage.color = Color.white;
    }
}