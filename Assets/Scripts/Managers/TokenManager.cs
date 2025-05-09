public class TokenPanelManager
{
    private TokenPanelModel _tokenPanelModel;
    private TokenPanelView _tokenPanelView;
    private TokenPanelController _tokenPanelController;

    public TokenPanelManager()
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
        //_tokenPanelModel = new TokenPanelModel();
    }

    private void InitializeTokenPanelView(TokenPanelView tokenPanelView)
    {
        _tokenPanelView = tokenPanelView;
    }   

    private void InitializeTokenPanelController()
    {
        _tokenPanelController = new TokenPanelController(_tokenPanelModel, _tokenPanelView);
    }

    // Handle Token Panel Events
}