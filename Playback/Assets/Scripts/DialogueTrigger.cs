using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool triggerOn;

    private void Start()
    {
        triggerOn = true;
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        triggerOn = false;
    }

    public void ResetTrigger()
    {
        triggerOn = true;
    }
}
