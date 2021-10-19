using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    public Text uIText;
    public bool exit;
    //public AudioClip freezeClip;
    //public AudioClip playerHurtClip;
    public AudioSource audioSource;
    public GameObject pickUp;
    public MeshRenderer pickUpMeshRenderer;
    public Light pickUpLight;
    void Awake()
    {
        uIText = GameObject.Find("UIText").GetComponent<Text>();
        uIText.enabled = false;
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        //freezeClip = GetComponent<AudioClip>();
        //playerHurtClip = GetComponent<AudioClip>();
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h, v);
        Turning();
        Animating(h, v);
    }
    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }
    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }
    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);

    }
    void OnTriggerEnter(Collider other)
    {
        string msg = "";
        int type = -1;
        if (other.gameObject.CompareTag("2x Damage"))
        {   
            PlayerShooting.damagePerShot = 40;
            msg = "2x Damage for 15 seconds!";
            type = 0;
        }
        else if (other.gameObject.CompareTag("Freeze"))
        {
            audioSource.Play();
            EnemyMovement.freeze = true;
            msg = "Freeze enemies for 15 seconds!";
            type = 1;
        }
        if (type != -1)
        {
            other.gameObject.SetActive(false);
            StartCoroutine(ShowMessage(msg, 3));
            StartCoroutine(Delay(15, type));
        }
    }
    public IEnumerator ShowMessage(string message, float delay)
    {
        uIText.text = message;
        uIText.enabled = true;
        yield return new WaitForSeconds(delay);
        uIText.enabled = false;
    }
    public IEnumerator Delay(float delay, int type)
    {
        yield return new WaitForSeconds(delay);
        string msg = "";
        if (type == 0) //2xdamage
        {
             PlayerShooting.damagePerShot = 20;
            msg = "Normal Damage";
        }
        else if(type == 1) // freeze
        {
             EnemyMovement.freeze = false;
            msg = "Unfreeze";
            audioSource.Stop();
        }
        StartCoroutine(ShowMessage(msg, 3));
       
    }
}
