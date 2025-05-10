using System;
using System.Collections.Generic;
using System.Linq;

public class TokenPanelModel
{    
    public TokenPanelModel()
    {

    }


    public TokenType[] AllTokenTypes => Enum.GetValues(typeof(TokenType)).Cast<TokenType>().ToArray();

}