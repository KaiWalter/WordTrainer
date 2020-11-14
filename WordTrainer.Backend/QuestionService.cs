using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WordTrainer.Model;
using System.Linq;

namespace WordTrainer.Backend
{
    public static class QuestionService
    {
        private static readonly Random _rnd = new Random();

        private static readonly List<Word> _wordList = new List<Word>()
        {
            new Word {PrimaryLanguage = "darüber", SecondaryLanguage = "above", Group = "location"},
            new Word {PrimaryLanguage = "darunter", SecondaryLanguage = "below", Group = "location"},
            new Word {PrimaryLanguage = "dahinter", SecondaryLanguage = "behind", Group = "location"},
            new Word {PrimaryLanguage = "davor", SecondaryLanguage = "before", Group = "location"},
            new Word {PrimaryLanguage = "Eimer", SecondaryLanguage = "bucket", Group = "object"},
            new Word {PrimaryLanguage = "kleiner Spaten", SecondaryLanguage = "trowel", Group = "object"},
            new Word {PrimaryLanguage = "Haus", SecondaryLanguage = "house", Group = "object"},
            new Word {PrimaryLanguage = "Tonpfeife", SecondaryLanguage = "clay pipe", Group = "object"},
            new Word {PrimaryLanguage = "wax figure", SecondaryLanguage = "Wachsfigur", Group = "object"},
        };

        [FunctionName("GetNextQuestion")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        => new OkObjectResult(NextQuestion());

        private static Question NextQuestion()
        {
            bool fromPrimary = _rnd.Next(0, 50) <= 25;

            int index = _rnd.Next(0, _wordList.Count);
            var selectedWord = _wordList[index];

            var q = new Question
            {
                Word = selectedWord.Get(fromPrimary),
                Answer = selectedWord.Get(!fromPrimary),

            };

            var optionalWords = new List<string>()
                {
                    selectedWord.Get(!fromPrimary)
                };

            var maxOptions = _rnd.Next(4, 5);
            var wordGroup = _wordList.Where(w => w.Group == selectedWord.Group && w.PrimaryLanguage != selectedWord.PrimaryLanguage).ToList();
            for (int i = 0; i <= maxOptions && wordGroup.Count > 0; i++)
            {
                var optionalWordIndex = _rnd.Next(0, wordGroup.Count);
                optionalWords.Add(wordGroup[optionalWordIndex].Get(!fromPrimary));
                wordGroup.RemoveAt(optionalWordIndex);
            }

            q.Options = Shuffle(optionalWords);

            return q;
        }

        private static List<string> Shuffle(List<string> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rnd.Next(n + 1);
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

    }
}
