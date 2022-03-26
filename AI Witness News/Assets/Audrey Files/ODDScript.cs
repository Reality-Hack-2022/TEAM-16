//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Microsoft.CognitiveServices.Speech;
//using System;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.CognitiveServices.Speech.Audio;



//public class ODDScript : MonoBehaviour
//{

//     public async void SynthesizeAudioAsync()
//    {
//        var config = SpeechConfig.FromSubscription("2bed4c7bce304f0e8bd2403c851b9b8a", "eastus");
//        // Note: if only language is set, the default voice of that language is chosen.
//        config.SpeechSynthesisLanguage = "en-US"; // For example, "de-DE"
//        using var synthesizer = new SpeechSynthesizer(config, null);

//        var ssml = File.ReadAllText("./StreamingAssets/ssml.xml");
//        var result = await synthesizer.SpeakSsmlAsync(ssml);

//        using var stream = AudioDataStream.FromResult(result);
//        await stream.SaveToWaveFileAsync("./StreamingAssets/file.wav");                                                                   // The voice setting will overwrite the language setting.
//                                                                                                                                      // The voice setting will not overwrite the voice element in input SSML.
//                                                                                                                                      // config.SpeechSynthesisVoiceName = "<your-wanted-voice>";
//    }
//}
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

public class ODDScript : MonoBehaviour
{
    public Text outputText;
    public Button startRecoButton;

    // PULLED OUT OF BUTTON CLICK
    SpeechRecognizer recognizer;
    SpeechConfig config;

    private object threadLocker = new object();
    private bool speechStarted = false; //checking to see if you've started listening for speech
    private string message;
    private bool waitingForReco = false;
    private bool micPermissionGranted = false;

    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        lock (threadLocker)
        {
            message = e.Result.Text;
        }
    }
    public async void ButtonClick()
    {
        // Creates an instance of a speech config with specified subscription key and service region.
        // Replace with your own subscription key and service region (e.g., "westus").
        var config = SpeechConfig.FromSubscription("2bed4c7bce304f0e8bd2403c851b9b8a", "eastus");

        // Make sure to dispose the recognizer after use!
        using (var recognizer = new SpeechRecognizer(config))
        {
            lock (threadLocker)
            {
                waitingForReco = true;
            }

            // Starts speech recognition, and returns after a single utterance is recognized. The end of a
            // single utterance is determined by listening for silence at the end or until a maximum of 15
            // seconds of audio is processed.  The task returns the recognition text as result.
            // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
            // shot recognition like command or query.
            // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

            // Checks result.
            string newMessage = string.Empty;
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                newMessage = result.Text;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                newMessage = "NOMATCH: Speech could not be recognized.";
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
            }

            lock (threadLocker)
            {
                message = newMessage;
                waitingForReco = false;
            }
        }
    }

    void Start()
    {
        if (outputText == null)
        {
            UnityEngine.Debug.LogError("outputText property is null! Assign a UI Text element to it.");
        }
        else if (startRecoButton == null)
        {
            message = "startRecoButton property is null! Assign a UI Button to it.";
            UnityEngine.Debug.LogError(message);
        }
        else
        {
            // Continue with normal initialization, Text and Button objects are present.
        }
    }

    void Update()
    {
        lock (threadLocker)
        {
            if (startRecoButton != null)
            {
                //startRecoButton.interactable = !waitingForReco && micPermissionGranted;
            }
        }
    }

       
 
}
