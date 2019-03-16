using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracingMaster : MonoBehaviour
{
    private void OnEnable()
    {
        m_currentSample = 0;
        SetUpScene();
    }

    private void OnDisable()
    {
        if (m_spheresBuffer != null) m_spheresBuffer.Release();
    }

    private void Update()
    {
        if(transform.hasChanged || m_directionalLight.transform.hasChanged)
        {
            m_currentSample = 0;
            transform.hasChanged = m_directionalLight.transform.hasChanged = false;
        }

        if(m_sphereScene.HasChanged)
        {
            if (!m_sphereScene.IsValid())
            {
                Debug.LogError("Scene is no longer valid!");
                gameObject.SetActive(false);
                return;
            }
            
            m_currentSample = 0;
            m_sphereScene.HasChanged = false;
        }
        
        if(m_camera.fieldOfView != m_lastFOV)
        {
            m_currentSample = 0;
            m_lastFOV = m_camera.fieldOfView;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SetUpScene();
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetShaderParameters();
        Render(destination);
    }

    private void Render(RenderTexture destination)
    {
        InitRenderTexture();

        RayTracingShader.SetTexture(0, "Result", m_target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        if(m_addMaterial == null)
        {
            m_addMaterial = new Material(Shader.Find("Hidden/AddShader"));
        }

        m_addMaterial.SetFloat("_Sample", m_currentSample);

        Graphics.Blit(m_target, destination, m_addMaterial);

        m_currentSample++;
    }

    private void InitRenderTexture()
    {
        if(m_target == null || m_target.width != Screen.width || m_target.height != Screen.height)
        {
            if(m_target != null)
            {
                m_target.Release();
            }

            m_target = new RenderTexture(Screen.width, Screen.height, 0, 
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            m_target.enableRandomWrite = true;
            m_target.Create();

            m_currentSample = 0;
        }
    }

    private void SetShaderParameters()
    {
        RayTracingShader.SetTexture(0, "_SkyboxTexture", m_sphereScene.SkyboxTexture);
        RayTracingShader.SetFloat("_SkyBoxFactor", m_sphereScene.SkyboxFactor);
        RayTracingShader.SetInt("_ReflectionBounces", m_rayMaxBounces);

        RayTracingShader.SetVector("_GroundAlbedo", m_sphereScene.GroundAlbedo);
        RayTracingShader.SetVector("_GroundSpecular", m_sphereScene.GroundSpecular);

        RayTracingShader.SetMatrix("_CameraToWorld", m_camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", m_camera.projectionMatrix.inverse);
        RayTracingShader.SetVector("_PixelOffset", new Vector2(Random.value, Random.value));

        Vector3 l = m_directionalLight.transform.forward;
        RayTracingShader.SetVector("_DirectionalLight", new Vector4(l.x, l.y, l.z, m_directionalLight.intensity));

        if(m_spheresBuffer != null)
        {
            RayTracingShader.SetBuffer(0, "_Spheres", m_spheresBuffer);
        }
    }

    private void SetUpScene()
    {
        if(m_sphereScene == null || !m_sphereScene.IsValid())
        {
            Debug.LogError("No scene assigned or it's not valid!");
            gameObject.SetActive(false);
            return;
        }

        if(m_spheresBuffer != null)
        {
            m_spheresBuffer.Release();
        }

        List<Sphere> spheres = m_sphereScene.GetRandomSphereScene();

        m_spheresBuffer = new ComputeBuffer(spheres.Count, Sphere.SizeOf);
        m_spheresBuffer.SetData(spheres);
        m_currentSample = 0;
    }

    [SerializeField] private int m_rayMaxBounces = 8;
    [SerializeField] private ComputeShader RayTracingShader = null;
    [SerializeField] private Camera m_camera = null;
    [SerializeField] private Light m_directionalLight = null;

    [SerializeField] private SphereScene m_sphereScene = null;

    private RenderTexture m_target = null;

    private uint m_currentSample = 0;
    private float m_lastFOV = 0.0f;
    private Material m_addMaterial = null;

    private ComputeBuffer m_spheresBuffer = null;
}
