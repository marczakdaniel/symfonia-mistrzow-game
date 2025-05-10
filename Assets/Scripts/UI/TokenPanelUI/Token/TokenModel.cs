
using System;

public class TokenModel
{
    public Action OnNumberOfTokensChanged;
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
        OnNumberOfTokensChanged?.Invoke();
    }

    public void RemoveToken(int value = 1)
    {
        NumberOfTokens = Math.Max(0, NumberOfTokens - value);
        OnNumberOfTokensChanged?.Invoke();
    }
}