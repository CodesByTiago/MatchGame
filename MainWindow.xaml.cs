using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MatchGame;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly DispatcherTimer _timer = new DispatcherTimer();
    private int _tenthsOfSecondElapsed;
    private int _matchesFound;
    
    public MainWindow()
    {
        InitializeComponent();
        _timer.Interval = TimeSpan.FromSeconds(.1);
        _timer.Tick += Timer_Tick;
        SetUpGame();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _tenthsOfSecondElapsed++;
        TimeTextBlock.Text = (_tenthsOfSecondElapsed / 10F).ToString("0.0s");
        if (_matchesFound != 8) return;
        _timer.Stop();
        TimeTextBlock.Text = TimeTextBlock.Text + " - Play again?";
    }

    private void SetUpGame()
    {
        var animalEmoji = new List<string>
        {
            "🦑", "🦑",
            "🐠", "🐠",
            "🐘", "🐘",
            "🐳", "🐳",
            "🐪", "🐪",
            "🦖", "🦖",
            "🦘", "🦘",
            "🦔", "🦔"
        };

        var random = new Random();

        foreach (var textBlock in MainGrid.Children.OfType<TextBlock>())
        {
            if (textBlock.Name == "TimeTextBlock") continue;
            textBlock.Visibility = Visibility.Visible;
            var index = random.Next(animalEmoji.Count);
            var nextEmoji = animalEmoji[index];
            textBlock.Text = nextEmoji;
            animalEmoji.RemoveAt(index);

        }
        
        _timer.Start();
        _tenthsOfSecondElapsed = 0;
        _matchesFound = 0;
    }

    private TextBlock _lastTextBlockClicked = null!;
    private bool _findingMatch;
    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var textBlock = sender as TextBlock;
        if (_findingMatch == false)
        {
            textBlock!.Visibility = Visibility.Hidden;
            _lastTextBlockClicked = textBlock;
            _findingMatch = true;
        }
        else if (textBlock!.Text == _lastTextBlockClicked.Text)
        {
            _matchesFound++;
            textBlock.Visibility = Visibility.Hidden;
            _findingMatch = false;
        }
        else
        {
            _lastTextBlockClicked.Visibility = Visibility.Visible;
            _findingMatch = false;
        }
    }

    private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (_matchesFound == 8)
        {
            SetUpGame();
        }
    }
}