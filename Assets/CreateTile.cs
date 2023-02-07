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

    [SerializeField] private Color grassColor1;
    [SerializeField] private Color grassColor2;

    [SerializeField] private Color sandColor1;
    [SerializeField] private Color sandColor2;

    [SerializeField] private Color waterColor1;
    [SerializeField] private Color waterColor2;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    Vector3[] verts;
    int[] tris;

    public void MakeQuad(Vector3 nw, Vector3 ne, Vector3 se, Vector3 sw, float noise)
    {
        Mesh mesh = meshFilter.mesh;
        verts = new Vector3[4];
        tris = new int[6];

        //Rounding noise
        noise = (float)Math.Round(noise, 1);
        print(noise);

        //Setting tiletype
        if (noise < 0.35)
        {
            tileType = TileType.Water;
        }
        else if(noise > 0.45)
        {
            tileType = TileType.Grass;
        }
        else
        {
            tileType = TileType.Sand;
        }

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
            meshRenderer.material.color = Color.Lerp(grassColor1, grassColor2, noise);
        }
        else if (tileType == TileType.Sand)
        {
            meshRenderer.material.color = Color.Lerp(sandColor1, sandColor2, noise);
        }
        else
        {
            meshRenderer.material.color = Color.Lerp(waterColor1, waterColor2, noise);
        }
        

        //Create the mesh
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
