using System;
using Newtonsoft.Json;

namespace QnAGame.Models
{
    [Serializable]
    internal class Player
    {
        [JsonProperty("name")] private string _name;
        [JsonProperty("highscore")] private int _highscore;

        [JsonIgnore] public string Name => _name;
        [JsonIgnore] public int Highscore => _highscore;

        public Player() { }
        public Player(string name, int highscore)
        {
            _name = name;
            _highscore = highscore;
        }
    }
}
