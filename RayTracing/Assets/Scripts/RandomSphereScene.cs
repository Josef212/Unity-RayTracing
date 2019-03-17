using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomSphereScene", menuName = "Scenes/RandomSphereScene")]
public class RandomSphereScene : SphereScene
{
    
    public override List<Sphere> GetSceneSpheres()
    {
        List<Sphere> spheres = new List<Sphere>();

        for (int i = 0; i < m_spheresMax; i++)
        {
            Sphere sphere = new Sphere();

            Vector2 randomPos = Random.insideUnitCircle * m_spherePlacementRadius;
            sphere.radius = m_sphereRadius.x + Random.value * (m_sphereRadius.y - m_sphereRadius.x);
            sphere.position = new Vector3(randomPos.x, sphere.radius, randomPos.y);

            foreach (Sphere other in spheres)
            {
                float minDist = sphere.radius + other.radius;
                if (Vector3.SqrMagnitude(sphere.position - other.position) < minDist * minDist)
                    continue;
            }

            Color color = Random.ColorHSV();
            float chance = Random.value;
            if (chance < 1.0f - (m_emissiveProbability / 100.0f))
            {
                bool metal = chance < m_metallicProbability / 100.0f;
                sphere.albedo = metal ? Vector4.zero : new Vector4(color.r, color.g, color.b);
                sphere.specular = metal ? new Vector4(color.r, color.g, color.b) : new Vector4(0.04f, 0.04f, 0.04f);
                sphere.smoothness = Random.value;
            }
            else
            {
                Color emission = Random.ColorHSV(0, 1, 0, 1, 3.0f, 8.0f);
                sphere.emission = new Vector3(emission.r, emission.g, emission.b);
            }

            spheres.Add(sphere);
        }

        return spheres;
    }

    [Header("RandomSphereScene")]
    [SerializeField] private Vector2 m_sphereRadius = new Vector2(3.0f, 8.0f);
    [SerializeField] private uint m_spheresMax = 100;
    [SerializeField] private float m_spherePlacementRadius = 100.0f;
    [SerializeField] private float m_emissiveProbability = 20.0f;
    [SerializeField] private float m_metallicProbability = 40.0f;
}
