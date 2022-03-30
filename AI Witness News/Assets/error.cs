public class Error
{
	public int code;
	public string message;
	public string label;
 
	public override string ToString()
	{
		return string.Format(
			"code: {0}\nmessage: {1}\nlabel: {2}",
			code, message, label);
	}
}