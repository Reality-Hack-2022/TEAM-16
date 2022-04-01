using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;
using UnitySentiment;

public class SendTextToAnalyse : MonoBehaviour {

	public SentimentAnalysis predictionObject ;
	// public InputField textToSend;

	// public Image ChangeSentimentalColor;
	// public Color PositiveResponse;
	// public Color NegativeResponse;
	// public Color NeutralResponse;

	// public Text PositivePercent;
	// public Text NegativePercent;
	// public Text NeutralPercent;

	private bool responseFromThread = false;
	private bool threadStarted = false;
	private Vector3 SentimentAnalysisResponse;
	public SpeechManager _speechManny;

	void OnEnable() 
	{
		Application.runInBackground = true;
		// Initialize the local database
		predictionObject.Initialize();
		// Listedn to the Events
		// Sentiment analysis response
		SentimentAnalysis.OnAnlysisFinished += GetAnalysisFromThread;
		// Error response
		SentimentAnalysis.OnErrorOccurs += Errors;
	}

	void OnDestroy()
	{
		// Unload Listeners
		SentimentAnalysis.OnAnlysisFinished -= GetAnalysisFromThread;
		SentimentAnalysis.OnErrorOccurs -= Errors;
	}

	public void SendPredictionText(string textToAnalyze)
	{
		// Thread-safe computations
		predictionObject.PredictSentimentText(textToAnalyze);

		if (!threadStarted)
		{// Thread Started
			threadStarted = true;
			StartCoroutine(WaitResponseFromThread());
		}
	}

	// Sentiment Analysis Thread
	private void GetAnalysisFromThread(Vector3 analysisResult)
	{		
		SentimentAnalysisResponse = analysisResult;
		responseFromThread = true;
		//trick to call method to the main Thread
	}

	private IEnumerator WaitResponseFromThread()
	{
		while(!responseFromThread) // Waiting For the response
		{
			yield return null;
		}
		// Main Thread Action
		PrintAnalysis();
		// Reset
		responseFromThread = false;
		threadStarted = false;
	}

	private void PrintAnalysis()
	{
		// PositivePercent.text = SentimentAnalysisResponse.x + " % : Positive"; 
		// NegativePercent.text = SentimentAnalysisResponse.y + " % : Negative";
		// NeutralPercent.text = SentimentAnalysisResponse.z + " % : Neutral";
		
		// if ( SentimentAnalysisResponse.x >  SentimentAnalysisResponse.y &&  SentimentAnalysisResponse.x >  SentimentAnalysisResponse.z)
		// {
		// 	ChangeSentimentalColor.color = PositiveResponse;
		// }
		// else if (SentimentAnalysisResponse.y >  SentimentAnalysisResponse.x &&  SentimentAnalysisResponse.y >  SentimentAnalysisResponse.z)
		// {
		// 	ChangeSentimentalColor.color = NegativeResponse;
		// }
		// else if (SentimentAnalysisResponse.z >  SentimentAnalysisResponse.x &&  SentimentAnalysisResponse.z >  SentimentAnalysisResponse.y)
		// {
		// 	ChangeSentimentalColor.color = NeutralResponse;
		// }
		ourCateredResponse();
	}
	public UIManagerr _uimanager;
	public twitterspitter _twitterspitter;
	public void ourCateredResponse(){
		string keyword = _speechManny.keywordString;
		string location = _speechManny.locationString;
		string exampleTweet = _twitterspitter.exampleTweet;
		string exampleTweetAuthor = _twitterspitter.exampleTweetAuthor;
		if ( SentimentAnalysisResponse.x >  SentimentAnalysisResponse.y &&  SentimentAnalysisResponse.x >  SentimentAnalysisResponse.z)
		{
			//ChangeSentimentalColor.color = PositiveResponse;
			_uimanager.SpeechPlayback("Peaople in " + location + " feel positive about " + keyword + ". Recently, "+location+" local, " +exampleTweetAuthor+" tweeted , "+ exampleTweet);
		}
		else if (SentimentAnalysisResponse.y >  SentimentAnalysisResponse.x &&  SentimentAnalysisResponse.y >  SentimentAnalysisResponse.z)
		{
			//ChangeSentimentalColor.color = NegativeResponse;
			_uimanager.SpeechPlayback("Peaople in " + location + " feel negative about " + keyword + ". Recently, "+location+" local, " +exampleTweetAuthor+" tweeted , "+ exampleTweet);
		}
		else if (SentimentAnalysisResponse.z >  SentimentAnalysisResponse.x &&  SentimentAnalysisResponse.z >  SentimentAnalysisResponse.y)
		{
			//ChangeSentimentalColor.color = NeutralResponse;
			_uimanager.SpeechPlayback("Peaople in " + location + " feel nuetral about " + keyword + ". Recently, "+location+" local, " +exampleTweetAuthor+" tweeted , "+ exampleTweet);
		}

		
	}
	// Sentiment Analysis Thread
	private void Errors(int errorCode, string errorMessage)
	{
		Debug.Log(errorMessage + "\nCode: " + errorCode);
	}
    // public Vector3[] sentimentSample;
	// public int sentimentCounter = 0;
    // public void sentimentAverager(Vector3 response){
    //     sentimentSample[sentimentCounter] = response;
	// 	sentimentCounter++;
    // }	
}