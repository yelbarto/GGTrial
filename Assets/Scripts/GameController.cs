using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Cube, Block
    private Tuple<Transform, Transform>[,,] maze;

    public Vector3[] spawnPlaceLoc = { new Vector3(-8f, 1f, -4f),
    new Vector3(-6f, 1f, -6f),
    new Vector3(-4f, 1f, -8f) };


    //possible spawn objects
    public GameObject[] blocks;
    //spawned blocks
    public Block[] availableBlocks = new Block[3];
    
    //grid
    public GameObject parent;


    private int lockBreaker;
    private int levels = 3;

    public Text finalScore;
    public Text visualFeedback;
    public Text bottomText;
    public Text score;
    public Image finishScene;
    private int scoreInt;
    private Animator anim;
    private Animator bottomAnim;
    private bool finish = false;


    void Update()
    {
        if(!finish)
            isFinished();
    }

    public void setUp(float gridX, float gridZ)
    {
        lockBreaker = 0;
        float max = Math.Max(gridX, gridZ);
        //improve :/
        if(max > 7)
        {
            float dif = max - 7;
            Vector3 camLoc = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(camLoc.x - (4f * dif), camLoc.y + (5f * dif), camLoc.z - (4f * dif));
            spawnPlaceLoc[0] = new Vector3(spawnPlaceLoc[0].x - (2f * dif), spawnPlaceLoc[0].y, spawnPlaceLoc[0].z - (1f * dif));
            spawnPlaceLoc[1] = new Vector3(spawnPlaceLoc[1].x - (1.5f * dif), spawnPlaceLoc[1].y, spawnPlaceLoc[1].z - (1.5f * dif));
            spawnPlaceLoc[2] = new Vector3(spawnPlaceLoc[2].x - (1f * dif), spawnPlaceLoc[2].y, spawnPlaceLoc[2].z - (2f * dif));

        }
        maze = new Tuple<Transform, Transform> [(int) gridX,(int) gridZ, levels];
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                for (int y = 0; y < levels; y++)
                {
                     maze[x, z, y] = null;

                }
            }
        }
        spawnBlock();
        scoreInt = 0;
        
    }

    void lineCheckerX(int x) 
    {
        for(int z=0; z<maze.GetLength(1); z++)
        {
            if (maze[x, z, 0] == null)
                return;
        }
        scoreInt += 30;
        score.text = scoreInt.ToString();

        for (int z=0; z<maze.GetLength(1); z++)
        {
            Transform remove = maze[x, z, 0].Item1;
            Rigidbody rb = (Rigidbody)remove.GetComponent(typeof(Rigidbody));
            rb.useGravity = true;
            Destroy(remove.gameObject, 5);
            //Change parent's remaining cubes
            Transform parent = maze[x, z, 0].Item2;
            BlockHandler bh = (BlockHandler)parent.GetComponent(typeof(BlockHandler));
            bh.cubeAmount--;
            anim = visualFeedback.gameObject.GetComponent<Animator>();
            anim.SetTrigger("Active");

            if (maze[x, z, 1] != null)
            {
                Transform dummy = maze[x, z, 1].Item1;
                maze[x, z, 0] = new Tuple<Transform, Transform>(dummy, parent);
                dummy.localPosition = new Vector3(dummy.localPosition.x, 0f, dummy.localPosition.z);
                if (maze[x, z, 2] != null)
                {
                    dummy = maze[x, z, 2].Item1;
                    maze[x, z, 1] = new Tuple<Transform, Transform>(dummy, parent);
                    dummy.localPosition = new Vector3(dummy.localPosition.x, dummy.localScale.y, dummy.localPosition.z);
                    maze[x, z, 2] = null;
                }
                else
                {
                    maze[x, z, 1] = null;
                }
            }
            else
            {
                maze[x, z, 0] = null;
                //Destroy block
                if (bh.cubeAmount == 0)
                {
                    Destroy(parent.gameObject, 7);
                }
            }
        }
    }

    void lineCheckerZ(int z)
    {
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            if (maze[x, z, 0] == null)
                return;
        }
        scoreInt += 30;
        score.text = scoreInt.ToString();

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            //Destroy cube
            Transform remove = maze[x, z, 0].Item1;
            Rigidbody rb = (Rigidbody)remove.GetComponent(typeof(Rigidbody));
            rb.useGravity = true;
            Destroy(remove.gameObject, 5);
            //Change parent's remaining cubes
            Transform parent = maze[x, z, 0].Item2;
            BlockHandler bh = (BlockHandler) parent.GetComponent(typeof(BlockHandler));
            bh.cubeAmount--;
            anim = visualFeedback.gameObject.GetComponent<Animator>();
            anim.SetTrigger("Active");

            if (maze[x, z, 1] != null)
            {
                Transform dummy = maze[x, z, 1].Item1;
                maze[x, z, 0] = new Tuple<Transform, Transform>(dummy, parent);
                dummy.localPosition = new Vector3(dummy.localPosition.x, 0f, dummy.localPosition.z);               
                if (maze[x, z, 2] != null)
                {
                    dummy = maze[x, z, 2].Item1;
                    maze[x, z, 1] = new Tuple<Transform, Transform>(dummy, parent);
                    dummy.localPosition = new Vector3(dummy.localPosition.x, dummy.localScale.y, dummy.localPosition.z);
                    maze[x, z, 2] = null;
                }
                else
                {
                    maze[x, z, 1] = null;
                }
            }
            else
            {
                maze[x, z, 0] = null;
                //Destroy block
                if (bh.cubeAmount == 0)
                {
                    Destroy(parent.gameObject, 7);
                }
            }
        }
    }

    int fillLevels(Transform transform, Tuple<int, int> mazeLoc, int[] levelPlace, int modular, int counter, float level)
    {
        float rotation = parent.transform.rotation.y;
        GameObject go;
        for (int i=0; i<levelPlace.Length; i++)
        {
            int dummyX = i / modular;
            int dummyZ = i % modular;
            int placeX;
            int placeZ;
            if (levelPlace[i] == 1)
            {
                switch (rotation)
                {
                    case 0f:
                        placeX = mazeLoc.Item1 + dummyX;
                        placeZ = mazeLoc.Item2 + dummyZ;
                        go = GameObject.Find(placeX.ToString() + placeZ.ToString());
                        transform.GetChild(counter).position = new Vector3(go.transform.position.x, 0.1f + level * 0.3f, go.transform.position.z);
                        maze[mazeLoc.Item1 + dummyX, mazeLoc.Item2 + dummyZ, (int)level] = new Tuple<Transform, Transform>(transform.GetChild(counter), transform);
                        counter += 1;
                        break;
                    case 0.7071068f:
                        if (parent.transform.rotation.w == parent.transform.rotation.y)
                        {
                            placeX = mazeLoc.Item1 - dummyZ;
                            placeZ = mazeLoc.Item2 + dummyX;
                            go = GameObject.Find(placeX.ToString() + placeZ.ToString());
                            transform.GetChild(counter).position = new Vector3(go.transform.position.x, 0.1f + level * 0.3f, go.transform.position.z);

                            maze[mazeLoc.Item1 - dummyZ, mazeLoc.Item2 + dummyX, (int)level] = new Tuple<Transform, Transform>(transform.GetChild(counter), transform);
                            counter += 1;
                        }
                        else
                        {
                            placeX = mazeLoc.Item1 + dummyZ;
                            placeZ = mazeLoc.Item2 - dummyX;
                            go = GameObject.Find(placeX.ToString() + placeZ.ToString());
                            transform.GetChild(counter).position = new Vector3(go.transform.position.x, 0.1f + level * 0.3f, go.transform.position.z);

                            maze[mazeLoc.Item1 + dummyZ, mazeLoc.Item2 - dummyX, (int)level] = new Tuple<Transform, Transform>(transform.GetChild(counter), transform);
                            counter += 1;
                        }
                        break;
                    case 1f:
                        placeX = mazeLoc.Item1 - dummyX;
                        placeZ = mazeLoc.Item2 - dummyZ;
                        go = GameObject.Find(placeX.ToString() + placeZ.ToString());
                        transform.GetChild(counter).position = new Vector3(go.transform.position.x, 0.1f + level * 0.3f, go.transform.position.z);

                        maze[mazeLoc.Item1 - dummyX, mazeLoc.Item2 - dummyZ, (int)level] = new Tuple<Transform, Transform>(transform.GetChild(counter), transform);
                        counter += 1;
                        break;
                    default:
                        Debug.LogError("Rotation is wrong");
                        break;
                }
                
            }
        }
        bottomAnim = bottomText.gameObject.GetComponent<Animator>();
        bottomAnim.SetTrigger("Active");
        return counter;
    }


    public void fillMaze(Block block, Transform transform, Tuple<int, int> mazeLoc, int rotation)
    {
        GameObject go = GameObject.Find(mazeLoc.Item1.ToString() + mazeLoc.Item2.ToString());
        transform.position = new Vector3(go.transform.position.x, 0.1f, go.transform.position.z);
        int counter = 0;
        if(block.levels==3)
        {
            counter = fillLevels(transform, mazeLoc, block.level1, block.modular, counter, 0f);
            counter = fillLevels(transform, mazeLoc, block.level2, block.modular, counter, 1f);
            fillLevels(transform, mazeLoc, block.level3, block.modular, counter, 2f);
        }
        else if(block.levels==2)
        {
            counter = fillLevels(transform, mazeLoc, block.level1, block.modular, counter, 0f);
            fillLevels(transform, mazeLoc, block.level2, block.modular, counter, 1f);
        }
        else
        {         
            fillLevels(transform, mazeLoc, block.level1, block.modular, counter, 0f);                       
        }

        lockBreaker++;
        if (lockBreaker == 3)
        {
            spawnBlock();
            lockBreaker = 0;
        }

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            lineCheckerX(x);
        }
        for (int z = 0; z < maze.GetLength(1); z++)
        {
            lineCheckerZ(z);
        }

    }

    public Tuple<int, int> placeChecker(Block block, Transform child)
    {

        Tuple<float, float> coord = new Tuple<float, float>(child.position.x, child.position.z);

        Vector3 location = new Vector3(child.position.x, 0.1f, child.position.z);
        var collidedGameObjects =
          Physics.OverlapSphere(location, 0.050f /*Radius*/)
          .Except(new[] { GetComponent<Collider>() })
          .Select(c => c.gameObject)
          .ToArray();
        for(int k=0; k< collidedGameObjects.Length; k++)
        {
            GameObject go = collidedGameObjects[k];
            if (go.tag == "Tile")
            {
                int x = Int16.Parse(go.name[0].ToString());
                int z = Int16.Parse(go.name[1].ToString());
                if (maze[x, z, 0] == null)
                {
                    float rotation = parent.transform.rotation.y;

                    for (int i = 0; i < block.level1.Length; i++)
                    {
                        if (block.level1[i] == 1)
                        {                      
                        int dummyX = i / block.modular;
                        int dummyZ = i % block.modular;
                        switch (rotation)
                        {
                            case 0f:

                                if (((x + dummyX) >= (maze.GetLength(0))) || ((z + dummyZ) >= (maze.GetLength(1))) || (maze[x + dummyX, z + dummyZ, 0] != null))
                                    return null;                                
                                break;
                            case 0.7071068f:
                                if (parent.transform.rotation.w == parent.transform.rotation.y)
                                {
                                    if (((x - dummyZ) >= (maze.GetLength(0))) || ((x - dummyZ) < 0) ||
                                            ((z + dummyX) >= (maze.GetLength(1))) || (maze[x - dummyZ, z + dummyX, 0] != null))
                                        return null;
                                } 
                                else
                                {
                                    if (((x + dummyZ) >= (maze.GetLength(0))) || ((z - dummyX) < 0) ||
                                            ((z - dummyX) >= (maze.GetLength(1))) || (maze[x + dummyZ, z - dummyX, 0] != null))
                                        return null;
                                }                               
                                break;
                            case 1f:
                                if (((x - dummyX) >= (maze.GetLength(0))) || ((x - dummyX) < 0) ||
                                        ((z - dummyZ) >= (maze.GetLength(1))) || ((z - dummyZ) < 0) || (maze[x - dummyX, z - dummyZ, 0] != null))
                                    return null;
                                break;
                            default:
                                Debug.LogError("Rotation is wrong");
                                break;
                        }
                        }
                    }                   
                    return new Tuple<int, int>(x,z);
                }
                else
                {
                    return null;
                }
            }
        }
        return null;
    }

    void spawnBlock()
    {
        for (int i = 0; i < 3; i++)
        {
            int rand = UnityEngine.Random.Range(0, blocks.Length);
            GameObject block = Instantiate(blocks[rand], spawnPlaceLoc[i], Quaternion.identity);  
            BlockHandler bh = (BlockHandler)block.GetComponent(typeof(BlockHandler));
            bh.block.originalScale = block.transform.localScale;
            bh.spawnPlace = i;
            bh.block.place = i;
            availableBlocks[i] = bh.block;
            block.transform.localScale /= 2;                
        }
    }

    bool isFinished()
    {
        for (int i = 0; i < availableBlocks.Length; i++) {
            if(availableBlocks[i] != null)
            {
                Block block = availableBlocks[i];
                for (int x = 0; x < maze.GetLength(0); x++)
                {
                    for (int z = 0; z < maze.GetLength(1); z++)
                    {
                        if (maze[x, z, 0] == null)
                        {
                            if (isAvailable(x, z, block))
                            {
                                return false;
                            }
                        }

                    }
                }
            }        
        }
        finalScore.text = finalScore.text + "\nYour Score: " + scoreInt.ToString();
        Animator fAnimator = finishScene.gameObject.GetComponent<Animator>();
        fAnimator.SetTrigger("Finish");
        finish = true;
        return true;

    }

    bool isAvailable(int x, int z, Block block)
    {
        for (int j = 0; j < 4; j++)
        {
            int found = 0;
            for (int i = 0; i < block.level1.Length; i++)
            {
                
                if (block.level1[i] == 1)
                {
                    int dummyX = i / block.modular;
                    int dummyZ = i % block.modular;

                    switch (j)
                    {
                        case 0:
                            if (((x + dummyX) >= (maze.GetLength(0))) || ((z + dummyZ) >= (maze.GetLength(1))) || (maze[x + dummyX, z + dummyZ, 0] != null))
                                i = block.level1.Length;
                            else
                                found++;
                            break;
                        case 1:
                            if (((x - dummyZ) >= (maze.GetLength(0))) || ((x - dummyZ) < 0) ||
                                    ((z + dummyX) >= (maze.GetLength(1))) || (maze[x - dummyZ, z + dummyX, 0] != null))
                                i = block.level1.Length;
                            else
                                found++;
                            break;
                        case 2:

                            if (((x + dummyZ) >= (maze.GetLength(0))) || ((z - dummyX) < 0) ||
                                    ((z - dummyX) >= (maze.GetLength(1))) || (maze[x + dummyZ, z - dummyX, 0] != null))
                                i = block.level1.Length;
                            else
                                found++;

                            break;
                        case 3:
                            if (((x - dummyX) >= (maze.GetLength(0))) || ((x - dummyX) < 0) ||
                                    ((z - dummyZ) >= (maze.GetLength(1))) || ((z - dummyZ) < 0) || (maze[x - dummyX, z - dummyZ, 0] != null))
                                return false;
                            break;
                        default:
                            Debug.LogError("Rotation is wrong");
                            break;
                    }
                    if (found == block.level1.Length)
                    {
                        return true;
                    }
                }
            }
        }
        return true;
    }

    public void setAvailableBlock(int i)
    {
        availableBlocks[i] = null;
    }
}
