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

namespace algorandapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DryrunDebugging : ContentPage
    {
        public DryrunDebugging()
        {
            // UI code to be completed - testing only
            InitializeComponent();
            DryrunDebuggingExample();
        }

        void DryrunDebuggingExample()
        {

            string ALGOD_API_ADDR = "http://hackathon.algodev.network:9100";
            string ALGOD_API_TOKEN = "ef920e2e7e002953f4b29a8af720efe8e4ecc75ff102b165e0472834b25832c1";

            string SRC_ACCOUNT = "buzz genre work meat fame favorite rookie stay tennis demand panic busy hedgehog snow morning acquire ball grain grape member blur armor foil ability seminar";
            Account acct1 = new Account(SRC_ACCOUNT);
            var acct2Address = "QUDVUXBX4Q3Y2H5K2AG3QWEOMY374WO62YNJFFGUTMOJ7FB74CMBKY6LPQ";

          //  byte[] source = File.ReadAllBytes("V2\\contract\\sample.teal");
            byte[] source = ExtractResource("algorandapp.Contract.sample.teal");
            byte[] program = Convert.FromBase64String("ASABASI=");

            LogicsigSignature lsig = new LogicsigSignature(program, null);

            // sign the logic signaure with an account sk
            acct1.SignLogicsig(lsig);

            var algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);
            Algorand.V2.Model.TransactionParametersResponse transParams;
            try
            {
                transParams = algodApiInstance.TransactionParams();
            }
            catch (ApiException e)
            {
                throw new Exception("Could not get params", e);
            }

            Transaction tx = Utils.GetPaymentTransaction(acct1.Address, new Address(acct2Address), 1000000,
                "tx using in dryrun", transParams);

            try
            {
                //bypass verify for non-lsig
                SignedTransaction signedTx = Account.SignLogicsigTransaction(lsig, tx);
                //一切准备就绪，本可以直接发送到网络，也可使得Dryrun的方法来进行调试
                //var id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                //Console.WriteLine("Successfully sent tx logic sig tx id: " + id);
                // dryrun source
                var dryrunResponse = Utils.GetDryrunResponse(algodApiInstance, signedTx, source);
                Debug.WriteLine("Dryrun compiled repsonse : " + dryrunResponse.ToJson()); // pretty print

                // dryrun logic sig transaction
                //var dryrunResponse2 = Utils.GetDryrunResponse(algodApiInstance, signedTx);
                //Debug.WriteLine("Dryrun source repsonse : " + dryrunResponse2.ToJson()); // pretty print
            }
            catch (ApiException e)
            {
                // This is generally expected, but should give us an informative error message.
                Debug.WriteLine("Exception when calling algod#rawTransaction: " + e.Message);
            }


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
    }
}