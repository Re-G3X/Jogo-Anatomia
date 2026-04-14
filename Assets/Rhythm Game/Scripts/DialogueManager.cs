using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public TMP_Text buttonText;

    public Image characterImage;

    public Animator animator;

    private Queue<DialogueLine> lines;

    [SerializeField] private string nextSceneName;

    void Start()
    {
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        lines.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            lines.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines.Dequeue();

        nameText.text = line.name;
        characterImage.sprite = line.sprite;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.sentence));

        if (lines.Count == 0)
            buttonText.text = "Começar \u2192";
        else
            buttonText.text = "Continuar \u2192";
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(0.3f); 
        SceneManager.LoadScene(nextSceneName);
    }
}