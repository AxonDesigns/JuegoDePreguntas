using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QnAGame.Models
{
    [Serializable]
    internal class GameSettings
    {
        [JsonProperty("players")] private List<Player> _players = new List<Player>();
        [JsonProperty("quizzes")] private List<Quiz> _quizzes = new List<Quiz>();

        [JsonIgnore] public List<Player> Players => _players;
        [JsonIgnore] public List<Quiz> Quizes => _quizzes;

        public GameSettings()  { }

        public GameSettings(List<Player> players, List<Quiz> quizzes)
        {
            _players = players;
            _quizzes = quizzes;
        }
    }
}
