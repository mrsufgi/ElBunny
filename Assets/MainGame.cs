using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour
{
    public int i = 0;
    void Start()
    {

        TKLongPressRecognizer recognizer = new TKLongPressRecognizer();
        recognizer.minimumPressDuration = 0.01f;
        recognizer.allowableMovementCm = 50f;
        recognizer.gestureRecognizedEvent += (r) =>
        {
            Debug.Log("tap recognizer fired: " + r);
        };
        TouchKit.addGestureRecognizer(recognizer);

        recognizer.gestureCompleteEvent += (r) =>
        {
            GameObject obj3 = GameObject.Find("bg_layer4(Clone)");
            print(obj3.name);
            float period = LeanTween.tau / 10 * i;
            float red = Mathf.Sin(period + LeanTween.tau * 0f / 3f) * 0.5f + 0.5f;
            float green = Mathf.Sin(period + LeanTween.tau * 1f / 3f) * 0.5f + 0.5f;
            float blue = Mathf.Sin(period + LeanTween.tau * 2f / 3f) * 0.5f + 0.5f;
            Color rainbowColor = new Color(red, green, blue);
            LeanTween.color(obj3, rainbowColor, 6f);
     
            GameObject obj = GameObject.Find("bg_layer4");
            LeanTween.color(obj, rainbowColor, 6f);
            i++;
        };


    }
}
