public static class ErrorExt
{
	public static string ToStringExt(this Error[] errors, string separator = "\n")
	{
		string s = "";

		for (int i = 0; i < errors.Length; i++)
		{
			s += errors[i].ToString();
			if (i < errors.Length - 1)
				s += separator;
		}

		if (string.IsNullOrEmpty(s))
			s = "NONE";

		return s;
	}
}