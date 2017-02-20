using Mapbox.Vector.Tile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App16
{
    // --------------------------------------------------------------------------------------------
    // Code copied from https://github.com/bertt/mapbox-vector-tile-cs/files/782466/TestCode.txt
    // --------------------------------------------------------------------------------------------
    public class Fetcher
    {
        private Stream _vectorTileStream;
        public Fetcher(Stream vectorTileStream)
        {
            _vectorTileStream = vectorTileStream;
        }

        public void ParseTile()
        {
            try
            {
                var l = VectorTileParser.Parse(_vectorTileStream);
            }
            catch (Exception ex)
            {
                var explanation = ex.ToString();
            }
        }
    }

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var path = ApplicationData.Current.LocalFolder.Path;
            const string fileName = "cadastral.pbf";
            loadMVT(fileName);
        }

        private async void loadMVT(string name)
        {
            // StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(name);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + name));
            IBuffer buffer = await FileIO.ReadBufferAsync(file);
            byte[] bytes = buffer.ToArray();
            List<Fetcher> fetchers = new List<Fetcher>();
            for (int i = 0; i < 5; ++i)
            {
                fetchers.Add(new Fetcher(new MemoryStream(bytes)));
            }

            Task.Factory.StartNew(() => fetchers[0].ParseTile());
            Task.Factory.StartNew(() => fetchers[1].ParseTile());
            Task.Factory.StartNew(() => fetchers[2].ParseTile());
            Task.Factory.StartNew(() => fetchers[3].ParseTile());
            Task.Factory.StartNew(() => fetchers[4].ParseTile());
        }

    }
}
