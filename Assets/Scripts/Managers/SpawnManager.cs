using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private SpawnPoint currentSpawnPoint;

    public static SpawnManager instance { get; private set; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            GameObject dontDestroyObject = gameObject.GetDeepestParent();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void AssignSpawnPoint(SpawnPoint spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
    }

    public void SpawnPlayer()
    {
        if(currentSpawnPoint == null)
        {
            Debug.LogError("No assigned spawnpoint, look at this");
            return;
        }

        if(PlayerManagement.player!=null)
        {
            PlayerManagement.player.transform.position = currentSpawnPoint.transform.position;
            PlayerManagement.player.transform.up = -GravityManager.sceneGravity.normalized;
            PlayerManagement.player.ResetPlayerVelocity();

            Vector3 cameraPos = PlayerManagement.player.transform.position + PlayerManagement.player.transform.forward * -5;

            PlayerManagement.instance.SetCameraPosition(cameraPos);
        }
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            SpawnPlayer();
        }
    }
}
