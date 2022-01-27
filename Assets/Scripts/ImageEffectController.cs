using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffectController : MonoBehaviour
{
    [SerializeField] private Material effectMaterial = null;
    [SerializeField] private float lerpFactor = 1.0f;
    public bool invert = false;
    [SerializeField] private Color color = Color.white;

    private float invertValue = 0.0f;

    public static ImageEffectController instance = null;

    void Start()
    {
        instance = this;
        UpdateColor();
    }

    [ContextMenu("Update Color")]
    public void UpdateColor()
    {
        effectMaterial.SetColor("_Color", color);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        invertValue = Mathf.Lerp(invertValue, invert ? 1.0f : 0.0f, lerpFactor * Time.deltaTime);
        effectMaterial.SetFloat("_Invert", invertValue);

        if(effectMaterial != null)
            Graphics.Blit(src, dst, effectMaterial);
    }
}
