﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;
using QuickPool;
using System;

public class Floaties : MonoBehaviour
{

    public List<GameObject> TileComponents;
    private List<List<GameObject>> m_Tiles;
    //public GameObject Parent;

    // Use this for initialization
    void Awake ()
    {
        m_Tiles = new List<List<GameObject>>();
        //Spawn();
    }

    public Vector3 topLeft 
    {
        get { return m_Tiles[0][0].transform.position; }
    }

    public Vector3 topRight
    {
        get { return m_Tiles[m_Tiles.Count - 1][0].transform.position; }
    }

    public float xEnd
    {
        get { return transform.position.x + size; }
    }

    public float size
    {
        get { return (float) m_Tiles.Count; }
    }

    private void buildPillar(int index, float xPosition, int size)
    {
        float i = 0;
        GameObject tile;
        var Pilar = new List<GameObject>();
        while (i < size)
        {
            tile = TileComponents[index].Spawn(new Vector2(xPosition, -i), Quaternion.identity);
            tile.transform.SetParent(transform, false);
            Pilar.Add(tile);

            if (i == 0)
            {
                i += 0.4f;
                index = 4;
                var col = tile.GetComponent<Collider2D>();
                if (col != null)
                {
                    col.isTrigger = false;
                }
            }
            else
            {
                i += 0.8f;
            }
        }
        m_Tiles.Add(Pilar);
    }

    public void Spawn(int size)
    {   
        if (size == 1)
        {
            buildPillar(0, 0.5f, 15);
        }
        else
        {
            buildPillar(1, 0.5f, 15);

            for (int j = 1; j <= size - 2; j++)
            {
                buildPillar(2, 0.5f + j, 15);
            }
            buildPillar(3, size - 0.5f, 15);
        }
    }

    internal void DespawnFloatie()
    {
        foreach (List<GameObject> pilar in m_Tiles)
        {
            foreach (GameObject tile in pilar)
            {
                tile.Despawn();
            }
        }
    
        // 
        m_Tiles.Clear();

    }
}
