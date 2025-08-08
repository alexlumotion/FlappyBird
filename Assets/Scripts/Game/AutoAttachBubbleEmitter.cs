using UnityEngine;

[RequireComponent(typeof(Transform))]
public class AutoAttachBubbleEmitter : MonoBehaviour
{
    [Header("Бульбашковий матеріал (Particles/Standard Unlit with Fade)")]
    public Material bubbleMaterial;

    [Header("Налаштування емісії")]
    public float emissionRate = 5f;
    public float startSpeed = 0.4f;
    public Vector2 startSize = new Vector2(0.2f, 0.5f);
    public float lifetime = 3f;
    public Vector2 startDelay = new Vector2(0f, 2f);

    private void Start()
    {
        CreateBubbleEmitter();
    }

    void CreateBubbleEmitter()
    {
        GameObject emitterGO = new GameObject("BubbleEmitter");
        emitterGO.transform.SetParent(transform);
        emitterGO.transform.localPosition = Vector3.zero;

        var ps = emitterGO.AddComponent<ParticleSystem>();
        var renderer = emitterGO.GetComponent<ParticleSystemRenderer>();

        // MAIN
        var main = ps.main;
        main.loop = true;
        main.startLifetime = lifetime;
        main.startSpeed = startSpeed;
        main.startSize = new ParticleSystem.MinMaxCurve(startSize.x, startSize.y);
        main.startColor = Color.white;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 1000;
        main.playOnAwake = true;
        main.startDelay = new ParticleSystem.MinMaxCurve(startDelay.x, startDelay.y);

        // EMISSION
        var emission = ps.emission;
        emission.rateOverTime = emissionRate;

        // SHAPE
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(0.3f, 0.1f, 0.3f); // невелика область емісії

        // SIZE OVER LIFETIME
        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0f, 0.3f);
        sizeCurve.AddKey(1f, 1f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        // COLOR OVER LIFETIME (fade out)
        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f) },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0.0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.1f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorOverLifetime.color = grad;

        // VELOCITY OVER LIFETIME
        var velocity = ps.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;

        // Усі осі в режимі "Two Constants"
        velocity.x = new ParticleSystem.MinMaxCurve(0f, 0f);
        velocity.y = new ParticleSystem.MinMaxCurve(0.3f, 0.6f);
        velocity.z = new ParticleSystem.MinMaxCurve(0f, 0f);

        // NOISE
        var noise = ps.noise;
        noise.enabled = true;
        noise.strengthX = 0.1f;
        noise.strengthY = 0.05f;
        noise.strengthZ = 0.1f;
        noise.frequency = 0.2f;
        noise.scrollSpeed = 0.1f;

        // RENDERER
        renderer.material = bubbleMaterial;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.sortingOrder = 1;
        renderer.alignment = ParticleSystemRenderSpace.View;

        ps.Play();
    }
}