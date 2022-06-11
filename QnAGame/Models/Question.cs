using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnAGame.Models
{
    [Serializable]
    internal class Question
    {
        [JsonProperty("title")]
        private string _title;
        [JsonProperty("choices")]
        private List<string> _choices;
        [JsonProperty("category")]
        private string _category;

        [JsonIgnore] public string Title => _title;
        [JsonIgnore] public List<string> Choices => _choices;
        [JsonIgnore] public string Category => _category;

        public Question() { }

        public Question(string title, string category, List<string> choices)
        {
            _title = title;
            _category = category;
            _choices = choices;
        }
    }
}
