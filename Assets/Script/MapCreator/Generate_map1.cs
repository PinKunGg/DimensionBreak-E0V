using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_map1 : MonoBehaviour
{
    [SerializeField] int width,Height;
    [SerializeField] int MapWidth, MapHeight;
    [SerializeField] GameObject Ground1, Ground2, MapGenerate, Player;

    private void Start()
    {
        Generation();
    }
    void Generation()
    {
        for(int x = 0; x < width * MapWidth; x += MapWidth)
        {
            int minHeight = Height - 1;
            int maxHeight = Height + 2;

            Height = Random.Range(minHeight,maxHeight);

            for(int y = 0; y < Height; y++)
            {
                SpawnObj(Ground2,x,y * MapHeight);
            }
            SpawnObj(Ground1,x,Height * MapHeight);
        }
        Player.transform.position = new Vector2(Player.transform.position.x,Height * MapHeight * 2f);
    }

    void SpawnObj(GameObject obj, int width, int height)
    {
        obj = Instantiate(obj, new Vector2(width,height), Quaternion.identity);
        obj.transform.parent = MapGenerate.transform;
    }
}
