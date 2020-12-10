using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnemySpawn : MonoBehaviour
{
    public GameObject GroundEnemy, FlyEnemy;
    public Transform[] EnemySpawn;
    public GameObject EnemyParentObj;

    public int EnemySpawnPerPoint;

    private void Start()
    {
        foreach(var SpawnerPoint in  EnemySpawn)
        {
            for(int i = 0; i < EnemySpawnPerPoint; i++)
            {
                float RandPosX = Random.Range(-0.5f,0.6f);
                float RandPosY = Random.Range(0.1f,0.4f);

                if(SpawnerPoint.GetComponent<SpawnerPoint>().GetGroundSpawn == true && SpawnerPoint.GetComponent<SpawnerPoint>().GetFlySpawn == false)
                {
                    GameObject EnemyList = Instantiate(GroundEnemy,new Vector2(SpawnerPoint.position.x + RandPosX, SpawnerPoint.position.y),Quaternion.identity);
                    EnemyList.transform.parent = EnemyParentObj.transform;
                }
                if(SpawnerPoint.GetComponent<SpawnerPoint>().GetGroundSpawn == false && SpawnerPoint.GetComponent<SpawnerPoint>().GetFlySpawn == true)
                {
                    GameObject EnemyList = Instantiate(FlyEnemy,new Vector2(SpawnerPoint.position.x + RandPosX, SpawnerPoint.position.y + RandPosY),Quaternion.identity);
                    EnemyList.transform.parent = EnemyParentObj.transform;
                }
                if(SpawnerPoint.GetComponent<SpawnerPoint>().GetGroundSpawn == true && SpawnerPoint.GetComponent<SpawnerPoint>().GetFlySpawn == true)
                {
                    GameObject EnemyList = Instantiate(GroundEnemy,new Vector2(SpawnerPoint.position.x + RandPosX, SpawnerPoint.position.y + RandPosY),Quaternion.identity);
                    GameObject EnemyList2 = Instantiate(FlyEnemy,new Vector2(SpawnerPoint.position.x + RandPosX, SpawnerPoint.position.y + RandPosY),Quaternion.identity);
                    EnemyList.transform.parent = EnemyParentObj.transform;
                    EnemyList2.transform.parent = EnemyParentObj.transform;
                }
            }
        }
    }
}
