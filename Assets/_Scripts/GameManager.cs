using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DynamicCamera cam;
    
    public Transform[] spawnPoints; // Set spawn points in Inspector
    private int nextSpawnIndex = 0;

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"Player {playerInput.playerIndex} joined with {playerInput.devices[0]}");

        // Add player as target to camera
        cam.AddTarget(playerInput.gameObject.transform);

        // Assign a spawn position
        playerInput.transform.position = spawnPoints[nextSpawnIndex].position;
        nextSpawnIndex = (nextSpawnIndex + 1) % spawnPoints.Length;
    }
}
