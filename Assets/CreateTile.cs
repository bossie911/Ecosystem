using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTile : MonoBehaviour
{
    private enum TileType
    {
        Grass,
        Sand,
        Water
    }

    [SerializeField] private TileType tileType;

    [SerializeField] private Material grassMaterial;
    [SerializeField] private Material sandMaterial;
    [SerializeField] private Material waterMaterial;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    Vector3[] verts;
    int[] tris;

    public void MakeQuad(Vector3 nw, Vector3 ne, Vector3 se, Vector3 sw, float noise)
    {
        Mesh mesh = meshFilter.mesh;
        verts = new Vector3[4];
        tris = new int[6];

        //Setting tiletype
        if (noise < 0.32)
        {
            tileType = TileType.Water;
        }
        else if(noise > 0.365)
        {
            tileType = TileType.Grass;
        }
        else
        {
            tileType = TileType.Sand;
        }

        //Set water tiles lower

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

        //Applying color
        if (tileType == TileType.Grass)
        {
            meshRenderer.material = grassMaterial;
        }
        else if (tileType == TileType.Sand)
        {
            meshRenderer.material = sandMaterial;
        }
        else
        {
            meshRenderer.material = waterMaterial;
        }
        
        //Create the mesh
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
