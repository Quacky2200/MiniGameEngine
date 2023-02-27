using System;
using System.Drawing;
using System.Windows.Forms;
using MiniGameEngine;
using MiniGameEngine.UI;

namespace CountdownDemo
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            GameContainer Game = new GameContainer(this) {
                Enabled = true, AutomaticallyPause = false, ShowFPS = false,
                MustAntiAliasText = true, MustInterpolate = true, MustSmooth = true,
            };
            Icon = Icon.FromHandle(GameContainer.GetDefaultIcon().GetHicon());
            DateTime EpochStart = new DateTime(1970, 01, 01);
            Game.AddScene(new CountdownDemo(
                // Epoch date + (epoch ms time * Ticks per millisecond)
                // Epoch date + (epoch s time * Ticks per second)
                Game, EpochStart.AddTicks(1680940800 * TimeSpan.TicksPerSecond)
            ), true);
        }
    }

    public class CountdownDemo : Scene
    {
        private readonly CountdownComponent CountdownComponent = new CountdownComponent();

        public DateTime CountdownDate { get { return CountdownComponent.CountdownDate; }  set { CountdownComponent.CountdownDate = value; } }
        public string CountdownTitle { get { return CountdownComponent.CountdownTitle; } set { CountdownComponent.CountdownTitle = value; } }

        private readonly TextElement CountdownTitleElement = new TextElement()
        {
            Color = Color.White, 
            Font = new Font("Arial", 40, FontStyle.Regular),
            HorizontalAlignment = MiniGameEngine.UI.HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Position = Point.Empty
        };

        private readonly TextElement CountdownDateElement = new TextElement()
        {
            Color = Color.White,
            Font = new Font("Arial", 30, FontStyle.Regular),
            HorizontalAlignment = MiniGameEngine.UI.HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Position = Point.Empty
        };

        public CountdownDemo(GameContainer Game, DateTime To, string Title = null) : base(Game)
        {
            CountdownDate = To;
            BackgroundColor = Color.Black;

            // Refer them to component to update their values.
            CountdownComponent.DateElement = CountdownDateElement;
            CountdownComponent.TitleElement = CountdownTitleElement;
            RefreshPositions();

            CountdownTitle = (!string.IsNullOrEmpty(Title) ? Title : $"Countdown to {To.ToLongDateString()} {To.ToShortTimeString()}");

            CountdownDateElement.AddComponent(CountdownComponent);
            AddGameObject(CountdownDateElement);
            AddGameObject(CountdownTitleElement);


            Game.Window.Resize += Window_Resize;
        }

        private void Window_Resize(object sender, EventArgs e)
        {
            RefreshPositions();
        }

        private void RefreshPositions()
        {
            var middle = Game.MIDDLE_POS;
            middle.Offset(0, -40);
            CountdownTitleElement.Position = middle;
            middle.Offset(0, 60); // Yuck, why can't we return a new point :(
            CountdownDateElement.Position = middle;
        }

        public override void KeyDown(Keys KeyCode)
        {
            if (KeyCode == Keys.F11)
            {
                Game.ToggleFullscreen();
                RefreshPositions();
            }
        }
    }

    public class CountdownComponent : Component
    {
        public DateTime CountdownDate { get; set; }
        public string CountdownTitle { get; set; }

        public TextElement TitleElement;
        public TextElement DateElement;

        private StatisticVariable<string> CountdownUpdater;

        public CountdownComponent()
        {
            CountdownUpdater = new StatisticVariable<string>(() =>
            {
                var Time = TimeSpan.FromTicks(CountdownDate.Ticks - DateTime.Now.Ticks);
                return $"{Time.Days} Days {Time.Hours.ToString("00")}:{Time.Minutes.ToString("00")}:{Time.Seconds.ToString("00")}.{Time.Milliseconds.ToString("000")}";
            }, TimeSpan.FromMilliseconds(1));
        }

        public override void Update(double Delta)
        {
            DateElement.Text = CountdownUpdater.Value;
            TitleElement.Text = CountdownTitle;
        }
    }


    public class MiniComponent : Component
    {
        private readonly Action<double> Work;
        public MiniComponent(Action<double> Work) : base()
        {
            this.Work = Work;
        }
        public override void Update(double Delta)
        {
            Work(Delta);
        }
    }
}
