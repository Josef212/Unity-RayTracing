using System.Collections.Generic;
using UnityEngine;

public class SphereScene : ScriptableObject
{
    public static Color ReferenceGroundAlbedo = new Color(0.8f, 0.8f, 0.8f);
    public static Color ReferenceGroundSpecular = new Color(0.03f, 0.03f, 0.03f);
    public static float ReferenceGroundSmoothness = 0.2f;
    public static Color ReferenceGroundEmission = new Color(0.0f, 0.0f, 0.0f);

    public int SceneSeed { get { return m_sceneSeed; } }

    public Vector3 GroundAlbedo { get { return new Vector3(m_groundAlbedo.r, m_groundAlbedo.g, m_groundAlbedo.b); } }
    public Vector3 GroundSpecular { get { return new Vector3(m_groundSpecular.r, m_groundSpecular.g, m_groundSpecular.b); } }
    public float GroundSmoothness { get { return m_groundSmoothness; } }
    public Vector3 GroundEmission { get { return new Vector3(m_groundEmission.r, m_groundEmission.g, m_groundEmission.b); } }

    public Texture SkyboxTexture { get { return m_skyboxTexture; } }
    public float SkyboxFactor { get { return m_skyboxFactor; } }

    public bool HasChanged { get; set; } = false;

    [ContextMenu("Reset ground")]
    public void ResetGround()
    {
        m_groundAlbedo = ReferenceGroundAlbedo;
        m_groundSpecular = ReferenceGroundSpecular;
        m_groundSmoothness = ReferenceGroundSmoothness;
        m_groundEmission = ReferenceGroundEmission;

        HasChanged = true;
    }

    public virtual bool IsValid()
    {
        return m_skyboxTexture != null;
    }

    public virtual List<Sphere> GetSceneSpheres()
    {
        throw new System.NotImplementedException("GetSceneSpheres not implemented yet!");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        HasChanged = true;
    }
#endif


    [SerializeField] private int m_sceneSeed = 0;
    
    [Header("Ground")]
    [SerializeField] private Color m_groundAlbedo = Color.black;
    [SerializeField] private Color m_groundSpecular = Color.black;
    [SerializeField] private float m_groundSmoothness = 0.2f;
    [SerializeField] private Color m_groundEmission = Color.black;

    [Header("Skybox")]
    [SerializeField] private Texture m_skyboxTexture = null;
    [SerializeField] private float m_skyboxFactor = 1.8f;
}
