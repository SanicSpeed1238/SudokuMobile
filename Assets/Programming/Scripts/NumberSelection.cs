using UnityEngine;

public class NumberSelection : MonoBehaviour
{
    public void UseNumber(int number)
    {
        GridManager.Instance.FillCell(number);
    }
}