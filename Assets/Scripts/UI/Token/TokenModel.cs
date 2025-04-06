
using System;

public class TokenModel
{
    public TokenType TokenType { get; set; }
    public int NumberOfTokens { get; set; }

    public TokenModel(TokenType tokenType, int initialNumberOfTokens)
    {
        TokenType = tokenType;
        NumberOfTokens = initialNumberOfTokens;
    }

    public void AddToken(int value = 1)
    {
        NumberOfTokens += value;
    }

    public void RemoveToken(int value = 1)
    {
        NumberOfTokens = Math.Max(0, NumberOfTokens - value);
    }
}