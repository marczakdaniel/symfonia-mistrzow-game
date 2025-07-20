using Cysharp.Threading.Tasks;
using Models;
using UnityEngine;
using Events;

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
                Debug.Log($"[ExamplePresenter] Gra rozpoczęta:");
                
                // Symuluj pracę UI (np. animacje)
                await UniTask.Delay(500);
                
                Debug.Log($"[ExamplePresenter] Zakończono inicjalizację UI");
            }
        }

        /*
         * PRZYKŁAD 2: Tworzenie handlerów z różnymi priorytetami
         */
        public class CriticalSystemHandler : IAsyncEventHandler<GameStartedEvent>
        {
            public async UniTask HandleAsync(GameStartedEvent gameEvent)
            {
                Debug.Log($"[CriticalSystemHandler] Rozpoczynam inicjalizację krytycznych systemów (priorytet: Critical)");
                await UniTask.Delay(100);
                Debug.Log($"[CriticalSystemHandler] Krytyczne systemy zainicjalizowane");
            }
        }

        public class CriticalSystemHandler2 : IAsyncEventHandler<GameStartedEvent>
        {
            public async UniTask HandleAsync(GameStartedEvent gameEvent)
            {
                Debug.Log($"[CriticalSystemHandler2] Rozpoczynam inicjalizację drugiego krytycznego systemu (priorytet: Critical)");
                await UniTask.Delay(150);
                Debug.Log($"[CriticalSystemHandler2] Drugi krytyczny system zainicjalizowany");
            }
        }

        public class HighPriorityHandler : IAsyncEventHandler<GameStartedEvent>
        {
            public async UniTask HandleAsync(GameStartedEvent gameEvent)
            {
                Debug.Log($"[HighPriorityHandler] Rozpoczynam konfigurację wysokiego priorytetu (priorytet: High)");
                await UniTask.Delay(200);
                Debug.Log($"[HighPriorityHandler] Konfiguracja wysokiego priorytetu zakończona");
            }
        }

        public class HighPriorityHandler2 : IAsyncEventHandler<GameStartedEvent>
        {
            public async UniTask HandleAsync(GameStartedEvent gameEvent)
            {
                Debug.Log($"[HighPriorityHandler2] Rozpoczynam drugą konfigurację wysokiego priorytetu (priorytet: High)");
                await UniTask.Delay(180);
                Debug.Log($"[HighPriorityHandler2] Druga konfiguracja wysokiego priorytetu zakończona");
            }
        }

        public class NormalPriorityHandler : IAsyncEventHandler<GameStartedEvent>
        {
            public async UniTask HandleAsync(GameStartedEvent gameEvent)
            {
                Debug.Log($"[NormalPriorityHandler] Rozpoczynam standardową inicjalizację (priorytet: Normal)");
                await UniTask.Delay(300);
                Debug.Log($"[NormalPriorityHandler] Standardowa inicjalizacja zakończona");
            }
        }

        public class LowPriorityHandler : IAsyncEventHandler<GameStartedEvent>
        {
            public async UniTask HandleAsync(GameStartedEvent gameEvent)
            {
                Debug.Log($"[LowPriorityHandler] Rozpoczynam inicjalizację niskiego priorytetu (priorytet: Low)");
                await UniTask.Delay(400);
                Debug.Log($"[LowPriorityHandler] Inicjalizacja niskiego priorytetu zakończona");
            }
        }

        /*
         * PRZYKŁAD 3: Subskrybowanie eventów z priorytetami
         */
        public void SubscribeToEventsWithPriorities()
        {
            // Subskrybuj handlery z różnymi priorytetami
            // Critical - wykonają się jako pierwsze (równolegle między sobą)
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(new CriticalSystemHandler(), EventPriority.Critical);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(new CriticalSystemHandler2(), EventPriority.Critical);
            
            // High - wykonają się po zakończeniu wszystkich Critical (równolegle między sobą)
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(new HighPriorityHandler(), EventPriority.High);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(new HighPriorityHandler2(), EventPriority.High);
            
            // Normal - wykonają się po zakończeniu wszystkich High
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(new NormalPriorityHandler(), EventPriority.Normal);
            
            // Low - wykonają się jako ostatnie
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(new LowPriorityHandler(), EventPriority.Low);
            
            // Domyślny priorytet to Normal
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(new ExamplePresenter());
        }

        /*
         * PRZYKŁAD 4: Subskrybowanie eventów (stary sposób - bez priorytetów)
         */
        public void SubscribeToEvents()
        {
            var presenter = new ExamplePresenter();
            
            // Subskrybuj eventy (domyślny priorytet: Normal)
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(presenter);
            
            // Można subskrybować wiele różnych eventów
            // AsyncEventBus.Instance.Subscribe<CardPurchasedEvent>(presenter);
        }

        /*
         * PRZYKŁAD 5: Publikowanie eventów w Command
         */
        /*public class ExampleCommand : BaseCommand
        {
            public override string CommandType => "ExampleCommand";
            
                    public override async UniTask<bool> Validate()
        {
            return true;
        }
            
            public override async UniTask<bool> Execute()
            {
                // Wykonaj logikę biznesową
                Debug.Log("[ExampleCommand] Wykonuję logikę biznesową...");
                
                // Publikuj event i czekaj na zakończenie wszystkich UI updates
                var gameStartedEvent = new GameStartedEvent();
                await AsyncEventBus.Instance.PublishAndWaitAsync(gameStartedEvent);
                
                Debug.Log("[ExampleCommand] Wszystkie UI updates zakończone!");
                
                return true;
            }
        }*/

        /*
         * PRZYKŁAD 6: Czyszczenie event bus
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
         * 5. Wszystkie zasubskrybowane Presenterzy otrzymują event sekwencyjnie według priorytetów:
         *    - FAZA 1: Wszystkie handlery Critical wykonują się równolegle, czekamy na ich zakończenie
         *    - FAZA 2: Wszystkie handlery High wykonują się równolegle, czekamy na ich zakończenie
         *    - FAZA 3: Wszystkie handlery Normal wykonują się równolegle, czekamy na ich zakończenie
         *    - FAZA 4: Wszystkie handlery Low wykonują się równolegle, czekamy na ich zakończenie
         * 6. Presenterzy czytają nowy stan z modelu i aktualizują UI
         * 7. Po zakończeniu wszystkich UI updates, Command kończy wykonanie
         * 
         * ZALETY:
         * - Command czeka na zakończenie wszystkich UI updates
         * - Separacja odpowiedzialności: Command = logika biznesowa, Presenter = UI
         * - Loose coupling między warstwami
         * - Możliwość łatwego dodawania nowych UI komponentów
         * - Kontrola kolejności wykonywania handlerów poprzez priorytety
         * - Krytyczne systemy są inicjalizowane przed standardowymi
         * - Sekwencyjne wykonywanie priorytetów zapewnia przewidywalny flow
         * - Handlery tego samego priorytetu wykonują się równolegle dla lepszej wydajności
         */
    }
} 