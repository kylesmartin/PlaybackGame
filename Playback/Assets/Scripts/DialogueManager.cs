using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text dialogueText;
    public Text nextOrClose;
    private Queue<string> sentences;
    public DialogueAnimator dialogueAnimator;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        if (dialogueAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Invisible"))
        {
            dialogueAnimator.animator.SetTrigger("DialogueOn");
        }
        nextOrClose.text = "Next";

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (sentences.Count == 0)
        {
            nextOrClose.text = "Close";
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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
        if (dialogueAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Visible"))
        {
            dialogueAnimator.animator.SetTrigger("DialogueOff");
        }
    }
}
