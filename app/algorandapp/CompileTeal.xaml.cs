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
            // UI code to be completed - testing only
            InitializeComponent();
          //  CompileTealExample();
        }
        //void CompileTealExample()
        //{
        //    //public string ALGOD_API_TOKEN_BETANET = "050e81d219d12a0888dafddaeafb5ff8d181bf1256d1c749345995678b16902f";
        //    //public string ALGOD_API_ADDR_BETANET = "http://betanet-hackathon.algodev.network:8180";
        //    //public string ALGOD_API_TOKEN_TESTNET = "ef920e2e7e002953f4b29a8af720efe8e4ecc75ff102b165e0472834b25832c1";
        //    //public string ALGOD_API_ADDR_TESTNET = "http://hackathon.algodev.network:9100";

        //    //public const string ALGOD_API_TOKEN_BETANET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";
        //    //public const string ALGOD_API_ADDR_BETANET = "https://betanet-algorand.api.purestake.io/ps2";
        //    //public const string ALGOD_API_ADDR_TESTNET = "https://testnet-algorand.api.purestake.io/ps2";
        //    //public const string ALGOD_API_TOKEN_TESTNET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";

        //    // string ALGOD_API_ADDR = "https://testnet-algorand.api.purestake.io/ps2";
        //    // string ALGOD_API_TOKEN = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";

        //    string ALGOD_API_ADDR = "http://hackathon.algodev.network:9100";
        //    string ALGOD_API_TOKEN = "ef920e2e7e002953f4b29a8af720efe8e4ecc75ff102b165e0472834b25832c1";

        //    AlgodApi algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);
 
        //    byte[] data = ExtractResource("algorandapp.Contract.sample.teal");
        //    var response = algodApiInstance.TealCompile(data);
        //    Debug.WriteLine("response: " + response);
        //    Debug.WriteLine("Hash: " + response.Hash);
        //    Debug.WriteLine("Result: " + response.Result);
        
     

        //    //result
        //    //Hash: 6Z3C3LDVWGMX23BMSYMANACQOSINPFIRF77H7N3AWJZYV6OH6GWTJKVMXY
        //    //Result: ASABASI=

        //}
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

        async void CompileTeal_Clicked(System.Object sender, System.EventArgs e)
        {
            client = await helper.CreateApiInstance();
 //           network = await helper.GetNetwork();



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