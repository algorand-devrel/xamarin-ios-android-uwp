using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


using Algorand;
using Algorand.V2;
using Algorand.V2.Model;
using Account = Algorand.Account;


using Org.BouncyCastle.Crypto.Parameters;
using System.IO;
using Xamarin.Essentials;
using System.Numerics;
using Newtonsoft.Json;
using System.Threading;

namespace algorandapp
{
    public partial class ASA : ContentPage
    {


        public Account account1;
        public Account account2;
        public Account account3;

        //   default to TESTNET
        public AlgodApi algodApiInstance;

        public static helper helper = new helper();

        public string network = "";
        //public ulong? assetID = 0;
        public long? assetID = 0;

        public ASA()
        {
            InitializeComponent();
            Appearing += ASA_Appearing;
        }

        private async void ASA_Appearing(object sender, EventArgs e)
        {
            algodApiInstance = await helper.CreateApiInstance();
            network = await helper.GetNetwork();

            // restore accounts
            var accounts = await helper.RestoreAccounts();
            account1 = accounts[0];
            account2 = accounts[1];
            account3 = accounts[2];
            var nodetype = await SecureStorage.GetAsync(helper.StorageNodeType);
            NetworkLabel.Text = "Network: " + network + " " + nodetype;

            var lastASAbutton = await SecureStorage.GetAsync(helper.StorageLastASAButton);
            if (string.IsNullOrEmpty(lastASAbutton) || lastASAbutton == " ")
            {
                 buttonstate("init");
            }
            else
            {
                buttonstate(lastASAbutton);
            }

          //  StorageLastASAButton




        }
        public async void buttonstate(string buttonclicked)
        {
            Status.Text = "";
            NetworkLabel.Text = "Network: " + network;
            var AssetID = await SecureStorage.GetAsync(helper.StorageAssetIDName);
            if (!(String.IsNullOrEmpty(AssetID) || AssetID == " "))
            {
                if (AssetID != " ")
                { 
                myAsset.Text = "Asset ID = " + AssetID;
                assetID = (long?)Convert.ToUInt64(AssetID);
                }
            }



            switch (buttonclicked)
            {
                case "init":
                    StackCongfigureManagerRole.IsEnabled = false;
                    CongfigureManagerRole.Opacity = .4;

                    StackOptIn.IsEnabled = false;
                    OptIn.Opacity = .4;

                    StackTransferAsset.IsEnabled = false;
                    TransferAsset.Opacity = .4;

                    StackFreezeAsset.IsEnabled = false;
                    FreezeAsset.Opacity = .4;

                    StackRevokeAsset.IsEnabled = false;
                    RevokeAsset.Opacity = .4;

                    StackDestroyAsset.IsEnabled = false;
                    DestroyAsset.Opacity = .4;
                    break;

                case "create":
                    StackCongfigureManagerRole.IsEnabled = true;
                    CongfigureManagerRole.Opacity = 1;

                    StackOptIn.IsEnabled = false;
                    OptIn.Opacity = .4;

                    StackTransferAsset.IsEnabled = false;
                    TransferAsset.Opacity = .4;

                    StackFreezeAsset.IsEnabled = false;
                    FreezeAsset.Opacity = .4;

                    StackRevokeAsset.IsEnabled = false;
                    RevokeAsset.Opacity = .4;

                    StackDestroyAsset.IsEnabled = false;
                    DestroyAsset.Opacity = .4;
                    break;

                case "manage":
                    StackCongfigureManagerRole.IsEnabled = false;
                    CongfigureManagerRole.Opacity = .4;

                    StackOptIn.IsEnabled = true;
                    OptIn.Opacity = 1;

                    StackTransferAsset.IsEnabled = false;
                    TransferAsset.Opacity = .4;

                    StackFreezeAsset.IsEnabled = false;
                    FreezeAsset.Opacity = .4;

                    StackRevokeAsset.IsEnabled = false;
                    RevokeAsset.Opacity = .4;

                    StackDestroyAsset.IsEnabled = false;
                    DestroyAsset.Opacity = .4;
                    break;

                case "optin":
                    StackCongfigureManagerRole.IsEnabled = false;
                    CongfigureManagerRole.Opacity = .4;

                    StackOptIn.IsEnabled = true;
                    OptIn.Opacity = 1;

                    StackTransferAsset.IsEnabled = true;
                    TransferAsset.Opacity = 1;

                    StackFreezeAsset.IsEnabled = false;
                    FreezeAsset.Opacity = .4;

                    StackRevokeAsset.IsEnabled = false;
                    RevokeAsset.Opacity = .4;

                    StackDestroyAsset.IsEnabled = false;
                    DestroyAsset.Opacity = .4;
                    break;

                case "transfer":
                    StackCongfigureManagerRole.IsEnabled = false;
                    CongfigureManagerRole.Opacity = .4;

                    StackOptIn.IsEnabled = true;
                    OptIn.Opacity = 1;

                    StackTransferAsset.IsEnabled = true;
                    TransferAsset.Opacity = 1;

                    StackFreezeAsset.IsEnabled = true;
                    FreezeAsset.Opacity = 1;

                    StackRevokeAsset.IsEnabled = false;
                    RevokeAsset.Opacity = .4;

                    StackDestroyAsset.IsEnabled = false;
                    DestroyAsset.Opacity = .4;
                    break;

                case "freeze":
                    StackCongfigureManagerRole.IsEnabled = false;
                    CongfigureManagerRole.Opacity = .4;

                    StackOptIn.IsEnabled = true;
                    OptIn.Opacity = 1;

                    StackTransferAsset.IsEnabled = true;
                    TransferAsset.Opacity = 1;

                    StackFreezeAsset.IsEnabled = true;
                    FreezeAsset.Opacity = 1;

                    StackRevokeAsset.IsEnabled = true;
                    RevokeAsset.Opacity = 1;

                    StackDestroyAsset.IsEnabled = false;
                    DestroyAsset.Opacity = .4;
                    break;

                case "revoke":
                    StackCongfigureManagerRole.IsEnabled = false;
                    CongfigureManagerRole.Opacity = .4;

                    StackOptIn.IsEnabled = true;
                    OptIn.Opacity = 1;

                    StackTransferAsset.IsEnabled = true;
                    TransferAsset.Opacity = 1;

                    StackFreezeAsset.IsEnabled = true;
                    FreezeAsset.Opacity = 1;

                    StackRevokeAsset.IsEnabled = true;
                    RevokeAsset.Opacity = 1;

                    StackDestroyAsset.IsEnabled = true;
                    DestroyAsset.Opacity = 1;
                    break;

                case "destroy":
                    StackCongfigureManagerRole.IsEnabled = false;
                    CongfigureManagerRole.Opacity = .4;

                    StackOptIn.IsEnabled = false;
                    OptIn.Opacity = .4;

                    StackTransferAsset.IsEnabled = false;
                    TransferAsset.Opacity = .4;

                    StackFreezeAsset.IsEnabled = false;
                    FreezeAsset.Opacity = .4;

                    StackRevokeAsset.IsEnabled = false;
                    RevokeAsset.Opacity = .4;

                    StackDestroyAsset.IsEnabled = false;
                    DestroyAsset.Opacity = .4;
                    await SecureStorage.SetAsync(helper.StorageLastASAButton, "init");

                    break;
                default:
                    break;
            }

        }

        private async void CreateAsset_click(System.Object sender, System.EventArgs e)
        {
            CreateAsset.Opacity = .2;
            HtmlWebViewSource htmlSource = new HtmlWebViewSource();

            var transParams = algodApiInstance.TransactionParams();

            // The following parameters are asset specific
            // and will be re-used throughout the example. 

            // Create the Asset
            // Total number of this asset available for circulation = 10000

            var ap = new AssetParams(creator: account1.Address.ToString(), name: "latikum22",
                unitName: "LAT", defaultFrozen: false, total: 10000,
                decimals: 0, url: "http://this.test.com", metadataHash: Encoding.ASCII.GetBytes("16efaa3924a6fd9d3a4880099a4ac65d"))
            {
                Manager = account2.Address.ToString()
            };

            // Specified address can change reserve, freeze, clawback, and manager
            // you can leave as default, by default the sender will be manager/reserve/freeze/clawback

            var tx = Utils.GetCreateAssetTransaction(ap, transParams, "asset tx message");

            // Sign the Transaction by sender
            SignedTransaction signedTx = account1.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
          
            try
            {
                PostTransactionsResponse id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Transaction ID: " + id.TxId);
                Console.WriteLine(Utils.WaitTransactionToComplete(algodApiInstance, id.TxId));
                // Now that the transaction is confirmed we can get the assetID
                var ptx = algodApiInstance.PendingTransactionInformation(id.TxId);
                assetID = ptx.AssetIndex;


                var assetIDstr = assetID.ToString();
                await SecureStorage.SetAsync(helper.StorageAssetIDName, assetIDstr);
   
                await SecureStorage.SetAsync(helper.StorageLastASAButton, "create");
                buttonstate("create");
                CreateAsset.Opacity = 1;
                var act = algodApiInstance.GetAssetByID(assetID).ToJson();
             //   var act = algodApiInstance.AssetInformation((long?)assetID).ToJson();

                htmlSource.Html = @"<html><body><h3>" + "AssetID = " + assetID.ToString() + "</h3>" +
                    "<h3>" + "Asset Info = " + act.ToString() + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.StackTrace);
                CreateAsset.Opacity = 1;
                htmlSource.Html = @"<html><body><h3>" + "Error = " + err.Message + "</h3>" +
                    "</body></html>";
                myWebView.Source = htmlSource;
                return;
            }
            Console.WriteLine("AssetID = " + assetID);
            // now the asset already created
        }


        async void CongfigureManagerRole_click(System.Object sender, System.EventArgs e)
        {
            CongfigureManagerRole.Opacity = .2;
            // Change Asset Configuration:
            // Next we will change the asset configuration
            // First we update standard Transaction parameters
            // To account for changes in the state of the blockchain
            var transParams = algodApiInstance.TransactionParams();
            Asset ast = algodApiInstance.GetAssetByID(assetID);
  
            // Note that configuration changes must be done by
            // The manager account, which is currently account2
            // Note in this transaction we are re-using the asset
            // creation parameters and only changing the manager
            // and transaction parameters like first and last round
            // now update the manager to account1
            ast.Params.Manager = account1.Address.ToString();
            var tx = Utils.GetConfigAssetTransaction(account2.Address, ast, transParams, "config trans");

            // The transaction must be signed by the current manager account
            // We are reusing the signedTx variable from the first transaction in the example    
            var signedTx = account2.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            string mytx;
            try
            {
                PostTransactionsResponse id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Transaction ID: " + id.TxId);
                mytx= id.TxId;
                var wait = Utils.WaitTransactionToComplete(algodApiInstance, id.TxId);
                CongfigureManagerRole.Opacity = 1;
                Console.WriteLine(wait);
                await SecureStorage.SetAsync(helper.StorageLastASAButton, "manage");
                buttonstate("manage");
               // var act = algodApiInstance.AssetInformation((long?)assetID).ToJson();
                // Get the asset information for the newly changed asset            
                ast = algodApiInstance.GetAssetByID(assetID);
                //The manager should now be the same as the creator

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Transaction ID = " + mytx + "</h3>" +
                    "<h3>" + "Asset ID = " + assetID.ToString() + "</h3>" +
                    "<h3>" + "Asset Info = " + ast.ToString() + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
            

            }
            catch (Exception err)
            {
                //e.printStackTrace();
                Console.WriteLine(err.Message);
                CongfigureManagerRole.Opacity = 1;
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Error = " + err.Message + "</h3>" +
                    "</body></html>";
                myWebView.Source = htmlSource;
                return;
            }


        }



        async void OptIn_Clicked(System.Object sender, System.EventArgs e)
        {
            OptIn.Opacity = .2;

            // Opt in to Receiving the Asset

            // Opting in to transact with the new asset
            // All accounts that want receive the new asset
            // Have to opt in. To do this they send an asset transfer
            // of the new asset to themselves with an amount of 0
            // In this example we are setting up the 3rd recovered account to 
            // receive the new asset        
            // First, we update standard Transaction parameters
            // To account for changes in the state of the blockchain

            var transParams = algodApiInstance.TransactionParams();
          //  var tx = Utils.GetActivateAssetTransaction(account3.Address, assetID, transParams, "opt in transaction");
            var tx = Utils.GetAssetOptingInTransaction(account3.Address, assetID, transParams, "opt in transaction");

            // The transaction must be signed by the current manager account
            // We are reusing the signedTx variable from the first transaction in the example    
            var signedTx = account3.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            Algorand.V2.Model.Account account = null;
            string mytx;
            try
            {
                PostTransactionsResponse id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Transaction ID: " + id.TxId);
                var wait = Utils.WaitTransactionToComplete(algodApiInstance, id.TxId);
                mytx = id.TxId;
                OptIn.Opacity = 1;
                Console.WriteLine(wait);
                // We can now list the account information for acct3 
                // and see that it can accept the new asset
           
                account = algodApiInstance.AccountInformation(account3.Address.ToString());
                var assetholding = account.Assets;
                Console.WriteLine(assetholding);
                await SecureStorage.SetAsync(helper.StorageLastASAButton, "optin");
                buttonstate("optin");

                account = algodApiInstance.AccountInformation(account3.Address.ToString());

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Transaction ID = " + mytx + "</h3>" +
                    "<h3>" + "Account 3 Asset Amount = " + account.Assets.Find(h => h.AssetId == assetID).Amount + "</h3>" +
                    "<h3>" + "Asset ID = " + assetID.ToString() + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
            }
            catch (Exception err)
            {
                //e.printStackTrace();
                Console.WriteLine(err.Message);
                OptIn.Opacity = 1;
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Error = " + err.Message + "</h3>" +
                    "</body></html>";
                myWebView.Source = htmlSource;
                return;
            }
        }



        async void FreezeAsset_Clicked(System.Object sender, System.EventArgs e)
        {
            FreezeAsset.Opacity = .2;
            // Freeze the asset
            // The asset was created and configured to allow freezing an account
            // If the freeze address is blank, it will no longer be possible to do this.
            // In this example we will now freeze account3 from transacting with the 
            // The newly created asset. 
            // The freeze transaction is sent from the freeze account
            // Which in this example is account2 
            // First we update standard Transaction parameters
            // To account for changes in the state of the blockchain

            var transParams = algodApiInstance.TransactionParams();
            // Next we set asset xfer specific parameters
            // The sender should be freeze account acct2
            // Theaccount to freeze should be set to acct3
            var tx = Utils.GetFreezeAssetTransaction(account2.Address, account3.Address, assetID, true, transParams, "freeze transaction");
            // The transaction must be signed by the freeze account acct2
            // We are reusing the signedTx variable from the first transaction in the example    
            var signedTx = account2.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            string mytx;
            try
            {
                PostTransactionsResponse id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Transaction ID: " + id.TxId);
                Console.WriteLine(Utils.WaitTransactionToComplete(algodApiInstance, id.TxId));
                mytx = id.TxId;
                FreezeAsset.Opacity = 1;
                // We can now list the account information for acct3 
                // and see that it now frozen 
   
                var act = algodApiInstance.AccountInformation(account3.Address.ToString());
                Console.WriteLine(act.Assets.Find(h => h.AssetId == assetID));
                await SecureStorage.SetAsync(helper.StorageLastASAButton, "freeze");
                buttonstate("freeze");
                var asset = algodApiInstance.GetAssetByID((long?)assetID).ToJson();
                Algorand.V2.Model.Account account = algodApiInstance.AccountInformation(account3.Address.ToString());

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Transaction ID = " + mytx + "</h3>" +
                       "<h3>" + "Asset ID = " + assetID.ToString() + "</h3>" +
                    "<h3>" + "Account 3 Asset Freeze = " + account.Assets.Find(h => h.AssetId == assetID) + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;

            }
            catch (Exception err)
            {
                //e.printStackTrace();
                Console.WriteLine(err.Message);
                FreezeAsset.Opacity = 1;
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Error = " + err.Message + "</h3>" +
                    "</body></html>";
                myWebView.Source = htmlSource;
                return;
            }


        }



        async void RevokeAsset_Clicked(System.Object sender, System.EventArgs e)
        {
            RevokeAsset.Opacity = .2;
            // Revoke the asset:
            // The asset was also created with the ability for it to be revoked by 
            // clawbackaddress. If the asset was created or configured by the manager
            // not allow this by setting the clawbackaddress to a blank address  
            // then this would not be possible.
            // We will now clawback the 10 assets in account3. Account2
            // is the clawbackaccount and must sign the transaction
            // The sender will be the clawback adress.
            // the recipient will also be the creator acct1 in this case  
            // First we update standard Transaction parameters
            // To account for changes in the state of the blockchain
            var transParams = algodApiInstance.TransactionParams();
            // Next we set asset xfer specific parameters
            ulong assetAmount = 10;
            var tx = Utils.GetRevokeAssetTransaction(account2.Address, account3.Address, account1.Address, assetID, assetAmount, transParams, "revoke transaction");
            // The transaction must be signed by the clawback account
            // We are reusing the signedTx variable from the first transaction in the example    
            var signedTx = account2.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            string mytx;
            try
            {
                PostTransactionsResponse id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Transaction ID: " + id);
                Console.WriteLine(Utils.WaitTransactionToComplete(algodApiInstance, id.TxId));
                mytx = id.TxId;
                RevokeAsset.Opacity = .4;
                RevokeAsset.IsEnabled = false;
                // We can now list the account information for acct3 
                // and see that it now has 0 of the new asset
                var act = algodApiInstance.AccountInformation(account3.Address.ToString());

                Console.WriteLine(act.Assets.Find(h => h.AssetId == assetID).Amount);
                await SecureStorage.SetAsync(helper.StorageLastASAButton, "revoke");
                buttonstate("revoke");
                Algorand.V2.Model.Account account = algodApiInstance.AccountInformation(account3.Address.ToString());

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Transaction ID = " + mytx + "</h3>" +
                       "<h3>" + "Asset ID = " + assetID.ToString() + "</h3>" +
                    "<h3>" + "Account 3 Asset Amount = " + account.Assets.Find(h => h.AssetId == assetID).Amount + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
            }
            catch (Exception err)
            {
                //e.printStackTrace();
                Console.WriteLine(err.Message);
                RevokeAsset.Opacity = .4;
                RevokeAsset.IsEnabled = false;
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Error = " + err.Message + "</h3>" +
                    "</body></html>";
                myWebView.Source = htmlSource;
                return;
            }



        }


        async void DestroyAsset_Clicked(System.Object sender, System.EventArgs e)
        {
            DestroyAsset.Opacity = .2;
            // Destroy the Asset:
            // All of the created assets should now be back in the creators
            // Account so we can delete the asset.
            // If this is not the case the asset deletion will fail
            // The address for the from field must be the creator
            // First we update standard Transaction parameters
            // To account for changes in the state of the blockchain
            var transParams = algodApiInstance.TransactionParams();
            // Next we set asset xfer specific parameters
            // The manager must sign and submit the transaction
            // This is currently set to acct1
            var tx = Utils.GetDestroyAssetTransaction(account1.Address, assetID, transParams, "destroy transaction");
            // The transaction must be signed by the manager account
            // We are reusing the signedTx variable from the first transaction in the example    
            var signedTx = account1.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            string mytx;
            try
            {
                PostTransactionsResponse id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Transaction ID: " + id.TxId);
                //waitForTransactionToComplete(algodApiInstance, signedTx.transactionID);
                //Console.ReadKey();
                Console.WriteLine(Utils.WaitTransactionToComplete(algodApiInstance, id.TxId));
                mytx = id.TxId;
                // We can now list the account information for acct1 
                // and see that the asset is no longer there
                var act = algodApiInstance.AccountInformation(account1.Address.ToString());

                Console.WriteLine("Does AssetID: " + assetID + " exist? " +
                    act.Assets.Find(h => h.AssetId == assetID));


                await SecureStorage.SetAsync(helper.StorageAssetIDName, "");
            
                myAsset.Text = "";
                DestroyAsset.Opacity = 1;
                await SecureStorage.SetAsync(helper.StorageLastASAButton, "destroy");
                buttonstate("destroy");
                Algorand.V2.Model.Account account = algodApiInstance.AccountInformation(account3.Address.ToString());

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Transaction ID = " + mytx + "</h3>" +
                    "<h3>" + "AssetID: " + assetID + " destroyed "  + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
            }
            catch (Exception err)
            {
                //e.printStackTrace();
                DestroyAsset.Opacity = 1;
                Console.WriteLine(err.Message);
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Error = " + err.Message + "</h3>" +
                    "</body></html>";
                myWebView.Source = htmlSource;
                return;
            }


        }

        async void TransferAsset_Clicked(System.Object sender, System.EventArgs e)
        {
            TransferAsset.Opacity = .2;
            // Transfer the Asset:
            // Now that account3 can receive the new asset 
            // we can transfer assets in from the creator
            // to account3
            // First we update standard Transaction parameters
            // To account for changes in the state of the blockchain

            var transParams = algodApiInstance.TransactionParams();
            // Next we set asset xfer specific parameters
            // We set the assetCloseTo to null so we do not close the asset out
            Address assetCloseTo = new Address();
            ulong assetAmount = 10;
            var tx = Utils.GetTransferAssetTransaction(account1.Address, account3.Address, assetID, assetAmount, transParams, null, "transfer message");
            // The transaction must be signed by the sender account
            // We are reusing the signedTx variable from the first transaction in the example    
            var signedTx = account1.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            string mytx;
            try
            {
                PostTransactionsResponse id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Transaction ID: " + id.TxId);
                mytx = id.TxId;
                Console.WriteLine(Utils.WaitTransactionToComplete(algodApiInstance, id.TxId));
                // We can now list the account information for acct3 
                // and see that it now has 5 of the new asset
                var act = algodApiInstance.AccountInformation(account3.Address.ToString());
                Console.WriteLine(act.Assets.Find(h => h.AssetId == assetID).Amount);
     
                TransferAsset.Opacity = 1;
                await SecureStorage.SetAsync(helper.StorageLastASAButton, "transfer");
                buttonstate("transfer");
                var asset = algodApiInstance.GetAssetByID((long?)assetID).ToJson();
                Algorand.V2.Model.Account account = algodApiInstance.AccountInformation(account3.Address.ToString());

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Transaction ID = " + mytx + "</h3>" +
                       "<h3>" + "Asset ID = " + assetID.ToString() + "</h3>" +
                    "<h3>" + "Account 3 Asset Amount = " + account.Assets.Find(h => h.AssetId == assetID).Amount + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
            }
            catch (Exception err)
            {
                //e.printStackTrace();
                Console.WriteLine(err.Message);
                TransferAsset.Opacity = 1;
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Error = " + err.Message + "</h3>" +
                    "</body></html>";
                myWebView.Source = htmlSource;
                return;
            }


        }

        async void Reset_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await SecureStorage.SetAsync(helper.StorageAssetIDName, " ");
            }
            else
            {
                await SecureStorage.SetAsync(helper.StorageAssetIDName, "");
            }
         
            await SecureStorage.SetAsync(helper.StorageLastASAButton, "init");

            myAsset.Text = "";
            buttonstate("init");
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body></body></html>";

            myWebView.Source = htmlSource;
            await DisplayAlert("Clear Asset ", "Asset Removed", "Cancel");
        }



    }
}

