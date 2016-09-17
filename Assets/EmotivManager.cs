using UnityEngine;
using System.Collections;
using Emotiv;
using System.Collections.Generic;

public struct EmotivChannelFrequenciesPerformance {
	public EmotivChannelFrequenciesPerformance(EdkDll.IEE_DataChannel_t i_DataChannel) {
		this.DataChannel = i_DataChannel;
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

	public EdkDll.IEE_DataChannel_t DataChannel {
		get;
		private set;
	}
}

public class EmotivManager : MonoBehaviour {

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
        engine.Connect();
    }

    // Update is called once per frame
    void Update () {
        engine.ProcessEvents();
		updateFreqBands ();
    }

	/*
	 * This method calculate the average power for theta, alpha, low_beta, 
	 * high_beta, gamma input from all channels.
	*/
	private void updateFreqBands() {
		if (m_UserID >= 0) {
			double[] alpha = new double[1];
			double[] low_beta = new double[1];
			double[] high_beta = new double[1];
			double[] gamma = new double[1];
			double[] theta = new double[1];

			double totalAlpha = 0, totalLowBeta = 0, totalHighBeta = 0, totalGamma = 0, totalTheta = 0;
			int activeChannelCount = m_DataChannels.Count;
			List<EmotivChannelFrequenciesPerformance> updatedDataChannels = new List<EmotivChannelFrequenciesPerformance> ();
			foreach (EmotivChannelFrequenciesPerformance channel in m_DataChannels) {
				activeChannelCount++;
				engine.IEE_GetAverageBandPowers ((uint)m_UserID, channel.DataChannel, theta, alpha, low_beta, high_beta, gamma);
				EmotivChannelFrequenciesPerformance updatedChannel = new EmotivChannelFrequenciesPerformance (channel.DataChannel) {
					Alpha = alpha [0],
					LowBeta = low_beta [0],
					HighBeta = high_beta [0],
					Theta = theta [0],
					Gamma = gamma [0]
				};
			}

			// Change ref. 
			m_DataChannels = updatedDataChannels;
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
