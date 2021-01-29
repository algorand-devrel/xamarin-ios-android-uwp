using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;


using Algorand;
using Algorand.V2;
using Algorand.Client;
using Account = Algorand.Account;



namespace algorandapp
{
    public partial class AtomicTransfers : ContentPage
    {
        public Account account1;
        public Account account2;
        public Account account3;

        //   default to TESTNET
        public AlgodApi algodApiInstance;

        public static helper helper = new helper();

        public string network = "";
        public string nodetype = "";
        public AtomicTransfers()
        {
            InitializeComponent();
            Appearing += AtomicTransfers_Appearing;
        }

        private async void AtomicTransfers_Appearing(object sender, EventArgs e)
        {
            algodApiInstance = await helper.CreateApiInstance();
            network = await helper.GetNetwork();

            // restore accounts
            var accounts = await helper.RestoreAccounts();
            account1 = accounts[0];
            account2 = accounts[1];
            account3 = accounts[2];

            nodetype = await SecureStorage.GetAsync(helper.StorageNodeType);
            NetworkLabel.Text = "Network: " + network + " " + nodetype;
            buttonstate();
        }

        public async void buttonstate()
        {
            NetworkLabel.Text = "Network: " + network + " " + nodetype;
            var myAT = await SecureStorage.GetAsync(helper.StorageAtomicTransaction);
            if (string.IsNullOrEmpty(myAT) || myAT == " ")
            {
                AtomicTransferInfo.IsEnabled = false;
            }
            else
            {
                AtomicTransferInfo.IsEnabled = true;
            }


        }
        async void AtomicTransfer_Clicked(System.Object sender, System.EventArgs e)
        {
            //var transParams = algodApiInstance.TransactionParams();
            AtomicTransfer.Opacity = .2;
            StackAtomicTransfers.IsEnabled = false;
            Algorand.V2.Model.TransactionParametersResponse transParams = null;
            AtomicTransfer.IsEnabled = false;

            try
            {
                transParams = algodApiInstance.TransactionParams();
            }
            catch (ApiException err)
            {
                throw new Exception("Could not get params", err);
            }
            // let's create a transaction group
            var amount = Utils.AlgosToMicroalgos(1);
            var tx = Utils.GetPaymentTransaction(account1.Address, account2.Address, amount, "pay message", transParams);
            var tx2 = Utils.GetPaymentTransaction(account1.Address, account3.Address, amount, "pay message", transParams);
            //SignedTransaction signedTx2 = src.SignTransactionWithFeePerByte(tx2, feePerByte);
            Digest gid = TxGroup.ComputeGroupID(new Algorand.Transaction[] { tx, tx2 });
            tx.AssignGroupID(gid);
            tx2.AssignGroupID(gid);
            // already updated the groupid, sign
            var signedTx = account1.SignTransaction(tx);
            var signedTx2 = account1.SignTransaction(tx2);
            try
            {
                //contact the signed msgpack
                List<byte> byteList = new List<byte>(Algorand.Encoder.EncodeToMsgPack(signedTx));
                byteList.AddRange(Algorand.Encoder.EncodeToMsgPack(signedTx2));
                var act = await algodApiInstance.AccountInformationAsync(account1.Address.ToString());
                var before = "Account 1 balance before: " + act.Amount.ToString();
                var id = algodApiInstance.RawTransaction(byteList.ToArray());
                var wait = Utils.WaitTransactionToComplete(algodApiInstance, id.TxId);
                Console.WriteLine(wait);

                // Console.WriteLine("Successfully sent tx group with first tx id: " + id);

                act = await algodApiInstance.AccountInformationAsync(account1.Address.ToString());
        
                await SecureStorage.SetAsync(helper.StorageAtomicTransaction, wait.ToString());

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + before + " </h3>" +
                    "<h3>" + wait + "</h3>" + "<h3> Account 1 balance after: " + act.Amount.ToString() + "</h3></body></html>";

                myWebView.Source = htmlSource;
                AtomicTransfer.IsEnabled = true;
            }
            catch (ApiException err)
            {
                // This is generally expected, but should give us an informative error message.
                Console.WriteLine("Exception when calling algod#rawTransaction: " + err.Message);
                AtomicTransfer.IsEnabled = true;
            }
            AtomicTransfer.Opacity = 1;
            StackAtomicTransfers.IsEnabled = true;
            AtomicTransferInfo.IsEnabled = true;
            buttonstate();
        }

        async void AtomicTransferInfo_Clicked(System.Object sender, System.EventArgs e)
        {
            var act = await algodApiInstance.AccountInformationAsync(account1.Address.ToString());
            var mytx = await SecureStorage.GetAsync(helper.StorageAtomicTransaction);

          

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h3>" + mytx + " </h3>" +
                 "Account 1 balance : " +act.Amount.ToString() + "</body></html>";

            myWebView.Source = htmlSource;


        }

        async void Reset_Clicked(System.Object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                await SecureStorage.SetAsync(helper.StorageAtomicTransaction, " ");
            }
            else
            {
                await SecureStorage.SetAsync(helper.StorageAtomicTransaction, "");
            }

            AtomicTransferInfo.IsEnabled = false;

        }
    }
}
