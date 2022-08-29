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
		List<Vector2> verticles = new List<Vector2>();
        List<int> id = new List<int>();
        List<Vector2> uv = new List<Vector2>();
		List<Vector3> normal = new List<Vector3>();
		for (int i = 0; i < listPos.Count; i++)
        {
            verticles.Add(new Vector2(listPos[i].x, listPos[i].y));
            id.Add(i);
			uv.Add(new Vector2(listPos[i].x + 0.5f, listPos[i].y + 0.5f));
			normal.Add(new Vector3(0, 0, -1));
        }

        Mesh mesh = new Mesh();
        mesh.vertices = listPos.ToArray();

		List<Triangulator.Triangle> listTriangle = Triangulator.Triangulate(verticles, id);

		List<int> temp = new List<int>();
        for (int i = 0; i < listTriangle.Count; i++)
        {
			temp.Add(listTriangle[i].d);
			temp.Add(listTriangle[i].e);
			temp.Add(listTriangle[i].f);

            temp.Add(listTriangle[i].d);
            temp.Add(listTriangle[i].f);
            temp.Add(listTriangle[i].e);
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

	
}
