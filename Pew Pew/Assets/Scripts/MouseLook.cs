using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    
    float mouseY;
    float xRotation;
    public float mouseSensitivity = 100;
    //public Transform playerBody;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       MouseInput();
    }

    void MouseInput()
    {
        
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation , -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
}
