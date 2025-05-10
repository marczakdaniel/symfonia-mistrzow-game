using System;
using UnityEditor;

public class TokenController
{
    private TokenModel Model;
    private TokenView View;

    public Action<TokenModel> OnTokenClicked;

    public TokenController(TokenModel model, TokenView view)
    {
        Model = model;
        View = view;

        ConnectModelEvents();
        
        View.Setup(model);
        View.OnTokenClicked += HandleClick;
    }

    private void ConnectModelEvents()
    {
        Model.OnNumberOfTokensChanged += HandleModelNumberOfTokensChanged;
    }

    private void HandleModelNumberOfTokensChanged()
    {
        View.UpdateView(Model);
    }

    public void RemoveToken(int value = 1)
    {
        Model.RemoveToken(value);
    }

    public void AddToken(int value = 1)
    {
        Model.AddToken(value);
    }

    private void HandleClick()
    {
        OnTokenClicked?.Invoke(Model);
    }
}