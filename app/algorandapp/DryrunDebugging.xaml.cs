using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Algorand;
using Algorand.Client;
using Algorand.V2;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
//using Algorand.V2.Model;
using Account = Algorand.Account;

namespace algorandapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DryrunDebugging : ContentPage
    {
        static helper helper = new helper();
        AlgodApi client;
        Account account1;
        Account account2;   
        public DryrunDebugging()
        {
            InitializeComponent();
            Appearing += DryrunDebugging_Appearing;         
        }

        private async void DryrunDebugging_Appearing(object sender, EventArgs e)
        {
            client = await helper.CreateApiInstance();
            // restore accounts
            var accounts = await helper.RestoreAccounts();
            account1 = accounts[0];
            account2 = accounts[1];

            Console.WriteLine("Account 1: " + account1.Address);
            Console.WriteLine("Account 2: " + account2.Address);
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

        void DryrunDebugging_Clicked(System.Object sender, System.EventArgs e)
        {
            //Account acct1 = new Account(SRC_ACCOUNT);
            //var acct2.Address = "QUDVUXBX4Q3Y2H5K2AG3QWEOMY374WO62YNJFFGUTMOJ7FB74CMBKY6LPQ";

            //  byte[] source = File.ReadAllBytes("V2\\contract\\sample.teal");
            byte[] source = ExtractResource("algorandapp.Contract.sample.teal");
            byte[] program = Convert.FromBase64String("ASABASI=");

            LogicsigSignature lsig = new LogicsigSignature(program, null);

            // sign the logic signaure with an account sk
            account1.SignLogicsig(lsig);

         //   var algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);
            Algorand.V2.Model.TransactionParametersResponse transParams;
            try
            {
                transParams = client.TransactionParams();
            }
            catch (ApiException err)
            {
                throw new Exception("Could not get params", err);
            }

            Transaction tx = Utils.GetPaymentTransaction(account1.Address, account2.Address, 1000000,
                "tx using in dryrun", transParams);

            try
            {
                //bypass verify for non-lsig
                SignedTransaction signedTx = Account.SignLogicsigTransaction(lsig, tx);


                // dryrun source
                var dryrunResponse = Utils.GetDryrunResponse(client, signedTx, source);
                Debug.WriteLine("Dryrun compiled repsonse : " + dryrunResponse.ToJson()); // pretty print
                DisplayInfo("Dryrun compiled repsonse : " + dryrunResponse.ToJson());
                // dryrun logic sig transaction - omit source
                //var dryrunResponse2 = Utils.GetDryrunResponse(algodApiInstance, signedTx);
                //Debug.WriteLine("Dryrun source repsonse : " + dryrunResponse2.ToJson()); // pretty print

                //var id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                //Console.WriteLine("Successfully sent tx logic sig tx id: " + id);
            }
            catch (ApiException err)
            {
                // This is generally expected, but should give us an informative error message.
                Debug.WriteLine("Exception when calling algod#rawTransaction: " + err.Message);
            }

        }
        void DisplayInfo(string text)
        {
         
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h2>" +
                 text + "</h2></body></html>";
            myWebView.Source = htmlSource;
        }
    }
}