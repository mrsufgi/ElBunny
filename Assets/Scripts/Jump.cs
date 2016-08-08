using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Jump : MonoBehaviour
{

    public Image powerBar;
    private Rigidbody2D Player;
    private Animator CatAnimator;
    private float SoundPower, jumpPower, tempCounter;
    private bool time, jumpyReady, grounded;

    private bool cameraFinished = false;
    private bool startedRotating = false;

    public GameObject WallOfScore, smallJetStream, largeJetStream, startSmallJetStream;

    private ParticleSystem largeJetEmitter;

    bool firstParticleDestroy = true;

    public Animator plusAnimator;
    public Text scoreText, plusText, finalScore, highScore;
    public int score = 0;

    public bool firstJumpHappened = false;
    private AudioSource meowSource;

    // Use this for initialization
    void Start()
    {

        smallJetStream = null;
        largeJetEmitter = largeJetStream.GetComponent<ParticleSystem>();
        largeJetEmitter.Pause();

        powerBar.fillAmount = 0;
        CatAnimator = GetComponent<Animator>();
        Player = GetComponent<Rigidbody2D>();
        CatAnimator.SetBool("notgrounded", false);
        grounded = true;
        time = true;
        jumpyReady = false;
        meowSource = WallOfScore.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (startedRotating)
        {
            return;
        }

        // Doesn't move cat unless camera finished animation
        if (!cameraFinished)
        {
            //cameraFinished = LevelManager.manager.cameraFinished;
            return;
        }

        // this is backwards because it's 4 AM and I am way too fucking tired to change it
      /*  if (LevelManager.manager.touch)
        {

            /*SoundPower = GetComponent<SoundForm>().getLoudness() * 10 * LevelManager.manager.sliderValue;

            Debug.Log(SoundPower);

            if (SoundPower >= 5f && grounded)
            {
                if (!time)
                {
                    jumpyReady = true;
                    if (SoundPower > jumpPower)
                    {
                        jumpPower = SoundPower;
                    }
                }
                else
                {
                    if (smallJetStream == null)
                    {
                        if (firstParticleDestroy)
                        {
                            smallJetStream = (GameObject)Instantiate(startSmallJetStream, new Vector3(-2.277f, -6.872f, -3.25f), startSmallJetStream.transform.rotation);
                        }
                        else
                        {

                        }
                    }
                    jumpyReady = true;
                    powerBar.fillAmount = jumpPower / 10;
                    jumpPower += 0.2f;
                    //Debug.Log(jumpPower);

                }
            }
            #1#
            if (jumpyReady && SoundPower < 1 && grounded)
            {

                plusAnimator.SetBool("Increased", false);

                //meowSource.Play ();

                if (!firstJumpHappened)
                {
                    firstJumpHappened = true;
                }
                Destroy(smallJetStream);
                largeJetEmitter.Play();
                jumpPower = Mathf.Clamp(jumpPower, 2.5f, 10f);

                Player.velocity = new Vector3(0, jumpPower, 0);
                jumpPower = 0;
                CatAnimator.SetBool("notgrounded", true);
                grounded = false;
                jumpyReady = false;
            }

        }*/
        else
        {

            if (Input.touchCount > 0)
            {
                if (grounded)
                {

                    Touch touch = Input.touches[0];

                    if (smallJetStream == null)
                    {
                        if (firstParticleDestroy)
                        {
                            smallJetStream = (GameObject)Instantiate(startSmallJetStream, new Vector3(-2.277f, -6.872f, -3.25f), startSmallJetStream.transform.rotation);
                        }
                        else
                        {

                        }
                    }
                    jumpyReady = true;
                    powerBar.fillAmount = jumpPower / 10;
                    jumpPower += 0.2f;



                    switch (touch.phase)
                    {

                        case TouchPhase.Ended:
                            if (grounded)
                            {
                                plusAnimator.SetBool("Increased", false);

                                //meowSource.Play ();

                                if (!firstJumpHappened)
                                {
                                    firstJumpHappened = true;
                                }
                                Destroy(smallJetStream);
                                largeJetEmitter.Play();
                                jumpPower = Mathf.Clamp(jumpPower, 2.5f, 10f);

                                Player.velocity = new Vector3(0, jumpPower, 0);
                                jumpPower = 0;
                                CatAnimator.SetBool("notgrounded", true);
                                grounded = false;
                                jumpyReady = false;
                            }
                            break;
                    }
                }
            }
        }

        //Debug.Log(SoundPower);
        /*if (Input.GetKeyUp (KeyCode.Space) ) {
			grounded = false;
			power = GetComponent<SoundForm>().getLoudness() * 20f;
			power = Mathf.Clamp(power, 3, 10);
			Player.velocity = new Vector3(0, power, 0);
		}
		*/
        if (!grounded)
        {
            Vector3 newPos = new Vector3(0.045f, 0, 0);
            transform.position += newPos;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == 11)
        {
            CatAnimator.SetBool("Rotating", true);
            startedRotating = true;
        }

        if (collision.gameObject.tag == "Terrain")
        {

            /*int platformCount = WallOfScore.GetComponent<WallOfScore>().platformCount;

            WallOfScore.GetComponent<WallOfScore>().platformCount = 0;
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

            plusAnimator.SetBool("Increased", true);
            scoreText.text = "" + score;
            finalScore.text = "" + score;

            CatAnimator.SetBool("notgrounded", false);
            Debug.Log("no?");
            grounded = true;
            powerBar.fillAmount = 0;
            */
        }
    }

}