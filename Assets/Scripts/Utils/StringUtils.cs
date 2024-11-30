using System;

public class StringUtility
{
	/// <summary>
	/// Checks if a specific word is contained in the input string.
	/// </summary>
	/// <param name="input">The input string to search in.</param>
	/// <param name="word">The word to look for.</param>
	/// <returns>True if the word is found; otherwise, false.</returns>
	public static bool ContainsWord(string input, string word)
	{
		if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(word))
			return false;

		// Use StringComparison to make the search case-insensitive if needed
		string[] words = input.Split(new[] { ' ', '.', ',', '?' }, StringSplitOptions.RemoveEmptyEntries);

		foreach (string w in words)
		{
			if (string.Equals(w, word, StringComparison.OrdinalIgnoreCase)) // Case-insensitive comparison
				return true;
		}

		return false;
	}
}