using UnityEngine;

/// <summary>
/// Rotates the transform forward towards the target position.
/// </summary>
public class LookAtPosition : MonoBehaviour
{
    [SerializeField]
    private float turnSpeed = 10f;

    private Vector3? targetPosition;

    /// <summary>
    /// Set the target position.
    /// </summary>
    /// <param name="targetPosition">The position to rotate towards.</param>
    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    /// <summary>
    /// Clear the target position.
    /// </summary>
    public void ClearTargetPosition()
    {
        targetPosition = null;
    }

    private void Update()
    {
        if (!targetPosition.HasValue)
        {
            return;
        }

        Vector3 targetForward = (transform.position - targetPosition.Value).normalized * -1f;

        if (Vector3.Distance(transform.forward, targetForward) < 0.1f)
        {
            transform.forward = targetForward;
            targetPosition = null;
            return;
        }

        transform.forward = Vector3.Slerp(
            transform.forward,
            targetForward,
            turnSpeed * Time.deltaTime
        );
    }
}
