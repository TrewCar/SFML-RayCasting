using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Utils
{
    public class FPSCounter
    {
        private Stopwatch stopwatch = new Stopwatch();
        private int frameCount = 0;
        private double elapsedTime = 0;

        public FPSCounter()
        {
            stopwatch.Start();
        }

        public void Update()
        {
            frameCount++;
            elapsedTime += stopwatch.ElapsedMilliseconds;
            
            double fps = frameCount / (elapsedTime / 1000);
            Console.WriteLine($"FPS: {fps:F1}");
            frameCount = 0;
            elapsedTime = 0;
            
            stopwatch.Restart(); // Сбросить таймер для следующего кадра
        }
    }
}
