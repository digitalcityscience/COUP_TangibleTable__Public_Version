using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using UnityEngine;

namespace TriangleNet
{
	public class MeshDrawer : MonoBehaviour
	{


		public void GenerateMesh(List<Vector2> vertexList)
		{

			Polygon poly = new Polygon();
			poly.Add(vertexList);

			var triangleNetMesh = (TriangleNetMesh)poly.Triangulate();

			GameObject go = new GameObject("Generated mesh");
			var mf = go.AddComponent<MeshFilter>();
			var mesh = triangleNetMesh.GenerateUnityMesh();
			mesh.uv = GenerateUv(mesh.vertices);
			mf.mesh = mesh;
			var mr = go.AddComponent<MeshRenderer>();
			//mr.sharedMaterial = _MeshMaterial;

			//var collider = go.AddComponent<PolygonCollider2D>();
			//collider.points = _Contour.ToArray();

			//var rb = go.AddComponent<Rigidbody2D>();
			//rb.mass = triangleNetMesh.Triangles.Sum(tris => tris.Area);

		}

		private Vector2[] GenerateUv(Vector3[] vertices)
		{
			Vector2[] uvs = new Vector2[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				uvs[i] = new Vector2(vertices[i].x * 1, vertices[i].y * 1);
			}

			return uvs;
		}

	}	
}