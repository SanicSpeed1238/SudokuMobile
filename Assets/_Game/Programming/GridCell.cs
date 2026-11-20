using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridCell : MonoBehaviour
{
    [Header("Visual Components")]
    [SerializeField] private TextMeshProUGUI textNumber;
    [SerializeField] private Image bkgdImage;

    [Header("Color Communication")]
    [SerializeField] private Color bkgdColorSelected;
    [SerializeField] private Color bkgdColorRelated;
    [SerializeField] private Color textColorCorrect;
    [SerializeField] private Color textColorIncorrect;

    // Variables Needed
    private int currentNumber;
    private bool emptyCell;
    [HideInInspector] public int rowID;
    [HideInInspector] public int colID;

    #region Initialization Functions
    public void SetRowColumn(int row, int column)
    {
        rowID = row;
        colID = column;
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
    #endregion

    #region Number Get/Set
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
    #endregion

    #region Visual Coloring
    public void HighlightCell(bool playerSelected)
    {
        if (playerSelected) bkgdImage.color = bkgdColorSelected;
        else bkgdImage.color = bkgdColorRelated;
    }
    public void UnhighlightCell()
    {
        bkgdImage.color = Color.white;
    }
    public void UpdateTextColor(bool correct)
    {
        if (!IsOriginallyEmpty())
        {
            textNumber.color = Color.white;
            return;
        }
        textNumber.color = (correct ? textColorCorrect : textColorIncorrect);
    }
    #endregion
}