using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/ScreenOverlay (Grr)")]
public class ScreenOverlay : MonoBehaviour {

    [SerializeField]
    Shader shader;
    Material mat;

    [SerializeField]
    Color _color = Color.black;
    public Color color {
        get { return _color; }
        set { _color = value; }
    }
    
    void Start() {
        if (!shader)
            Debug.LogError("There is no Shader attached to the Screen Overlay");
    }
    
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (mat == null) {
            mat = new Material(shader);
            mat.hideFlags = HideFlags.DontSave;
        }
        mat.SetColor("_Color", _color);
        Graphics.Blit(source, destination, mat, 0);
    }
}