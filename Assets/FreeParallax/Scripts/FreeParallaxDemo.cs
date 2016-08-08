using System;
using UnityEngine;
using System.Collections;

public class FreeParallaxDemo : MonoBehaviour
{

    public FreeParallax parallax;
    public GameObject cloud;
    private Animator anim;
    private Animator anim2;

    // Use this for initialization
    void Start()
    {
        if (cloud != null)
        {
            cloud.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 0.0f);
        }

        // LeanTween.color(obj, rainbowColor, 6f);
        GameObject obj = GameObject.Find("bg_layer4");
        //AnimationClip[] clipContainer = obj.GetComponent<Animator>().runtimeAnimatorController.animationClips;
        //Debug.Log(clipContainer.Length);
        //Debug.Log(clipContainer[0].GetType());
        //clip.SetCurve("", typeof(SpriteRenderer), "m_Color.a", curve);
        //clip[0].SetCurve("", typeof(SpriteRenderer), "m_Color.r", new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(10, 120, 0, 0)));
        //AnimationCurve curve = AnimationCurve.Linear(0, 1, 2, 0);

        //clipContainer[0].SetCurve("", typeof(SpriteRenderer), "m_Color.r", curve);
        //clipContainer[0].SetCurve("", typeof(SpriteRenderer), "m_Color.g", curve);
        //clipContainer[0].SetCurve("", typeof(SpriteRenderer), "m_Color.b", curve);
        //clipContainer[0].SetCurve("", typeof(SpriteRenderer), "m_Color.a", curve);

        GameObject obj3 = GameObject.Find("bg_layer4(Clone)");
     /*   //print(obj3.name);
        float period = LeanTween.tau / 10 * 1;
        float red = Mathf.Sin(period + LeanTween.tau * 0f / 3f) * 0.5f + 0.5f;
        float green = Mathf.Sin(period + LeanTween.tau * 1f / 3f) * 0.5f + 0.5f;
        float blue = Mathf.Sin(period + LeanTween.tau * 2f / 3f) * 0.5f + 0.5f;
        Color rainbowColor = new Color(red, green, blue);
        LeanTween.color(obj3, rainbowColor, 6f);*/
       // LeanTween.color(obj, rainbowColor, 6f);

    }

    // Update is called once per frame
    void Update()
    {
       
        if (parallax != null)
        { 
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                parallax.Speed = 15.0f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                parallax.Speed = -15.0f;
            }
            else
            {
                //parallax.Speed = 1.0f;
            }
        }
    }
}
