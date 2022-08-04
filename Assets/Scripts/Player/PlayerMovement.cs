using UnityEngine;

/// <summary>
/// Controls player movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float sneakSpeed;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;

    private Vector3 targetMovement;

    private bool isSneaking;

    /// <summary>
    /// Get whether the player is sneaking.
    /// </summary>
    /// <returns>Whether the player is sneaking.</returns>
    public bool GetIsSneaking()
    {
        return isSneaking;
    }
}
