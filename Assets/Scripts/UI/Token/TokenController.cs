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
        
        View.Setup(model);
        View.OnTokenClicked += HandleClick;
    }

    public void RemoveToken(int value = 1)
    {
        Model.RemoveToken(value);
        View.UpdateView(Model);
    }

    public void AddToken(int value = 1)
    {
        Model.AddToken(value);
        View.UpdateView(Model);
    }

    private void HandleClick()
    {
        OnTokenClicked?.Invoke(Model);
    }
}