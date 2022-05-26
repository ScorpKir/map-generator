using ScottPlot;
using System;
using System.Windows;

namespace MapGenerator.Views
{
    public partial class MapWindow : Window
    {
        private double[,] _map;
        private int SIZE;
        private double _seed;
        private double _roughness;

        public MapWindow(int degree, double seed, double roughness)
        {

            this.SIZE = Convert.ToInt32(Math.Pow(2, degree) + 1);
            this._seed = seed;
            this._roughness = roughness;
            this._map = new double[SIZE, SIZE];
            this.plot = new WpfPlot();
            _map[0, 0] = _map[SIZE - 1, 0] = _map[0, SIZE - 1] = _map[SIZE - 1, SIZE - 1] = _seed;
            InitializeComponent();
            fillMap();
            fillModel();
        }

        private void fillMap()
        {
            double h = _roughness;
            Random r = new Random();

            for (int sideLength = SIZE - 1; sideLength >= 2; sideLength /= 2, h /= 2.0)
            {
                int halfSide = sideLength / 2;

                //square part
                for (int x = 0; x < SIZE - 1; x += sideLength)
                {
                    for (int y = 0; y < SIZE - 1; y += sideLength)
                    {
                        double avg = (_map[x, y] + _map[x + sideLength, y] + _map[x, y + sideLength] + _map[x + sideLength, y + sideLength]) / 4.0;
                        _map[x + halfSide, y + halfSide] = avg + r.NextDouble() * 2 * h - h;
                    }
                }

                //diamond part
                for (int x = 0; x < SIZE - 1; x += halfSide)
                {
                    for (int y = (x + halfSide) % sideLength; y < SIZE; y += sideLength)
                    {
                        double avg = (_map[(x - halfSide + SIZE) % SIZE, y] +
                            _map[(x + halfSide) % SIZE, y] + _map[x, (y + halfSide) % SIZE] + _map[x, (y - halfSide + SIZE) % SIZE]) / 4.0;

                        avg += (r.NextDouble() * 2 * h) - h;
                        _map[x, y] = avg;

                        if (x == 0) _map[SIZE - 1, y] = avg;
                        if (y == 0) _map[x, SIZE - 1] = avg;
                    }
                }
            }
        }

        private void fillModel()
        {
            plot.Plot.Title("Map");
            plot.Plot.SetOuterViewLimits(0, SIZE, 0, SIZE);
            var vm = plot.Plot.AddHeatmap(_map, ScottPlot.Drawing.Colormap.Topo, lockScales: false);
            plot.Plot.AddColorbar(vm);
            plot.Plot.Margins(0, 0);
            plot.Refresh();
        }
    }
}
