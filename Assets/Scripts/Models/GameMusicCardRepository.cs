using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Models {
    public class GameMusicCardRepository {
        /*private readonly Dictionary<string, MusicCardModel> musicCards = new Dictionary<string, MusicCardModel>();

        public MusicCardModel CreateCard(MusicCardData musicCardData) {
            if (musicCardData == null) {
                throw new ArgumentNullException(nameof(musicCardData));
            }

            if (musicCards.ContainsKey(musicCardData.id)) {
                Debug.LogWarning($"Card with ID {musicCardData.id} already exists");
            }

            var musicCardModel = new MusicCardModel(musicCardData);
            musicCards.Add(musicCardData.id, musicCardModel);
            return musicCardModel;
        }

        public MusicCardModel GetCard(string id) {
            if (musicCards.TryGetValue(id, out var musicCard)) {
                return musicCard;
            }
            Debug.LogWarning($"Card with ID {id} not found in repository");
            return null;
        }
        
        public IEnumerable<MusicCardModel> GetAllCards() {
            return musicCards.Values;
        }

        public IEnumerable<MusicCardModel> GetCardsByState(MusicCardState state) {
            return musicCards.Values.Where(card => card.State.Value == state);
        }

        public IEnumerable<MusicCardModel> GetCardsByOwner(string ownerId) {
            if (string.IsNullOrEmpty(ownerId)) {
                Debug.LogWarning("Owner ID is null or empty");
                return Enumerable.Empty<MusicCardModel>();
            }

            return musicCards.Values.Where(card => card.OwnerId?.Value == ownerId);
        }

        */
    }
}