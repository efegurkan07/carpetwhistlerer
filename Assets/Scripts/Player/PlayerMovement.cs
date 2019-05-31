using UnityEngine;
using System.Linq;

// this script controls the player movement according to pitch detection. Place this on the Player game object.
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    new Rigidbody2D rigidbody;


    // Sound Input variables
    [SerializeField]
    float pitch;                            // This is the extracted frequency from the microphone.

    float previousPitch;

    [SerializeField]
    int maxPitch;                           // Maximum freq

    [SerializeField]
    int minPitch;                           // Minimum freq

    public float height;                    // The height level that the player object will move to.
    const float ceiling = 12f, floor = -14f;// Minimum and Maximum y position

    // Moving Average - Smoothing Variables
    private const int PitchArraySize = 8;           // Average this many values (used as size of pitch_array)
    private float[] pitchArray;               // A container for detected peak frequencies.
    private int pitchIndex;					// Index of the pitch_array

    void Awake()
    {
        // Enforce singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        instance.ResetPosition();
        // Set the constants and other parameters. 
        maxPitch = 1750;                                // Default Maximum frequency
        minPitch = 700;                                 // Default Minimum frequency

        //Averaging the last pitches
        pitchArray = new float[PitchArraySize];
        for (int i = 0; i < pitchArray.Length; i++)
        {
            pitchArray[i] = (maxPitch + minPitch) / 2f;
        }
        pitchIndex = pitchArray.Length - 1;
        pitch = 0;

        pitch = 0;
    }// end of start function


    // FUNCTIONS
    // reset the position of the player object.
    public void ResetPosition()
    {
        rigidbody.position.Set(15f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        previousPitch = pitch;
        if (MicrophoneAnalyzer.instance.dbVal > -5f)
        {
            GameManager.instance.noteAnimation.SetActive(true);
            pitch = MicrophoneAnalyzer.instance.pitchVal;                       // Acquire the latest sound pitch from AudioStuff.

            // H = Player wants to set the new maximum frequency (high)
            if (Input.GetKey(KeyCode.H))
            {   // get high value.
                maxPitch = Mathf.RoundToInt(Mathf.Clamp(pitch, minPitch, 20000f));
                print("calibrated high f = " + maxPitch + " hz");
            }
            // L = Player wants to set the new minimum frequency (high)
            else if (Input.GetKey(KeyCode.L))
            {   // get low value.
                minPitch = Mathf.RoundToInt(Mathf.Clamp(pitch, 20f, maxPitch));
                print("calibrated low f = " + minPitch + " hz");
            }

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            pitchArray[pitchIndex] = Mathf.RoundToInt(pitch);               // Add this value to the recorded pitches array.
            if (++pitchIndex >= pitchArray.Length)                          // update the index edit: make mod.
                pitchIndex = 0;

            pitch = pitchArray.Average();

            height = (((pitch - minPitch) / (maxPitch - minPitch)) * (ceiling - floor)) + floor;     // Get pitch, subtract min freq --> height. multiply by step size (ss = (max f - min f) / n_steps), add to floor (offset)

            rigidbody.position = new Vector2(rigidbody.position.x, height);
        }
        else
        {
            pitch = previousPitch;
            GameManager.instance.noteAnimation.SetActive(false);
        }

    }//end of fixed update.

}// end of player mvoement
