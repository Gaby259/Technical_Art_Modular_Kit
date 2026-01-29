using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerControllerConfig", menuName = "Scriptable Objects/PlayerControllerConfig")]
public class PlayerControllerConfig : ScriptableObject
{

    [Header("Movement")]
    [field: SerializeField] public float MovementSpeed { get; private set; } = 6f;
    public float groundAcceleration = 10f;
    public float groundDeceleration = 10f;
    public float accelerationMultiplier = 2f;

    [Header("Jump")]
    public float jumpHeight = 1.5f;   // altura real del salto
    public float gravity = 1f;        // multiplicador de Physics.gravity

    [Header("Air Control")]
    [field: SerializeField] public float AirAcceleration { get; private set; } = 5f;

    [Header("Rotation")]
    [field: SerializeField] public float CameraBounds { get; private set; } = 80f;
    public float mouseSensitivity = 100f;
}
