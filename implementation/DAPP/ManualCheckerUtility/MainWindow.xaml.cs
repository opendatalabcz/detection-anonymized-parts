using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using DAPPAnalyzer.Models;
using Infrastructure.Services;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace ManualCheckerUtility
{
    public partial class MainWindow : System.Windows.Window
    {
        private FileHandleService _fileHandleService = new();
        private List<BitmapImage> images = [];
        private int currentIndex = 0;

        private string path = "..\\..\\..\\..\\..\\smlouvy\\";
        private int totalImages = 0;
        private int countScan = 0;
        private int countDigital = 0;
        private int countAnonymized = 0;
        private int countNotAnonymized = 0;
        private int countBlackSquare = 0;
        private int countColoredSticker = 0;
        private int countNoise = 0;
        private int countOther = 0;
        private IEnumerator<string> nextFolder;
        public MainWindow()
        {
            InitializeComponent();
            LoadImages();
            DisplayCurrentImage();

            nextFolder = GetNextFolder();
            nextFolder.MoveNext();
        }

        private void LoadImages()
        {
            // if there are no files in the folder
            if (Directory.GetDirectories(path).Length == 0)
            {
                // the odkazySmluv.txt contains urls in form : "https://.../smlouva_123456.pdf"\n"https://.../smlouva_123457.pdf"...
                var urls = File.ReadAllLines("..\\..\\..\\..\\..\\odkazySmluv.txt");
                Random.Shared.Shuffle(urls);
                int max = 100;
                for (int i = 0; i < max; i++)
                {
                    var url = urls[i];
                    // download the pdf
                    try
                    {
                        var pdfBytes = new WebClient().DownloadData(url.Trim('\"'));
                    
                        // convert the pdf to images
                        var contractName = url.Split("/").Last().Split(".").First();
                        var pdf = Task.Run(() => DappPDF.Create(pdfBytes, contractName, url)).Result;
                        // save the images to the folder
                        var j = 0;
                        foreach (var page in pdf.Pages)
                        {
                            var imageFilePath = $"{path}/{contractName.ToLowerInvariant().Normalize()}/{j++}.jpg";
                            Directory.CreateDirectory(Path.GetDirectoryName(imageFilePath)!);
                            File.WriteAllBytes(imageFilePath, page.ToBytes(".jpg"));
                        }
                    }
                    catch (Exception) { };
                }
            }
        // load first folder of images
        LoadImagesFromFolder(Directory.GetDirectories(path).First());
        }

        private IEnumerator<string> GetNextFolder()
        {
            var folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                yield return folder;
            }
        }

        private void LoadImagesFromFolder(string v)
        {
            images.Clear();
            var files = Directory.GetFiles(v);
            foreach (var file in files)
            {
                var uri = new Uri(Path.GetFullPath(file));
                images.Add(new BitmapImage(uri));
            }
        }

        private void DisplayCurrentImage()
        {
            if (images.Count > 0)
            {
                imageDisplay.Source = images[currentIndex];
            }
        }
        private void PreviousImage_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayCurrentImage();
            }
        }

        private void NextImage_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < images.Count - 1)
            {
                currentIndex++;
                DisplayCurrentImage();
            }
        }

        private void BtnSaveContinue_Click(object sender, RoutedEventArgs e)
        {
            // Increment counters based on selection
            totalImages++;
            countScan += radioScan.IsChecked == true ? 1 : 0;
            countDigital += radioDigital.IsChecked == true ? 1 : 0;
            countAnonymized += radioAnonymized.IsChecked == true ? 1 : 0;
            countNotAnonymized += radioNotAnonymized.IsChecked == true ? 1 : 0;

            countBlackSquare += checkBoxBlackSquare.IsChecked == true ? 1 : 0;
            countColoredSticker += checkBoxColoredSticker.IsChecked == true ? 1 : 0;
            countNoise += checkBoxNoise.IsChecked == true ? 1 : 0;
            countOther += checkBoxOther.IsChecked == true ? 1 : 0;

            // Update UI
            textBlockTotalCounter.Text = $"Total: {totalImages}";
            textBlockScanDigitalCounter.Text = $"Scanned / Digital: {countScan} / {countDigital}";
            textBlockAnonymizedCounter.Text = $"Anonymized / Not Anonymized: {countAnonymized} / {countNotAnonymized}";
            textBlockAnonymizedTypeBlackSquareCounter.Text = $"BlackSquare: {countBlackSquare}";
            textBlockAnonymizedTypeColoredStickerCounter.Text = $"ColoredSticker: {countColoredSticker}";
            textBlockAnonymizedTypeNoiseCounter.Text = $"Noise: {countNoise}";
            textBlockAnonymizedTypeOtherCounter.Text = $"Other: {countOther}";


            checkBoxBlackSquare.IsChecked = false;
            checkBoxColoredSticker.IsChecked = false;
            checkBoxNoise.IsChecked = false;
            checkBoxOther.IsChecked = false;

            if (nextFolder.MoveNext())
            {
                LoadImagesFromFolder(nextFolder.Current);
                currentIndex = 0;
                DisplayCurrentImage();
            }
            else
            {
                MessageBox.Show("All images have been checked.");
            }


        }
    }
}
