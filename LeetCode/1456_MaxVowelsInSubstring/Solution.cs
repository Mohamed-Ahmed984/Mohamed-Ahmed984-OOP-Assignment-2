public class Solution
{
    public int MaxVowels(string s, int k)
    {
        int count = 0;
        for (int i = 0; i < k; i++)
        {
            if (IsVowel(s[i]))
                count++;
        }
        int maxCount = count;
        for (int i = k; i < s.Length; i++)
        {
            if (IsVowel(s[i - k]))
                count--;
            if (IsVowel(s[i]))
                count++;
            if (count > maxCount)
                maxCount = count;
        }
        return maxCount;
    }

    private bool IsVowel(char letter)
    {
        return letter == 'a' || letter == 'e' || letter == 'i' || letter == 'o' || letter == 'u';
    }
}
