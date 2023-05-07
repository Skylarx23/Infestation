using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    public float speed = 12f;
    public Vector3 velocity;
    public float gravity = -9.81f;
    private bool CanDash = true;

    [SerializeField] Transform groundCheck;
    public float groundRadius = 0.4f;
    public LayerMask groundLayer;

    bool isGrounded;
    public float jumpHeight = 20f;
    public Vector3 startPosition;
    Vector3 move;

    // Start is called before the first frame update
    void Start()
    {   
        // initializing start position
        startPosition = new Vector3(0,2,0);
    }

    // Update is called once per frame
    void Update()
    {
        // Creates a small sphere to check if the player is touching anything with "ground" layer
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && CanDash)
        {
            StartCoroutine(Dash());
        }

            // Sets the varibles to the Key's direction 
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

        // Moves the player (Relative to Itself) using the previous varibles 
        Vector3 move = transform.right * x + transform.forward * z;

        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 24f;
            controller.Move(move * speed * Time.deltaTime);
        }
        else
        {
            speed = 12f;
            controller.Move(move * speed * Time.deltaTime);
        }

        // Applys Gravity To The Player
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // removed for the moment, might come back
        //if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        //{
        //velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}

        // if the player goes below y -5 they are jumped back to the start, incase they fall off of the map


        if (groundCheck.position.y  < -5)
            {
                transform.position = startPosition;
            }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dashed");
        float dashCooldown = 2f;
        // CanDash = false;
        controller.Move(move * 3000f * Time.deltaTime);
        yield return new WaitForSeconds(dashCooldown);
        CanDash = true;
    }
}