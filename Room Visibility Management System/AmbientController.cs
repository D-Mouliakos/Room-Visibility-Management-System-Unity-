using System.Collections;
using UnityEngine;


/// <summary>
/// Handles smooth transitions of ambient light intensity and character emission color
/// based on room changes. Designed to be used with RoomManager.
/// </summary>
public class AmbientController : MonoBehaviour
{
    [Header("Ambient Light Settings")]
    [SerializeField] private float ambientTransitionDuration = 2f; // Transition time for ambient light

    [Header("Character Emission Settings")]
    [SerializeField] private Material characterSharedMaterial;     // Assign the character's material
    [SerializeField] private float transitionDuration = 1f;        // Transition time for emission color
    [SerializeField] private float emissionIntensity = 1f;         // Strength of the emission

    private Coroutine ambientTransitionCoroutine;
    private Coroutine emissionTransitionCoroutine;



    // ====================================================
    // Public Methods
    // ====================================================

    /// <summary>
    /// Smoothly transitions the scene's ambient light and reflection intensity.
    /// </summary>
    /// <param name="targetIntensity">Target ambient intensity.</param>
    /// <param name="targetReflection">Target reflection intensity.</param>
    public void SetAmbientIntensitySmooth(float targetIntensity, float targetReflection)
    {
        if (ambientTransitionCoroutine)
            StopCoroutine(ambientTransitionCoroutine);

        ambientTransitionCoroutine = StartCoroutine(
            TransitionAmbientIntensity(targetIntensity, targetReflection)
        );
    }


    /// <summary>
    /// Smoothly transitions the character's emission color.
    /// </summary>
    /// <param name="targetColor">Target emission color.</param>
    /// <param name="instant">If true, applies the color instantly.</param>
    public void SetEmissionColorSmooth(Color targetColor, bool instant)
    {
        if (emissionTransitionCoroutine != null)
            StopCoroutine(emissionTransitionCoroutine);

        emissionTransitionCoroutine = StartCoroutine(
            TransitionEmissionColor(targetColor, instant)
        );
    }

    // ====================================================
    // Private Coroutines
    // ====================================================

    private IEnumerator TransitionAmbientIntensity(float targetIntensity, float targetReflection)
    {
        float startIntensity = RenderSettings.ambientIntensity;
        float startReflection = RenderSettings.reflectionIntensity;
        float timeElapsed = 0f;

        while (timeElapsed < ambientTransitionDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / ambientTransitionDuration;

            RenderSettings.ambientIntensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            RenderSettings.reflectionIntensity = Mathf.Lerp(startReflection, targetReflection, t);

            yield return null;
        }

        RenderSettings.ambientIntensity = targetIntensity;
        RenderSettings.reflectionIntensity = targetReflection;
    }


    private IEnumerator TransitionEmissionColor(Color targetColor, bool instant)
    {
        if (!characterSharedMaterial) yield break;

        characterSharedMaterial.EnableKeyword("_EMISSION");

        Color startColor = characterSharedMaterial.GetColor("_EmissionColor");
        float timeElapsed = 0f;

        if (!instant)
        {
            while (timeElapsed < transitionDuration)
            {
                timeElapsed += Time.deltaTime;
                float t = timeElapsed / transitionDuration;

                Color lerpedColor = Color.Lerp(startColor, targetColor, t);
                characterSharedMaterial.SetColor("_EmissionColor", lerpedColor * emissionIntensity);

                yield return null;
            }
        }

        // Apply final color (ensures exact target)
        characterSharedMaterial.SetColor("_EmissionColor", targetColor * emissionIntensity);
    }

}
