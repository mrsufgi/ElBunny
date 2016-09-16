using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;
    public bool cameraFinished, mute, touch;
    public bool first;
    public int highScore;
    public float sliderValue = 0.5f;
    public int adsCounter;
    public bool disableMicMenuSlider;
    public bool firstJumpHappened = false;
    public Animator animatorCamera, menuAnimator, logoAnimator, scoreAnimator;
    public bool firstRun;
    public Canvas menuCanvas, scoreCanvas, logoCanvas, LoseCanvas;

    public bool playing = false;


    public bool jumping = false;
    public bool next = false;

    private string fileEnding = ".dat";

    void Awake()
    {

         highScore = Load();

         if (!first)
         {
             Debug.Log("First Time");
             first = true;
             touch = false;
             sliderValue = 0.5f;
             Save();
         }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        /*
       if (Advertisement.isSupported) {
           Advertisement.allowPrecache = true;
           Advertisement.Initialize("30796", false);
       }*/


        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }



    void Update()
    {
        if (animatorCamera != null)
        {

            if (animatorCamera.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
            {
                animatorCamera.enabled = false;
                scoreCanvas.enabled = true;
                scoreAnimator.enabled = true;
                manager.playing = true;
                cameraFinished = true;
            }

            if (animatorCamera.GetCurrentAnimatorStateInfo(0).IsName("Keep State") && firstRun)
            {
                menuAnimator.enabled = true;
                menuAnimator.SetBool("Fade", true);
            }

        }

        if (firstJumpHappened)
        {
            //GetComponent<AudioSource>().Stop();
            scoreAnimator.SetBool("FadeOut", true);
        }
    }
    void Start()
    {

        cameraFinished = false;

        if (adsCounter == 0)
        {
            adsCounter = 10;
        }

        scoreAnimator.enabled = false;
        // logoAnimator.enabled = true;
        animatorCamera.enabled = true;
        scoreCanvas.enabled = false;
        menuCanvas.enabled = true;
        menuCanvas.GetComponent<CanvasGroup>().alpha = 0;
        menuAnimator.enabled = false;
        firstRun = true;
        
    }

    public void Play()
    {
       // LevelManager.manager.disableMicMenuSlider = true;
       // micMenu.GetComponent<MicMenu>().enabled = false;
        menuAnimator.SetBool("Fade", false);
        animatorCamera.SetBool("Zoom", true);
        firstRun = false;
        menuCanvas.enabled = false;
    }

    public void Retry()
    {
       
    }


    //Overwrites data, need to fix
    public void Save()
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/ApplicationInfo" + fileEnding);
        LevelData dataToSave = new LevelData();

        dataToSave.highScore = highScore;
        dataToSave.mute = mute;
        dataToSave.touch = touch;
        dataToSave.sliderValue = sliderValue;
        dataToSave.first = first;
        dataToSave.adCount = adsCounter;
        bFormatter.Serialize(file, dataToSave);
        file.Close();

    }

    public void DisplayAd()
    {
        adsCounter = 10;
      //  Advertisement.Show(null, new ShowOptions { pause = true, resultCallback = result => { } });
    }

    public int Load()
    {
        if (File.Exists(Application.persistentDataPath + "/ApplicationInfo" + fileEnding))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ApplicationInfo" + fileEnding, FileMode.Open);

            LevelData dataToLoad = (LevelData)bf.Deserialize(file);
            this.adsCounter = dataToLoad.adCount;
            this.touch = dataToLoad.touch;
            this.mute = dataToLoad.mute;
            this.sliderValue = dataToLoad.sliderValue;
            this.highScore = dataToLoad.highScore;
            this.first = dataToLoad.first;
            file.Close();
            return highScore;
        }
        else
        {
            return 0;
        }
    }
}

[Serializable]
class LevelData
{
    public int highScore;
    public bool first;
    public bool mute;
    public float sliderValue;
    public bool touch;
    public int adCount;
}