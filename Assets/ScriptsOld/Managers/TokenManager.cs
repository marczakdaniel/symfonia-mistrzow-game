using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TokenManager
{
    private TokenPanelModel _tokenPanelModel;
    private TokenPanelView _tokenPanelView;
    private TokenPanelPresenter _tokenPanelPresenter;

    public TokenManager()
    {
        
    }

    public void InitializeTokenPanelMVC(TokenPanelView tokenPanelView)
    {
        InitializeTokenPanelModel();
        InitializeTokenPanelView(tokenPanelView);
        InitializeTokenPanelPresenter();
        ConnectTokenPanelEvents();
    }

    private void InitializeTokenPanelModel()
    {
        _tokenPanelModel = new TokenPanelModel();
    }

    private void InitializeTokenPanelView(TokenPanelView tokenPanelView)
    {
        _tokenPanelView = tokenPanelView;
    }   

    private void InitializeTokenPanelPresenter()
    {
        _tokenPanelPresenter = new TokenPanelPresenter(_tokenPanelModel, _tokenPanelView);
    }

    private void ConnectTokenPanelEvents()
    {
        _tokenPanelPresenter.OnTokenClicked += HandleTokenClicked;
    }

    // Handle Token Panel Events
    private void HandleTokenClicked(TokenModel tokenModel)
    {
        _tokenPanelPresenter.AddToken(tokenModel.TokenType, 1);
    }   

    // Expose necessary methods to change board

    public void InitializeTokenPanel(Dictionary<TokenType, int> initialTokensValues)
    {
        _tokenPanelPresenter.InitializeTokenPanel(initialTokensValues).Forget();
    }

    public void AddToken(TokenType tokenType, int value)
    {
        _tokenPanelPresenter.AddToken(tokenType, value);
    }

    public void RemoveToken(TokenType tokenType, int value)
    {
        _tokenPanelPresenter.RemoveToken(tokenType, value);
    }
    
}