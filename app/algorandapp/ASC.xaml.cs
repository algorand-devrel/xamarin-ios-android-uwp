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
    public partial class ASC : ContentPage
    {
        public Account account1;
        public Account account2;
        public Account account3;

        //   default to TESTNET
        public AlgodApi algodApiInstance;

        public static helper helper = new helper();

        public string network = "";
        public string nodetype = "";
        public ulong? assetID = 0;

        public ASC()
        {
            InitializeComponent();
            Appearing += ASC_Appearing;
        }

        private async void ASC_Appearing(object sender, EventArgs e)
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
        public void buttonstate()
        {
            NetworkLabel.Text = "Network: " + network + " " + nodetype;
        }
        void ASCContractAccount_Clicked(System.Object sender, System.EventArgs e)
        {
            StackASCContractAccount.IsEnabled = false;
            ASCContractAccount.Opacity = .2;
            ASCContractAccount.IsEnabled = false;
            Algorand.V2.Model.TransactionParametersResponse transParams = null;
            try
            {
                transParams = algodApiInstance.TransactionParams();
            }
            catch (ApiException err)
            {
                throw new Exception("Could not get params", err);
            }
            // format and send logic sig
            // int 1, returns true
            // byte[] program = { 0x01, 0x20, 0x01, 0x01, 0x22 };
            // int 0, returns false, so rawTransaction will fail below
            // byte[] program = Convert.FromBase64String("ASABASI=");
            byte[] program = { 0x01, 0x20, 0x01, 0x00, 0x22 };
            LogicsigSignature lsig = new LogicsigSignature(program, null);
            Console.WriteLine("Escrow address: " + lsig.ToAddress().ToString());
            var tx = Utils.GetPaymentTransaction(lsig.Address, account1.Address, 100000, "draw algo from contract", transParams);

          //  Algorand.Transaction tx = Utils.GetLogicSignatureTransaction(lsig, account1.Address, transParams, "logic sig message");
            if (!lsig.Verify(tx.sender))
            {
                string msg = "Verification failed";
                Console.WriteLine(msg);
            }
            else
            {
                try
                {
                    SignedTransaction stx = Account.SignLogicsigTransaction(lsig, tx);
                    byte[] encodedTxBytes = Encoder.EncodeToMsgPack(stx);
                    // int 0 is the teal program, which returns false, 
                    // so rawTransaction will fail below   
                    var id = algodApiInstance.RawTransaction(encodedTxBytes);
                    Console.WriteLine("Successfully sent tx logic sig tx id: " + id);
                    var wait = Utils.WaitTransactionToComplete(algodApiInstance, id.TxId);
                    Console.WriteLine(wait);
                    var htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = @"<html><body>" +
    "<h3>" + "Successfully sent tx logic sig tx: " + wait + "</h3>" +
    "</body></html>";
                    myWebView.Source = htmlSource;
                    ASCContractAccount.IsEnabled = true;
                }
                catch (ApiException err)
                {
                    // This is generally expected, but should give us an informative error message.
                    Console.WriteLine("Fail expected");
                    Console.WriteLine("Exception when calling algod#rawTransaction: " + err.Message);
                    var htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = @"<html><body>" +
                        "<h3>" + "Expected error: Exception when calling algod#rawTransaction: " + err.Message + "</h3>" +
                        "</body></html>";
                    myWebView.Source = htmlSource;
                    ASCContractAccount.IsEnabled = true;
                }
            }
            StackASCContractAccount.IsEnabled = true;
            ASCContractAccount.Opacity = 1;
        }

        void ASCAccountDelegation_Clicked(System.Object sender, System.EventArgs e)
        {
            ASCAccountDelegation.Opacity = .2;
            StackASCAccountDelegation.IsEnabled = false;
            ASCAccountDelegation.IsEnabled = false;
            Algorand.V2.Model.TransactionParametersResponse transParams = null;
            try
            {
                transParams = algodApiInstance.TransactionParams();
            }
            catch (ApiException err)
            {
                throw new Exception("Could not get params", err);
            }
            // format and send logic sig
            // int 1, returns true
            // byte[] program = { 0x01, 0x20, 0x01, 0x01, 0x22 };
            // int 0, returns false, so rawTransaction will fail below
            byte[] program = { 0x01, 0x20, 0x01, 0x00, 0x22 };

            LogicsigSignature lsig = new LogicsigSignature(program, null);

            // sign the logic signature with an account sk
            account1.SignLogicsig(lsig);

            Console.WriteLine("Escrow address: " + lsig.ToAddress().ToString());
            Transaction tx = Utils.GetPaymentTransaction(new Address(account1.Address.ToString()), new Address(account2.Address.ToString()), 1000000,
    "draw algo with logic signature", transParams);
          //  Algorand.Transaction tx = Utils.GetLogicSignatureTransaction(account1.Address, account2.Address, transParams, "logic sig message");
            try
            {
                SignedTransaction stx = Account.SignLogicsigTransaction(lsig, tx);
                byte[] encodedTxBytes = Encoder.EncodeToMsgPack(stx);
                // int 0 is the teal program, which returns false, 
                // so rawTransaction will fail below   

                var id = Utils.SubmitTransaction(algodApiInstance, stx);

                var wait = Utils.WaitTransactionToComplete(algodApiInstance, id.TxId);
                Console.WriteLine(wait);
                Console.WriteLine("Successfully sent tx logic sig tx id: " + id);
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body>" +
"<h3>" + "Successfully sent tx logic sig tx: " + wait + "</h3>" +
"</body></html>";
                myWebView.Source = htmlSource;
                ASCAccountDelegation.IsEnabled = true;
            }
            catch (ApiException err)
            {
                // This is generally expected, but should give us an informative error message.

                Console.WriteLine("Exception when calling algod#rawTransaction: " + err.Message);
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3> Expected error: Exception when calling algod#rawTransaction: " + err.Message + "</h3>" + "</body></html>";
                myWebView.Source = htmlSource;
                ASCAccountDelegation.IsEnabled = true;
            }
            ASCAccountDelegation.Opacity = 1;
            StackASCAccountDelegation.IsEnabled = true;
        }
    }
}
