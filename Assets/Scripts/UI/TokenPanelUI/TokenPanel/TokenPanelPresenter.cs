using System;
using System.Collections.Generic;

public class TokenPanelPresenter
{
    private TokenPanelModel Model;
    private TokenPanelView View;

    private Dictionary<TokenType, TokenPresenter> _tokenControllers;

    public Action<TokenModel> OnTokenClicked;

    public TokenPanelPresenter(TokenPanelModel model, TokenPanelView view)
    {
        Model = model;
        View = view;

        InitializeController();
        InitializeChildControllers();
        ConnectChildControllersEvents();
        ConnectModelEvents();
    }

    private void InitializeController()
    {
        _tokenControllers = new Dictionary<TokenType, TokenPresenter>();
    }

    private void InitializeChildControllers()
    {
        var index = 0;
        foreach (var tokenType in Model.AllTokenTypes)
        {
            var tokenModel = Model.GetTokenModel(tokenType);
            var tokenView = View.GetTokenView(index);

            _tokenControllers[tokenType] = new TokenPresenter(tokenModel, tokenView);
            index++;
        }
    }

    // Child Presenter -> Presenter

    private void ConnectChildControllersEvents()
    {
        foreach (var tokenController in _tokenControllers)
        {
            tokenController.Value.OnTokenClicked += HandleClick;
        }
    }

    private void HandleClick(TokenModel tokenModel)
    {
        OnTokenClicked?.Invoke(tokenModel);
    }

    // Model -> Presenter -> View
    
    private void ConnectModelEvents()
    {

    }

    // Father -> Child Presenter

    public void AddToken(TokenType tokenType, int value = 1)
    {
        _tokenControllers[tokenType].AddToken(value);
    }

    public void RemoveToken(TokenType tokenType, int value = 1)
    {
        _tokenControllers[tokenType].RemoveToken(value);
    }
    

}