using System;
using System.Collections.Generic;
using System.Linq;

public class TokenPanelModel
{
    private Dictionary<TokenType, TokenModel> _tokenModels; 
    
    public TokenPanelModel(Dictionary<TokenType, int> initialTokensValues)
    {
        _tokenModels = new Dictionary<TokenType, TokenModel>();
        
        foreach (var token in (TokenType[]) Enum.GetValues(typeof(TokenType)))
        {
            initialTokensValues.TryGetValue(token, out var initialTokensValue);
            _tokenModels[token] = new TokenModel(token, initialTokensValue);
        }
    }

    public TokenModel GetTokenModel(TokenType tokenType) => _tokenModels[tokenType];

    public TokenType[] AllTokenTypes => Enum.GetValues(typeof(TokenType)).Cast<TokenType>().ToArray();

}