using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string name;
    public Sprite sprite;

    [TextArea(3, 10)]
    public string sentence;
}

[System.Serializable]
public class Dialogue
{
    public DialogueLine[] lines;
}