using UnityEngine;
using UnityEngine.UI;

public class GridElementView : CustomUIComponent
{
    [SerializeField] private GridElementSO _gridElementSO;
    [SerializeField] private Image _background;
    [SerializeField] private Image _tickImage;

    protected override void Setup()
    {
        
    }

    protected override void Configure()
    {
        _background.sprite = _gridElementSO.Background;
        _tickImage.sprite = _gridElementSO.TickImage;
    }
}
