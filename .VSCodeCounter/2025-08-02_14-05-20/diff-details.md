# Diff Details

Date : 2025-08-02 14:05:20

Directory c:\\Users\\danie\\Desktop\\symfonia-mistrzow-game\\Assets\\Scripts

Total : 47 files,  703 codes, -92 comments, 127 blanks, all 738 lines

[Summary](results.md) / [Details](details.md) / [Diff Summary](diff.md) / Diff Details

## Files
| filename | language | code | comment | blank | total |
| :--- | :--- | ---: | ---: | ---: | ---: |
| [Assets/Scripts/Commands/Command.cs](/Assets/Scripts/Commands/Command.cs) | C# | 33 | 0 | -1 | 32 |
| [Assets/Scripts/Commands/CommandFactory.cs](/Assets/Scripts/Commands/CommandFactory.cs) | C# | 36 | 4 | 13 | 53 |
| [Assets/Scripts/Commands/GameUICommand.cs](/Assets/Scripts/Commands/GameUICommand.cs) | C# | 143 | -62 | 35 | 116 |
| [Assets/Scripts/Data/ConcertCardData.cs](/Assets/Scripts/Data/ConcertCardData.cs) | C# | 26 | 0 | 8 | 34 |
| [Assets/Scripts/Data/ConcertCardDeckData.cs](/Assets/Scripts/Data/ConcertCardDeckData.cs) | C# | 10 | 0 | 2 | 12 |
| [Assets/Scripts/Data/MusicCardData.cs](/Assets/Scripts/Data/MusicCardData.cs) | C# | 6 | 0 | 3 | 9 |
| [Assets/Scripts/Events/GameEvents.cs](/Assets/Scripts/Events/GameEvents.cs) | C# | 2 | 0 | 0 | 2 |
| [Assets/Scripts/Events/UIEvents.cs](/Assets/Scripts/Events/UIEvents.cs) | C# | 83 | 3 | 13 | 99 |
| [Assets/Scripts/Models/BoardModel.cs](/Assets/Scripts/Models/BoardModel.cs) | C# | 9 | 0 | 1 | 10 |
| [Assets/Scripts/Models/ConcertCardModel.cs](/Assets/Scripts/Models/ConcertCardModel.cs) | C# | 36 | 0 | 6 | 42 |
| [Assets/Scripts/Models/GameConfig.cs](/Assets/Scripts/Models/GameConfig.cs) | C# | 11 | 0 | 2 | 13 |
| [Assets/Scripts/Models/GameModel.cs](/Assets/Scripts/Models/GameModel.cs) | C# | 10 | 0 | 1 | 11 |
| [Assets/Scripts/Models/MusicCardCollectionModel.cs](/Assets/Scripts/Models/MusicCardCollectionModel.cs) | C# | 18 | 0 | 1 | 19 |
| [Assets/Scripts/Models/PlayerModel.cs](/Assets/Scripts/Models/PlayerModel.cs) | C# | 35 | 0 | 7 | 42 |
| [Assets/Scripts/Models/ResourceCollectionModel.cs](/Assets/Scripts/Models/ResourceCollectionModel.cs) | C# | 30 | 0 | 3 | 33 |
| [Assets/Scripts/Services/BoardService.cs](/Assets/Scripts/Services/BoardService.cs) | C# | 9 | 0 | 1 | 10 |
| [Assets/Scripts/Services/PlayerService.cs](/Assets/Scripts/Services/PlayerService.cs) | C# | 0 | 0 | 2 | 2 |
| [Assets/Scripts/Services/TurnService.cs](/Assets/Scripts/Services/TurnService.cs) | C# | 77 | 1 | 24 | 102 |
| [Assets/Scripts/Test/GameTestManager.cs](/Assets/Scripts/Test/GameTestManager.cs) | C# | 3 | 0 | 0 | 3 |
| [Assets/Scripts/UI/Board/BoardMusicCardPanel/BoardMusicCardPanelPresenter.cs](/Assets/Scripts/UI/Board/BoardMusicCardPanel/BoardMusicCardPanelPresenter.cs) | C# | -27 | -4 | -7 | -38 |
| [Assets/Scripts/UI/Board/BoardMusicCardPanel/BoardMusicCard/BoardMusicCardPresenter.cs](/Assets/Scripts/UI/Board/BoardMusicCardPanel/BoardMusicCard/BoardMusicCardPresenter.cs) | C# | -54 | -24 | -25 | -103 |
| [Assets/Scripts/UI/Board/BoardMusicCardPanel/BoardMusicCard/BoardMusicCardView.cs](/Assets/Scripts/UI/Board/BoardMusicCardPanel/BoardMusicCard/BoardMusicCardView.cs) | C# | -19 | -2 | 0 | -21 |
| [Assets/Scripts/UI/Board/BoardMusicCardPanel/BoardMusicCard/BoardMusicCardViewModel.cs](/Assets/Scripts/UI/Board/BoardMusicCardPanel/BoardMusicCard/BoardMusicCardViewModel.cs) | C# | -124 | 0 | -24 | -148 |
| [Assets/Scripts/UI/Board/BoardPresenter.cs](/Assets/Scripts/UI/Board/BoardPresenter.cs) | C# | 12 | 0 | 4 | 16 |
| [Assets/Scripts/UI/Board/BoardToken/BoardTokenPresenter.cs](/Assets/Scripts/UI/Board/BoardToken/BoardTokenPresenter.cs) | C# | -35 | -3 | -6 | -44 |
| [Assets/Scripts/UI/Board/BoardToken/BoardTokenViewModel.cs](/Assets/Scripts/UI/Board/BoardToken/BoardTokenViewModel.cs) | C# | -126 | -1 | -25 | -152 |
| [Assets/Scripts/UI/Board/BoardView.cs](/Assets/Scripts/UI/Board/BoardView.cs) | C# | 8 | 0 | 1 | 9 |
| [Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseSingleToken/CardPurchaseSingleTokenPresenter.cs](/Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseSingleToken/CardPurchaseSingleTokenPresenter.cs) | C# | 8 | 0 | 1 | 9 |
| [Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseSingleToken/CardPurchaseSingleTokenViewModel.cs](/Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseSingleToken/CardPurchaseSingleTokenViewModel.cs) | C# | 2 | 0 | 0 | 2 |
| [Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseWindowPresenter.cs](/Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseWindowPresenter.cs) | C# | 8 | 0 | 1 | 9 |
| [Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseWindowView.cs](/Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseWindowView.cs) | C# | 10 | 0 | 0 | 10 |
| [Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseWindowViewModel.cs](/Assets/Scripts/UI/CardPurchaseWindow/CardPurchaseWindowViewModel.cs) | C# | 5 | 0 | 0 | 5 |
| [Assets/Scripts/UI/ConcertCardsWindow/ConcertCardsWindowPresenter.cs](/Assets/Scripts/UI/ConcertCardsWindow/ConcertCardsWindowPresenter.cs) | C# | 58 | 0 | 13 | 71 |
| [Assets/Scripts/UI/ConcertCardsWindow/ConcertCardsWindowView.cs](/Assets/Scripts/UI/ConcertCardsWindow/ConcertCardsWindowView.cs) | C# | 52 | 0 | 11 | 63 |
| [Assets/Scripts/UI/Elements/CardRequirementContainer.cs](/Assets/Scripts/UI/Elements/CardRequirementContainer.cs) | C# | 19 | 0 | 2 | 21 |
| [Assets/Scripts/UI/Elements/ConcertCardElement.cs](/Assets/Scripts/UI/Elements/ConcertCardElement.cs) | C# | 60 | 0 | 10 | 70 |
| [Assets/Scripts/UI/GameWindow/GameWindowPresenter.cs](/Assets/Scripts/UI/GameWindow/GameWindowPresenter.cs) | C# | -4 | -4 | -3 | -11 |
| [Assets/Scripts/UI/GameWindow/GameWindowView.cs](/Assets/Scripts/UI/GameWindow/GameWindowView.cs) | C# | 6 | 0 | 0 | 6 |
| [Assets/Scripts/UI/InfoWindow/InfoWindowPresenter.cs](/Assets/Scripts/UI/InfoWindow/InfoWindowPresenter.cs) | C# | 57 | 0 | 12 | 69 |
| [Assets/Scripts/UI/InfoWindow/InfoWindowView.cs](/Assets/Scripts/UI/InfoWindow/InfoWindowView.cs) | C# | 37 | 0 | 9 | 46 |
| [Assets/Scripts/UI/MusicCardDetailsPanel/DetailsMusicCardView.cs](/Assets/Scripts/UI/MusicCardDetailsPanel/DetailsMusicCardView.cs) | C# | 34 | 0 | 3 | 37 |
| [Assets/Scripts/UI/PlayerResourcesWindow/PlayerResourcesWindowPresenter.cs](/Assets/Scripts/UI/PlayerResourcesWindow/PlayerResourcesWindowPresenter.cs) | C# | 12 | 0 | 2 | 14 |
| [Assets/Scripts/UI/PlayerResourcesWindow/PlayerResourcesWindowView.cs](/Assets/Scripts/UI/PlayerResourcesWindow/PlayerResourcesWindowView.cs) | C# | 10 | 0 | 2 | 12 |
| [Assets/Scripts/UI/ReturnTokenWindow/ReturnTokenWindowPresenter.cs](/Assets/Scripts/UI/ReturnTokenWindow/ReturnTokenWindowPresenter.cs) | C# | 13 | 0 | 2 | 15 |
| [Assets/Scripts/UI/ReturnTokenWindow/ReturnTokenWindowView.cs](/Assets/Scripts/UI/ReturnTokenWindow/ReturnTokenWindowView.cs) | C# | 3 | 0 | -1 | 2 |
| [Assets/Scripts/UI/StartPageWindow/StartPagePresenter.cs](/Assets/Scripts/UI/StartPageWindow/StartPagePresenter.cs) | C# | 63 | 0 | 15 | 78 |
| [Assets/Scripts/UI/StartPageWindow/StartPageView.cs](/Assets/Scripts/UI/StartPageWindow/StartPageView.cs) | C# | 38 | 0 | 9 | 47 |

[Summary](results.md) / [Details](details.md) / [Diff Summary](diff.md) / Diff Details