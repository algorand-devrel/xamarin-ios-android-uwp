using Algorand.V2;
using Algorand.V2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace algorandapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompileTeal : ContentPage
    {
        
        static helper helper = new helper();
        AlgodApi client;
        public CompileTeal()
        {
            InitializeComponent();
            Appearing += CompileTeal_Appearing;
        }

        private async void CompileTeal_Appearing(object sender, EventArgs e)
        {
            client = await helper.CreateApiInstance();
        }

        public static byte[] ExtractResource(String filename)
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            using (Stream resFilestream = a.GetManifestResourceStream(filename))
            {
                if (resFilestream == null) return null;
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }

        void CompileTeal_Clicked(System.Object sender, System.EventArgs e)
        {
  
          //network = await helper.GetNetwork();
            //string ALGOD_API_ADDR = "http://hackathon.algodev.network:9100";
            //string ALGOD_API_TOKEN = "ef920e2e7e002953f4b29a8af720efe8e4ecc75ff102b165e0472834b25832c1";
            // AlgodApi algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);

            byte[] data = ExtractResource("algorandapp.Contract.sample.teal");
            var response = client.TealCompile(data);
            Debug.WriteLine("response: " + response);
            Debug.WriteLine("Hash: " + response.Hash);
            Debug.WriteLine("Result: " + response.Result);
            DisplayInfo( "Hash: " + response.Hash + "\nResult: " + response.Result);

        }
        void DisplayInfo(string text)
        {
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h2>" + text + "</h2></body></html>";
            myWebView.Source = htmlSource;

        }
    }
}