using System.Net.Http;
using System.Text.Json;

namespace WordTrainer
{
    public class WordSelector
    {
        private const string _url = "...";

        private HttpClient _client;

        private HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new HttpClient();
                }

                return _client;
            }
        }

        internal Question NextQuestion()
        {
            var result = Client.GetStringAsync(_url).ConfigureAwait(true).GetAwaiter().GetResult();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var question = JsonSerializer.Deserialize<Question>(result, options);

            return question;
        }
    }
}
