using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character : MonoBehaviour, IPunObservable
{
    [Header("Stats")]
    [SerializeField]
    private float speed = 600f;

    [SerializeField]
    private float jumpForce = 800f;

    [SerializeField]
    private Animator animator;

    private Rigidbody2D rb;
    private float desiredMovementAxis = 0f;


    private PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;

    private bool LookingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;
    }
    private void Update()
    {
        if (pv.IsMine)
        {
            CheckInputs();
        }
        else
        {
            SmoothReplicate();
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * speed, rb.velocity.y);
    }
    private void CheckInputs()
    {
            
            desiredMovementAxis = Input.GetAxisRaw("Horizontal");

            Flip(desiredMovementAxis);

            animator.SetFloat("Speed", Mathf.Abs(desiredMovementAxis));
            
            OnLanding();

        if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0f) && CheckGround.IsGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            animator.SetBool("IsJumping", true);
           
        }
        //Codido disparo
        if (Input.GetKeyDown(KeyCode.LeftControl)) { Disparo(); }
    }
    private void SmoothReplicate()
    {
        transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime * 20);
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            enemyPosition = (Vector3)stream.ReceiveNext();
        }
    }

    private void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }
    private void Disparo() 
    {
        PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
    }

    private void Flip(float desiredMovementAxis)
    {
        if (LookingRight == true && desiredMovementAxis < 0 || LookingRight == false && desiredMovementAxis > 0)
        {
            LookingRight = !LookingRight;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    public void Damage()
    {
        pv.RPC("NetworkDamage", RpcTarget.All);
    }
    [PunRPC]
    public void NetworkDamage()
    {
        //HACERSE DAÑO
        //Destroy(this.gameObject);
    }
}
