using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex3 : MonoBehaviour
{
    float a = 0;
    bool isHold = false;
    public float speed = 10;
    public float r = 0.5f;
    public float R = 1f;

    public Material[] mat;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isHold = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            isHold = false;
        }
        if(isHold)
        {
            a += speed * Time.deltaTime;
            if (a > 360) a = 360;
        }
        else
        {
            a -= speed * Time.deltaTime * 0.5f;
            if (a < 0)
            {
                a = 0;
                GetComponent<MeshRenderer>().material = mat[Random.Range(0, mat.Length)];
            }
        }
        CreateMesh(a);
    }
    void CreateMesh(float a)
    {
        int n_circle = 360;
        int realAngle = (int)a;
        float angle = 2 * Mathf.PI / n_circle;
        Vector3[] verticles = new Vector3[realAngle * 4];
        Vector3[] normal = new Vector3[realAngle * 4];
        int[] triangle = new int[realAngle * 3 * 10];

        for (int i = 0; i < realAngle; i++)
        {
            verticles[i * 4] = new Vector3(Mathf.Cos(angle * i), 0, Mathf.Sin(angle * i)) * R;
            verticles[i * 4 + 1] = new Vector3(Mathf.Cos(angle * i), 0, Mathf.Sin(angle * i)) * r;
            verticles[i * 4 + 2] = new Vector3(Mathf.Cos(angle * i) * R, -0.5f, Mathf.Sin(angle * i) * R);
            verticles[i * 4 + 3] = new Vector3(Mathf.Cos(angle * i) * r, -0.5f, Mathf.Sin(angle * i) * r);
            normal[i * 4] = new Vector3(0, 1, 0);
            normal[i * 4 + 1] = new Vector3(0, 1, 0);
            normal[i * 4 + 2] = new Vector3(0, 1, 0);
            normal[i * 4 + 3] = new Vector3(0, 1, 0);
        }
        for (int i = 0; i < realAngle-1; i++)
        {
            triangle[i * 30] = i * 4;
            triangle[i * 30 + 1] = i * 4 + 1;
            triangle[i * 30 + 2] = i * 4 + 4;
            triangle[i * 30 + 3] = i * 4 + 4;
            triangle[i * 30 + 4] = i * 4 + 1;
            triangle[i * 30 + 5] = i * 4 + 5;

            triangle[i * 30 + 6] = i * 4 + 1;
            triangle[i * 30 + 7] = i * 4 + 3;
            triangle[i * 30 + 8] = i * 4 + 5;
            triangle[i * 30 + 9] = i * 4 + 5;
            triangle[i * 30 + 10] = i * 4 + 3;
            triangle[i * 30 + 11] = i * 4 + 7;

            triangle[i * 30 + 12] = i * 4;
            triangle[i * 30 + 14] = i * 4 + 2;
            triangle[i * 30 + 13] = i * 4 + 4;
            triangle[i * 30 + 15] = i * 4 + 4;
            triangle[i * 30 + 17] = i * 4 + 2;
            triangle[i * 30 + 16] = i * 4 + 6;

            if (i == 0)
            {
                triangle[i * 30 + 18] = i * 4;
                triangle[i * 30 + 19] = i * 4 + 2;
                triangle[i * 30 + 20] = i * 4 + 1;
                triangle[i * 30 + 21] = i * 4 + 1;
                triangle[i * 30 + 22] = i * 4 + 2;
                triangle[i * 30 + 23] = i * 4 + 3;
            }

            if (i == realAngle - 2)
            {
                triangle[i * 30 + 24] = i * 4 + 4;
                triangle[i * 30 + 26] = i * 4 + 6;
                triangle[i * 30 + 25] = i * 4 + 5;
                triangle[i * 30 + 27] = i * 4 + 5;
                triangle[i * 30 + 29] = i * 4 + 6;
                triangle[i * 30 + 28] = i * 4 + 7;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verticles;
        mesh.triangles = triangle;
        mesh.normals = normal;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
