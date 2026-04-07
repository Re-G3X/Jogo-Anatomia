using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Sprite characterImage;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        TriggerDialogue();        
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, characterImage);
    }
}
