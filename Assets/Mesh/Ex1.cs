using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex1 : MonoBehaviour
{
    public MeshFilter square;
    public MeshFilter circle;
    public int n_circle;
    public float radius;
    void Start()
    {
        CreateSquare();
        CreateCircle();
    }

    void CreateSquare()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            new Vector3(-1, -1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, -1, 0) };
        mesh.triangles = new int[] { 0, 2, 1, 0, 1, 3 };
        square.sharedMesh = mesh;
    }

    void CreateCircle()
    {
        float angle = 2 * Mathf.PI / n_circle;
        Vector3[] verticles = new Vector3[n_circle + 1];
        int[] triangle = new int[n_circle * 3];
        verticles[n_circle] = Vector3.zero;
        for (int i = 0; i < n_circle; i++)
        {
            verticles[i] = new Vector3(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0) * radius;
        }
        for (int i = 0; i < n_circle; i++)
        {
            triangle[i * 3] = n_circle;
            triangle[i * 3 + 2] = i;
            if (i != n_circle - 1)
            {
                triangle[i * 3 + 1] = i + 1;
            }
            else
            {
                triangle[i * 3 + 1] = 0;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verticles;
        mesh.triangles = triangle;
        circle.sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        float angle = 2 * Mathf.PI / n_circle;
        for (int i = 0; i < n_circle; i++)
        {
            Gizmos.DrawSphere(new Vector3(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0) * radius, 0.1f);
        }
    }
}
