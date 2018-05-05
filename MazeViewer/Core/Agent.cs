using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MazeViewer.Core
{
    public class Agent
    {
        public Point Pos { get; private set; } = new Point(0.0, 0.0);
        public Stopwatch TotalTime { get; } = new Stopwatch();
        public Stopwatch[] GoalTime { get; } = Enumerable.Repeat(new Stopwatch(), Consts.MaxGoalCount).ToArray();

        public Maze InternalMazeData { get; set; }

        private State agentState = State.Start;

        public void Simulate()
        {
            TotalTime.Reset();
            TotalTime.Start();
        }

        public void Clear()
        {
            TotalTime.Reset();
            foreach (var stopwatch in GoalTime) stopwatch.Reset();
        }

        private State NextState()
        {
            switch (this.agentState)
            {
                default: return this.agentState;
            }
        }
        
        private DispatcherTimer renderTimer = null;
        public void StartRender(Panel panel)
        {
            this.renderTimer = new DispatcherTimer(DispatcherPriority.Render, panel.Dispatcher)
            {
                Interval = TimeSpan.FromMilliseconds(1000.0 / 60.0),
            };
            this.renderTimer.Tick += (object sender, EventArgs e) =>
            {
                Render(panel);
            };
        }

        public void StopRender()
        {
            if(this.renderTimer != null)
            {
                this.renderTimer.Stop();
                this.renderTimer = null;
            }
        }
        
        public void Render(Panel panel)
        {
            panel.Children.Clear();

            var mouse = new Path
            {
                Data = new EllipseGeometry
                {
                    Center = Pos,
                    RadiusX = 50.0,
                    RadiusY = 50.0,
                },
                Stroke = new SolidColorBrush(Colors.Red),
            };

            panel.Children.Add(mouse);
        }
        
        internal enum State
        {
            Start,
            Initialize,
            Search,
            Fast1,
            Fast2,
            Fast3,
            Fast4,
        }
    }
}
