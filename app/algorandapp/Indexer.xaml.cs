using Algorand.V2;
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
        public Indexer()
        {
            // UI code to be completed - testing only
            InitializeComponent();
            IndexerExamples();

        }

        void IndexerExamples()
        {
            string INDEXER_API_ADDR = "https://testnet-algorand.api.purestake.io/idx2";
            string INDEXER_API_TOKEN = "GeHdp7CCGt7ApLuPNppXN4LtrW07Mm1kaFNJ5Ovr";

            IndexerApi indexer = new IndexerApi(INDEXER_API_ADDR, INDEXER_API_TOKEN);
            //AlgodApi algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);
            var health = indexer.MakeHealthCheck();
            Debug.WriteLine("Make Health Check: " + health.ToJson());

            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var address = "KV2XGKMXGYJ6PWYQA5374BYIQBL3ONRMSIARPCFCJEAMAHQEVYPB7PL3KU";
            var acctInfo = indexer.LookupAccountByID(address);
            Debug.WriteLine("Look up account by id: " + acctInfo.ToJson());

            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var transInfos = indexer.LookupAccountTransactions(address, 10);
            Debug.WriteLine("Look up account transactions(limit 10): " + transInfos.ToJson());

            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var appsInfo = indexer.SearchForApplications(limit: 10);
            Debug.WriteLine("Search for application(limit 10): " + appsInfo.ToJson());

            var appIndex = appsInfo.Applications[0].Id;
            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var appInfo = indexer.LookupApplicationByID(appIndex);
            Debug.WriteLine("Look up application by id: " + appInfo.ToJson());

            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var assetsInfo = indexer.SearchForAssets(limit: 10, unit: "LAT");
            Debug.WriteLine("Search for assets" + assetsInfo.ToJson());

            var assetIndex = assetsInfo.Assets[0].Index;
            System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
            var assetInfo = indexer.LookupAssetByID(assetIndex);
            Debug.WriteLine("Look up asset by id:" + assetInfo.ToJson());
        }
    }
}