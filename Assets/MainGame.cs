using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;
using DG.Tweening;
using Random = UnityEngine.Random;

public class MainGame : MonoBehaviour
{

    public List<Floaties> Floties;

    private Floaties m_ActiveFloatie;
    private Floaties m_PassiveFloatie;
    private Floaties m_OffFloatie;
    private int currentFloatieState = 0;
    private const float c_OffScreenPositionX = 15f;
    private float width;
    public Player Bunny;
    public FloatieManager Floaties;

    void Next()
    {

        m_ActiveFloatie = Floties[currentFloatieState % 3];
   //     m_ActiveFloatie.transform.localScale = new Vector3(width / 15.0f, width / 15.0f, width / 15.0f);
        m_ActiveFloatie.transform.name = "Active";
        m_PassiveFloatie = Floties[(currentFloatieState + 2) % 3];
        m_PassiveFloatie.transform.name = "Passive";
        m_OffFloatie = Floties[(currentFloatieState + 1) % 3];
        m_OffFloatie.transform.name = "Off";
        currentFloatieState++;
    }

    //TODO ooohhh find a way better way to do it.
    void Update()
    {

        if (LevelManager.manager.next)
        {
            if (!LevelManager.manager.jumping)
            {
                LevelManager.manager.next = false;
                Bunny.MoveToEndOnFloatie();

                Floaties.Reposition();
            }
        }
    }
    void Start()
    {
        /*   width = Screen.width;

           DOTween.defaultEaseType = Ease.InOutCubic;
           DOTween.defaultEasePeriod = 3f;
*/
        // Next();

        /*   m_ActiveFloatie.Spawn(4);
           m_ActiveFloatie.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(width / 2, 0, 10));
           m_PassiveFloatie.Spawn(3);
           m_PassiveFloatie.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x - m_PassiveFloatie.m_Tiles.Count + 1f, 0);
           m_OffFloatie.Spawn(3);
           m_OffFloatie.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(width, 0, 10));
*/
        /* Bunny.transform.SetParent(m_ActiveFloatie.transform, true);*/



        /* TKLongPressRecognizer recognizer = new TKLongPressRecognizer
          {
              minimumPressDuration = 0.01f,
              allowableMovementCm = 50f
          };


         recognizer.gestureRecognizedEvent += (r) =>
            {
                Debug.Log("tap recognizer fired: " + r);
            };
            TouchKit.addGestureRecognizer(recognizer);


         recognizer.gestureCompleteEvent += (r) =>
         { 
            // Bunny.MoveToEndOnFloatie();
            // Floaties.Reposition();
          /*   Bunny.transform.DOLocalMoveX(Bunny.m_Tiles.Count - 0.5f, 2f).OnPlay(() =>
             {
                 var anim = Bunny.GetComponent<Animator>();
                 anim.SetBool("isRun", true);


             }).OnComplete(() =>
             {
                 var anim = Bunny.GetComponent<Animator>();
                 anim.SetBool("isRun", false);
             });#1#
         };
         /*  
                 GameObject obj3 = GameObject.Find("bg_layer4(Clone)");
                  print(obj3.name);
                  float period = LeanTween.tau / 10 * i;
                  float red = Mathf.Sin(period + LeanTween.tau * 0f / 3f) * 0.5f + 0.5f;
                  float green = Mathf.Sin(period + LeanTween.tau * 1f / 3f) * 0.5f + 0.5f;
                  float blue = Mathf.Sin(period + LeanTween.tau * 2f / 3f) * 0.5f + 0.5f;
                  Color rainbowColor = new Color(red, green, blue);
                  LeanTween.color(obj3, rainbowColor, 6f);

                  GameObject obj = GameObject.Find("bg_layer4");
                  LeanTween.color(obj, rainbowColor, 6f);#2#
                 //generateRandomTiles();


                 float xCount = Random.Range(1, 6);
                 float yCount = Random.Range(0, -5);



                 m_OffFloatie.transform.DOMoveX(xCount, 3f);

                 m_ActiveFloatie.transform.DOMoveX(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x - m_ActiveFloatie.m_Tiles.Count + 1f, 3f);

                 Bunny.transform.DOLocalMoveX(m_ActiveFloatie.m_Tiles.Count - 0.5f, 2f).OnPlay(() =>
                 {
                    var anim =  Bunny.GetComponent<Animator>();
                     anim.SetBool("isRun", true);


                 }).OnComplete(() =>
                 {
                     var anim = Bunny.GetComponent<Animator>();
                     anim.SetBool("isRun", false);
                 });
                 m_PassiveFloatie.transform.DOMoveX(-c_OffScreenPositionX, 3f).OnComplete(() =>
                 {
                     print("Despawn");
                     m_OffFloatie.DespawnFloatie();
                     m_OffFloatie.transform.position = new Vector3(c_OffScreenPositionX, 0, 0);
                     m_OffFloatie.Spawn(xCount, yCount);
                 });

                 Next();

                 mySequence = DOTween.Sequence();

                 mySequence.Append(DOTween.To(() => parallax.Speed, x => parallax.Speed = x, 15f, 1f).SetEase(Ease.InOutCubic, 4f));
                 mySequence.AppendInterval(1f);
                 mySequence.Append(DOTween.To(() => parallax.Speed, x => parallax.Speed = x, 0, 1f).SetEase(Ease.InOutCubic, 4f));
                 mySequence.Play();

                 i++;
             };#1#
 */
    }
}


    //Sequence mySequence;
//}
