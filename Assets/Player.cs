using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRigidbody2D;
    private Animator animator;
    private Floaties m_OnFloatie;

    private float jumpForce;
    private bool isJumping;
    private TKLongPressRecognizer recognizer;


    public Image powerBar;
    private float jumpPower, tempCounter;
    //private bool time, jumpyReady, grounded;

    // private bool cameraFinished = false;
    //private bool startedRotating = false;

    //public GameObject WallOfScore, smallJetStream, largeJetStream, startSmallJetStream;

    //private ParticleSystem largeJetEmitter;

    //bool firstParticleDestroy = true;

    public Animator plusAnimator;
    public Text scoreText, plusText, finalScore, highScore;
    public int score;
    public float angle;

    // Use this for initialization

    IEnumerator MyCoroutine()
    {
        while (charging)
        {
            jumpForce += 0.25f;
          //  print("COROUTINE");
            powerBar.DOFillAmount(jumpForce / 10, 0.025f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private Coroutine co;
    private bool charging = false;
    void Start()
    {

        powerBar.fillAmount = 0;
        score = 0;


        isJumping = false;
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerRigidbody2D.freezeRotation = true;
        animator = GetComponent<Animator>();

        recognizer = new TKLongPressRecognizer
        {
            minimumPressDuration = 0.01f,
            allowableMovementCm = 50f
        };

        recognizer.gestureRecognizedEvent += (r) =>
        {
            //    Debug.Log("tap recognizer fired: " + r);
            //   print("ChARGING!");
            if (LevelManager.manager.playing)
            {
                charging = true;
                co = StartCoroutine(MyCoroutine());
            }
        };

        recognizer.gestureCompleteEvent += (r) =>
        {

            //  print("Fly!");

            if (LevelManager.manager.playing)
            {
                charging = false;
                transform.SetParent(null, true);
                jumpForce = Mathf.Clamp(jumpForce, 0f, 10f);
                jumpForce *= 110;
              //  print(jumpForce);
              //  print(angle);

                //angle = Math.Abs(angle);
                //angle += 45;

                // playerRigidbody2D.AddForce((Vector2.up * 2/3 + Vector2.right * 1/3) * jumpForce, ForceMode2D.Force);

                Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
                playerRigidbody2D.AddForce((dir / 3 + Vector3.up * 2 / 3) * jumpForce, ForceMode2D.Force);
                isJumping = true;
                LevelManager.manager.jumping = true;
                animator.SetBool("isJump", true);

                jumpForce = 0;

                LevelManager.manager.next = true;

                // first jump
                if (!LevelManager.manager.firstJumpHappened)
                {
                    LevelManager.manager.firstJumpHappened = true;
                }

                print("removing");
                TouchKit.removeGestureRecognizer(recognizer);

            }

        };


        TouchKit.addGestureRecognizer(recognizer);
    }

    // Update is called once per frame
    void Update()
    {



    }

   public void Reset()
    {
        transform.position = new Vector3(jumpedTileCollider.transform.position.x, 15f, 0);
        score = 0;
        animator.SetBool("isJump", false);  
        playerRigidbody2D.WakeUp();
       isJumping = false;
       jumpedTileCollider.isTrigger = false;
        TouchKit.addGestureRecognizer(recognizer);
        powerBar.DOFillAmount(0, 1f);
       scoreText.text = score + "";
    }



    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Edge")
        {
            isJumping = false;


            // animator.SetBool("isJump", false);
        }
    }

    private Collider2D jumpedTileCollider;

    void OnCollisionExit2D(Collision2D coll)
    {
        jumpedTileCollider = coll.gameObject.GetComponent<Collider2D>();
        jumpedTileCollider.isTrigger = true;
    }

    private int point = 1;
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (LevelManager.manager.playing)
        {
            if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Edge")
            {

                if (jumpedTileCollider != coll.gameObject.GetComponent<Collider2D>())
                {
                    // isJumping = false;
                    transform.SetParent(coll.gameObject.transform.parent);
                    m_OnFloatie = transform.parent.GetComponent<Floaties>();
                    animator.SetBool("isJump", false);
                    LevelManager.manager.jumping = false;

                    int platformCount = point;

                    plusAnimator.SetBool("Increased", true);

                    switch (platformCount)
                    {
                        case 1:
                            score += 1;
                            plusText.text = "+1";
                            break;
                        case 2:
                            score += 4;
                            plusText.text = "+4";
                            break;
                        case 3:
                            score += 8;
                            plusText.text = "+8";
                            break;
                        case 4:
                            score += 16;
                            plusText.text = "+16";
                            break;
                    }
                    Debug.Log("FU");
                    plusAnimator.SetBool("Increased", true);
                    scoreText.text = "" + score;
                    //finalScore.text = "" + score;
                    point = 0;

                }
            }
        }
    }

    public void MoveToEndOnFloatie()
    {

        powerBar.DOFillAmount(0, 2f);
        transform.DOLocalMoveX(m_OnFloatie.size - 0.5f, 2f).OnPlay(() =>
        {
            var anim = GetComponent<Animator>();
            anim.SetBool("isRun", true);


        }).OnComplete(() =>
        {
            var anim = GetComponent<Animator>();
            anim.SetBool("isRun", false);
            plusAnimator.SetBool("Increased", false);
            point = 1;
            jumpedTileCollider.isTrigger = false;
            TouchKit.addGestureRecognizer(recognizer);
        });

    }
}
