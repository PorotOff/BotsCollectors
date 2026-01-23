using TMPro;
using UnityEngine;

public class BaseResourcesStatisticDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourcesCountText;

    public void Display(int resourcesCount)
    {
        _resourcesCountText.text = $"{resourcesCount}";
    }
}