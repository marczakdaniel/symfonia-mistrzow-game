using System;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DefaultNamespace.Managers
{
    public class TokenTestManager : MonoBehaviour
    {
        [SerializeField] private TokenPanelView tokenPanelView;

        private TokenManager tokenManager;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                CreateTestModel();
            }
        }

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            tokenManager = new TokenManager();
            tokenManager.InitializeTokenPanelMVC(tokenPanelView);
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

            tokenManager.InitializeTokenPanel(initialValues);
        }
        
        
    }
}