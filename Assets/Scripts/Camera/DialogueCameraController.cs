using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the camera during dialogue.
/// </summary>
public class DialogueCameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject virtualCamera;

    [SerializeField]
    private LookAtPosition lookAtPosition;

    private void Start()
    {
        DialogueSystem.Instance.OnDialogueStarted += DialogueSystem_OnDialogueStarted;
        DialogueSystem.Instance.OnDialogueEnded += DialogueSystem_OnDialogueEnded;
        DialogueSystem.Instance.OnDialogueExchange += DialogueSystem_OnDialogueExchange;
    }

    private void DialogueSystem_OnDialogueStarted(object sender, List<Actor> actors)
    {
        Vector3 midPoint = Vector3.zero;
        Vector3 min = Vector3.zero;
        Vector3 max = Vector3.zero;
        foreach (Actor actor in actors)
        {
            midPoint += actor.transform.position;
            if (min == Vector3.zero || actor.transform.position.magnitude < min.magnitude)
            {
                min = actor.transform.position;
            }

            if (max == Vector3.zero || actor.transform.position.magnitude > max.magnitude)
            {
                max = actor.transform.position;
            }
        }

        midPoint /= actors.Count;
        Vector3 offsetA = Vector3.Cross((max - min), Vector3.up).normalized;
        Vector3 offsetB = Vector3.Cross((min - max), Vector3.up).normalized;
        Vector3 offset = offsetA.z < offsetB.z ? offsetA : offsetB;

        transform.position = midPoint + offset;
        virtualCamera.SetActive(true);
    }

    private void DialogueSystem_OnDialogueEnded(object sender, List<Actor> actors)
    {
        virtualCamera.SetActive(false);
    }

    private void DialogueSystem_OnDialogueExchange(object sender, DialogueExchange exchange)
    {
        Actor actor = DialogueSystem.Instance.GetActor(exchange.actorID);
        lookAtPosition.SetTargetPosition(actor.transform.position);
    }
}