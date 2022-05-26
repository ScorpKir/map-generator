using MapGenerator.Views;
using System;
using System.Windows;

namespace MapGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void generatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(sizeBox.Text, out var size) &&
            double.TryParse(seedBox.Text, out var seed) &&
            double.TryParse(roughnessBox.Text, out var roughness))
            {
                MapWindow newWindow = new MapWindow(size, seed, roughness);
                newWindow.Show();
            }
            else
            {
                MessageBox.Show("Не все данные были введены правильно!");
            }
        }
    }
}
