using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGap : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] public Vector3[] verts;
    int[] tris;

    public void MakeQuad(Vector3 nw, Vector3 ne, Vector3 se, Vector3 sw, Tile.TileType type)
    {
        Mesh mesh = meshFilter.mesh;
        verts = new Vector3[4];
        tris = new int[6];

        //Placing vertices
        verts[0] = nw;
        verts[1] = ne;
        verts[2] = se;
        verts[3] = sw;
        //Triangle 1
        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;
        //Triangle 2
        tris[3] = 0;
        tris[4] = 2;
        tris[5] = 3;

        //Setting the mesh
        if(type == Tile.TileType.Sand)
        {
            meshRenderer.material = WorldGenerationData.Instance.sandMaterial;
        }
        else
        {
            meshRenderer.material = WorldGenerationData.Instance.grassMaterial;
        }

        //Create the mesh
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
