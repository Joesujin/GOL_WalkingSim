using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Tile 
{
    public GameObject block;
    public float x, y;
    public bool state;
    public bool nextState;
    List<Tile> neighbours;
    public bool shifted;
    public Vector3 currentPosition;
    public Vector3 targetPosition;

    public Tile(float ex, float why)
    {
        x = ex;
        y = why;
        if (Random.Range(0,3) > 1)
        {
            nextState = true;
            targetPosition = new Vector3(x, 0, y);
        }
        else
        {
            nextState = false;
            targetPosition = new Vector3(x, -1, y);
        }
        state = nextState;
        currentPosition = targetPosition;
        neighbours = new List<Tile>();
    }

    public void addNeighbour(Tile tile)
    {
        neighbours.Add(tile);
    }

    public void calcNextState()
    {
        int liveCount = 0;
        for (int i = 0; i < neighbours.Count; i++)
        {
            if(neighbours[i].state == true)
            {
                liveCount++;
            }
        }

        if (this.state == true)
        {
            if(liveCount == 2 || liveCount == 3)
            {
                this.nextState = true;
            }
            else
            {
                this.nextState = false;
            }
        }
        else
        {
            if(liveCount == 3)
            {
                nextState = true;
            }
        }
    }

    public void StateShift()
    {
        this.state = this.nextState;
        //this.currentPosition = this.targetPosition;
        this.shifted = false;
    }
    
}
