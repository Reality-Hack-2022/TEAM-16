public class OAuthResponse
{
	// 1
	public string token_type;
	public string access_token;
	public Error[] errors;
	// 2
	public bool isValid
	{
		get
		{
			return !string.IsNullOrEmpty(token_type) && !string.IsNullOrEmpty(access_token);
		}
	}

	// 3
	public override string ToString()
	{
		return string.Format(                                     // NEW 
			"isValid: {0}\ntoken_type: {1}\naccess_token: {2}\nerror(s):{3}",
			isValid,
			(string.IsNullOrEmpty(token_type) ? "NULL" : token_type),
			(string.IsNullOrEmpty(access_token) ? "NULL" : access_token),
			// NEW
			((errors == null || errors.Length == 0) ? "NONE" : "\n" + errors.ToStringExt())
			);
	}
}