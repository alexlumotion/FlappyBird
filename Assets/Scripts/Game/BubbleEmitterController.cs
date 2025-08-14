using UnityEngine;
using System.Collections.Generic;

public enum BubbleMode
{
    FromMeshVertices,
    FromBottomToTop,
    PulsedCluster,
    RandomArea,
    EdgeEmission
}

[System.Serializable]
public class BubbleSettings
{
    public float lifetime = 3f;
    public float speedY = 0.5f;
    public Vector2 sizeRange = new Vector2(0.2f, 0.5f);
    public float emissionRate = 5f;
}

[System.Serializable]
public class AdvancedBubbleSettings
{
    [Header("Загальні")]
    public float lifetime = 3f;
    public Vector2 sizeRange = new Vector2(0.2f, 0.5f);
    public float speedY = 0.5f;
    public float emissionRate = 5f;

    [Header("Рух")]
    public bool useVelocityOverLifetime = true;
    public Vector2 velocityYRange = new Vector2(0.4f, 0.6f);
    public Vector2 velocityXRange = new Vector2(0f, 0f);
    public Vector2 velocityZRange = new Vector2(0f, 0f);

    [Header("Шум (Noise)")]
    public bool useNoise = false;
    public float noiseStrength = 0.1f;
    public float noiseFrequency = 0.5f;

    [Header("Burst (для Pulsed)")]
    public bool useBurst = false;
    public int minBurstCount = 3;
    public int maxBurstCount = 7;
    public float burstInterval = 1f;
}

public class BubbleEmitterController : MonoBehaviour
{
    [Header("Режим генерації бульбашок")]
    public BubbleMode mode = BubbleMode.FromBottomToTop;

    [Header("Матеріал спрайта бульбашки")]
    public Material[] bubbleMaterials;

    [Header("Налаштування для FromMeshVertices")]
    public AdvancedBubbleSettings fromMeshVerticesSettings = new AdvancedBubbleSettings();

    [Header("Налаштування для FromBottomToTop")]
    public AdvancedBubbleSettings fromBottomToTopSettings = new AdvancedBubbleSettings();

    [Header("Налаштування для PulsedCluster")]
    public AdvancedBubbleSettings pulsedClusterSettings = new AdvancedBubbleSettings();

    [Header("Налаштування для RandomArea")]
    public AdvancedBubbleSettings randomAreaSettings = new AdvancedBubbleSettings();

    [Header("Налаштування для EdgeEmission")]
    public AdvancedBubbleSettings edgeEmissionSettings = new AdvancedBubbleSettings();

    public int randomIndex = 0;

    public bool start, stop = false;

    public ParticleSystem bubbleSystem;

    private void Start()
    {
        randomIndex = Random.Range(0, bubbleMaterials.Length);

        mode = BubbleMode.FromMeshVertices;

        switch (mode)
        {
            case BubbleMode.FromMeshVertices:
                fromMeshVerticesSettings.lifetime *= 1.3f;
                fromMeshVerticesSettings.emissionRate *= 1.5f;
                CreateFromVertices(fromMeshVerticesSettings);
                break;
            case BubbleMode.FromBottomToTop:
                fromBottomToTopSettings.velocityXRange = Vector2.zero;
                fromBottomToTopSettings.velocityZRange = Vector2.zero;
                CreateFromBoxBelow(fromBottomToTopSettings);
                break;
            case BubbleMode.PulsedCluster:
                pulsedClusterSettings.lifetime *= 1.2f;
                pulsedClusterSettings.minBurstCount += 2;
                pulsedClusterSettings.maxBurstCount += 2;
                CreatePulsed(pulsedClusterSettings);
                break;
            case BubbleMode.RandomArea:
                CreateRandomArea(randomAreaSettings);
                break;
            case BubbleMode.EdgeEmission:
                CreateEdgeEmitter(edgeEmissionSettings);
                break;
        }
    }

    void Update()
    {
        if (start)
        {
            start = false;
            StartEmitting();
        }
        if (stop)
        {
            stop = false;
            StopEmitting();
        }
    }

    void CreateFromVertices(AdvancedBubbleSettings settings)
    {
        GameObject emitter = CreateBaseEmitter("Bubble_FromVertices", settings);

        var shape = emitter.GetComponent<ParticleSystem>().shape;
        shape.shapeType = ParticleSystemShapeType.Mesh;
        shape.mesh = GetComponentInChildren<MeshFilter>().sharedMesh;
        shape.meshShapeType = ParticleSystemMeshShapeType.Vertex;
    }

    void CreateFromBoxBelow(AdvancedBubbleSettings settings)
    {
        GameObject emitter = CreateBaseEmitter("Bubble_FromBottom", settings);
        var shape = emitter.GetComponent<ParticleSystem>().shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(0.5f, 0.1f, 0.5f);
        emitter.transform.localPosition = new Vector3(0, -0.2f, 0);
        shape.rotation = Vector3.zero;

        var velocity = emitter.GetComponent<ParticleSystem>().velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.x = new ParticleSystem.MinMaxCurve(0f, 0f);
        velocity.y = new ParticleSystem.MinMaxCurve(settings.speedY * 0.9f, settings.speedY * 1.1f);
        velocity.z = new ParticleSystem.MinMaxCurve(0f, 0f);
    }

    void CreatePulsed(AdvancedBubbleSettings settings)
    {
        GameObject emitter = CreateBaseEmitter("Bubble_Pulsed", settings);
    }

    void CreateRandomArea(AdvancedBubbleSettings settings)
    {
        GameObject emitter = CreateBaseEmitter("Bubble_RandomArea", settings);

        var shape = emitter.GetComponent<ParticleSystem>().shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;
    }

    void CreateEdgeEmitter(AdvancedBubbleSettings settings)
    {
        GameObject emitter = CreateBaseEmitter("Bubble_EdgeEmitter", settings);

        var shape = emitter.GetComponent<ParticleSystem>().shape;
        shape.shapeType = ParticleSystemShapeType.Mesh;
        shape.mesh = GetComponent<MeshFilter>().sharedMesh;
        shape.meshShapeType = ParticleSystemMeshShapeType.Edge;

        var main = emitter.GetComponent<ParticleSystem>().main;
        main.loop = true; // переконатися, що луп увімкнено
    }

    GameObject CreateBaseEmitter(string name, AdvancedBubbleSettings settings)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;

        var ps = go.AddComponent<ParticleSystem>();
        var renderer = go.GetComponent<ParticleSystemRenderer>();

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        renderer.material = bubbleMaterials[randomIndex];
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        var main = ps.main;
        main.playOnAwake = false;
        main.loop = true;
        main.startLifetime = settings.lifetime;
        main.startSpeed = settings.speedY;
        main.startSize = new ParticleSystem.MinMaxCurve(settings.sizeRange.x, settings.sizeRange.y);
        main.startColor = Color.white;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 1000;

        var emission = ps.emission;
        emission.rateOverTime = settings.emissionRate;

        if (settings.useBurst)
        {
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] {
                new ParticleSystem.Burst(0f, (short)settings.minBurstCount, (short)settings.maxBurstCount, 1, settings.burstInterval)
            });
        }

        if (settings.useVelocityOverLifetime)
        {
            var velocity = ps.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.Local;
            velocity.x = new ParticleSystem.MinMaxCurve(settings.velocityXRange.x, settings.velocityXRange.y);
            velocity.y = new ParticleSystem.MinMaxCurve(settings.velocityYRange.x, settings.velocityYRange.y);
            velocity.z = new ParticleSystem.MinMaxCurve(settings.velocityZRange.x, settings.velocityZRange.y);
        }

        if (settings.useNoise)
        {
            var noise = ps.noise;
            noise.enabled = true;
            noise.strength = settings.noiseStrength;
            noise.frequency = settings.noiseFrequency;
        }

        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0.4f);
        curve.AddKey(1f, 1f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0f) },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(1f, 0.1f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = grad;

        bubbleSystem = ps;

        return go;
    }

    public void StartEmitting()
    {
        if (bubbleSystem != null)
        {
            bubbleSystem.Play();
        }
    }

    public void StopEmitting()
    {
        if (bubbleSystem != null)
        {
            //bubbleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            bubbleSystem.Stop();
        }
    }
}