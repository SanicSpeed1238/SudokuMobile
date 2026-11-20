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
    private bool emptyCell;
    [HideInInspector] public int rowID;
    [HideInInspector] public int colID;

    public void SetRowColumn(int row, int column)
    {
        rowID = row;
        colID = column;
    }

    public void SetNumber(int number)
    {
        currentNumber = number;
        GridManager.Instance.EvaluateCell(this);

        textNumber.text = "";
        if (number != 0) textNumber.text += number;
    }
    public int GetCurrentNumber()
    {
        return currentNumber;
    }

    public void SetEmpty()
    {
        emptyCell = true;
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

    public void ColorText(Color color)
    {
        textNumber.color = color;
    }
}