using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveAni : MonoBehaviour
{
    public FollowCamera flcam;                          
    public bool choose, check, Opening, die;

    public GameController gamecontroller;
    public CongAnimation dooranmation;
    public TurretAl turret;

    public GameObject Panel_GameOver;
    public GameObject gameOver;
    public GameObject watch_Video;

    public SkeletonAnimation anim;
    private Rigidbody2D rb;

    public float speed;
    public float jumpForce;

    //Kiem tra va cham --> Jump
    bool isGrounded;
    public Transform groundCehck;
    public LayerMask groundLayer;

    //kiem tra di chuyen
     bool moveRight, moveleft;

    public bool downded = false; // check cong tac da dc dam~ xuong'
    public Vector3 vstart;

    private AudioSource audioS;
    public AudioClip Select_Diamon, hoi_sinh, audio_die;

    [SpineAnimation] public string IdleAnim; //cac hanh dong
    [SpineAnimation] public string walkAnim;
    [SpineAnimation] public string jumpAnim;
    [SpineAnimation] public string isDie;
    [SpineAnimation] public string happy;
    [SpineAnimation] public string Win;
    [SpineAnimation] public string Fall_Down;

    [SpineAtlasRegion] public SpineSkin skin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveRight = false;
        moveleft = false;
        GetComponent<Image>();

        audioS = GetComponent<AudioSource>();

        //if (AdsManager.Instance != null)
        //{
        //    AdsManager.Instance.acVideoComplete += HandleVideoReward;
        //}
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.acVideoComplete += Revive;
        }
        //vstart = transform.position;      
    }
    private void OnDisable()
    {
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.acVideoComplete -= HandleVideoReward;

        }
        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.acVideoComplete -= Revive;
        }
    }
    private void HandleVideoReward()
    {
       // Application.LoadLevel(Application.loadedLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (!choose)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }
        Handmove();

        HandJump();
        MoveManage();

        isGrounded = Physics2D.OverlapCircle(groundCehck.position, 0.2f, groundLayer);

        if (rb.linearVelocity == Vector2.zero && !die && cam_Fail & Time.timeScale != 0)
        {
            flcam.hyo = true;
        }

    }
    bool cam_Fail = true;
    public void PlayAninmation(string _strAnim)
    {
        if (!anim.AnimationName.Equals(_strAnim))
        {

            anim.AnimationState.SetAnimation(0, _strAnim, true);
        }
    }
    public void PlayAninmationDie(string _strAnim)
    {
        if (!anim.AnimationName.Equals(_strAnim))
        {

            anim.AnimationState.SetAnimation(0, _strAnim, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (check)
        {
            if (collision.gameObject.tag == "Dimon2")
            {
                Destroy(collision.gameObject);
                gamecontroller.getScore2();

                audioS.clip = Select_Diamon;
                audioS.Play();
            }
            if (collision.gameObject.tag == "Finish2" && gamecontroller.num2 == 0)
            {
                dooranmation.OpenDoor2();
                Opening = true;

            }

            if (collision.gameObject.tag.Equals("Dieblue") || collision.gameObject.tag.Equals("Die") 
                                                    || (collision.gameObject.tag.Equals("trap")))
            {
                rb.linearVelocity = new Vector3(0f, 0f);
                Died();
                die = true;
                PlayAninmationDie(isDie);
                StartCoroutine(Dieing());
                // Panel_GameOver.SetActive(true);
                audioS.clip = audio_die;
                audioS.Play();
              //  StartCoroutine(Destroy_Player());
            }
          

            //check cong tac
            if (collision.gameObject.tag.Equals("Swich2"))
            {
                downded = true;
            }
            //check enemy
            if (collision.gameObject.tag.Equals("Enemy") && !die)
            {
                turret.turret_Awake();
                turret.check = false;
               
            }

            //check gio
            if (collision.gameObject.tag.Equals("Wind"))
            {
                if (rb.linearVelocity.y == 0)
                {
                    rb.linearVelocity = Vector2.up * 10f;

                }
                else
                {

                    rb.linearVelocity = Vector2.up * 1.5f;
                }
                PlayAninmationDie(Fall_Down);
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }


        else
        {
            if (collision.gameObject.tag == "Dimon")
            {
                Destroy(collision.gameObject);
                gamecontroller.getScore1();

                audioS.clip = Select_Diamon;
                audioS.Play();
            }
            if (collision.gameObject.tag == "Finish" && gamecontroller.num == 0)
            {
                dooranmation.OpenDoor1();
                Opening = true;

            }
            if (collision.gameObject.tag.Equals("DieRed") || collision.gameObject.tag.Equals("Die") 
                                                    || (collision.gameObject.tag.Equals("trap")))
                     
            {
                rb.linearVelocity = new Vector3(0f, 0f);
                Died();
                die = true;
                PlayAninmationDie(isDie);
                StartCoroutine(Dieing());

                audioS.clip = audio_die;
                audioS.Play();
              //  StartCoroutine(Destroy_Player());
            }
    
            //check cong tac
            if (collision.gameObject.tag.Equals("Swich2"))
            {
                downded = true;
            }
//check enemy
            if (collision.gameObject.tag.Equals("Enemy") )
            {
               
                turret.turret_Awake();
                turret.check = true;
            }

//check gio
            if (collision.gameObject.tag.Equals("Wind"))
            {
                if (rb.linearVelocity.y == 0)
                {
                    rb.linearVelocity = Vector2.up * 10f;
                 
                }
                else
                {
                   
                    rb.linearVelocity = Vector2.up * 1.5f;
                }
                PlayAninmationDie(Fall_Down);
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
        }

    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Wind")) {
            rb.linearVelocity =  Vector2.up* 1.5f;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            PlayAninmationDie(Fall_Down);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (check)
        {
            if (collision.gameObject.tag == "Finish2")
            {
                dooranmation.CloseDoor2();
                Opening = false;

            }


            if (collision.gameObject.tag.Equals("Swich2"))
            {
                downded = false;
            }

            if (collision.gameObject.tag.Equals("Enemy"))
            {
                turret.turret_Sleep();
            }
        }
        else
        {
            if (collision.gameObject.tag == "Finish")
            {
                dooranmation.CloseDoor1();
                Opening = false;
            }


            if (collision.gameObject.tag.Equals("Swich2"))
            {
                downded = false;
            }

            if (collision.gameObject.tag.Equals("Enemy"))
            {
                turret.turret_Sleep();
            }
        }

    }

    private void MoveManage()
    {
        if (moveRight)
        {
            rb.linearVelocity = new Vector2(+speed, rb.linearVelocity.y);
            anim.skeleton.ScaleX = 1;
            if (isGrounded)
            {
                PlayAninmation(walkAnim);
            }
            else
            {
                PlayAninmationDie(Fall_Down);
            }
        }
        //else
        //{
        //    rb.velocity = new Vector2(0f, rb.velocity.y);
        //}

        if (moveleft)
        {
            rb.linearVelocity = new Vector3(-speed, rb.linearVelocity.y);
            anim.skeleton.ScaleX = -1;
            if (isGrounded)
            {
                PlayAninmation(walkAnim);
            }
            else
            {
                PlayAninmationDie(Fall_Down);
            }
        }
    }
    public void MoveLeft()
    {

        if (!die )
        { 
            moveleft = true;

            flcam.hyo = false;
            
        }  
    }

    public void MoveRight()
    {
        if (!die)
        {
            moveRight = true;

            flcam.hyo = false;
        }      
    }

   public bool falldown;
    public void Jump()
    {
        
        if (!choose) return;
        if (!die)
        {
            
            if (isGrounded)
            {            
                    PlayAninmation(jumpAnim);
                    rb.linearVelocity = Vector2.up * jumpForce;
                    flcam.hyo = false;
                    isGrounded = false;
                    falldown = true;       
            }
            cam_Fail = false;
        }
    }
    public void stopjum()
    {
      
        if (!choose) return;
        if (!die)
        {

            if (falldown)
            {
                PlayAninmationDie(Fall_Down);
               
            }
            if (rb.linearVelocity.x == 0)
            {
                falldown = false;
                PlayAninmation(IdleAnim);
            }
            cam_Fail = true;
        }
    }

    public void StopMoving()
    {
        if (!die)
        {
            moveleft = false;
            moveRight = false;
          
            PlayAninmation(IdleAnim);
         
        }
    }

    public void Handmove()
    {
        if (!die)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                rb.linearVelocity = new Vector2(+speed, rb.linearVelocity.y);
                // anim.skeleton.FlipX = false;
                anim.skeleton.ScaleX = 1;

                if (isGrounded)
                {
                    PlayAninmation(walkAnim);
                }
                else
                {
                    PlayAninmationDie(Fall_Down);
                }
            }
            else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                PlayAninmation(IdleAnim);
            }
            else
            {
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
                //  PlayAninmation(IdleAnim);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                // anim.skeleton.FlipX = true;
                anim.skeleton.ScaleX = -1;
                if (isGrounded)
                {
                    PlayAninmation(walkAnim);

                }
                else
                {
                    PlayAninmationDie(Fall_Down);
                }
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                
                    PlayAninmation(IdleAnim);
              
            }

            if (Input.GetKey(KeyCode.H))
            {
                PlayAninmation(happy);
            }
            if (Input.GetKey(KeyCode.P))
            {
                PlayAninmation(Win);
            }
        }
    }

    bool dbjump;
    private void HandJump()
    {
        if (!die)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (isGrounded)
                {
                    //if (rb.velocity.y == 0)

                    rb.linearVelocity = Vector2.up * jumpForce;
                    PlayAninmation(jumpAnim);
                    dbjump = true;
                    falldown = true;
                    isGrounded = false;
                }            
                else if (dbjump)
                {
                    jumpForce = jumpForce / 1.5f;
                    rb.linearVelocity = Vector2.up * jumpForce;
                    PlayAninmation(jumpAnim);
                    dbjump = false;
                    jumpForce = jumpForce * 1.5f;

                }
                
            }

            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (falldown)
                {
                    PlayAninmationDie(Fall_Down);
                }

                if (rb.linearVelocity.x == 0)
                {
                    falldown = false;
                    PlayAninmation(IdleAnim);
                }
            }
        }
    }

    IEnumerator Dieing()
    {
        yield return new WaitForSeconds(1.5f);

        // AdsManager.Instance.ShowVideoReward();
      
        Panel_GameOver.SetActive(true);

    }

    public void Died()
    {
        moveleft = false;
        moveRight = false;
    }

    public void WatchVideo() /// ham goi quang cao 
    {
        AdsManager.Instance.ShowVideoReward();

    }

    private void Revive() /// sau kho xem xong video se hoi sinh nhan vat
    {
        if (choose)
        {
            die = false;
            PlayAninmation(IdleAnim);
            transform.position = AdsManager.Instance.vRevive;
            Panel_GameOver.SetActive(false);

            audioS.clip = hoi_sinh;
            audioS.Play();
        }
      
    }

    public void Close_WatchVideo() /// dong quang cao
    {
        gameOver.SetActive(true);
        watch_Video.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Snow"))
        {
            Debug.Log("Snowmotion");

            speed = 1;
            jumpForce = 4;

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Snow"))
        {
            Debug.Log("Snowmotion");       
          
                speed = 1;
                jumpForce = 4;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Snow"))
        {
            Debug.Log("Not Snowmotion");
            
                speed = 4;
                jumpForce = 9;
            
        }
    }

}






