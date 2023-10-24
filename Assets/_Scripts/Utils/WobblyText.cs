using System;
using TMPro;
using UnityEngine;

public class WobblyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textComponent;

    private void Update()
    {
        _textComponent.ForceMeshUpdate();
        var textInfo = _textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; ++j)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] =
                    orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
            }

            for (int j = 0; j < textInfo.meshInfo.Length; ++j)
            {
                var meshInfo = textInfo.meshInfo[j];
                meshInfo.mesh.vertices = meshInfo.vertices;
                _textComponent.UpdateGeometry(meshInfo.mesh, j);
            }
        }
    }
}
