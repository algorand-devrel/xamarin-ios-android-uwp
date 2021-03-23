using Algorand.V2;
using Algorand.V2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace algorandapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Indexer : ContentPage
    {
        private HtmlWebViewSource htmlSource = new HtmlWebViewSource();
        static string INDEXER_API_ADDR = "https://testnet-algorand.api.purestake.io/idx2";
        static string INDEXER_API_TOKEN = "GeHdp7CCGt7ApLuPNppXN4LtrW07Mm1kaFNJ5Ovr";
        IndexerApi indexer = new IndexerApi(INDEXER_API_ADDR, INDEXER_API_TOKEN);
        string address = "KV2XGKMXGYJ6PWYQA5374BYIQBL3ONRMSIARPCFCJEAMAHQEVYPB7PL3KU";
        ApplicationsResponse appsInfo;
        AssetsResponse assetsInfo;

        public Indexer()
        {
            InitializeComponent();
        }

        void DisplayInfo(string text)
        {         
            htmlSource.Html = @"<html><body><h3>" +
                "<h3>" + text + "</h3></body></html>";

            myWebView.Source = htmlSource ;
        }

   

        void Health_Clicked(System.Object sender, System.EventArgs e)
        {
            var health = indexer.MakeHealthCheck();
            Debug.WriteLine("Make Health Check: " + health.ToJson());
            DisplayInfo("Make Health Check:" + health.ToJson());
        }

        void LookupAccount_Clicked(System.Object sender, System.EventArgs e)
        {
            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var acctInfo = indexer.LookupAccountByID(address);
            Debug.WriteLine("Look up account by id: " + acctInfo.ToJson());
            DisplayInfo("Look up account by id: " + acctInfo.ToJson());
        }

        void LookupAccountTransactions_Clicked(System.Object sender, System.EventArgs e)
        {
            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var transInfos = indexer.LookupAccountTransactions(address, 10);
            Debug.WriteLine("Look up account transactions(limit 10): " + transInfos.ToJson());
            DisplayInfo("Look up account transactions(limit 10):  " + transInfos.ToJson());
        }

        void SearchForApplications_Clicked(System.Object sender, System.EventArgs e)
        {        
            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            appsInfo = indexer.SearchForApplications(limit: 10);
            Debug.WriteLine("Search for application(limit 10): " + appsInfo.ToJson());
            DisplayInfo("Search for application(limit 10): " + appsInfo.ToJson());
            LookupApplicationByID.IsEnabled = true;
        }

        void LookupApplicationByID_Clicked(System.Object sender, System.EventArgs e)
        {
            var appIndex = appsInfo.Applications[0].Id;
            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var appInfo = indexer.LookupApplicationByID(appIndex);
            Debug.WriteLine("Look up application by id: " + appInfo.ToJson());
            DisplayInfo("Look up application by id: " + appInfo.ToJson());
        }

        void SearchForAssets_Clicked(System.Object sender, System.EventArgs e)
        {
            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            assetsInfo = indexer.SearchForAssets(limit: 10, unit: "LAT");
            Debug.WriteLine("Search for assets" + assetsInfo.ToJson());
            LookupAssetByID.IsEnabled = true;
            DisplayInfo("Search for assets" + assetsInfo.ToJson());

        }

        void LookupAssetByID_Clicked(System.Object sender, System.EventArgs e)
        {
            var assetIndex = assetsInfo.Assets[0].Index;
            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var assetInfo = indexer.LookupAssetByID(assetIndex);
            Debug.WriteLine("Look up asset by id:" + assetInfo.ToJson());
            DisplayInfo("Look up asset by id:" + assetsInfo.ToJson());
        }
    }
}