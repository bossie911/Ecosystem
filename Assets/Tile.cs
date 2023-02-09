using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Grass,
        Sand,
        Water
    }

    [SerializeField] public TileType tileType;



    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] public Vector3[] verts;
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
        if(tileType == TileType.Water)
        {
            Vector3 offset = new Vector3(0, WorldGenerationData.Instance.waterTileOffset, 0);
            nw -= offset;
            ne -= offset;
            se -= offset;
            sw -= offset;
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
            meshRenderer.material = WorldGenerationData.Instance.grassMaterial;
        }
        else if (tileType == TileType.Sand)
        {
            meshRenderer.material = WorldGenerationData.Instance.sandMaterial;
        }
        else
        {
            meshRenderer.material = WorldGenerationData.Instance.waterMaterial;
        }
        
        //Create the mesh
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}
