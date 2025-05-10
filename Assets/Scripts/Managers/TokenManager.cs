using System.Collections.Generic;

public class TokenManager
{
    private TokenPanelModel _tokenPanelModel;
    private TokenPanelView _tokenPanelView;
    private TokenPanelPresenter _tokenPanelController;

    public TokenManager()
    {
        
    }

    public void InitializeTokenPanelMVC(TokenPanelView tokenPanelView)
    {
        InitializeTokenPanelModel();
        InitializeTokenPanelView(tokenPanelView);
        InitializeTokenPanelController();
    }

    private void InitializeTokenPanelModel()
    {
        _tokenPanelModel = new TokenPanelModel();
    }

    private void InitializeTokenPanelView(TokenPanelView tokenPanelView)
    {
        _tokenPanelView = tokenPanelView;
    }   

    private void InitializeTokenPanelController()
    {
        _tokenPanelController = new TokenPanelPresenter(_tokenPanelModel, _tokenPanelView);
        HandleTokenPanelEvents();
    }

    // Handle Token Panel Events

    private void HandleTokenPanelEvents()
    {
        _tokenPanelController.OnTokenClicked += HandleTokenClicked;
    }

    private void HandleTokenClicked(TokenModel tokenModel)
    {
        // TODO: Handle token clicked
    }   

    // Expose necessary methods to change board

    public void InitializeTokenPanel(Dictionary<TokenType, int> initialTokensValues)
    {
        _tokenPanelModel.InitializeTokenPanel(initialTokensValues);
    }

    public void AddToken(TokenType tokenType, int value)
    {
        _tokenPanelModel.AddToken(tokenType, value);
    }

    public void RemoveToken(TokenType tokenType, int value)
    {
        _tokenPanelModel.RemoveToken(tokenType, value);
    }
    
}