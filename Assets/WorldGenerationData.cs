using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationData : MonoBehaviour
{
    //Variables for generation
    public int mapSize = 100;




    public static WorldGenerationData Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
