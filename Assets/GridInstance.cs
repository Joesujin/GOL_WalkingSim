using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInstance : MonoBehaviour
{
    public GameObject goal;
    public GameObject Block;
    public Material Nextstate;
    public Tile[,] tiles;
    public int numX;
    public int numY;
    public float scl = 1;
    public bool isChanging;

    // Start is called before the first frame update
    void Start()
    {
        restart();
        goal.GetComponent<Transform>().transform.position = new Vector3(numX*scl, 0, numY*scl);
        //StartCoroutine(ChangeTiles());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ShiftState();
        }

    }

    private void OnEnable()
    {
        Events.findTile += ChangeTileState;
    }

    private void OnDisable()
    {
        Events.findTile -= ChangeTileState;

    }

    public void ChangeTileState(Vector3 TargetTilePos)
    {
        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                if(tiles[i,j].block.transform.position == TargetTilePos)
                {
                    tiles[i, j].nextState = !tiles[i, j].nextState;

                    tiles[i, j].calcNextNextState();

                    if (tiles[i, j].nextnextState)
                    {
                        tiles[i, j].block.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else
                    {
                        tiles[i, j].block.GetComponent<MeshRenderer>().material.color = Color.grey;
                    }
                    

                }

            }
        }
    }

    public void restart()
    {
        tiles = new Tile[numX, numY];
        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                Tile newTile = new Tile(i, j);
                tiles[i, j] = newTile;
            }
        }

        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                int above = j - 1;
                int below = j + 1;
                int left = i - 1;
                int right = i + 1;

                if (above < 0)
                {
                    above = numX - 1;
                }
                if (below == numY)
                {
                    below = 0;
                }
                if (left < 0)
                {
                    left = numX - 1;
                }
                if (right == numX)
                {
                    right = 0;
                }

                tiles[i, j].addNeighbour(tiles[left, above]);
                tiles[i, j].addNeighbour(tiles[left, j]);
                tiles[i, j].addNeighbour(tiles[left, below]);
                tiles[i, j].addNeighbour(tiles[i, below]);
                tiles[i, j].addNeighbour(tiles[right, below]);
                tiles[i, j].addNeighbour(tiles[right, j]);
                tiles[i, j].addNeighbour(tiles[right, above]);
                tiles[i, j].addNeighbour(tiles[i, above]);

            }
        }

        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                tiles[i, j].block = Instantiate(Block, tiles[i,j].currentPosition*scl, Quaternion.identity);
            }
        }

        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                tiles[i, j].calcNextNextState();

                if (tiles[i, j].nextnextState)
                {
                    tiles[i, j].block.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    tiles[i, j].block.GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }
        }
    }

    public void ShiftState()
    {
        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                tiles[i, j].calcNextState();
            }
        }

        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                tiles[i, j].calcNextNextState();
            }
        }


        for (int i = 0; i < numX; i++)
        {
            for (int j = 0; j < numY; j++)
            {
                tiles[i, j].StateShift();

                if (tiles[i, j].nextnextState)
                {
                    tiles[i, j].block.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    tiles[i, j].block.GetComponent<MeshRenderer>().material.color = Color.white;
                }

                if (tiles[i, j].state)
                {
                    //tiles[i, j].block.GetComponent<Transform>().transform.position = new Vector3(tiles[i, j].x * scl, 0f, tiles[i, j].y * scl);

                    moveBlocks(i, j, true);
                }
                else
                {
                    //tiles[i, j].block.GetComponent<Transform>().transform.position = new Vector3(tiles[i, j].x * scl, -1f*scl, tiles[i, j].y * scl);

                    moveBlocks(i, j, false);
                }



            }
        }
    }


    /*--------------------------------------------------------- Automatic change of state
    IEnumerator ChangeTiles()
    {
        while(gameObject)
        {
            for (int i = 0; i < numX; i++)
            {
                for (int j = 0; j < numY; j++)
                {
                    tiles[i, j].calcNextState();
                }
            }

            for (int i = 0; i < numX; i++)
            {
                for (int j = 0; j < numY; j++)
                {
                    tiles[i, j].calcNextNextState();
                }
            }


            for (int i = 0; i < numX; i++)
            {
                for (int j = 0; j < numY; j++)
                {
                    tiles[i, j].StateShift();

                    if (tiles[i,j].nextnextState)
                    {
                        tiles[i, j].block.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else
                    {
                        tiles[i, j].block.GetComponent<MeshRenderer>().material.color = Color.white;
                    }

                    if (tiles[i,j].state)
                    {
                        //tiles[i, j].block.GetComponent<Transform>().transform.position = new Vector3(tiles[i, j].x * scl, 0f, tiles[i, j].y * scl);

                        moveBlocks(i, j, true);
                    }
                    else
                    {
                        //tiles[i, j].block.GetComponent<Transform>().transform.position = new Vector3(tiles[i, j].x * scl, -1f*scl, tiles[i, j].y * scl);

                        moveBlocks(i, j, false);
                    }

                    

                }
            }
            
            //yield return new WaitUntil(() => isChanging);

            yield return new WaitForSeconds(3f);
        }
    }
    */

    public void moveBlocks(int i, int j, bool dir)
    {
        if (dir)
        {
            //for (float b = 0; b < 100; b++)
            //{
                tiles[i, j].block.GetComponent<Transform>().transform.position = Vector3.Lerp(new Vector3(tiles[i, j].x * scl, -1f * scl, tiles[i, j].y * scl), new Vector3(tiles[i, j].x * scl, 0f, tiles[i, j].y * scl),  Time.time);
            //}
        }
        else
        {
            //for (float b = 0; b < 100; b++)
            //{
                tiles[i, j].block.GetComponent<Transform>().transform.position = Vector3.Lerp(new Vector3(tiles[i, j].x * scl, 0f, tiles[i, j].y * scl), new Vector3(tiles[i, j].x * scl, -1f * scl, tiles[i, j].y * scl),  Time.time);
            //}
        }
    }



}

