using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QnAGame.Models
{
    [Serializable]
    internal class Quiz
    {
        [JsonProperty("gameName")]
        private string _gameName;
        [JsonProperty("categories")]
        private List<string> _categories;
        [JsonProperty("questions")]
        private List<Question> _questions;

        [JsonIgnore] public string Name => _gameName;
        [JsonIgnore] public List<string> Categories => _categories;
        [JsonIgnore] public List<Question> Questions => _questions;

        public Quiz() { }

        public Quiz(string creatorName, List<string> categories, List<Question> questions)
        {
            _gameName = creatorName;
            _categories = categories;
            _questions = questions;
        }
    }
}
