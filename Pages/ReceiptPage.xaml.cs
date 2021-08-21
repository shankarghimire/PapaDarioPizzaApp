using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PapaDarioPizzaApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReceiptPage : Page
    {
        public ReceiptPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.StorageFolder storageFolder =
    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =  await storageFolder.GetFileAsync("sample.txt");
            if(sampleFile != null)
            {
                Windows.Storage.Streams.IRandomAccessStream randomAccess = await sampleFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                //string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                RicchEditBox_Receipt.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randomAccess);
            }
            

        }
    }
}
