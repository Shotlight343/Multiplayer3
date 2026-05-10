using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundedGravity = -2f;

    private CharacterController characterController;
    private float verticalVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputDirection = new Vector2(horizontalInput, verticalInput);
        if (IsServer)
        {
            MovePlayer(inputDirection);
        }
        else
        { 
        MovePlayerRpc(inputDirection);
        }
    }

    [Rpc(SendTo.Server)]
    private void MovePlayerRpc(Vector2 movementInput)
    {
        MovePlayer(movementInput);
    }
    private void MovePlayer(Vector2 movementInput)
    {
        if (characterController.isGrounded && verticalVelocity < 0)
        {

            verticalVelocity = groundedGravity;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;    
        }
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        Vector3 horizontalMovement = moveDirection * moveSpeed;
        Vector3 verticalMovement = moveDirection * verticalVelocity;
        Vector3 finalMovement = horizontalMovement + verticalMovement;
        characterController.Move(finalMovement * Time.deltaTime);
    }    
}
