using UnityEngine;

/// <summary>
/// Controls behaviors that affect a building as a whole.
/// </summary>
public class Building : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] roofMeshRenderers;

    private void Start()
    {
        Player.OnAnyBuildingEntered += Player_OnAnyBuildingEntered;
        Player.OnAnyBuildingExited += Player_OnAnyBuildingExited;
    }

    private void Player_OnAnyBuildingEntered(object sender, Building building)
    {
        if (building != this)
        {
            return;
        }

        foreach (MeshRenderer meshRenderer in roofMeshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    private void Player_OnAnyBuildingExited(object sender, Building building)
    {
        if (building != this)
        {
            return;
        }

        foreach (MeshRenderer meshRenderer in roofMeshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }
}
