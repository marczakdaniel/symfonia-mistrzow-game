using System;
using System.Collections.Generic;

public class TokenPanelController
{
    private TokenPanelModel Model;
    private TokenPanelView View;

    private Dictionary<TokenType, TokenController> _tokenControllers;

    public Action<TokenModel> OnTokenClicked;

    public TokenPanelController(TokenPanelModel model, TokenPanelView view)
    {
        Model = model;
        View = view;

        InitializeController();
    }

    private void InitializeController()
    {
        _tokenControllers = new Dictionary<TokenType, TokenController>();
        foreach (var tokenType in Model.AllTokenTypes)
        {
            var tokenModel = Model.GetTokenModel(tokenType);
            var tokenView = View.GetTokenView(tokenType);

            _tokenControllers[tokenType] = new TokenController(tokenModel, tokenView);
            _tokenControllers[tokenType].OnTokenClicked += HandleClick;
        }
    }

    public void RemoveToken(TokenType tokenType, int value = 1)
    {
        _tokenControllers[tokenType].RemoveToken(value);
    }

    public void AddToken(TokenType tokenType, int value = 1)
    {
        _tokenControllers[tokenType].AddToken(value);
    }
    
    private void HandleClick(TokenModel tokenModel)
    {
        OnTokenClicked?.Invoke(tokenModel);
    }
}