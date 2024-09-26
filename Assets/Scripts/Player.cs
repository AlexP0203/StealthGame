using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices.WindowsRuntime;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask environmentOnly;
    [SerializeField] Animator anim;
    [SerializeField] PlayerStats stats;

    IA_PlayerActions playerActions;
    float groundCheckDistance = 1;
    Rigidbody rb;
    bool onGround = false;
    Vector2 input;
    Transform cam;


    // Start is called before the first frame update
    void Start()
    {
        playerActions = new IA_PlayerActions();
        playerActions.Player.Enable();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = (Physics.Raycast(transform.position, Vector3.up * -1, groundCheckDistance, environmentOnly));

        input = playerActions.Player.Move.ReadValue<Vector2>();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Sneak();
        }
        else
        {
            speed = 5;
            anim.SetBool("sneak", false);
        }
        
        

    }

    private void FixedUpdate()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camForward.Normalize();

        camRight.y = 0;
        camRight.Normalize();

        Vector3 forwardRelative = input.y * camForward;
        Vector3 rightRelative = input.x * camRight;

        Vector3 movementVector = (forwardRelative + rightRelative) * speed;
        movementVector.y = rb.velocity.y;
        rb.velocity = movementVector;

        anim.SetFloat("speed", movementVector.magnitude);

        if (movementVector.sqrMagnitude > .01f)
        {
            anim.transform.forward = movementVector;
        }
    }

    private void Sneak()
    {
        speed = 3;
        anim.SetBool("sneak", true);
    }
    
}
