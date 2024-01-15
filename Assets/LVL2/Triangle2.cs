using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class Triangle2 : MonoBehaviour
{
    [SerializeField]
    private Vector3 size = Vector3.one;

    void Start()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshBuilder meshBuilder = new MeshBuilder(1);

        Vector3 p0 = new Vector3(size.x, size.y, -size.z);
        Vector3 p1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 p2 = new Vector3(-size.x, size.y, size.z);

        meshBuilder.BuildTriangle(p0, p1, p2, 0);

        meshFilter.mesh = meshBuilder.CreateMesh();
    }
}
