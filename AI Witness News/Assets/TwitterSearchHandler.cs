using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


public class TwitterSearchHandler : MonoBehaviour
{     
		
    private string query = "covid";

    public Text responseTxt;


    
    private void MakeSearchRequest(OAuthResponse response)
    {
        if (response == null || !response.isValid)
        {
            Debug.LogError("response is null or invalid");
            return;
        }
        
        Debug.Log("Authorization received");
    }   
    // 1 - The Consumer API Key you received from Twitter
    private string apiKey = "";
    // 2 - The Consumer Secret you received from Twitter
    private string secret = "bdx";

    // 3 - The coroutine the will make the authorization request
    private IEnumerator MakeOAuthRequest(string apiKey, string apiSecret)
    {
        string oAuthUrl = "https://api.twitter.com/oauth2/token";
        
        // 3-1
        byte[] body = Encoding.UTF8.GetBytes("grant_type=client_credentials");

        // 3-2
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string base64KeyAndSecret = 
            Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + apiSecret));

        headers.Add("Authorization", "Basic " + base64KeyAndSecret);

        // 3-3
        WWW request = new WWW(oAuthUrl, body, headers);
        yield return request;

        // 3-4
        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.LogErrorFormat("UnityEngine.WWW Error: {0}", request.error);
        }
        responseTxt.text = "response is: "+ request.text;
        Debug.Log("Response: " + request.text);
    }

    // 4 - Just run authorization on start for now.
    private void Start() 
    {
        StartCoroutine(MakeOAuthRequest(apiKey, secret));
    }

}