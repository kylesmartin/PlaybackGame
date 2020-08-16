using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAnimator : MonoBehaviour
{
    public Animator animator;
    public RecordAndPlayback recordAndPlayback;
    public playerMovement PlayerMovement;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        animator.SetFloat("Time", time);
    }

    // Update is called once per frame
    public void updateRunningLoop()
    {
        if (!recordAndPlayback.isPlayback)
        {
            if (PlayerMovement.h == 0)
            {
                setRun(false);
            }
            else
            {
                setRun(true);
                setDirection(PlayerMovement.h);
            }
        }
    }

    public void setRun(bool state)
    {
        animator.SetBool("isRunning", state);
    }

    public void launch()
    {
        time = 0f;
        animator.SetFloat("Time", time);
        animator.SetTrigger("Launch");
    }

    public void setJumping(bool state)
    {
        animator.SetBool("isJumping", state);
    }

    public void setDirection(float dir)
    {
        animator.SetFloat("Direction", dir);
    }
}
