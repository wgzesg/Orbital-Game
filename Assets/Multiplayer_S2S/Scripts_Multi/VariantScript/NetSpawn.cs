using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using Photon.Pun;
using System.IO;

public class NetSpawn : Spawn
{
    PhotonView PV;

    public override void Start()
    {
        base.Start();
        PV = GetComponent<PhotonView>();
    }

    public override void onStartGameSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            base.onStartGameSpawn();
        }
        
    }

    public override void removeEnemyHandler(EnemyController diedOne, int enemyleft)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            base.removeEnemyHandler(diedOne, enemyleft);
        }
    }

    public override void SpwanNewWave()
    {
        List<Transform> spawnPointList = spawnPoints.OrderBy(x => Guid.NewGuid()).Take(levelDataFile.levelsystem[numOfwaves].numberOfSpots).ToList();
        int numPerPoint = levelDataFile.levelsystem[numOfwaves].numToSpawnAtEachPoint;
        foreach (Transform spawnpoint in spawnPointList)
        {
            for (int i = 0; i < numPerPoint; i++){
                Vector3 randomVector = new Vector3(Random.Range(5, -5), 0, Random.Range(5, -5));
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetEnemy_HoverBot"), spawnpoint.position + randomVector, Quaternion.identity);
            }
        }
        int totalDeployed = levelDataFile.levelsystem[numOfwaves].numberOfSpots * levelDataFile.levelsystem[numOfwaves].numToSpawnAtEachPoint;
        PV.RPC("OnSpawnBraodCast", RpcTarget.AllBuffered, numOfwaves, totalDeployed);
        numOfwaves++;
    }


    [PunRPC]
    public void OnSpawnBraodCast(int numOfwaves, int totalDeployed)
    {
        if (onSpawn != null)
        {
            onSpawn.Invoke(numOfwaves, totalDeployed);
        }
    }


}
