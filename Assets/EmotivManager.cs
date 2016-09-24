using System;

using UnityEngine;
using System.Collections;
using Emotiv;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

public class EmotivChannelFrequenciesPerformance {
	public EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t i_Data)
	{
	    this.Data = i_Data;
	}

	public double Theta {
		get;
		set;
	}

	public double Alpha {
		get;
		set;
	}

	public double LowBeta {
		get;
		set;
	}

	public double HighBeta {
		get;
		set;
	}

	public double Gamma {
		get;
		set;
	}

	public EdkDll.IEE_DataChannel_t Data {
		get;
		private set;
	}

    public override string ToString()
    {
        return string.Format(
            "Channel: {0}, Alpha: {1}, Low Beta: {2}, High Beta: {3}, Theta: {4}, Gamma: {5}",
            Data,
            Alpha,
            LowBeta,
            HighBeta,
            Theta,
            Gamma);
    }
}

public class EmotivManager : MonoBehaviour
{

    public bool debug = true;
	public bool useStub = true;
    private EmoEngine engine;

	// average freq 
	private double m_AvgAlpha;
	private double m_AvgLowBeta;
	private double m_AvgHighBeta;
	private double m_AvgTheta;
	private double m_AvgGamma;

	// SENSORS
	private List<EmotivChannelFrequenciesPerformance> m_DataChannels;
	private int m_UserID = -1;

	private void createChannelList() {
		m_DataChannels = new List<EmotivChannelFrequenciesPerformance> ();
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_AF3));
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_F7)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_F3)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_FC5)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_T7)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_P7)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_O1)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_O2)); 
		// m_DataChannels.Add (EdkDll.IEE_DataChannel_t.IED_Pz);  Missing from Emotiv API ? 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_P8)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_T8)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_FC6));
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_F4)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_F8)); 	
		m_DataChannels.Add (new EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t.IED_AF4)); 	
	}
    void Awake()
    {
        engine = EmoEngine.Instance;
        engine.EmoEngineConnected += new EmoEngine.EmoEngineConnectedEventHandler(EmotivConnected);
		engine.UserAdded += new EmoEngine.UserAddedEventHandler (EmotivUserAdded);
		engine.UserRemoved += new EmoEngine.UserRemovedEventHandler (EmotivUserRemoved); 
        engine.EmoEngineDisconnected += new EmoEngine.EmoEngineDisconnectedEventHandler(EmotivDisconnected);
        engine.EmoEngineEmoStateUpdated += new EmoEngine.EmoEngineEmoStateUpdatedEventHandler(EmotivStateUpdated);
        createChannelList();
        engine.Connect();
    }

    // Update is called once per frame
    void Update () {
        engine.ProcessEvents();
		updateFreqBands ();
    }

    private void CalcAvg()
    {
        double alpha = 0, low_beta = 0, high_beta = 0, theta = 0, gamma = 0;
        int size = this.m_DataChannels.Count;
        foreach (var item in this.m_DataChannels)
        {
            alpha += item.Alpha;
            low_beta += item.LowBeta;
            high_beta += item.HighBeta;
            theta += item.Theta;
            gamma += item.Gamma;
        }

        this.m_AvgTheta = theta / size;
        this.m_AvgAlpha = alpha / size;
        this.m_AvgLowBeta = low_beta / size;
        this.m_AvgHighBeta = high_beta / size;
        this.m_AvgGamma = gamma / size;
    }

	private void stubAvgValues() {
		this.m_AvgTheta = UnityEngine.Random.Range(0,300f);
		this.m_AvgAlpha = UnityEngine.Random.Range(0,300f);
		this.m_AvgLowBeta = UnityEngine.Random.Range(0,300f);
		this.m_AvgHighBeta = UnityEngine.Random.Range(0,300f);
		this.m_AvgGamma = UnityEngine.Random.Range(0,300f);

		print (m_AvgGamma);
	}

	/*
	 * This method calculate the average power for theta, alpha, low_beta, 
	 * high_beta, gamma input from all channels.
	*/
	private void updateFreqBands() {
		if (useStub) { 
			stubAvgValues ();
		} else {
			if (m_UserID >= 0) {
				double[] alpha = new double[1];
				double[] low_beta = new double[1];
				double[] high_beta = new double[1];
				double[] gamma = new double[1];
				double[] theta = new double[1];
				List<EmotivChannelFrequenciesPerformance> updatedDataChannels = new List<EmotivChannelFrequenciesPerformance> ();
				foreach (EmotivChannelFrequenciesPerformance channel in m_DataChannels) {
					engine.IEE_GetAverageBandPowers ((uint)m_UserID, channel.Data, theta, alpha, low_beta, high_beta, gamma);
					EmotivChannelFrequenciesPerformance updatedChannel = new EmotivChannelFrequenciesPerformance (channel.Data) {
						Alpha = alpha [0],
						LowBeta = low_beta [0],
						HighBeta = high_beta [0],
						Theta = theta [0],
						Gamma = gamma [0]
					};
					updatedDataChannels.Add (updatedChannel);
					if (this.debug) {
						Debug.Log (updatedChannel);
					}
				} 

				// Change ref. 
				m_DataChannels = updatedDataChannels;
				CalcAvg ();

			}
		}
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
        print("Emotiv Connected!!");
    }

	void EmotivUserRemoved(object sender, EmoEngineEventArgs e)
	{
		print("User Removed");
		m_UserID = -1;

	}

	void EmotivUserAdded(object sender, EmoEngineEventArgs e)
	{
		print("User Added");
		m_UserID = (int)e.userId;
		EmoEngine.Instance.IEE_FFTSetWindowingType((uint)m_UserID, EdkDll.IEE_WindowingTypes.IEE_HAMMING);
	}

    void EmotivDisconnected(object sender, EmoEngineEventArgs e)
    {
        print("Disconnected!");
    }

    void EmotivStateUpdated(object sender, EmoStateUpdatedEventArgs e)
    {
        EmoState es = e.emoState; 
    }

    public double AverageAlpha
    {
        get
        {
            return this.m_AvgAlpha;
        }
    }

    public double AverageLowBeta        
    {
        get
        {
            return this.m_AvgLowBeta;
        }
    }

    public double AverageHighBeta 
    {
        get
        {
            return this.m_AvgHighBeta;
        }
    }

    public double AverageTheta
    {
        get
        {
            return this.m_AvgTheta;
        }
    }

    public double AverageGamma
    {
        get
        {
            return this.m_AvgGamma;
        }
    }

    /*
     * The following methods need to be implemented based on the signal processing and ML and
     * out of scope for this project.
     */

    // TODO: Change Stub methods to actual implementation 
    public double Happy
    {
        get
        {
            return this.CaculateScale(this.m_AvgAlpha);
        }
    }


    public double Fear
    {
        get
        {
            return this.CaculateScale(this.m_AvgGamma);
        }
    }

    // Using specific data channels
    public double Disgust
    {
        get
        {
            return this.CaculateScale(this.m_AvgLowBeta);
        }
    } 

    public double Excitment
    {
        get
        {
            return this.CaculateScale(this.m_AvgHighBeta);
        }
    }

    public double Natural
    {
        get
        {
            return this.CaculateScale(this.m_AvgTheta);
        }
    }

    // return a number between 0 and 1 based on raw score. 
    // TODO get real min and max scale and not arbitrary numbers.
    double CaculateScale(double rawScore)
    {
        double maxScale = 1000;
        double minScale = 0;
        double scaledScore;

        if (rawScore < minScale)
            scaledScore = 0;
        else if (rawScore > maxScale)
            scaledScore = 1;
        else
            scaledScore = (rawScore - minScale) / (maxScale - minScale);

        return scaledScore;
    }


}
