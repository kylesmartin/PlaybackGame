using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumpVelocity;
    public bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public RecordAndPlayback recordAndPlayback;
    public float rememberGroundedFor;
    public float lastTimeGrounded;
    public UpdateAnimator updateAnimator;
    public GameObject player;
    public GameObject spawn;
    public float h;
    public bool jumpCondition;
    public Collider2D collider;
    public GameObject hintButton;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -20f)
        {
            transform.position = spawn.transform.position;
            recordAndPlayback.recordedPlayerVelocities.Clear();
            recordAndPlayback.runningInputs.Clear();
            recordAndPlayback.launches.Clear();
            recordAndPlayback.airborne.Clear();
            recordAndPlayback.runDir.Clear();
            recordAndPlayback.RecordUI.GetComponent<SpriteRenderer>().enabled = false;
            recordAndPlayback.CornersUI.GetComponent<SpriteRenderer>().enabled = false;
            recordAndPlayback.PlayUI.GetComponent<SpriteRenderer>().enabled = false;
            DialogueTrigger[] triggers = FindObjectsOfType<DialogueTrigger>();
            foreach (DialogueTrigger trigger in triggers) {
                trigger.ResetTrigger();
            }
        } else if (!recordAndPlayback.isPlayback)
        {
            Move();
            CheckIfGrounded();
            Jump();
            BetterJump();
        } else
        {
            isGrounded = false;
        }
    }

    void Move()
    {
        h = Input.GetAxis("Horizontal");
        float moveBy = h * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);
        updateAnimator.updateRunningLoop();
    }

    void Jump()
    {
        jumpCondition = Input.GetKeyDown(KeyCode.W) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor);
        if (jumpCondition)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            updateAnimator.launch();
        }
    }

    void CheckIfGrounded()
    {
        collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            isGrounded = true;
            updateAnimator.setJumping(false);
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
            updateAnimator.setJumping(true);
        }
    }

    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SpawnPoint")
        {
            spawn = collision.gameObject;
        } else if (collision.tag == "EndPoint")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else if (collision.tag == "Dialogue" && collision.GetComponent<DialogueTrigger>().triggerOn)
        {
            collision.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
            if (collision.gameObject.name == "DialogueTrigger3")
            {
                hintButton.GetComponent<Image>().enabled = true;
                hintButton.GetComponent<Button>().enabled = true;
                hintButton.GetComponentInChildren<Text>().enabled = true;
            }
        } else if (collision.tag == "Hint")
        {
            hintButton.GetComponent<DialogueTrigger>().dialogue = collision.gameObject.GetComponent<DialogueTrigger>().dialogue;
        }
    }
}
