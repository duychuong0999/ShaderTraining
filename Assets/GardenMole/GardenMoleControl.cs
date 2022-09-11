using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenMoleControl : MonoBehaviour
{
    public Texture2D image;
    public Mesh mesh;
    public float radius;
    public int leng;

    private void Start()
    {
        Mesh temp = new Mesh();
        List<Vector3> verticle = new List<Vector3>();
        List<int> triangle = new List<int>();
        List<Color> color = new List<Color>();
        List<Vector2> newPoint = new List<Vector2>();
        for (int i = 0; i < leng; i++)
        {
            for (int j = 0; j < leng; j++) 
            {
                for (int k = 0; k < mesh.vertices.Length; k++)
                {
                    verticle.Add(mesh.vertices[k] + new Vector3(i * radius, 0, j * radius));
                    color.Add(image.GetPixel(i * 4, j * 4));
                    newPoint.Add(new Vector2(Random.Range(-0.04f, 0.04f), 0));
                }
                for (int k = 0; k < mesh.triangles.Length; k++)
                {
                    triangle.Add(mesh.triangles[k] + (i * leng + j) * mesh.vertices.Length);
                }
            }
        }
        temp.vertices = verticle.ToArray();
        temp.triangles = triangle.ToArray();
        temp.colors = color.ToArray();
        temp.uv = newPoint.ToArray();
        GetComponent<MeshFilter>().mesh = temp;
    }
}
