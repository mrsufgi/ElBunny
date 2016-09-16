using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LoseManager : MonoBehaviour
{
    public Canvas loseScreen;
    public Text highScore;
    public Text yourScore;
    public Animator scoreAnimator;
    public Player Player;

    public Image medal;

    public GameObject MockBackground;
    public GameObject themeObject;

    public Sprite background1;
    public Sprite background2;

    public Sprite medalBronze;
    public Sprite medalSilver;
    public Sprite medalGold;
    public Sprite medalAwesome;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int score = Player.GetComponent<Player>().score;
            /*
               MockBackground.GetComponent<Animator>().SetBool("Fade", true);

               Destroy(other.GetComponent<Collider2D>());
               other.GetComponent<Jump>().enabled = false;
               other.GetComponent<Rigidbody2D>().isKinematic = true;

              

               if (score > 9 && score < 20)
               {
                   medal.sprite = medalBronze;
               }
               else if (score > 19 && score < 30)
               {
                   medal.sprite = medalSilver;
               }
               else if (score > 29 && score < 50)
               {
                   medal.sprite = medalGold;
               }
               else if (score > 49)
               {
                   medal.sprite = medalAwesome;
               }


               LevelManager.manager.adsCounter--;
               if (LevelManager.manager.adsCounter <= 0) {
                   LevelManager.manager.DisplayAd();
               }
               */

            if (score > LevelManager.manager.highScore)
            {
                highScore.text = "" + score;
                LevelManager.manager.highScore = score;
                LevelManager.manager.Save();
            }
            else
            {
                highScore.text = "" + LevelManager.manager.highScore;
            }
            yourScore.text = "" + score;

            scoreAnimator.SetBool("FadeBar", true);
            loseScreen.enabled = true;

           
            var rigit = other.GetComponent<Rigidbody2D>();
            rigit.velocity = Vector3.zero;
            rigit.angularVelocity = 0;
            rigit.Sleep();
        }

     
    }

    public void Reset()
    {
        Player.Reset();
        scoreAnimator.SetBool("FadeBar", false);
        scoreAnimator.SetBool("FadeOut", false);
        loseScreen.enabled = false;

    }
}
