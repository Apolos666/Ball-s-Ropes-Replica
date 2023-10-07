using Apolos.System;
using UnityEngine;
using UnityEngine.UI;

public class ToogleAudio : MonoBehaviour
{
    [SerializeField] private Sprite _unMute;
    [SerializeField] private Sprite _mute;
    [SerializeField] private Image _image;

    private bool _isSoundOn = true;

    public void ToggleAudioManager()
    {
        _isSoundOn = !_isSoundOn;

        if (_isSoundOn)
        {
            _image.sprite = _unMute;
            AudioManager.Instance.ToggleAudio(_isSoundOn);
        }
        else
        {
            _image.sprite = _mute;
            AudioManager.Instance.ToggleAudio(_isSoundOn);
        }
    }
}
