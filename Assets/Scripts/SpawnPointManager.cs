using UnityEngine;
using Unity.Netcode;

// Creates a network-aware spawn manager script
public class SpawnPointManager : NetworkBehaviour
{
    //Stores which spawn point should be used next
    //Static means all player objects share this value
    private static int nextSpawnIndex;
    //Runs when 

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            Debug.Log("No server");
            return;
        }

        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (spawnPointObjects.Length == 0)
        {
            Debug.Log("No spawnpoint found");
            return;
        }

        Transform selectedSpawnpoint = spawnPointObjects[nextSpawnIndex].transform;
        CharacterController characterController = GetComponent<CharacterController>();

        //temporarily disabled the charactercontroller
        //before teleporting the player

        if (characterController != null) { 
            characterController.enabled = false;
            Debug.Log("Disabled temporarily");
        }

        //moves the player to selected spawn point
        transform.position = selectedSpawnpoint.position;
        //rotates the player to match the spawn point
        transform.rotation = selectedSpawnpoint.rotation;

        if (characterController != null)
        {
            characterController.enabled = true;
            Debug.Log("Enabled!");
        }

        nextSpawnIndex++;
        if (nextSpawnIndex >= spawnPointObjects.Length)
        { 
            nextSpawnIndex = 0;
        }

    }

    

}
