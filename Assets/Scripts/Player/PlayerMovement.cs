using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float baseSpeed = 10f;
    private float currentSpeed;

    private Vector2 targetPosition;
    private bool isMoving = false;

    private float speedModifier = 1f;
    private float speedModifierDuration = 0f;

    [SerializeField]
    [Tooltip("Move the player to the location of 'moveToLocation'.")]
    private InputAction moveTo = new InputAction(type: InputActionType.Button);

    [SerializeField]
    [Tooltip("Determine the location to 'moveTo'.")]
    private InputAction moveToLocation = new InputAction(type: InputActionType.Value, expectedControlType: nameof(Vector2));

    private Camera mainCamera;

    void OnValidate()
    {
        // Provide default bindings for the input actions.
        if (moveTo.bindings.Count == 0)
            moveTo.AddBinding("<Mouse>/leftButton");
        if (moveToLocation.bindings.Count == 0)
            moveToLocation.AddBinding("<Mouse>/position");
    }

    void OnEnable()
    {
        moveTo.Enable();
        moveToLocation.Enable();

        // Subscribe to the performed event of moveTo
        moveTo.performed += OnMoveToPerformed;
    }

    void OnDisable()
    {
        moveTo.Disable();
        moveToLocation.Disable();

        // Unsubscribe from the event
        moveTo.performed -= OnMoveToPerformed;
    }

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // Handle speed modifier duration
        if (speedModifierDuration > 0f)
        {
            speedModifierDuration -= Time.deltaTime;
            if (speedModifierDuration <= 0f)
            {
                speedModifier = 1f;
            }
        }

        // Move towards the target position
        if (isMoving)
        {
            float step = currentSpeed * speedModifier * Time.deltaTime;
            // Move the player towards the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

            // Check if the player has reached the target position
            if (Vector2.Distance(transform.position, targetPosition) < 0.001f)
            {
                isMoving = false;
            }
        }
    }

    private void OnMoveToPerformed(InputAction.CallbackContext context)
    {
        // Get mouse position in screen coordinates
        Vector2 mouseScreenPosition = moveToLocation.ReadValue<Vector2>();

        // Convert to world coordinates
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = transform.position.z;

        targetPosition = mouseWorldPosition;
        isMoving = true;
    }

    // Method to apply speed modifiers from orbs
    public void ApplySpeedModifier(float modifier, float duration)
    {
        speedModifier = modifier;
        speedModifierDuration = duration;
    }
}
