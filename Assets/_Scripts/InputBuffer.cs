using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    [SerializeField] private float bufferTime = 0.25f; // How much time before buffer resets

    private InputReader inputReader;
    private List<string> inputBuffer = new List<string>();
    private float bufferTimer;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        inputReader.OnJump += HandleJumpInput;
        inputReader.OnKick += HandleKickInput;
        inputReader.OnMoveStarted += HandleMoveInput;
        inputReader.OnPunch += HandlePunchInput;
        inputReader.OnSpecial += HandleSpecialInput;
    }

    private void OnDisable()
    {
        inputReader.OnJump -= HandleJumpInput;
        inputReader.OnKick -= HandleKickInput;
        inputReader.OnMoveStarted -= HandleMoveInput;
        inputReader.OnPunch -= HandlePunchInput;
        inputReader.OnSpecial -= HandleSpecialInput;
    }

    private void Update()
    {
        if (inputBuffer.Count > 0)
        {
            bufferTimer -= Time.deltaTime;
            if (bufferTimer <= 0f)
            {
                ProcessBuffer();
            }
        }
    }

    private void AddToBuffer(string input)
    {
        inputBuffer.Add(input);
        bufferTimer = bufferTime;
    }

    private void ProcessBuffer()
    {
        // I don't have anything to do with inputs right now so I am just going to log them to console and clear them
        Debug.Log(string.Join(", ", inputBuffer));

        inputBuffer.Clear();
        bufferTimer = 0f;
    }

    #region input handlers
    private void HandleMoveInput(Vector2 direction)
    {
        float xAbs = Mathf.Abs(direction.x);
        float yAbs = Mathf.Abs(direction.y);

        if (xAbs > 0.5f || yAbs > 0.5f)
        {
            if (xAbs > yAbs)
            {
                // Horizontal input
                if (direction.x > 0.55f) AddToBuffer("right");
                else if (direction.x < 0.45f) AddToBuffer("left");
            }
            else
            {
                // Vertical input
                if (direction.y > 0.55f) AddToBuffer("up");
                else if (direction.y < 0.45f) AddToBuffer("down");
            }
        }
    }
    private void HandleJumpInput() => AddToBuffer("jump");
    private void HandleKickInput() => AddToBuffer("kick");
    private void HandlePunchInput() => AddToBuffer("punch");
    private void HandleSpecialInput() => AddToBuffer("special");

    #endregion

    // Public methods
    public List<string> GetCurrentBuffer()
    {
        return new List<string>(inputBuffer); // I am making a new list because it protects the original list (if we return the other list it can get edited)
    }

    public void ClearBuffer()
    {
        inputBuffer.Clear();
        bufferTimer = 0f;
    }

}
