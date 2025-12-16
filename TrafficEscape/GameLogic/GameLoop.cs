using Microsoft.Maui.Animations;
using Microsoft.Maui.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.GameLogic;
    public class GameLoop
    {
        private readonly IDispatcherTimer timer;
        private readonly Action<double> onUpdate;
        private DateTime lastUpdate;

        public GameLoop(IDispatcher dispatcher, Action<double> onUpdate)
        {
            this.onUpdate = onUpdate;
            timer = dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += Tick;
        }

        public void Start()
        {
            lastUpdate = DateTime.Now;
            timer.Start();
        }

    public void Stop()
    {
        timer.Stop();
    }
    private void Tick(object? sender, EventArgs e) 
    { 
        var now = DateTime.Now;
        double refresh = (now - lastUpdate).TotalSeconds;
        onUpdate(refresh);
    }

}
