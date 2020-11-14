using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WordTrainer
{
    public partial class MainPage : ContentPage
    {
        private WordSelector _ws = new WordSelector();
        private Question _currentQuestion;

        public MainPage()
        {
            InitializeComponent();
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            _currentQuestion = null;

            try
            {
                _currentQuestion = _ws.NextQuestion();

            }
            catch (Exception) { }


            if (_currentQuestion?.Word == null || _currentQuestion?.Answer == null)
            {
                var serviceError = new Label { Text = "no data from service", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.Red };
                var btn = new Button { Text = "refresh" };
                btn.Clicked += OnNextButtonClicked;

                selectWord.Children.Clear();
                selectWord.Children.Add(serviceError);
                selectWord.Children.Add(btn);
                return;
            }

            var word = new Label { Text = _currentQuestion.Word, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };

            selectWord.Children.Clear();
            selectWord.Children.Add(word);

            foreach (var o in _currentQuestion.Options)
            {
                var btnOptionalWord = new Button { Text = o, FontSize = 30, TextTransform = TextTransform.None };
                btnOptionalWord.Clicked += OnButtonClicked;
                selectWord.Children.Add(btnOptionalWord);
            }
        }

        private void OnButtonClicked(object sender, EventArgs args)
        {
            var btnSelectedWord = sender as Button;

            selectWord.Children.Add(new Label { Text = "correct" });

            foreach (var e in selectWord.Children)
            {
                if (e is Button)
                {
                    var btnWord = e as Button;
                    btnWord.IsEnabled = false;
                    if (btnSelectedWord.Text == btnWord.Text)
                    {
                        btnWord.BackgroundColor = btnSelectedWord.Text == _currentQuestion.Answer ? Color.Green : Color.Red;
                    }
                }
            }

            var btn = new Button { Text = "next" };
            btn.Clicked += OnNextButtonClicked;

            selectWord.Children.Add(btn);
        }

        private void OnNextButtonClicked(object sender, EventArgs args)
        {
            GenerateButtons();
        }
    }
}
