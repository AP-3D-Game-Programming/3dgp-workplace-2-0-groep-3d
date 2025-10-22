using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    public float dayLengthInSeconds = 120f; // hoe lang duurt 1 dag-nacht cyclus
    public Light sunLight;
    public Gradient lightColor; // kan je instellen in de Inspector (kleur verloop van dag naar nacht)
    public AnimationCurve lightIntensity; // curve van intensiteit over tijd (0-1)

    private float cycleProgress = 0f; // waarde tussen 0 en 1

    void Update()
    {
        // Verplaats de tijd vooruit
        cycleProgress += Time.deltaTime / dayLengthInSeconds;
        if (cycleProgress >= 1f) cycleProgress = 0f;

        // Rotatie berekenen (360 graden per cyclus)
        float sunRotation = cycleProgress * 360f;
        transform.rotation = Quaternion.Euler(sunRotation - 90f, 170f, 0f);

        // Kleur en intensiteit aanpassen
        if (sunLight != null)
        {
            sunLight.color = lightColor.Evaluate(cycleProgress);
            sunLight.intensity = lightIntensity.Evaluate(cycleProgress);
        }
    }
}
