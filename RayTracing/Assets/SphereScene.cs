using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SphereScene", menuName = "Scenes/SphereScene")]
public class SphereScene : ScriptableObject
{
    public static Color ReferenceGroundAlbedo = new Color(0.8f, 0.8f, 0.8f);
    public static Color ReferenceGroundSpecular = new Color(0.03f, 0.03f, 0.03f);

    public Vector3 GroundAlbedo { get { return new Vector3(m_groundAlbedo.r, m_groundAlbedo.g, m_groundAlbedo.b); } }
    public Vector3 GroundSpecular { get { return new Vector3(m_groundSpecular.r, m_groundSpecular.g, m_groundSpecular.b); } }

    public Texture SkyboxTexture { get { return m_skyboxTexture; } }
    public float SkyboxFactor { get { return m_skyboxFactor; } }

    public bool HasChanged { get; set; } = false;

    [ContextMenu("Reset ground")]
    public void ResetGround()
    {
        m_groundAlbedo = ReferenceGroundAlbedo;
        m_groundSpecular = ReferenceGroundSpecular;
        HasChanged = true;
    }

    public bool IsValid()
    {
        return m_skyboxTexture != null;
    }

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

#if UNITY_EDITOR
    private void OnValidate()
    {
        HasChanged = true;
    }
#endif


    [SerializeField] private Vector2 m_sphereRadius = new Vector2(3.0f, 8.0f);
    [SerializeField] private uint m_spheresMax = 100;
    [SerializeField] private float m_spherePlacementRadius = 100.0f;

    [Header("Ground")]
    [SerializeField] private Color m_groundAlbedo = Color.black;
    [SerializeField] private Color m_groundSpecular = Color.black;

    [Header("Skybox")]
    [SerializeField] private Texture m_skyboxTexture = null;
    [SerializeField] private float m_skyboxFactor = 1.8f;
}
