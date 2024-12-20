using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GlitchEffect : MonoBehaviour
{
    public Material glitchMaterial;
    public float glitchInterval = 1.0f;
    public float glitchDuration = 0.1f;

    private float timeToNextGlitch;

    private void Start()
    {
        timeToNextGlitch = Time.time + glitchInterval;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (glitchMaterial != null)
        {
            if (Time.time >= timeToNextGlitch)
            {
                glitchMaterial.SetFloat("_Intensity", 1.0f); // Увеличить интенсивность
                timeToNextGlitch = Time.time + glitchInterval;
                Invoke("DisableGlitch", glitchDuration); // Сбросить эффект после времени
            }
            Graphics.Blit(source, destination, glitchMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void DisableGlitch()
    {
        glitchMaterial.SetFloat("_Intensity", 0.0f); // Отключить глитч
    }
}
