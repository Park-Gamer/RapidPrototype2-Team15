using StarterAssets;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    // Variables
    [SerializeField] LayerMask wallLayer; // Layer to determine what can be wall run on
    [SerializeField] float wallRunForce; // Force to apply forward to player when wallrunning
    [SerializeField] float gravityForce; // Force to apply downward to player when wallrunning
    [SerializeField] float forwardInput; // Checks if player is holding 'w'
    // Collision detection
    [SerializeField] float wallCheckDistance = .5f; // Distance for raycast checking
    [SerializeField] bool wallLeft; // Bool check if wall is on player left
    [SerializeField] bool wallRight; // Bool check if wall is on player left
    RaycastHit leftWallHit; // Raycast for left wall detection
    RaycastHit rightWallHit; // Raycast for left wall detection
    // Ground detection
    [SerializeField] LayerMask groundLayer; // Layer to determine what the ground is
    [SerializeField] float groundCheckDistance = 0.1f; // Distance for raycast ground checking
    private bool isGrounded; // Bool check if player is on ground
    // References
    [SerializeField] Transform playerDirection; // Transform to determine what direction player is facing
    CharacterController controller; // Character controller to add force while the player wall runs
    ThirdPersonController playerMovement; // Player movement script to disable while the player wall runs

    void Start()
    {
        // Gather player script/controller
        controller = GetComponent<CharacterController>();
        playerMovement = GetComponent<ThirdPersonController>();
    }
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer); // Raycast to determine if player is grounded
        DetectWall();
        MovementDetection();
    }
    private void FixedUpdate()
    {
        if (!playerMovement.enabled) // Checks if player movement script is off meaning the player is wallrunning
        {
            WallRunMovement();
        }
    }
    void DetectWall()
    {
        // Bools return true if raycast detects object with correct layer.
        wallRight = Physics.Raycast(transform.position, playerDirection.right, out rightWallHit, wallCheckDistance, wallLayer);
        wallLeft = Physics.Raycast(transform.position, -playerDirection.right, out leftWallHit, wallCheckDistance, wallLayer);
    }
    void MovementDetection()
    {
        forwardInput = Input.GetAxis("Vertical"); // Returns a value between 1/-1 depending on the player moving forward/backward

        if ((wallLeft || wallRight) && forwardInput > 0 && !isGrounded) // Check if player is touching wall and holding forward and not grounded
        {
            if (playerMovement.enabled) // Exception check
            {
                StartWallRun();
            }
        }
        else // When player isn't detecting wall or stops holding forward
        {
            if (!playerMovement.enabled)
            {
                StopWallRun();
            }
        }
    }
    void StartWallRun()
    {
        playerMovement.enabled = false; // Turn off player movement script during wallrunning to avoid malfunctions
    }
    void StopWallRun()
    {
        playerMovement.enabled = true; // Turn player movement script back on when not wall running
    }
    void WallRunMovement()
    {
        // Sets the Vector3 to the normal vector of either the right wall or the left wall, depending on whether bool is true or false.
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        // Vector3 gets the value of the perpendicular point between the wall normal vector and transform.up
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        // Checks if player's forward direction is closer to the negative of wallForward Vector than positive 
        if ((playerDirection.forward - wallForward).magnitude > (playerDirection.forward - -wallForward).magnitude)
        {
            // If the condition is true wallForward vector direction is flipped.
            wallForward = -wallForward; // Ensure that player forward direction aligns appropriately with a wall
        }

        controller.Move(wallForward * wallRunForce * Time.fixedDeltaTime); // Apply force forward to character controller while player is wallrunning
        controller.Move(Vector3.down * gravityForce * Time.fixedDeltaTime); // Apply force downward to character controller to simulate gravity
    }
}


