using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Ex2 : MonoBehaviour
{
    List<Vector3> listPos = new List<Vector3>();
    public Camera cam;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
				if (listPos.Count == 0 || Vector3.Distance(listPos[0], hit.point) > 0.1f)
				{
					listPos.Add(hit.point);
				}
                if (listPos.Count > 1 && Vector3.Distance(listPos[0], hit.point) < 0.1f)
                {
                    Release();
                }
            }
        }
    }
    void Release()
    {
		List<Vector3> verticles = new List<Vector3>();
		List<Vector2> uv = new List<Vector2>();
		List<Vector3> normal = new List<Vector3>();
		for (int i = 0; i < listPos.Count; i++)
        {
            verticles.Add(listPos[i]);
			uv.Add(new Vector2(listPos[i].x + 0.5f, listPos[i].y + 0.5f));
			normal.Add(new Vector3(0, 0, -1));
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verticles.ToArray();



		List<Triangle> listTriangle = TriangulateConcavePolygon(verticles);
		List<int> temp = new List<int>();
        for (int i = 0; i < listTriangle.Count; i++)
        {
			temp.Add(listTriangle[i].v1.id);
			temp.Add(listTriangle[i].v2.id);
			temp.Add(listTriangle[i].v3.id);
		}
		mesh.triangles = temp.ToArray();
		mesh.uv = uv.ToArray();
		mesh.normals = normal.ToArray();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < listPos.Count; i++)
        {
            Gizmos.DrawSphere(listPos[i], 0.05f);
        }
    }

	public static List<Triangle> TriangulateConcavePolygon(List<Vector3> points)
	{
		//The list with triangles the method returns
		List<Triangle> triangles = new List<Triangle>();

		//If we just have three points, then we dont have to do all calculations
		if (points.Count == 3)
		{
			triangles.Add(new Triangle(points[0], points[1], points[2], 0, 1, 2));

			return triangles;
		}



		//Step 1. Store the vertices in a list and we also need to know the next and prev vertex
		List<Vertex> vertices = new List<Vertex>();

		for (int i = 0; i < points.Count; i++)
		{
			vertices.Add(new Vertex(points[i], i));
		}

		//Find the next and previous vertex
		for (int i = 0; i < vertices.Count; i++)
		{
			int nextPos = MathUtility.ClampListIndex(i + 1, vertices.Count);

			int prevPos = MathUtility.ClampListIndex(i - 1, vertices.Count);

			vertices[i].prevVertex = vertices[prevPos];

			vertices[i].nextVertex = vertices[nextPos];
		}



		//Step 2. Find the reflex (concave) and convex vertices, and ear vertices
		for (int i = 0; i < vertices.Count; i++)
		{
			CheckIfReflexOrConvex(vertices[i]);
		}

		//Have to find the ears after we have found if the vertex is reflex or convex
		List<Vertex> earVertices = new List<Vertex>();

		for (int i = 0; i < vertices.Count; i++)
		{
			IsVertexEar(vertices[i], vertices, earVertices);
		}


		//Step 3. Triangulate!
		while (true)
		{
			//This means we have just one triangle left
			if (vertices.Count == 3)
			{
				//The final triangle
				triangles.Add(new Triangle(vertices[0], vertices[0].prevVertex, vertices[0].nextVertex));
				break;
			}

			//Make a triangle of the first ear
			Vertex earVertex = earVertices[0];

			Vertex earVertexPrev = earVertex.prevVertex;
			Vertex earVertexNext = earVertex.nextVertex;

			Triangle newTriangle = new Triangle(earVertex, earVertexPrev, earVertexNext);

			triangles.Add(newTriangle);

			//Remove the vertex from the lists
			earVertices.Remove(earVertex);

			vertices.Remove(earVertex);

			//Update the previous vertex and next vertex
			earVertexPrev.nextVertex = earVertexNext;
			earVertexNext.prevVertex = earVertexPrev;

			//...see if we have found a new ear by investigating the two vertices that was part of the ear
			CheckIfReflexOrConvex(earVertexPrev);
			CheckIfReflexOrConvex(earVertexNext);

			earVertices.Remove(earVertexPrev);
			earVertices.Remove(earVertexNext);

			IsVertexEar(earVertexPrev, vertices, earVertices);
			IsVertexEar(earVertexNext, vertices, earVertices);
		}


		return triangles;
	}



	//Check if a vertex if reflex or convex, and add to appropriate list
	private static void CheckIfReflexOrConvex(Vertex v)
	{
		v.isReflex = false;
		v.isConvex = false;

		//This is a reflex vertex if its triangle is oriented clockwise
		Vector2 a = v.prevVertex.GetPos2D_XZ();
		Vector2 b = v.GetPos2D_XZ();
		Vector2 c = v.nextVertex.GetPos2D_XZ();

		if (MathUtility.IsTriangleOrientedClockwise(a, b, c))
		{
			v.isReflex = true;
		}
		else
		{
			v.isConvex = true;
		}
	}



	//Check if a vertex is an ear
	private static void IsVertexEar(Vertex v, List<Vertex> vertices, List<Vertex> earVertices)
	{
		//A reflex vertex cant be an ear!
		if (v.isReflex)
		{
			return;
		}

		//This triangle to check point in triangle
		Vector2 a = v.prevVertex.GetPos2D_XZ();
		Vector2 b = v.GetPos2D_XZ();
		Vector2 c = v.nextVertex.GetPos2D_XZ();

		bool hasPointInside = false;

		for (int i = 0; i < vertices.Count; i++)
		{
			//We only need to check if a reflex vertex is inside of the triangle
			if (vertices[i].isReflex)
			{
				Vector2 p = vertices[i].GetPos2D_XZ();

				//This means inside and not on the hull
				if (MathUtility.IsPointInTriangle(a, b, c, p))
				{
					hasPointInside = true;

					break;
				}
			}
		}

		if (!hasPointInside)
		{
			earVertices.Add(v);
		}
	}
}
