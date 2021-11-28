using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private PhotonView PV;
    float horizontalInput;
    float verticalInput;
    public CharacterController controller;
    public float speed = 15;
    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    float mouseX;
    public float mouseSensitivity = 100;
    

    void Start()
    {
        PV = GetComponent<PhotonView>();
        if(!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PV.IsMine)
        {
            return;
        }
        Movement();
        
    }

    void LateUpdate()
    {
        if(!PV.IsMine)
        {
            return;
        }
        RotateY();
    }

    void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    void RotateY()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    
}
