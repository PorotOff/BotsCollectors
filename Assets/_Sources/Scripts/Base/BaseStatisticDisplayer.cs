using TMPro;
using UnityEngine;

public class BaseStatisticDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourcesCountText;

    public void Display(int resourcesCount)
    {
        _resourcesCountText.text = $"{resourcesCount}";
    }
}