using UnityEngine;
using Unity.Netcode;
using Unity;

public class NetworkPlayerattack : NetworkBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private KeyCode attackKey = KeyCode.Space;

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner)
        {
            return;
        }
        if(Input.GetKeyDown(attackKey))
        {
            RequestAttackServerRpc();
            Debug.Log("Attack requested");
        }

        
    }
     [ServerRpc]
        private void RequestAttackServerRpc()
        {
            Vector3 attackCenter = transform.position + transform.forward;
            Collider[] hits = Physics.OverlapSphere(attackCenter, attackRange, playerLayer);
            foreach (Collider hit in hits)
            {
                if(hit.gameObject == gameObject)
                {
                    continue;
                }
                NetworkPlayerHealth targetHealth = hit.GetComponent<NetworkPlayerHealth>();
                if(targetHealth != null)
                {
                    targetHealth.TakeDamage(attackDamage);
                    break;
                }
            }
        }
}
