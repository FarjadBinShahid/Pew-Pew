using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{

    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex = -1;

    //PlayerManager playerManager;


    const float maxHealth = 100;
    float currentHealth = maxHealth;

    private PhotonView PV;
    float horizontalInput;
    float verticalInput;
    public CharacterController controller;
    public float speed = 15;
    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 2;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    float mouseX;
    float mouseY;
    float xRotation;
    public float mouseSensitivity = 200;
    
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if(PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!PV.IsMine)
        {
            return;
        }
        Movement();

        // for(int i=0 ; i<=items.Length; i++)
        // {
        //     if(Input.GetKeyDown((i+1).ToString()))
        //     {
        //         EquipItem(i);
        //         break;
        //     }
        // }

        if(Input.GetKeyDown("1"))
        {
            EquipItem(0);
        }
        if(Input.GetKeyDown("2"))
        {
            EquipItem(1);
        }

        if(Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }
        
    }

    void LateUpdate()
    {
        if(!PV.IsMine)
        {
            return;
        }
        MouseRotate();
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

    void MouseRotate()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation , -75, 75);
        items[itemIndex].transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void EquipItem(int _index)
    {
        if(_index == previousItemIndex)
            return;
        
        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if(previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;
        SyncItem();
    }

    void SyncItem()
    {
        if(PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changeProps)
    {        
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changeProps["itemIndex"]);
        }
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]

    void RPC_TakeDamage(float damage)
    {
        if(!PV.IsMine)
        {
            return;
        }
        currentHealth -=damage;
        if(currentHealth<=0)
        {
            Die();
        }
        Debug.Log("Took Damage " + damage);
    }

    void Die()
    {

    }
}
