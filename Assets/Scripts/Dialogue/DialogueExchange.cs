/// <summary>
/// Represents a single exchange during a dialogue.
/// </summary>
public struct DialogueExchange
{
    /// <summary>
    /// Identifies the actor who is speaking.
    /// </summary>
    public string actorID;

    /// <summary>
    /// The emotion of the speaking actor.
    /// </summary>
    public DialogueEmotion emotion;

    /// <summary>
    /// The text contents of this exchange.
    /// </summary>
    public string text;

    public DialogueExchange(string actorID, DialogueEmotion emotion, string text)
    {
        this.actorID = actorID;
        this.emotion = emotion;
        this.text = text;
    }
}
