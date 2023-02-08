using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenerateMap : MonoBehaviour
{
    [SerializeField] private GameObject tile;

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

        GenerateWorld();

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

                //Make the squad
                t.GetComponent<CreateTile>().MakeQuad(nw, ne, se, sw, noise);
            }
        }
    }


}
