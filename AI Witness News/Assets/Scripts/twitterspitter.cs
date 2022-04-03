using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Web.Helpers;
using Web.Twitter.API;
using Web.Twitter.DataStructures;

public class twitterspitter : MonoBehaviour
{

    // SpeechManager _speechcontrol;



    
    public string TwitterApiConsumerKey;
    public string TwitterApiConsumerSecret;

    // public Text tweetRecievingText;

    public WebAccessToken TwitterApiAccessToken;

    [Header("GetGlobalTrends")]
    public Trend[] GlobalTrends;
    [Header("GetLatestTweetsFromUserByScreenName")]
    public Tweet[] LatestTweetsFromUserByScreenName;
    [Header("GetLatestTweetsFromUserByUserId")]
    public Tweet[] LatestTweetsFromUserByUserId;
    [Header("GetLikedTweetsOfUserByUsername")]
    public Tweet[] LikedTweetsOfUserByUsername;
    [Header("GetLikedTweetsOfUserByUserID")]
    public Tweet[] LikedTweetsOfUserByUserID;
    [Header("GetLocalTrends")]
    public Trend[] LocalTrends;
    [Header("GetTweetByID")]
    public Tweet TweetById;
    [Header("GetUserProfileByUserId")]
    public UserProfile GetUserProfileByUserId;
    [Header("GetUserProfileByUsername")]
    public UserProfile GetUserProfileByUsername;
    [Header("SearchForTweets")]
    public Tweet[] SearchResults;

    private void Start()
    {
        if(TwitterApiAccessToken != null)
           TwitterApiAccessToken = WebHelper.GetTwitterApiAccessToken(TwitterApiConsumerKey,TwitterApiConsumerSecret);
        //ExampleFunction();
    }



    public async void ExampleFunction()
    {
        TweetById = await TwitterRestApiHelper.GetTweetByID("1221121465220771840", this.TwitterApiAccessToken.access_token);
        
        LatestTweetsFromUserByScreenName =  await TwitterRestApiHelper.GetLatestTweetsFromUserByScreenName("Twitter", this.TwitterApiAccessToken.access_token);
        LatestTweetsFromUserByUserId =      await TwitterRestApiHelper.GetLatestTweetsFromUserByUserId("783214", this.TwitterApiAccessToken.access_token);

        LikedTweetsOfUserByUsername =       await TwitterRestApiHelper.GetLikedTweetsOfUserByUsername("Twitter", this.TwitterApiAccessToken.access_token);
        LikedTweetsOfUserByUserID =         await TwitterRestApiHelper.GetLikedTweetsOfUserByUserID("783214", this.TwitterApiAccessToken.access_token);

        LocalTrends =                       await TwitterRestApiHelper.GetLocalTrends(2442047, this.TwitterApiAccessToken.access_token);
        GlobalTrends =                      await TwitterRestApiHelper.GetGlobalTrends(this.TwitterApiAccessToken.access_token);

        GetUserProfileByUserId =            await TwitterRestApiHelper.GetUserProfileByUserId("783214", this.TwitterApiAccessToken.access_token);
        GetUserProfileByUsername =          await TwitterRestApiHelper.GetUserProfileByUsername("Twitter", this.TwitterApiAccessToken.access_token);

        //SearchResults =                     await TwitterRestApiHelper.SearchForTweets("unity", this.TwitterApiAccessToken.access_token, 50);
    }
    public SendTextToAnalyse _SendTextToAnalyse;

    public async void searchByGeo(string laty, string longY, string keyword)
    { 
        //string searchQuery = _speechcontrol.keywordString;
        // string searchQuery = "Elon Musk";
        string locationString = "&geocode="+laty+","+longY+",50mi";
        //string locationString = "&geocode=30.267118,-97.74313,50mi";
        SearchResults = await TwitterRestApiHelper.SearchForTweets(keyword,locationString, this.TwitterApiAccessToken.access_token, 50);
        tweetObjectToText();
    }    
    public string exampleTweet = "";
    public string exampleTweetAuthor = "";
    public void tweetObjectToText(){
        Debug.Log("SearchResults.Length: " + SearchResults.Length);
        string fullString = "";
        exampleTweet = RemoveAllUrls(SearchResults[0].text);
        //exampleTweet = SearchResults[0].text;
        exampleTweetAuthor = SearchResults[0].user.screen_name;
        for (int i = 0; i < SearchResults.Length; i++)
        {
            fullString += " " + SearchResults[i].text;
            
        }        
        //tweetRecievingText.text = "Analyzing: " + fullString;
        _SendTextToAnalyse.SendPredictionText(fullString);
    }
    public static string RemoveAllUrls(string str)
    {
        var protocols = new string[] { "http://", "https://", "ftp://" };
 
        foreach(var protocol in protocols)
        {
            while (str.IndexOf(protocol, StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                var startIndex = str.IndexOf(protocol);
                var endIndex = str.IndexOf(" ", startIndex);
 
                // Account for URLs that end the string.
                var count = (endIndex != -1) ? endIndex - startIndex : str.Length - startIndex;
 
                str = str.Remove(startIndex, count);
            }
        }
 
        return str;
    }
	// Sentiment Analysis Thread
	private void Errors(int errorCode, string errorMessage)
	{
		Debug.Log(errorMessage + "\nCode: " + errorCode);
	}
    #region Instance
    private static twitterspitter m_Instance = null;
    public static twitterspitter instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (twitterspitter)FindObjectOfType(typeof(twitterspitter));
            }
            return m_Instance;
        }
    }

    public static bool hasInstance
    {
        get { return m_Instance != null; }
    }

    protected static void AssignMeAsInstance(twitterspitter _instance)
    {
        m_Instance = _instance;
    }

    public void Awake()
    {
         //AssignControllers();    
        if (hasInstance)
        {
            //Destroy(this);
        }
        else
        {
            AssignMeAsInstance(this);
            //GameObject.DontDestroyOnLoad(this);
        }
    }

    #endregion     
}

