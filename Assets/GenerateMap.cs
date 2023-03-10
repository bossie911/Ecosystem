using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenerateMap : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject fillPrefab;

    private int mapSize;

    private int seed = 55;
    private float noiseFrequency = 40;
    private float noiseAmplitude = 2.1f;

    public GameObject[,] tiles;

    // Start is called before the first frame update
    void Start()
    {
        //Get data from singleton
        mapSize = WorldGenerationData.Instance.mapSize;

        //A list to store all the tiles in
        tiles = new GameObject[mapSize, mapSize];

        //Make the tiles
        GenerateWorld();

        //Fill the gaps between water and land
        FillGaps();

        //Static batching 
        StaticBatchingUtility.Combine(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateWorld()
    {
        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                GameObject t = Instantiate(tilePrefab, transform);
                //Adding tile to list
                tiles[x, z] = t;

                //Applying noise
                float xNoiseCord = ((float)x / noiseFrequency) * noiseAmplitude + seed;
                float zNoiseCord = ((float)z / noiseFrequency) * noiseAmplitude + seed;
                float noise = Mathf.PerlinNoise(xNoiseCord, zNoiseCord);

                //Positions for the vertices for each tile
                Vector3 nw = new Vector3(transform.position.x - 0.5f + x, gameObject.transform.transform.position.y, transform.position.z + 0.5f + z);
                Vector3 ne = nw + Vector3.right;
                Vector3 sw = nw - Vector3.forward;
                Vector3 se = sw + Vector3.right;

                //Make the quad
                t.GetComponent<Tile>().MakeQuad(nw, ne, se, sw, noise);
            }
        }
    }

    private void FillGaps()
    {
        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (tiles[x,z].GetComponent<Tile>().tileType == Tile.TileType.Water)
                {
                    //Check if next tile is land
                    //Checking for south tile
                    if (z > 0)
                    {
                        CheckTile(x, z, 0, -1, 1, 0, 2, 3);
                    }
                    //Checking for north tile
                    if (z < mapSize - 1)
                    {
                        CheckTile(x, z, 0, 1, 3, 2, 0, 1);
                    }
                    //Checking for east tile
                    if (x < mapSize - 1)
                    {
                        CheckTile(x, z, 1, 0, 0, 3, 1, 2);
                    }
                    //Checking for west tile
                    if (x > 0)
                    {
                        CheckTile(x, z, -1, 0, 2, 1, 3, 0);
                    }
                }

                //Adding quads to the edge of the map 
                if (z == 0)
                {
                    AddEdgeQuad(x, z, 3, 2);
                }
                //Checking for north tile
                if (z == mapSize - 1)
                {
                    AddEdgeQuad(x, z, 1, 0);
                }
                //Checking for east tile
                if (x == mapSize - 1)
                {
                    AddEdgeQuad(x, z, 2, 1);
                }
                //Checking for west tile
                if (x == 0)
                {
                    AddEdgeQuad(x, z, 0, 3);    
                }
            }
        }
    }

    private void CheckTile(int xCoord, int zCoord, int xCoordOffset, int zCoordOffset, int nw_i, int ne_i, int sw_i, int se_i)
    {
        //Check if next tile is land
        Tile waterTile = tiles[xCoord, zCoord].GetComponent<Tile>();
        Tile nextTile = tiles[xCoord + xCoordOffset, zCoord + zCoordOffset].GetComponent<Tile>();

        if (nextTile.tileType == Tile.TileType.Sand || nextTile.tileType == Tile.TileType.Grass)
        {
            //Make a quad for the gap of water
            GameObject t = Instantiate(fillPrefab, transform);

            //Positions for the vertices for each tile
            Vector3 nw = nextTile.verts[nw_i];
            Vector3 ne = nextTile.verts[ne_i];
            Vector3 sw = waterTile.verts[sw_i];
            Vector3 se = waterTile.verts[se_i];

            //Make the quad
            t.GetComponent<FillGap>().MakeQuad(nw, ne, se, sw, nextTile.tileType);
        }
    }

    private void AddEdgeQuad(int xCoord, int zCoord, int vector_1, int vector_2)
    {
        Tile tile = tiles[xCoord, zCoord].GetComponent<Tile>();
        //Make a quad for the edge of the map
        GameObject t = Instantiate(fillPrefab, transform);

        //Positions for the vertices for each tile
        Vector3 v1 = tile.verts[vector_1];
        Vector3 v2 = tile.verts[vector_2];
        Vector3 v3 = tile.verts[vector_2] - Vector3.up;
        Vector3 v4 = tile.verts[vector_1] - Vector3.up;

        //Make quad smaller for water tiles
        if (tile.tileType == Tile.TileType.Water)
        {
            v3 = tile.verts[vector_2] - Vector3.up / 2;
            v4 = tile.verts[vector_1] - Vector3.up / 2;
        }

        //Make the quad
        t.GetComponent<FillGap>().MakeQuad(v1, v2, v3, v4, tile.tileType);
    }
}
