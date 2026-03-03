using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SearchService;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour //, IInteractable
{
  
    public float walkSpeed = 5.0f;
    public float runSpeed = 8.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;
    public float mouseSensitivity = 2.0f;
    public float lookXLimit = 25.0f;
    private float grabDistance = 1000000f;

    public Camera playerCamera;
    
    
    
    private CharacterController characterController;
    public GameObject lightSource;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    
    public GameObject heldItem1;
    public GameObject heldItem2;
    public GameObject hand1;
    public GameObject hand2;
    
    //private Material materialHand1, materialHand2;
    public Sprite[] textures = new Sprite[3];


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hand1 = transform.Find("Main Camera").Find("Hands").GetChild(0).GetChild(0).gameObject;
        hand2 = transform.Find("Main Camera").Find("Hands").GetChild(0).GetChild(1).gameObject;
        heldItem1 = null;
        heldItem2 = null;
        grabDistance = 10f;
        hand1.GetComponent<SpriteRenderer>().sprite = textures[0];
        hand2.GetComponent<SpriteRenderer>().sprite = textures[0];
        //materialHand1.mainTexture = textures[0];
        //materialHand2.mainTexture = textures[0];
        //hand1.transform.LookAt(playerCamera.transform, hand1.transform.forward);
        //hand2.transform.LookAt(playerCamera.transform, hand1.transform.forward);

    }

    void Update()
    {
        #region movement

        //Walking / Running
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * inputZ + right * inputX) * currentSpeed;

        //Jumping
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
        characterController.SimpleMove(8 * Vector3.down);
        #endregion

        #region camera
        //Camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        //hands looking at camera
        
        //hand1.transform.rotation.Set(hand1.transform.rotation.x, hand1.transform.rotation.y, 0, hand1.transform.rotation.w);
        //hand2.transform.rotation.Set(hand1.transform.rotation.x, hand1.transform.rotation.y, 0, hand1.transform.rotation.w);


        #endregion

        #region interaction
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray r = new Ray(playerCamera.transform.position, playerCamera.transform.forward); //send out raycast
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * grabDistance, UnityEngine.Color.white, 2f);
            if (Physics.Raycast(r, out RaycastHit hit, grabDistance))
            {

                if (hit.collider.gameObject.transform.CompareTag("Stationary") || hit.collider.gameObject.transform.CompareTag("Pickup"))
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        Interact(hand1, hit.collider.gameObject);
                        SoundFX_Manager.Instance.Play(SoundFX_Manager.SoundType.Item_Pickup); //Temp Sound
                    }
                    else
                    {
                        Interact(hand2, hit.collider.gameObject);
                        SoundFX_Manager.Instance.Play(SoundFX_Manager.SoundType.Item_Pickup); //Temp Sound
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        Interact(hand1);
                    }
                    else
                    {
                        Interact(hand2);
                    }
                }
                
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Interact(hand1);
                }
                else
                {
                    Interact(hand2);
                }
            }
        }

        // dropping code
        if (Input.GetKeyDown(KeyCode.Alpha1) && heldItem1 != null)
        {
            heldItem1.GetComponent<Interactable>().Drop(hand1.transform);
            heldItem1 = null;
            hand1.GetComponent<SpriteRenderer>().sprite = textures[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && heldItem2 != null)
        {
            heldItem2.GetComponent<Interactable>().Drop(hand2.transform);
            heldItem2 = null;
            hand2.GetComponent<SpriteRenderer>().sprite = textures[0];
        }
        #endregion
    }

    public void Interact(GameObject hand, GameObject obj)
    {
        //picks what hand were using and finds the item associated with said hand
        GameObject holding = heldItem1;
        if (hand == hand2)
        {
            holding = heldItem2;
        }

        if (obj.CompareTag("Stationary"))
        {
            if(holding == null)
            {
                obj.GetComponent<Interactable>().Use();
            }
            else
            {
                obj.GetComponent<Interactable>().Use(holding);
            }
        }
        else if ((holding == null) &&
                obj.CompareTag("Pickup"))
        {
            //replace hand sprite with held object sprite
            hand.GetComponent<SpriteRenderer>().sprite = textures[obj.GetComponent<Interactable>().GetSpriteID()];

            //pickup code
            holding = obj;
            obj.GetComponent<Interactable>().PickUp();

            //Set obj to heldItem
            if(hand == hand1)
            {
                heldItem1 = obj;
            }
            else
            {
                heldItem2 = obj;
            }
        }
        else
        {
            Debug.Log("failed to pickup");
        }
    }
    public void Interact(GameObject hand)
    {
        //picks what hand were using
        GameObject holding = heldItem1;
        if (hand == hand2)
        {
            holding = heldItem2;
        }

        //actual use part
        if (holding != null)
        {
            holding.GetComponent<Interactable>().Use(lightSource);
            
        }
    }
}