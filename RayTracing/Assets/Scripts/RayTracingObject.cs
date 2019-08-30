using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RayTracingObject : MonoBehaviour
{
    public MeshFilter MeshFilter { get; private set; }
    private void Awake()
    {
        MeshFilter = GetComponent<MeshFilter>();
    }

    private void OnEnable()
    {
        RayTracingMaster.RegisterObject(this);   
    }

    private void OnDisable()
    {
        RayTracingMaster.UnregisterObject(this);
    }
}
