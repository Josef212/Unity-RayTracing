using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracingMaster : MonoBehaviour
{
    private void Update()
    {
        if(transform.hasChanged)
        {
            m_currentSample = 0;
            transform.hasChanged = false;
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
        }
    }

    private void SetShaderParameters()
    {
        RayTracingShader.SetInt("_ReflectionBounces", 8); // TODO: Expose paramter
        RayTracingShader.SetFloat("_SkyBoxFactor", 1.2f); // TODO: Expose paramter

        RayTracingShader.SetInt("_SpheresXCount", 10); // TODO: Expose paramter
        RayTracingShader.SetInt("_SpheresYCount", 10); // TODO: Expose paramter

        RayTracingShader.SetMatrix("_CameraToWorld", m_camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", m_camera.projectionMatrix.inverse);
        RayTracingShader.SetTexture(0, "_SkyboxTexture", m_skyboxTexture);
        RayTracingShader.SetVector("_PixelOffset", new Vector2(Random.value, Random.value));
    }

    [SerializeField] private ComputeShader RayTracingShader = null;
    [SerializeField] private Camera m_camera = null;
    [SerializeField] private Texture m_skyboxTexture = null;

    private RenderTexture m_target = null;

    private uint m_currentSample = 0;
    private Material m_addMaterial = null;
}
