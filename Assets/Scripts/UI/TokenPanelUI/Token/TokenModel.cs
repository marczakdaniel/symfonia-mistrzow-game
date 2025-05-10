
using System;

public class TokenModel
{
    public Action<int> OnTokenAdded;
    public Action<int> OnTokenRemoved;
    public TokenType TokenType { get; set; }
    public int NumberOfTokens { get; set; }

    public TokenModel(TokenType tokenType)
    {
        TokenType = tokenType;
    }

    public void InitalizeToken(int initialNumberOfTokens)
    {
        NumberOfTokens = initialNumberOfTokens;
    }

    public void AddToken(int value = 1)
    {
        NumberOfTokens += value;
        OnTokenAdded?.Invoke(value);
    }

    public void RemoveToken(int value = 1)
    {
        NumberOfTokens = Math.Max(0, NumberOfTokens - value);
        OnTokenRemoved?.Invoke(value);
    }
}