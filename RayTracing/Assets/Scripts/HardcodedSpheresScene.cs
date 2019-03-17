using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HardcodedSphereScene", menuName = "Scenes/HardcodedSphereScene")]
public class HardcodedSpheresScene : SphereScene
{
    public override List<Sphere> GetSceneSpheres()
    {
        return m_spheres;
    }

    [SerializeField] private List<Sphere> m_spheres = new List<Sphere>();
}
