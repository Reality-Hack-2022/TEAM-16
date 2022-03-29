using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class twitterspitter : MonoBehaviour
{

	public Text responseTxt;

	public void searchKeyword(string searchWord) {
		responseTxt.text = searchWord;
	}


}
