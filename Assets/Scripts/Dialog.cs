using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string name;

    [TextArea(3, 10)] // Allows for multi-line text input in the inspector
    public string[] sentences;
}
