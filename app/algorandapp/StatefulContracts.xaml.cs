using Algorand;
using Algorand.Client;
using Algorand.V2;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Algorand.V2.Model;
using Account = Algorand.Account;
using System.Text;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace algorandapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatefulContracts : ContentPage
    {

        CompileResponse approvalProgram;
        CompileResponse clearProgram;
        CompileResponse approvalProgramRefactored;
        static helper helper = new helper();

        Account creator;
        Account user;
        AlgodApi client;
        long? appid = 0;
        Account account1;
        Account account2;
        string network = "";
        


        public StatefulContracts()
        {
            InitializeComponent();
            Appearing += StatefulContracts_Appearing;
      
        }

        private async void StatefulContracts_Appearing(object sender, EventArgs e)
        {

            client = await helper.CreateApiInstance();
            network = await helper.GetNetwork();

            // restore accounts
            var accounts = await helper.RestoreAccounts();
            creator = accounts[0];
            user = accounts[1];

            Console.WriteLine("creator account: " + creator.Address);
            Console.WriteLine("user account: " + user.Address);
            var nodetype = await SecureStorage.GetAsync(helper.StorageNodeType);
            //   NetworkLabel.Text = "Network: " + network + " " + nodetype;
            compileTealPrograms();
            OptInBtn.IsEnabled = false;
            CallAppBtn.IsEnabled = false;
            ReadLocalStateBtn.IsEnabled = false;
            ReadGlobalStateBtn.IsEnabled = false;
            UpdateAppBtn.IsEnabled = false;
            CallUpdatedAppBtn.IsEnabled = false;
            ReadLocalStateAgainBtn.IsEnabled = false;
            CloseOutAppBtn.IsEnabled = false;
            DeleteAppBtn.IsEnabled = false;
            ClearAppCtn.IsEnabled = false;
        }

        //void StatefulExamples()
        //{
        //    //string ALGOD_API_ADDR = "https://testnet-algorand.api.purestake.io/idx2";
        //    //string ALGOD_API_TOKEN = "GeHdp7CCGt7ApLuPNppXN4LtrW07Mm1kaFNJ5Ovr";
        //    //string ALGOD_API_ADDR = "http://localhost:4001";
        //    //string ALGOD_API_TOKEN = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";


        //    //string ALGOD_API_ADDR = "http://hackathon.algodev.network:9100";
        //    //string ALGOD_API_TOKEN = "ef920e2e7e002953f4b29a8af720efe8e4ecc75ff102b165e0472834b25832c1";

        //    //var creatorMnemonic = "benefit once mutual legal marble hurdle dress toe fuel country prepare canvas barrel divide major square name captain calm flock crumble receive economy abandon power";
        //    //var userMnemonic = "pledge become mouse fantasy matrix bunker ask tissue prepare vocal unit patient cliff index train network intact company across stage faculty master mom abstract above";
        //    //// create two account to create and user the stateful contract
        //    //creator = new Account(creatorMnemonic);
        //    //user = new Account(userMnemonic);

        //    //client = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);


        //    //var transParams = client.TransactionParams();
        //    //var tx = Utils.GetPaymentTransaction(admin.Address, creator.Address, 
        //    //    Utils.AlgosToMicroalgos(1), null, transParams);
        //    //var resp = Utils.SubmitTransaction(client, admin.SignTransaction(tx));
        //    //var waitResp = Utils.WaitTransactionToComplete(client, resp.TxId);
        //    //Console.WriteLine(string.Format("send 1 algo to account {0} at round {1}",
        //    //    creator.Address.ToString(), waitResp.ConfirmedRound));

        //    //tx = Utils.GetPaymentTransaction(admin.Address, user.Address,
        //    //    Utils.AlgosToMicroalgos(1), null, transParams);
        //    //resp = Utils.SubmitTransaction(client, admin.SignTransaction(tx));
        //    //waitResp = Utils.WaitTransactionToComplete(client, resp.TxId);
        //    //Console.WriteLine(string.Format("send 1 algo to account {0} at round {1}",
        //    //    user.Address.ToString(), waitResp.ConfirmedRound));

        //    // declare application state storage (immutable)
        //    ////ulong localInts = 1;
        //    ////ulong localBytes = 1;
        //    ////ulong globalInts = 1;
        //    ////ulong globalBytes = 0;

        //    // user declared approval program (initial)
        //    //  string approvalProgramSourceInitial = File.ReadAllText("V2/contract/stateful_approval_init.teal");
        //    //compileTealPrograms();

        //    try
        //    {
        //        // create new application
        //        //// appid = CreateApp(client, creator, new TEALProgram(approvalProgram.Result),
        //        ////    new TEALProgram(clearProgram.Result), globalInts, globalBytes, localInts, localBytes);

        //        // opt-in to application
        //        ////OptIn(client, user, appid);
        //        // call application without arguments
        //        ////CallApp(client, user, appid, null);
        //        // read local state of application from user account
        //        //// ReadLocalState(client, user, appid);

        //        // read global state of application
        //        ////ReadGlobalState(client, creator, appid);

        //        // update application
        //        ////UpdateApp(client, creator, appid,
        //        ////    new TEALProgram(approvalProgramRefactored.Result),
        //        ////   new TEALProgram(clearProgram.Result));
        //        //// call application with arguments
        //        ////var date = DateTime.Now;
        //        ////Console.WriteLine(date.ToString("yyyy-MM-dd 'at' HH:mm:ss"));
        //        ////List<byte[]> appArgs = new List<byte[]>
        //        ////{
        //        ////    Encoding.UTF8.GetBytes(date.ToString("yyyy-MM-dd 'at' HH:mm:ss"))
        //        ////};
        //        ////CallApp(client, user, appid, appArgs);

        //        // read local state of application from user account
        //        ////ReadLocalState(client, user, appid);

        //        // close-out from application
        //        ////CloseOutApp(client, user, (ulong)appid);

        //        // opt-in again to application
        //        //  OptIn(client, user, appid);

        //        // call application with arguments
        //        //  CallApp(client, user, appid, appArgs);

        //        // read local state of application from user account
        //        //   ReadLocalState(client, user, appid);

        //        // delete application
        //        ////DeleteApp(client, creator, appid);

        //        // clear application from user account
        //        ////ClearApp(client, user, appid);

        //        //Console.WriteLine("You have successefully arrived the end of this test, please press and key to exist.");
        //    }
        //    catch (ApiException e)
        //    {
        //        // This is generally expected, but should give us an informative error message.
        //        Console.WriteLine("Exception when calling algod#sendTransaction: " + e.Message);
        //    }

        //}

        private void compileTealPrograms()
        {
            // initial approval program without date/time arg
            string approvalProgramSourceInitial = ExtractResourceText("algorandapp.Assets.stateful_approval_init.teal");

            // user declared approval program (refactored) with date/time arg
 
            string approvalProgramSourceRefactored = ExtractResourceText("algorandapp.Assets.stateful_approval_refact.teal");

            // declare clear state program source

            string clearProgramSource = ExtractResourceText("algorandapp.Assets.stateful_clear.teal");
            // note: the approvalProgramSourceInitial and approvalProgramSourceRefactored

            // need to have the user address changed thrughout the Teal

            Console.WriteLine("NOTE: Edit the initial and refactored TEAL programs to use this user account: " + user.Address);

            approvalProgram =
                 client.TealCompile(Encoding.UTF8.GetBytes(approvalProgramSourceInitial));
            clearProgram =
                 client.TealCompile(Encoding.UTF8.GetBytes(clearProgramSource));
            approvalProgramRefactored =
                client.TealCompile(Encoding.UTF8.GetBytes(approvalProgramSourceRefactored));
        }

        public string ExtractResourceText(String filename)
         {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(filename);

            using (var reader = new System.IO.StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                return text;
            }
         }

         long? CreateApp(AlgodApi client, Account creator, TEALProgram approvalProgram,
                TEALProgram clearProgram, ulong? globalInts, ulong? globalBytes, ulong? localInts, ulong? localBytes)
            {
                try
                {
                    var transParams = client.TransactionParams();
                    var tx = Utils.GetApplicationCreateTransaction(creator.Address, approvalProgram, clearProgram,
                     new StateSchema(globalInts, globalBytes), new StateSchema(localInts, localBytes), transParams);
                    var signedTx = creator.SignTransaction(tx);
                    Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);
                   // DisplayInfo("Signed transaction with txid: " + signedTx.transactionID);
                    var id = Utils.SubmitTransaction(client, signedTx);
                    Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                    var resp = Utils.WaitTransactionToComplete(client, id.TxId);
                    Console.WriteLine("Application ID is: " + resp.ApplicationIndex.ToString());
                    DisplayInfo("Application ID is: " + resp.ApplicationIndex.ToString());
                    return resp.ApplicationIndex;
                }
                catch (ApiException e)
                {
                    Console.WriteLine("Exception when calling create application: " + e.Message);
                    return null;
                }

            }

       void DisplayInfo(string text)
            {
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + 
                    "<h3>" + text + "</h3></body></html>";
                myWebView.Source = htmlSource;

            }

        void OptIn(AlgodApi client, Account sender, long? applicationId)
        {
            try
            {
                var transParams = client.TransactionParams();
                var tx = Utils.GetApplicationOptinTransaction(sender.Address, (ulong?)applicationId, transParams);
                var signedTx = sender.SignTransaction(tx);
                Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);

                var id = Utils.SubmitTransaction(client, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                var resp = Utils.WaitTransactionToComplete(client, id.TxId);
                Console.WriteLine(string.Format("Address {0} optin to Application({1})",
                    sender.Address.ToString(), (resp.Txn as JObject)["txn"]["apid"]));
                DisplayInfo(string.Format("Address {0} optin to Application({1})",
                    sender.Address.ToString(), (resp.Txn as JObject)["txn"]["apid"]));

            }
            catch (ApiException e)
            {
                Console.WriteLine("Exception when calling create application: " + e.Message);
            }
        }
      void CallApp(AlgodApi client, Account sender, long? applicationId, List<byte[]> args)
        {
            try
            {
                var transParams = client.TransactionParams();
                var tx = Utils.GetApplicationCallTransaction(sender.Address, (ulong?)applicationId, transParams, args);
                var signedTx = sender.SignTransaction(tx);
                Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);

                var id = Utils.SubmitTransaction(client, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                var resp = Utils.WaitTransactionToComplete(client, id.TxId);
                Console.WriteLine("Confirmed at round: " + resp.ConfirmedRound);
                Console.WriteLine(string.Format("Call Application({0}) success.",
                    (resp.Txn as JObject)["txn"]["apid"]));
                DisplayInfo(string.Format("Call Application({0}) success.",
                    (resp.Txn as JObject)["txn"]["apid"]));

                //System.out.println("Called app-id: " + pTrx.txn.tx.applicationId);
                if (resp.GlobalStateDelta != null)
                {
                    var outStr = "    Global state: ";
                    foreach (var v in resp.GlobalStateDelta)
                    {
                        outStr += v.ToString();
                    }
                    Console.WriteLine(outStr);
                }
                if (resp.LocalStateDelta != null)
                {
                    var outStr = "    Local state: ";
                    foreach (var v in resp.LocalStateDelta)
                    {
                        outStr += v.ToString();
                    }
                    Console.WriteLine(outStr);
                }
            }
            catch (ApiException e)
            {
                Console.WriteLine("Exception when calling create application: " + e.Message);
            }
        }
        public void ReadLocalState(AlgodApi client, Account account, long? appId)
        {
            var acctResponse = client.AccountInformation(account.Address.ToString());
            var applicationLocalState = acctResponse.AppsLocalState;
            for (int i = 0; i < applicationLocalState.Count; i++)
            {
                if (applicationLocalState[i].Id == appId)
                {
                    var outStr = "User's application local state: ";
                    foreach (var v in applicationLocalState[i].KeyValue)
                    {
                        outStr += v.ToString();
                    }
                    Console.WriteLine(outStr);
                    DisplayInfo(outStr);
                }
            }
        }

        public void ReadGlobalState(AlgodApi client, Account account, long? appId)
        {
            var acctResponse = client.AccountInformation(account.Address.ToString());
            var createdApplications = acctResponse.CreatedApps;
            for (int i = 0; i < createdApplications.Count; i++)
            {
                if (createdApplications[i].Id == appId)
                {
                    var outStr = "Application global state: ";
                    foreach (var v in createdApplications[i].Params.GlobalState)
                    {
                        outStr += v.ToString();
                    }
                    Console.WriteLine(outStr);
                    DisplayInfo(outStr);
                }
            }
        }
        private void UpdateApp(AlgodApi client, Account creator, long? appid, TEALProgram approvalProgram, TEALProgram clearProgram)
        {
            try
            {
                var transParams = client.TransactionParams();
                var tx = Utils.GetApplicationUpdateTransaction(creator.Address, (ulong?)appid, approvalProgram, clearProgram, transParams);
                var signedTx = creator.SignTransaction(tx);
                Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);

                var id = Utils.SubmitTransaction(client, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                var resp = Utils.WaitTransactionToComplete(client, id.TxId);
                Console.WriteLine("Confirmed Round is: " + resp.ConfirmedRound);
                Console.WriteLine("Application ID is: " + appid);

                DisplayInfo("Successfully Updated with  tx id: " + id.TxId);
            }
            catch (ApiException e)
            {
                Console.WriteLine("Exception when calling create application: " + e.Message);
            }
        }

        void CloseOutApp(AlgodApi client, Account sender, ulong appId)
        {
            try
            {
                var transParams = client.TransactionParams();
                var tx = Utils.GetApplicationCloseTransaction(sender.Address, (ulong?)appId, transParams);
                var signedTx = sender.SignTransaction(tx);
                Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);

                var id = Utils.SubmitTransaction(client, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                var resp = Utils.WaitTransactionToComplete(client, id.TxId);
                Console.WriteLine("Confirmed Round is: " + resp.ConfirmedRound);
                Console.WriteLine("Application ID is: " + appId);
                DisplayInfo("Successfully Closed out with  tx id: " + id.TxId);
            }
            catch (ApiException e)
            {
                Console.WriteLine("Exception when calling create application: " + e.Message);
            }
        }
 
        void DeleteApp(AlgodApi client, Account sender, long? applicationId)
        {
            try
            {
                var transParams = client.TransactionParams();
                var tx = Utils.GetApplicationDeleteTransaction(sender.Address, (ulong?)applicationId, transParams);
                var signedTx = sender.SignTransaction(tx);
                Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);

                var id = Utils.SubmitTransaction(client, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                var resp = Utils.WaitTransactionToComplete(client, id.TxId);
                Console.WriteLine("Success deleted the application " + (resp.Txn as JObject)["txn"]["apid"]);
                DisplayInfo("Successfully Deleted out with  tx id: " + id.TxId);
            }
            catch (ApiException e)
            {
                Console.WriteLine("Exception when calling create application: " + e.Message);
            }
        }

        void ClearApp(AlgodApi client, Account sender, long? applicationId)
        {
            try
            {
                var transParams = client.TransactionParams();
                var tx = Utils.GetApplicationClearTransaction(sender.Address, (ulong?)applicationId, transParams);
                var signedTx = sender.SignTransaction(tx);
                Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);

                var id = Utils.SubmitTransaction(client, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                var resp = Utils.WaitTransactionToComplete(client, id.TxId);
                Console.WriteLine("Success cleared the application " + (resp.Txn as JObject)["txn"]["apid"]);
                DisplayInfo("Successfully Cleared out with  tx id: " + id.TxId);
            }
            catch (ApiException e)
            {
                Console.WriteLine("Exception when calling create application: " + e.Message);
            }
        }

        byte[] ExtractResource(String filename)
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

        void CreateApp_Clicked(System.Object sender, System.EventArgs e)
        {
            // declare application state storage (immutable)
            ulong localInts = 1;
            ulong localBytes = 1;
            ulong globalInts = 1;
            ulong globalBytes = 0;
            appid = CreateApp(client, creator, new TEALProgram(approvalProgram.Result),
   new TEALProgram(clearProgram.Result), globalInts, globalBytes, localInts, localBytes);
            if (appid > 0)
            {
                OptInBtn.IsEnabled = true;
                CallAppBtn.IsEnabled = false;
                ReadLocalStateBtn.IsEnabled = false;
                ReadGlobalStateBtn.IsEnabled = false;
                UpdateAppBtn.IsEnabled = false;
                CallUpdatedAppBtn.IsEnabled = false;
                ReadLocalStateAgainBtn.IsEnabled = false;
                CloseOutAppBtn.IsEnabled = false;
                DeleteAppBtn.IsEnabled = false;
                ClearAppCtn.IsEnabled = false;
            }

        }

        void OptIn_Clicked(System.Object sender, System.EventArgs e)
        {
            // opt-in to application
            OptIn(client, user, appid);
            OptInBtn.IsEnabled = true;
            CallAppBtn.IsEnabled = true;
            ReadLocalStateBtn.IsEnabled = false;
            ReadGlobalStateBtn.IsEnabled = false;
            UpdateAppBtn.IsEnabled = false;
            CallUpdatedAppBtn.IsEnabled = false;
            ReadLocalStateAgainBtn.IsEnabled = false;
            CloseOutAppBtn.IsEnabled = false;
            DeleteAppBtn.IsEnabled = false;
            ClearAppCtn.IsEnabled = false;

        }

        void CallApp_Clicked(System.Object sender, System.EventArgs e)
        {
            // call application without arguments
            CallApp(client, user, appid, null);
            OptInBtn.IsEnabled = true;
            CallAppBtn.IsEnabled = true;
            ReadLocalStateBtn.IsEnabled = true;
            ReadGlobalStateBtn.IsEnabled = true;
            UpdateAppBtn.IsEnabled = true;
            CallUpdatedAppBtn.IsEnabled = false;
            ReadLocalStateAgainBtn.IsEnabled = false;
            CloseOutAppBtn.IsEnabled = false;
            DeleteAppBtn.IsEnabled = false;
            ClearAppCtn.IsEnabled = false;

        }

        void ReadLocalState_Clicked(System.Object sender, System.EventArgs e)
        {
            // read local state of application from user account
            ReadLocalState(client, user, appid);

        }

        void ReadGlobalState_Clicked(System.Object sender, System.EventArgs e)
        {

            // read global state of application
            ReadGlobalState(client, creator, appid);

        }

        void UpdateApp_Clicked(System.Object sender, System.EventArgs e)
        {
            // update application
            UpdateApp(client, creator, appid,
                new TEALProgram(approvalProgramRefactored.Result),
                new TEALProgram(clearProgram.Result));
            OptInBtn.IsEnabled = true;
            CallAppBtn.IsEnabled = true;
            ReadLocalStateBtn.IsEnabled = true;
            ReadGlobalStateBtn.IsEnabled = true;
            UpdateAppBtn.IsEnabled = true;
            CallUpdatedAppBtn.IsEnabled = true;
            ReadLocalStateAgainBtn.IsEnabled = false;
            CloseOutAppBtn.IsEnabled = false;
            DeleteAppBtn.IsEnabled = false;
            ClearAppCtn.IsEnabled = false;
        }

        void CallUpdatedApp_Clicked(System.Object sender, System.EventArgs e)
        {
            var date = DateTime.Now;
            Console.WriteLine(date.ToString("yyyy-MM-dd 'at' HH:mm:ss"));
            List<byte[]> appArgs = new List<byte[]>
                {
                    Encoding.UTF8.GetBytes(date.ToString("yyyy-MM-dd 'at' HH:mm:ss"))
                };
            CallApp(client, user, appid, appArgs);
            OptInBtn.IsEnabled = true;
            CallAppBtn.IsEnabled = true;
            ReadLocalStateBtn.IsEnabled = true;
            ReadGlobalStateBtn.IsEnabled = true;
            UpdateAppBtn.IsEnabled = true;
            CallUpdatedAppBtn.IsEnabled = true;
            ReadLocalStateAgainBtn.IsEnabled = true;
            CloseOutAppBtn.IsEnabled = true;
            DeleteAppBtn.IsEnabled = false;
            ClearAppCtn.IsEnabled = false;
        }

        void CloseOutApp_Clicked(System.Object sender, System.EventArgs e)
        {
            // close-out from application
            CloseOutApp(client, user, (ulong)appid);
            OptInBtn.IsEnabled = true;
            CallAppBtn.IsEnabled = true;
            ReadLocalStateBtn.IsEnabled = true;
            ReadGlobalStateBtn.IsEnabled = true;
            UpdateAppBtn.IsEnabled = true;
            CallUpdatedAppBtn.IsEnabled = true;
            ReadLocalStateAgainBtn.IsEnabled = true;
            CloseOutAppBtn.IsEnabled = true;
            DeleteAppBtn.IsEnabled = false;
            ClearAppCtn.IsEnabled = true;
        }

        void DeleteApp_Clicked(System.Object sender, System.EventArgs e)
        {
            // delete application
            DeleteApp(client, creator, appid);
            OptInBtn.IsEnabled = true;
            CallAppBtn.IsEnabled = true;
            ReadLocalStateBtn.IsEnabled = true;
            ReadGlobalStateBtn.IsEnabled = true;
            UpdateAppBtn.IsEnabled = true;
            CallUpdatedAppBtn.IsEnabled = true;
            ReadLocalStateAgainBtn.IsEnabled = true;
            CloseOutAppBtn.IsEnabled = true;
            DeleteAppBtn.IsEnabled = true;
            ClearAppCtn.IsEnabled = true;
        }

        void ClearApp_Clicked(System.Object sender, System.EventArgs e)
        {


            // clear application from user account
            ClearApp(client, user, appid);
            OptInBtn.IsEnabled = true;
            CallAppBtn.IsEnabled = true;
            ReadLocalStateBtn.IsEnabled = true;
            ReadGlobalStateBtn.IsEnabled = true;
            UpdateAppBtn.IsEnabled = true;
            CallUpdatedAppBtn.IsEnabled = true;
            ReadLocalStateAgainBtn.IsEnabled = true;
            CloseOutAppBtn.IsEnabled = true;
            DeleteAppBtn.IsEnabled = true;
            ClearAppCtn.IsEnabled = true;
        }
    }

}