using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabGrid : MonoBehaviour
{

    public GameObject[] itemsToPickFrom;
    public float gridX;
    public float gridZ;
    public float gridSpacingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;
    public GameObject parent;
    private float squareLength = 0.122148f;
    public GameController engine;


    // Start is called before the first frame update
    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        parent.transform.localScale = new Vector3(gridX, 0.1f, gridZ);
        for (int x=0; x<gridX; x++)
        {
            for (int z=0; z<gridZ; z++)
            {
                Vector3 spawnPos = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin;
                if ((x+z)%2 == 0)
                {
                    GameObject clone = Instantiate(itemsToPickFrom[0], spawnPos, Quaternion.identity);
                    clone.transform.parent = parent.transform;
                    clone.transform.localPosition = new Vector3(clone.transform.localPosition.x - squareLength * (gridX / 2),
                        0,
                        clone.transform.localPosition.z - squareLength * (gridZ / 2));
                    clone.name = x.ToString() + z.ToString();
                    Tuple<float, float> coordXZ = new Tuple<float, float>(clone.transform.position.x, clone.transform.position.z);
                } else
                {
                    GameObject clone = Instantiate(itemsToPickFrom[1], spawnPos, Quaternion.identity);
                    clone.transform.parent = parent.transform;
                    clone.transform.localPosition = new Vector3(clone.transform.localPosition.x - squareLength * (gridX / 2),
                        0,
                        clone.transform.localPosition.z - squareLength * (gridZ / 2));
                    clone.name = x.ToString() + z.ToString();
                }
                
            }
        }
        engine.setUp(gridX, gridZ);
        

    }


}
