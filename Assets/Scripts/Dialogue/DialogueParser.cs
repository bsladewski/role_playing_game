using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parses dialogue from text files.
/// </summary>
public class DialogueParser
{
    private Queue<DialogueExchange> dialogueExchanges;

    private Regex actorRegex = new Regex(@"^(\[\s*)([\w|\.]+)(\s*\])$");
    private Regex emotionRegex = new Regex(@"^(\(\s*)([\w|\.]+)(\s*\)\s*)(.*)$");

    private string actorID;
    private string emotion;

    private bool isOneShot;

    /// <summary>
    /// Parses the supplied dialogue text.
    /// </summary>
    /// <param name="dialogueText">A text file containing dialogue.</param>
    public DialogueParser(TextAsset dialogueText)
    {
        dialogueExchanges = ParseDialogueText(dialogueText);
        if (dialogueExchanges.Count == 0)
        {
            throw new Exception($"Failed to parse dialogue: {dialogueText.name}");
        }

        isOneShot = dialogueExchanges.Count == 1;
    }

    /// <summary>
    /// Checks if this is a one-shot dialogue.
    /// </summary>
    /// <returns>Whether this is a one-shot dialogue.</returns>
    public bool IsOneShot()
    {
        return isOneShot;
    }

    /// <summary>
    /// Checks if additional dialogue exchanges exist.
    /// </summary>
    /// <returns>Whether there are more dialogue exchanges.</returns>
    public bool HasNext()
    {
        return dialogueExchanges.Count > 0;
    }

    /// <summary>
    /// Get the next dialogue exchange.
    /// </summary>
    /// <returns>The next dialogue exchange.</returns>
    public DialogueExchange Next()
    {
        return dialogueExchanges.Dequeue();
    }

    private Queue<DialogueExchange> ParseDialogueText(TextAsset dialogueText)
    {
        Queue<DialogueExchange> dialogueExchanges = new Queue<DialogueExchange>();

        StringReader reader = new StringReader(dialogueText.text);
        List<string> lines = new List<string>();
        string next = reader.ReadLine();
        while (next != null)
        {
            string line = next.Trim();
            next = reader.ReadLine();

            // check if the current line sets the actor
            Match match = actorRegex.Match(line);
            if (match.Success)
            {
                actorID = match.Groups[2].Value;
                continue;
            }

            // check if the current line specifies an emotion
            match = emotionRegex.Match(line);
            if (match.Success)
            {
                emotion = match.Groups[2].Value;
                line = match.Groups[4].Value;
            }
            else
            {
                emotion = "";
            }

            if (line == "")
            {
                // skip blank lines
                continue;
            }

            dialogueExchanges.Enqueue(new DialogueExchange(
                actorID, GetDialogueEmotion(), line
            ));
        }

        return dialogueExchanges;
    }

    private DialogueEmotion GetDialogueEmotion()
    {
        switch (emotion)
        {
            case "angry":
                return DialogueEmotion.ANGRY;
            case "excited":
                return DialogueEmotion.EXCITED;
            case "sad":
                return DialogueEmotion.SAD;
            case "scared":
                return DialogueEmotion.SCARED;
            default:
                return DialogueEmotion.NEUTRAL;
        }
    }
}
