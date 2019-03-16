using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SphereScene", menuName = "Scenes/SphereScene")]
public class SphereScene : ScriptableObject
{
    public Vector2 m_sphereRadius = new Vector2(3.0f, 8.0f);
    public uint m_spheresMax = 100;
    public float m_spherePlacementRadius = 100.0f;

    public List<Sphere> GetRandomSphereScene()
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
            bool metal = Random.value < 0.5f;

            sphere.albedo = metal ? Vector3.zero : new Vector3(color.r, color.g, color.b);
            sphere.specular = metal ? new Vector3(color.r, color.g, color.b) : Vector3.one * 0.04f;
            
            spheres.Add(sphere);
        }

        return spheres;
    }
}
