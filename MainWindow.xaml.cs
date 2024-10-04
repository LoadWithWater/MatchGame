using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchGame
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            TimeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
        }

        private void SetUpGame()
        {
            List<String> animalEmoji = new List<string>()
            {
                "🐭", "🐭",
                "🐱", "🐱",
                "🦊", "🦊",
                "🐰", "🐰",
                "🦝", "🦝",
                "🦁", "🦁",
                "🐯", "🐯",
                "🐣", "🐣",
            };

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;

                    if (animalEmoji.Count > 0)
                    {
                        int index = random.Next(animalEmoji.Count);
                        string nextEmoji = animalEmoji[index];
                        textBlock.Text = nextEmoji;
                        animalEmoji.RemoveAt(index);
                    }
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else
            {
                if (textBlock.Text == lastTextBlockClicked.Text)
                {
                    textBlock.Visibility = Visibility.Hidden;
                    matchesFound++;

                    if (matchesFound == 8)  // 모든 짝을 찾았을 때
                    {
                        timer.Stop();
                        TimeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s") + " - Play again?";
                    }
                }
                else
                {
                    lastTextBlockClicked.Visibility = Visibility.Visible;
                }
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
