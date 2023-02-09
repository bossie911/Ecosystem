using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenerateMap : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject fill;

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
                GameObject t = Instantiate(tile, transform);
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
                    //Checking for south tile
                    if (z > 0)
                    {
                        //Check if tile south is land
                        Tile waterTile = tiles[x, z].GetComponent<Tile>();
                        Tile landTile = tiles[x, z - 1].GetComponent<Tile>();

                        if (landTile.tileType == Tile.TileType.Sand || landTile.tileType == Tile.TileType.Grass)
                        {
                            //Make a quad for gap of water
                            GameObject t = Instantiate(fill, transform);

                            //Positions for the vertices for each tile
                            Vector3 nw = landTile.verts[1];
                            Vector3 ne = landTile.verts[0];
                            Vector3 sw = waterTile.verts[2];
                            Vector3 se = waterTile.verts[3];

                            //Make the quad
                            t.GetComponent<FillGap>().MakeQuad(nw, ne, se, sw, landTile.tileType);
                        }
                    }

                    //Checking for north tile
                    if (z < mapSize - 1)
                    {
                        //Check if tile north is land
                        Tile waterTile = tiles[x, z].GetComponent<Tile>();
                        Tile landTile = tiles[x, z + 1].GetComponent<Tile>();

                        if (landTile.tileType == Tile.TileType.Sand || landTile.tileType == Tile.TileType.Grass)
                        {
                            //Make a quad gap of water
                            GameObject t = Instantiate(fill, transform);

                            //Positions for the vertices for each tile
                            Vector3 nw = landTile.verts[3];
                            Vector3 ne = landTile.verts[2];
                            Vector3 sw = waterTile.verts[0];
                            Vector3 se = waterTile.verts[1];

                            //Make the quad
                            t.GetComponent<FillGap>().MakeQuad(nw, ne, se, sw, landTile.tileType);
                        }
                    }

                    //Checking for east tile
                    if (x < mapSize - 1)
                    {
                        //Check if tile east is land
                        Tile waterTile = tiles[x, z].GetComponent<Tile>();
                        Tile landTile = tiles[x + 1, z].GetComponent<Tile>();

                        if (landTile.tileType == Tile.TileType.Sand || landTile.tileType == Tile.TileType.Grass)
                        {
                            //Make a quad  gap of water
                            GameObject t = Instantiate(fill, transform);

                            //Positions for the vertices for each tile
                            Vector3 nw = landTile.verts[0];
                            Vector3 ne = landTile.verts[3];
                            Vector3 sw = waterTile.verts[1];
                            Vector3 se = waterTile.verts[2];

                            //Make the quad
                            t.GetComponent<FillGap>().MakeQuad(nw, ne, se, sw, landTile.tileType);
                        }
                    }

                    //Checking for west tile
                    if (x > 0)
                    {
                        //Check if tile north is land
                        Tile waterTile = tiles[x, z].GetComponent<Tile>();
                        Tile landTile = tiles[x - 1, z].GetComponent<Tile>();

                        if (landTile.tileType == Tile.TileType.Sand || landTile.tileType == Tile.TileType.Grass)
                        {
                            //Make a quad for the gap of water
                            GameObject t = Instantiate(fill, transform);

                            //Positions for the vertices for each tile
                            Vector3 nw = landTile.verts[2];
                            Vector3 ne = landTile.verts[1];
                            Vector3 sw = waterTile.verts[3];
                            Vector3 se = waterTile.verts[0];

                            //Make the quad
                            t.GetComponent<FillGap>().MakeQuad(nw, ne, se, sw, landTile.tileType);
                        }
                    }
                }
            }
        }
    }
}
