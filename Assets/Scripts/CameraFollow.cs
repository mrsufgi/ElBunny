using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{

    public GameObject Player;
    public Animator scoreAnimator;
    Player playerJump;

    private bool stopSound = false;

    // Use this for initialization
    void Start()
    {
        playerJump = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        if (stopSound)
        {
            //GetComponent<AudioSource>().Stop();
            scoreAnimator.SetBool("FadeOut", true);
        }
        else
        {
            stopSound = playerJump.firstJumpHappened;
        }

        if (Player != null && !LevelManager.manager.jumping)
        {
            transform.DOLocalMove(new Vector3(Player.transform.position.x + 5f, transform.position.y, -10), 3f);
        }
    }
}