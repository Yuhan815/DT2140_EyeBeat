// References
// https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
// https://forum.unity.com/threads/using-device-microphone-to-interact-with-objects.119595/#post-801852

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundDetect : MonoBehaviour
{

    private static int sample_size = 1024;  // sample size for each frame
    private static int sample_freq = 44100; // audio sampling frequency

    private string micro_;      // microphone name
    private AudioSource audio_; // audio source

    private float refValue = 0.1f;         // RMS value for 0 dB
    private float filtered_value = 0.0f;   // filtered result
    private float previous_dbValue = 0.0f; // previous sample's volume (used in highpass filter)

    public float thresh = 0.01f; // threshold
    public float low = 0.3f;   // low pass filter constant
    public float high = 0.0f;   // high pass filter constant

    public Text text_; // text showing current result

    private bool soundeIsDetected = false;

    void Start()
    {
        audio_ = GetComponent<AudioSource>();

        // Initialize text
        text_.text = "No sound";

        if (micro_ == null)
        {
            micro_ = Microphone.devices[0];
            print(micro_);
        }

        audio_.clip = Microphone.Start(micro_, true, 1, sample_freq);
        
        // TODO 
        //audio_.loop= true; audio_.Play();
    }

    void Update()
    {
        float[] samples = new float[sample_size];

        // Get index of the past <sample_size> data
        int mic_pos = Microphone.GetPosition(null) - (sample_size + 1);
        if (mic_pos < 0)
        {
            return;
        }

        // Get data
        audio_.clip.GetData(samples, mic_pos);

        // Calculate 
        float rmsValue = RMS(samples);
        float dbValue = 20.0F * Mathf.Log10(rmsValue/refValue); // calculate dB
        if (dbValue < -160)
        {
            dbValue = -160; // clamp it to -160dB min
        }

        // Filter value
        float lowpass_value;
        filtered_value = Lowpass(dbValue, filtered_value, low);
        //filtered_value = Highpass(lowpass_value, previous_dbValue, filtered_value, high);
        previous_dbValue = dbValue;

        //Debug.Log("filtered db = " + filtered_value);
        //Debug.Log("freq2rc = " + Freq2RC(low));

        // Based on a threshold decide if sound is detected or not 
        if (thresh < filtered_value)
        {
            text_.text = "Sound detected";
            Debug.Log("Sound detected");
            soundeIsDetected = true;
        }
        else
        {
            text_.text = "No sound";
            Debug.Log("No Sound");
            soundeIsDetected = false;
        }
    }

    public bool GetDetectionResult(){
        return soundeIsDetected;
    }

    // Calculate RMS
    static float RMS(float[] samples) {
        float sum = 0.0f;
        for (int i=0; i < samples.Length; i++)
        { 
            sum += samples[i] * samples[i];    // sum squared samples
        } 
        return Mathf.Sqrt(sum/samples.Length); // square root of average
    }

    // Highpass filter
    static float Highpass(float x1, float x2, float y, float a)
    {
        return a * y + a * (x1 - x2);
    }

    // Lowpass filter
    static float Lowpass(float x, float y, float a)
    {
        return a * x + (1-a) * y;
    }

    // Return RC bandpass-pass filter output, given input samples,
    // time interval dt, low and high cut-off frequencies
    static float[] Bandpass_samples(float[] x, float dt, float f_low, float f_high)
    {
        float[] y = new float[x.Length];

        Lowpass_samples(x, y, dt, Freq2RC(f_low));
        Highpass_samples(x, y, dt, Freq2RC(f_high));

        return y;
    }

    // Convert frequency (Hz) to time constant (resistance × capacitance (R×C)
    private static float Freq2RC(float f)
    {
        return 1 / (2 * Mathf.PI * f);
    }

    // Return RC low-pass filter output, given input samples,
    // time interval dt, and time constant RC
    private static void Lowpass_samples(float[] x, float[] y, float dt, float RC)
    {
        if (y.Length != x.Length)
        {
            return;
        }

        float a = dt / (RC + dt);
        y[0] = a * x[0];
        for (int i = 1; i< x.Length; i++)
        {
            y[i] = a * x[i] + (1-a) * y[i-1];
        }
    }

    // Return RC high-pass filter output, given input samples,
    // time interval dt, and time constant RC
    private static void Highpass_samples(float[] x, float[] y, float dt, float RC)
    {
        if (y.Length != x.Length)
        {
            return;
        }

        float a = dt / (RC + dt);
        y[0] = a * x[0];
        for (int i = 1; i< x.Length; i++)
        {
            y[i] = a * y[i-1] + a * (x[i] - x[i-1]);
        }
    }
}
