using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridCell))]
public class GridCellInputHandler : MonoBehaviour, IPointerClickHandler
{
    private GridCell gridCell;

    private void Awake()
    {
        gridCell = GetComponent<GridCell>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gridCell.IsOriginallyEmpty()) return;

        GridManager.Instance.UnhighlightCells();
        GridManager.Instance.HighlightCells(gridCell);
    }
}