using UnityEngine;

public class MicrophoneAnalyzer : MonoBehaviour
{
    public static MicrophoneAnalyzer instance;

    public float rmsVal;
    public float dbVal;
    public float pitchVal;

    public AudioSource microphone;

    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    float[] _samples;
    private float[] _spectrum;
    private float _fSample;

    void Awake()
    {
        if (instance == null)                               // Check if instance already exists
            instance = this;                                // if not, set instance to "this"
        else if (instance != this)                          // If instance already exists and it's not this:
            Destroy(gameObject);                            // Then destroy this. Enforces the singleton pattern.

        microphone = GetComponent<AudioSource>();
    }

    void Start()
    {
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;

        microphone.clip = Microphone.Start(FindMicrophone(), true, 10, 44100);
        microphone.loop = true;
        microphone.mute = false;
        microphone.Play();
    }

    void FixedUpdate()
    {
        AnalyzeSound();
    }

    // Selects the microphone with best specs. Creates the sample array (according to selected microphone's sampling rate ? No not yet.)
    public static string FindMicrophone()
    {
        // Determine which microphones are active right now, and choose the one with highest sampling rate.
        int maxFreq = 0;                           // maximum sampling frequency an input device offers.
        int minFreq;                               // dummy. not used at all.
        int currentFreq;                           // dummy sampling frequency for comparison.
        string deviceName = "";
        foreach (string device in Microphone.devices)
        {
            Microphone.GetDeviceCaps(device, out minFreq, out currentFreq);   // current device, -dummy-, max sampling rate it offers.
            if (currentFreq > maxFreq)
            {
                deviceName = device;               // This microphone is currently the best one.
                maxFreq = currentFreq;            // Update max frequency.
            }
        }
        return deviceName;
    }


    void AnalyzeSound()
    {
        microphone.GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        rmsVal = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        dbVal = 20 * Mathf.Log10(rmsVal / RefValue); // calculate dB
        if (dbVal < -160) dbVal = -160; // clamp it to -160dB min
                                        // get sound spectrum
        microphone.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        int maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // find max 
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
                continue;

            maxV = _spectrum[i];
            maxN = i; // maxN is the index of max
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < QSamples - 1)
        { // interpolate index using neighbours
            float dL = _spectrum[maxN - 1] / _spectrum[maxN];
            float dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        pitchVal = freqN * (_fSample / 2) / QSamples; // convert index to frequency
    }
}

