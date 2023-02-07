using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Water,
        Sand,
        Grass
    }

    Mesh mesh;

    Vector3[] verts;
    int[] tris;

    public void MakeQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, TileType tileType)
    {
        mesh = GetComponent<MeshFilter>().mesh;

        verts = new Vector3[4];
        tris = new int[6];

        verts[0] = v1;
        verts[1] = v2;
        verts[2] = v3;
        verts[3] = v4;

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        tris[3] = 0;
        tris[4] = 2;
        tris[5] = 3;

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
