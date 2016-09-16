using UnityEngine;
using System.Collections;
using Emotiv;

public class EmotivManager : MonoBehaviour {

    private EmoEngine engine;

    void Awake()
    {
        engine = EmoEngine.Instance;
        engine.EmoEngineConnected += new EmoEngine.EmoEngineConnectedEventHandler(EmotivConnected);
        engine.EmoEngineDisconnected += new EmoEngine.EmoEngineDisconnectedEventHandler(EmotivDisconnected);
        engine.EmoEngineEmoStateUpdated += new EmoEngine.EmoEngineEmoStateUpdatedEventHandler(EmotivStateUpdated);
        engine.Connect();
    }

    // Update is called once per frame
    void Update () {
        engine.ProcessEvents();
    }

    public void Close()
    {
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        engine.Disconnect();
    }

    void EmotivConnected(object sender, EmoEngineEventArgs e)
    {
        print("Connected!!");
    }

    void EmotivDisconnected(object sender, EmoEngineEventArgs e)
    {
        print("Disconnected!");
    }

    void EmotivStateUpdated(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState; 
    }


    void CaculateScale(double rawScore, double maxScale, double minScale, out double scaledScore)
    {
        if (rawScore < minScale)
            scaledScore = 0;
        else if (rawScore > maxScale)
            scaledScore = 1;
        else
            scaledScore = (rawScore - minScale) / (maxScale - minScale);
    }

}
