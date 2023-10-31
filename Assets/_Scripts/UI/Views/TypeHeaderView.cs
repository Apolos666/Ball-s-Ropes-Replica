using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TypeHeaderView : CustomUIComponent
{
    [FormerlySerializedAs("_typeHeader")] [SerializeField] private TypeHeaderSO _typeHeaderSo;
    private Image _image;
    private TextMeshProUGUI _text;
    
    protected override void Setup()
    {
        _image = gameObject.GetComponentInChildren<Image>();
        _text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void Configure()
    {
        _image.sprite = _typeHeaderSo.Image;
        _text.text = _typeHeaderSo.Text;
    }
}
