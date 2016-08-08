using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using QuickPool;
using Random = UnityEngine.Random;

namespace Assets
{
    class TileGroup 
    {
        public List<GameObject> prefabs;
        public List<GameObject> tiles; 
        public TileGroup(List<GameObject> prefabs)
        {
        this.prefabs = prefabs;
        this.tiles = new List<GameObject>();
        float xCount = Random.Range(1, 15);
        float yCount = Random.Range(0, -5);
        var groundList = new List<GameObject>();
        if (xCount == 1)
        {
                var tile = prefabs[0].Spawn(new Vector3(0, yCount, 0), Quaternion.identity);
                
        } else
        {
                var leftTile = prefabs[0].Spawn(new Vector3(0, yCount, 0), Quaternion.identity);
            for (int j = 1; j <= xCount - 2; j++)
            {
                    //Instantiate tile prefab at the desired position as a Transform object
                    var midTile = prefabs[0].Spawn(new Vector3(j * 1, yCount, 0), Quaternion.identity);
                //Set the tiles parent to the GameObject this script is attached to
                //tile.parent = transform;


            }
                var rightTile = prefabs[0].Spawn(new Vector3(xCount - 1, yCount, 0), Quaternion.identity);
        }
        }
    }
}
