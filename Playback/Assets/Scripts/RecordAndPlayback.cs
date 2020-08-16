using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAndPlayback : MonoBehaviour
{
    public bool isRecording;
    public bool isPlayback;
    public List<Vector2> recordedPlayerVelocities;
    public playerMovement PlayerMovement;
    public GameObject RecordUI;
    public GameObject CornersUI;
    public GameObject PlayUI;
    public List<bool> runningInputs;
    public UpdateAnimator updateAnimator;
    public List<bool> launches;
    public List<bool> airborne;
    public GameObject avatarSpace;
    public List<float> runDir;

    // Start is called before the first frame update
    void Start()
    {
        isRecording = false;
        isPlayback = false;
        PlayUI.GetComponent<SpriteRenderer>().enabled = false;
        RecordUI.GetComponent<SpriteRenderer>().enabled = false;
        CornersUI.GetComponent<SpriteRenderer>().enabled = false;
        recordedPlayerVelocities = new List<Vector2>();
        runningInputs = new List<bool>();
        launches = new List<bool>();
        airborne = new List<bool>();
        runDir = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        // Start recording
        if (Input.GetKeyDown(KeyCode.L) && isRecording == false && isPlayback == false)
        {
            recordedPlayerVelocities.Clear();
            runningInputs.Clear();
            airborne.Clear();
            launches.Clear();
            runDir.Clear();
            isRecording = true;
            RecordUI.GetComponent<SpriteRenderer>().enabled = true;
            CornersUI.GetComponent<SpriteRenderer>().enabled = true;
        }

        // Stop recording
        else if (Input.GetKeyDown(KeyCode.L) && isRecording == true)
        {
            isRecording = false;
            RecordUI.GetComponent<SpriteRenderer>().enabled = false;
            CornersUI.GetComponent<SpriteRenderer>().enabled = false;
        }

        // Start playback
        else if (Input.GetKeyDown(KeyCode.P) && isRecording == false && isPlayback == false && recordedPlayerVelocities.ToArray().Length > 0)
        {
            isPlayback = true;
            PlayUI.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (isRecording)
        {
            // record velocity
            recordedPlayerVelocities.Add(PlayerMovement.rb.velocity);

            // record run animation
            runDir.Add(PlayerMovement.h);
            if (PlayerMovement.h == 0)
            {
                runningInputs.Add(false);
            }
            else
            {
                runningInputs.Add(true);
            }

            // record launches
            if (PlayerMovement.jumpCondition)
            {
                launches.Add(true);
            } else
            {
                launches.Add(false);
            }

            // record jumps
            if (PlayerMovement.collider != null)
            {
                airborne.Add(false);
            }
            else
            {
                airborne.Add(true);
            }
        }

        if (isPlayback)
        {
            if (recordedPlayerVelocities.ToArray().Length > 0)
            {
                PlayerMovement.rb.velocity = recordedPlayerVelocities[0];
                updateAnimator.setRun(runningInputs[0]);
                updateAnimator.animator.SetFloat("Direction", runDir[0]);
                updateAnimator.setJumping(airborne[0]);
                if (launches[0]) updateAnimator.launch();
                // Remove
                runDir.RemoveAt(0);
                recordedPlayerVelocities.RemoveAt(0);
                runningInputs.RemoveAt(0);
                launches.RemoveAt(0);
                airborne.RemoveAt(0);
                avatarSpace.GetComponent<Rigidbody2D>().gravityScale = 0;
            } else
            {
                isPlayback = false;
                PlayUI.GetComponent<SpriteRenderer>().enabled = false;
                avatarSpace.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }
    }


}
