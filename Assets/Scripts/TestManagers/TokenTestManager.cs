using System;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DefaultNamespace.Managers
{
    public class TokenTestManager : MonoBehaviour
    {
        [SerializeField] private TokenPanelView tokenPanelView;

        private TokenPanelModel tokenPanelModel;
        private TokenPanelController tokenPanelController;

        private void Start()
        {
            CreateTestModel();
            InitializeGame();
        }

        private void InitializeGame()
        {
            tokenPanelController = new TokenPanelController(tokenPanelModel, tokenPanelView);
            tokenPanelController.OnTokenClicked += HandleClick;
        }

        private void HandleClick(TokenModel tokenModel)
        {
            tokenPanelController.RemoveToken(tokenModel.TokenType);
        }

        private void CreateTestModel()
        {
            var tokens = 0;
            var initialValues = new Dictionary<TokenType, int>();
            foreach (var tokenType in (TokenType[]) Enum.GetValues(typeof(TokenType)))
            {
                initialValues[tokenType] = tokens;
                tokens += 1;
            }

            tokenPanelModel = new TokenPanelModel(initialValues);
        }
        
        
    }
}