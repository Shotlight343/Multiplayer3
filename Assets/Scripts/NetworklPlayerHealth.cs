using UnityEngine;
using Unity.Netcode;   
using Unity; 

public class NetworkPlayerHealth : NetworkBehaviour
{
    [SerializeField] int maxHealth = 100;
    //this line creates network-synced health variable
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,//shares the value to the client,host and other servers can read it
        NetworkVariableWritePermission.Server//Only the server can change the health value
    );
    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            currentHealth.Value = maxHealth;
        }
        currentHealth.OnValueChanged += OnHealthChange;

    }

    public override void OnNetworkDespawn()
    {
       currentHealth.OnValueChanged -= OnHealthChange;
    }

    private void OnHealthChange(int previousValue, int newValue)
    {
        Debug.Log($"{gameObject.name} health changed: {previousValue} -> {newValue}");
    }

    public void TakeDamage(int damageAmount)
    {
        if (!IsServer)
        {
            return;
        }
        currentHealth.Value -= damageAmount;
        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth);
        if (currentHealth.Value <= 0)
        {
           Respawn();
        }
    }

    private void Respawn()
    {
        currentHealth.Value = maxHealth;
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int randomIndex = Random.Range(0, spawnPointObjects.Length);
        Transform selectedSpawn = spawnPointObjects[randomIndex].transform;
        CharacterController characterController = GetComponent<CharacterController>();
        if(characterController != null)
        {
            characterController.enabled = false;
        }
        transform.position = selectedSpawn.position;
        transform.rotation = selectedSpawn.rotation;
        if(characterController != null)
        {
            characterController.enabled = true;
        }
    }

    
}
