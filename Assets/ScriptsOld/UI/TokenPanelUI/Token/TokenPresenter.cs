using System;
using Cysharp.Threading.Tasks;
using UnityEditor;

public class TokenPresenter
{
    private TokenModel Model;
    private TokenView View;

    public Action<TokenModel> OnTokenClicked;

    public TokenPresenter(TokenModel model, TokenView view)
    {
        Model = model;
        View = view;

        ConnectModelEvents();
        ConnectViewEvents();
    }


    // View -> Presenter -> Father Presenter
    private void ConnectViewEvents()
    {
        View.OnTokenClicked += HandleClick;
    }

    private void HandleClick()
    {
        OnTokenClicked?.Invoke(Model);
    }

    // Model -> Presenter -> View

    private void ConnectModelEvents()
    {
        Model.OnTokenAdded += ShowAddToken;
        Model.OnTokenRemoved += ShowRemoveToken;
        Model.OnTokenInitialized += ShowInitializeToken;
    }

    private void HandleModelNumberOfTokensChanged()
    {
        View.UpdateView(Model);
    }

    private void ShowInitializeToken()
    {
        View.Setup(Model);
        View.InitializeToken(Model.NumberOfTokens);
    }

    private void ShowAddToken(int difference)
    {
        View.AddToken(Model.NumberOfTokens, difference).Forget();
    }

    private void ShowRemoveToken(int difference)
    {
        View.RemoveToken(Model.NumberOfTokens, difference).Forget();
    }

    // Father Presenter -> Presenter -> Model

    public void InitializeToken(int initialValue)
    {
        Model.InitializeToken(initialValue);
    }

    public void AddToken(int value)
    {
        Model.AddToken(value);
    }
    
    public void RemoveToken(int value)
    {
        Model.RemoveToken(value);
    }
}