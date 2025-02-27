using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class EditValues : MonoBehaviour
{
    [Range(0, 1)]
    public float ChromaticGlitch;

    [Range(0, 1)]
    public float FrameGlitch;

    [Range(0, 1)]
    public float PixelGlitch;

    void Update()
    {
        if (FastGlitchUrp.Instance == null) return;
        FastGlitchUrp.Instance.settings.ChromaticGlitch = ChromaticGlitch;
        FastGlitchUrp.Instance.settings.FrameGlitch = FrameGlitch;
        FastGlitchUrp.Instance.settings.PixelGlitch = PixelGlitch;
        FastGlitchUrp.Instance.Create();
    }

    public void EffectsOn() {
        ChromaticGlitch = 0.05f;
        FrameGlitch = 0.05f;
        PixelGlitch = 0.2f;
    }
    public void EffectsOff() {
        ChromaticGlitch = 0f;
        FrameGlitch = 0f;
        PixelGlitch = 0f;
    }
    public void EffectsMax() {
        ChromaticGlitch = 1;
        FrameGlitch = 1;
        PixelGlitch = 1;
    }
}
