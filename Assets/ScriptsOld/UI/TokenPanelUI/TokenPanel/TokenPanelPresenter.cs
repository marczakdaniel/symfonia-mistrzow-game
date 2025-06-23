using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TokenPanelPresenter
{
    private TokenPanelModel Model;
    private TokenPanelView View;

    private Dictionary<TokenType, TokenPresenter> _tokenPresenter;

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
        _tokenPresenter = new Dictionary<TokenType, TokenPresenter>();
    }

    private void InitializeChildControllers()
    {
        var index = 0;
        foreach (var tokenType in Model.AllTokenTypes)
        {
            var tokenModel = new TokenModel(tokenType);
            var tokenView = View.GetTokenView(index);

            _tokenPresenter[tokenType] = new TokenPresenter(tokenModel, tokenView);
            index++;
        }
    }

    // Child Presenter -> Presenter

    private void ConnectChildControllersEvents()
    {
        foreach (var tokenPresenter in _tokenPresenter)
        {
            tokenPresenter.Value.OnTokenClicked += HandleClick;
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

    public async UniTask InitializeTokenPanel(Dictionary<TokenType, int> initialTokensValues)
    {
        foreach (var token in (TokenType[]) Enum.GetValues(typeof(TokenType)))
        {
            initialTokensValues.TryGetValue(token, out var initialTokensValue);
            _tokenPresenter[token].InitializeToken(initialTokensValue);
        }
        await View.InitializeTokenPanel();
    }

    public void AddToken(TokenType tokenType, int value = 1)
    {
        _tokenPresenter[tokenType].AddToken(value);
    }

    public void RemoveToken(TokenType tokenType, int value = 1)
    {
        _tokenPresenter[tokenType].RemoveToken(value);
    }
    

}