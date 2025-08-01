using System;
using DefaultNamespace.Data;
using R3;
using UnityEngine;
namespace UI.Board.BoardTokenPanel.BoardToken
{

    public class BoardTokenViewModel
    {
        public ResourceType ResourceType { get; private set; }
        public int TokenCount { get; private set; } = 0;

        public BoardTokenViewModel(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }

        public void SetTokenCount(int tokenCount)
        {
            TokenCount = tokenCount;
        }
    }
}