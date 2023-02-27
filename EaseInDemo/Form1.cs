using MiniGameEngine;
using MiniGameEngine.Observers;
using MiniGameEngine.Transitions;
using MiniGameEngine.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EaseInDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GameContainer Game = new GameContainer(this)
            {
                Enabled = true,
                AutomaticallyPause = false,
                ShowFPS = true,
            };
            Icon = Icon.FromHandle(GameContainer.GetDefaultIcon().GetHicon());
            Game.AddScene(new BasicScene(Game), true);
        }
    }

    public class CustomSubscriber : Subscriber<Object>
    {
        public TextElement Element { get; private set; }

        public CustomSubscriber(TextElement element)
        {
            Element = element;
            Element.HorizontalAlignment = MiniGameEngine.UI.HorizontalAlignment.Center;
            Element.VerticalAlignment = VerticalAlignment.Center;
        }

        public override void Notify(object Value)
        {
            DoubleTransition transition = (DoubleTransition)this.Observer;
            Element.Text = (
                $"Value: {((double)Value).ToString("000.00")} ({(transition.Progress * 100).ToString("000.00")}%)" +
                Environment.NewLine +
                $"{transition.Direction}"
            );
            Element.Position = Element.Scene.Game.MIDDLE_POS;
        }
    }
    public class BasicScene : Scene
    {
        public BasicScene(GameContainer Game) : base(Game)
        {
            this.BackgroundColor = Color.Black;

            DoubleTransition DoubleNumberTransition = new DoubleTransition(
                0.00, 100.0, TimeSpan.FromSeconds(4), true
            )
            {
                Repeat = true,
                Reverse = true,
                EasingFunction = EasingFunctions.EaseIn
            };
            TextElement DoubleNumberElement = new TextElement("Double Number")
            {
                Font = new Font("Arial", 40, FontStyle.Bold),
                Position = Game.MIDDLE_POS,
                Color = Color.White
            };

            this.AddGameObject(DoubleNumberElement);
            DoubleNumberElement.AddTransition(DoubleNumberTransition);
            DoubleNumberTransition.Subscribe(new CustomSubscriber(DoubleNumberElement));
        }
    }
}
