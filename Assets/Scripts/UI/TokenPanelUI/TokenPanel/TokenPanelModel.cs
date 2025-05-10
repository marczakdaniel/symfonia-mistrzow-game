using System;
using System.Collections.Generic;
using System.Linq;

public class TokenPanelModel
{
    private Dictionary<TokenType, TokenModel> _tokenModels; 
    
    public TokenPanelModel()
    {
        _tokenModels = new Dictionary<TokenType, TokenModel>();
        foreach (var token in (TokenType[]) Enum.GetValues(typeof(TokenType)))
        {
            _tokenModels[token] = new TokenModel(token);
        }
    }

    public void InitializeTokenPanel(Dictionary<TokenType, int> initialTokensValues)
    {
        _tokenModels = new Dictionary<TokenType, TokenModel>();
        foreach (var token in (TokenType[]) Enum.GetValues(typeof(TokenType)))
        {
            initialTokensValues.TryGetValue(token, out var initialTokensValue);
            _tokenModels[token].InitalizeToken(initialTokensValue);
        }
    }

    public void AddToken(TokenType tokenType, int value)
    {
        _tokenModels[tokenType].AddToken(value);
    }
    
    public void RemoveToken(TokenType tokenType, int value)
    {
        _tokenModels[tokenType].RemoveToken(value);
    }

    public TokenModel GetTokenModel(TokenType tokenType) => _tokenModels[tokenType];

    public TokenType[] AllTokenTypes => Enum.GetValues(typeof(TokenType)).Cast<TokenType>().ToArray();

}