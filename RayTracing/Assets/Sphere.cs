﻿using UnityEngine;

public struct Sphere
{
    public Vector3 position;
    public float radius;
    public Vector3 albedo;
    public Vector3 specular;

    public static int SizeOf { get { return System.Runtime.InteropServices.Marshal.SizeOf(typeof(Sphere)); } }
};