using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Command
{
    /// <summary>
    /// Przykład użycia AsyncEventBus w systemie gry
    /// </summary>
    public class AsyncEventBusUsageExample : MonoBehaviour
    {
        /*
         * PRZYKŁAD 1: Tworzenie prostego event handlera
         */
        public class ExamplePresenter : IAsyncEventHandler<GameStartedEvent>
        {
            public async UniTask HandleAsync(GameStartedEvent gameEvent)
            {
                Debug.Log($"[ExamplePresenter] Gra rozpoczęta: {gameEvent.GameModel.GameId}");
                
                // Symuluj pracę UI (np. animacje)
                await UniTask.Delay(500);
                
                Debug.Log($"[ExamplePresenter] Zakończono inicjalizację UI");
            }
        }

        /*
         * PRZYKŁAD 2: Subskrybowanie eventów
         */
        public void SubscribeToEvents()
        {
            var presenter = new ExamplePresenter();
            
            // Subskrybuj eventy
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(presenter);
            
            // Można subskrybować wiele różnych eventów
            // AsyncEventBus.Instance.Subscribe<CardPurchasedEvent>(presenter);
        }

        /*
         * PRZYKŁAD 3: Publikowanie eventów w Command
         */
        public class ExampleCommand : BaseCommand
        {
            public override string CommandType => "ExampleCommand";
            
            public override bool Validate()
            {
                return true;
            }
            
            public override async UniTask<bool> Execute()
            {
                // Wykonaj logikę biznesową
                Debug.Log("[ExampleCommand] Wykonuję logikę biznesową...");
                
                // Publikuj event i czekaj na zakończenie wszystkich UI updates
                var gameStartedEvent = new GameStartedEvent(GameModel.Instance);
                await AsyncEventBus.Instance.PublishAndWaitAsync(gameStartedEvent);
                
                Debug.Log("[ExampleCommand] Wszystkie UI updates zakończone!");
                
                return true;
            }
        }

        /*
         * PRZYKŁAD 4: Czyszczenie event bus
         */
        public void CleanupEventBus()
        {
            // Oczyść wszystkie subskrypcje i oczekujące eventy
            AsyncEventBus.Instance.Clear();
        }

        /*
         * ARCHITEKTURA FLOW:
         * 
         * 1. User wykonuje akcję (np. kliknięcie przycisku)
         * 2. UI wywołuje Command przez CommandFactory
         * 3. Command wykonuje logikę biznesową (zmienia model)
         * 4. Command publikuje event używając PublishAndWaitAsync()
         * 5. Wszystkie zasubskrybowane Presenterzy otrzymują event
         * 6. Presenterzy czytają nowy stan z modelu i aktualizują UI
         * 7. Po zakończeniu wszystkich UI updates, Command kończy wykonanie
         * 
         * ZALETY:
         * - Command czeka na zakończenie wszystkich UI updates
         * - Separacja odpowiedzialności: Command = logika biznesowa, Presenter = UI
         * - Loose coupling między warstwami
         * - Możliwość łatwego dodawania nowych UI komponentów
         */
    }
} 