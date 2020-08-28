using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Algorand;
using Account = Algorand.Account;
using Algorand.Algod.Client.Api;
using Algorand.Algod.Client.Model;
using Algorand.Algod.Client;
using Transaction = Algorand.Transaction;

using Org.BouncyCastle.Crypto.Parameters;
using System.IO;
using Xamarin.Essentials;
using System.Numerics;

namespace algorandapp
{
    public partial class Accounts : ContentPage
    {

        public static helper helper = new helper();


        public AlgodApi algodApiInstance;

        public string network = "";

        public Accounts()
        {
            InitializeComponent();
            Appearing += Accounts_Appearing;
        }

        private async void Accounts_Appearing(object sender, EventArgs e)
        {

            algodApiInstance = await helper.CreateApiInstance();
            network = await helper.GetNetwork();

            buttonstate();
            if (Device.Idiom == TargetIdiom.Phone)
            {
                StackPhone.IsVisible = true;
                StackTablet.IsVisible = false;
            }
            else
            {
                StackPhone.IsVisible = false;
                StackTablet.IsVisible = true;
            }


        }

        public async void buttonstate()
        {

            var account1 = await SecureStorage.GetAsync(helper.StorageAccountName1);
            var account2 = await SecureStorage.GetAsync(helper.StorageAccountName2);
            var account3 = await SecureStorage.GetAsync(helper.StorageAccountName3);
            network = await SecureStorage.GetAsync(helper.StorageNetwork);
            var msig = await SecureStorage.GetAsync(helper.StorageMultisig);
            var transaction = await SecureStorage.GetAsync(helper.StorageTransaction);
            var multisigtransaction = await SecureStorage.GetAsync(helper.StorageMultisigTransaction);
            var nodetype = await SecureStorage.GetAsync(helper.StorageNodeType);

            NetworkLabel.Text = "Network: " + network + " " + nodetype;

            CreateMultiSig.IsVisible = true;
            Transaction.IsVisible = true;
            MultisigTransaction.IsVisible = true;

            CreateMultiSigp.IsVisible = true;
            Transactionp.IsVisible = true;
            MultisigTransactionp.IsVisible = true;

            // test for null (first time) and "" (after first time)
            if (string.IsNullOrEmpty(account1))
            {
                // this account is not generated yet
                GenerateAccount1.IsEnabled = true;
                GetAccount1Info.IsVisible = false;
                GenerateAccount1.Text = "Generate " + helper.StorageAccountName1;
                GenerateAccount1p.IsEnabled = true;
                GetAccount1Infop.IsVisible = false;
                GenerateAccount1p.Text = "Generate " + helper.StorageAccountName1;

            }
            else
            {
                GenerateAccount1.Text = helper.StorageAccountName1 + " created";
                GenerateAccount1.IsEnabled = false;
                GetAccount1Info.IsVisible = true;
                GenerateAccount1p.Text = helper.StorageAccountName1 + " created";
                GenerateAccount1p.IsEnabled = false;
                GetAccount1Infop.IsVisible = true;
                // DisableNetworkToggles(network);
            }
            if (string.IsNullOrEmpty(account2))
            {
                // this account is not generated yet
                GenerateAccount2.IsEnabled = true;
                GetAccount2Info.IsVisible = false;
                GenerateAccount2.Text = "Generate " + helper.StorageAccountName2;
                GenerateAccount2p.IsEnabled = true;
                GetAccount2Infop.IsVisible = false;
                GenerateAccount2p.Text = "Generate " + helper.StorageAccountName2;

            }
            else
            {
                GenerateAccount2.Text = helper.StorageAccountName2 + " created";
                GenerateAccount2.IsEnabled = false;
                GetAccount2Info.IsVisible = true;
                GenerateAccount2p.Text = helper.StorageAccountName2 + " created";
                GenerateAccount2p.IsEnabled = false;
                GetAccount2Infop.IsVisible = true;

                // DisableNetworkToggles(network);
            }
            if (string.IsNullOrEmpty(account3))
            {
                // this account is not generated yet
                GenerateAccount3.IsEnabled = true;
                GetAccount3Info.IsVisible = false;
                GenerateAccount3.Text = "Generate " + helper.StorageAccountName3;
                GenerateAccount3p.IsEnabled = true;
                GetAccount3Infop.IsVisible = false;
                GenerateAccount3p.Text = "Generate " + helper.StorageAccountName3;
            }
            else
            {

                GenerateAccount3.Text = helper.StorageAccountName3 + " created";
                GenerateAccount3.IsEnabled = false;
                GetAccount3Info.IsVisible = true;
                GenerateAccount3p.Text = helper.StorageAccountName3 + " created";
                GenerateAccount3p.IsEnabled = false;
                GetAccount3Infop.IsVisible = true;
                // DisableNetworkToggles(network);
            }

            if (!(string.IsNullOrEmpty(account1) ||
                string.IsNullOrEmpty(account2) ||
                string.IsNullOrEmpty(account3)))
            {
                // all accounts created - leave state

                //    DisableNetworkToggles(network);


                if (string.IsNullOrEmpty(msig))
                {
                    CreateMultiSig.IsEnabled = true;
                    CreateMultiSig.Text = "Create Multisig Address";
                    GetMultiSig.IsVisible = false;
                    CreateMultiSigp.IsEnabled = true;
                    CreateMultiSigp.Text = "Create Multisig Address";
                    GetMultiSigp.IsVisible = false;
                    // disbale multisig transaction
                }
                else
                {
                    //CreateMultiSig.IsEnabled = true;
                    CreateMultiSig.IsEnabled = false;
                    CreateMultiSig.Text = "Multisig created ";
                    GetMultiSig.IsVisible = true;
                    CreateMultiSigp.IsEnabled = false;
                    CreateMultiSigp.Text = "Multisig created ";
                    GetMultiSigp.IsVisible = true;
                    // todo store off version threshold and number of account
                    var htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = @"<html><body><h3>" + "Multisig created - version = 1, threshold = 2, number of accounts = 3 </h3>" +
                        "<h3>" + helper.StorageMultisig + " Address = " + msig.ToString() + "</h3>" +

                        "</body></html>";

                    myWebView.Source = htmlSource;
                    myWebViewp.Source = htmlSource;

                    // enable send multisig transaction
                }

                if (string.IsNullOrEmpty(transaction))
                {
                    Transaction.IsEnabled = true;

                    Transaction.Text = "Xfer " + helper.StorageAccountName1 + " to " + helper.StorageAccountName2;
                    GetTransaction.IsVisible = false;

                    Transactionp.IsEnabled = true;

                    Transactionp.Text = "Xfer " + helper.StorageAccountName1 + " to " + helper.StorageAccountName2;
                    GetTransactionp.IsVisible = false;

                }
                else
                {
                    Transaction.IsEnabled = true;
                    Transaction.Text = "Send " + helper.StorageAccountName1 + " to " + helper.StorageAccountName2 + "?";
                    GetTransaction.IsVisible = true;
                    Transactionp.IsEnabled = true;
                    Transactionp.Text = "Send " + helper.StorageAccountName1 + " to " + helper.StorageAccountName2 + "?";
                    GetTransactionp.IsVisible = true;

                }
                if (!(string.IsNullOrEmpty(msig)))

                {
                    if (string.IsNullOrEmpty(multisigtransaction))
                    {
                        // only enable if multisigaddress created
                        MultisigTransaction.IsEnabled = true;
                        MultisigTransaction.Text = "Send Multisig Tx to " + helper.StorageAccountName3;
                        GetMultiSigTx.IsVisible = false;
                        MultisigTransactionp.IsEnabled = true;
                        MultisigTransactionp.Text = "Send Multisig Tx to " + helper.StorageAccountName3;
                        GetMultiSigTxp.IsVisible = false;

                    }
                    else
                    {
                        MultisigTransaction.IsEnabled = true;
                        MultisigTransaction.Text = "Send Multisig Tx to " + helper.StorageAccountName3 + "?";
                        GetMultiSigTx.IsVisible = true;

                        MultisigTransactionp.IsEnabled = true;
                        MultisigTransactionp.Text = "Send Multisig Tx to " + helper.StorageAccountName3 + "?";
                        GetMultiSigTxp.IsVisible = true;
                    }
                }

            }


            if (!String.IsNullOrEmpty(account1))
            {
                FundsNeeded1.IsVisible = await ToggleFundButton(network, helper.StorageAccountName1);
                FundsNeeded1p.IsVisible = await ToggleFundButton(network, helper.StorageAccountName1);
            }
            if (!String.IsNullOrEmpty(account2))
            {
                FundsNeeded2.IsVisible = await ToggleFundButton(network, helper.StorageAccountName2);
                FundsNeeded2p.IsVisible = await ToggleFundButton(network, helper.StorageAccountName2);

            }
            if (!String.IsNullOrEmpty(account3))
            {
                FundsNeeded3.IsVisible = await ToggleFundButton(network, helper.StorageAccountName3);
                FundsNeeded3p.IsVisible = await ToggleFundButton(network, helper.StorageAccountName3);

            }
            if (!String.IsNullOrEmpty(msig))
            {
                FundsNeededMS.IsVisible = await ToggleFundButton(network, helper.StorageMultisig);
                FundsNeededMSp.IsVisible = await ToggleFundButton(network, helper.StorageMultisig);

            }
        }

        private async Task<bool> ToggleFundButton(string network, string accountname)
        {
            ulong? amount = await helper.GetAccountBalance(accountname);
            var account = await SecureStorage.GetAsync(helper.StorageAccountName1);
            if (!(String.IsNullOrEmpty(account)))
            {
                if (amount < helper.MIN_ACCOUNT_BALANCE)
                {
                    // dispense if more funds needed
                    return true;
                }
                else
                    return false;
            }
            return false;
        }




        public async void GenerateAccount1_click(System.Object sender, System.EventArgs e)
        {

            createaccounts(helper.StorageAccountName1);
            GenerateAccount1.Text = helper.StorageAccountName1 + " created";
            GenerateAccount1.IsEnabled = false;
            GetAccount1Info.IsVisible = true;

            GenerateAccount1p.Text = helper.StorageAccountName1 + " created";
            GenerateAccount1p.IsEnabled = false;
            GetAccount1Infop.IsVisible = true;

            var network = await SecureStorage.GetAsync(helper.StorageNetwork);
            //  DisableNetworkToggles(network);
            // test to make sure account has funds before doing state?

            buttonstate();

        }
        public async void GenerateAccount2_Clicked(System.Object sender, System.EventArgs e)

        {

            createaccounts(helper.StorageAccountName2);
            GenerateAccount2.Text = helper.StorageAccountName2 + " created";
            GenerateAccount2.IsEnabled = false;
            GetAccount2Info.IsVisible = true;

            GenerateAccount2p.Text = helper.StorageAccountName2 + " created";
            GenerateAccount2p.IsEnabled = false;
            GetAccount2Infop.IsVisible = true;

            var network = await SecureStorage.GetAsync(helper.StorageNetwork);
            //  DisableNetworkToggles(network);
            buttonstate();
        }

        public async void GenerateAccount3_Clicked(System.Object sender, System.EventArgs e)
        {

            createaccounts(helper.StorageAccountName3);
            GenerateAccount3.Text = helper.StorageAccountName3 + " created";
            GenerateAccount3.IsEnabled = false;
            GetAccount3Info.IsVisible = true;

            GenerateAccount3p.Text = helper.StorageAccountName3 + " created";
            GenerateAccount3p.IsEnabled = false;
            GetAccount3Infop.IsVisible = true;

            var network = await SecureStorage.GetAsync(helper.StorageNetwork);
            //  DisableNetworkToggles(network);
            buttonstate();

        }


        public async void createaccounts(string accountname)
        {


            string[] myAccountInfo = await helper.CreateAccount(accountname);

            var myAccountAddress = myAccountInfo[0].ToString();
            var myMnemonic = myAccountInfo[1].ToString();

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h3>" + accountname + " Address = " + myAccountAddress.ToString() +
                "<h3>" + accountname + " Mnemonic = " + myMnemonic.ToString() + "</h3>" +

                "</body></html>";

            myWebView.Source = htmlSource;
            myWebViewp.Source = htmlSource;
            try
            {
                await SecureStorage.SetAsync(accountname, myMnemonic);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                // Possible that device doesn't support secure storage on device.
            }

            // OpenDispenser(helper, network, myAccountAddress);
        }

        private static void OpenDispenser(helper helper, string network, string myAccountAddress)
        {
            if (network == "TestNet")
            {

                // https://bank.testnet.algorand.network/
                helper.openurl("https://bank.testnet.algorand.network/", myAccountAddress);

            }
            else
            {
                helper.openurl("https://bank.betanet.algodev.network/", myAccountAddress);

            }
        }

        public async void ClearAccounts_Clicked(System.Object sender, System.EventArgs e)
        {
            string action = await DisplayActionSheet("Are you sure you want to remove all accounts from this device?", "Cancel", null, "Yes", "No");
            Debug.WriteLine("Action: " + action);
            if (action == "Yes")
            {
                try
                {

                    await SecureStorage.SetAsync(helper.StorageAccountName1, "");
                    await SecureStorage.SetAsync(helper.StorageAccountName2, "");
                    await SecureStorage.SetAsync(helper.StorageAccountName3, "");
                    await SecureStorage.SetAsync(helper.StorageMultisig, "");
                    await SecureStorage.SetAsync(helper.StorageTransaction, "");
                    await SecureStorage.SetAsync(helper.StorageMultisigTransaction, "");
                
                    // keep storage network
                    // await SecureStorage.SetAsync(helper.StorageNetwork, "TestNet");

                    GenerateAccount1.IsEnabled = true;
                    GenerateAccount2.IsEnabled = true;
                    GenerateAccount3.IsEnabled = true;

                    MultisigTransaction.IsVisible = false;
                    GetMultiSigTx.IsVisible = false;
                    CreateMultiSig.IsVisible = false;
                    GetMultiSig.IsVisible = false;
                    Transaction.IsVisible = false;
                    GetTransaction.IsVisible = false;
                    GetAccount1Info.IsVisible = false;
                    GetAccount2Info.IsVisible = false;
                    GetAccount3Info.IsVisible = false;


                    CreateMultiSig.IsEnabled = false;
                    Transaction.IsEnabled = false;
                    MultisigTransaction.IsEnabled = false;

                    GenerateAccount1.Text = "Generate " + helper.StorageAccountName1;
                    GenerateAccount2.Text = "Generate " + helper.StorageAccountName2;
                    GenerateAccount3.Text = "Generate " + helper.StorageAccountName3;
                    CreateMultiSig.Text = "Create Multisig Address";
                    Transaction.Text = "Transaction from " + helper.StorageAccountName1 + " to " + helper.StorageAccountName2;
                    MultisigTransaction.Text = "Send Multisig Transaction to " + helper.StorageAccountName3;

                    //phone

                    GenerateAccount1p.IsEnabled = true;
                    GenerateAccount2p.IsEnabled = true;
                    GenerateAccount3p.IsEnabled = true;

                    MultisigTransactionp.IsVisible = false;
                    GetMultiSigTxp.IsVisible = false;
                    CreateMultiSigp.IsVisible = false;
                    GetMultiSigp.IsVisible = false;
                    Transactionp.IsVisible = false;
                    GetTransactionp.IsVisible = false;
                    GetAccount1Infop.IsVisible = false;
                    GetAccount2Infop.IsVisible = false;
                    GetAccount3Infop.IsVisible = false;


                    CreateMultiSigp.IsEnabled = false;
                    Transactionp.IsEnabled = false;
                    MultisigTransactionp.IsEnabled = false;

                    GenerateAccount1p.Text = "Generate " + helper.StorageAccountName1;
                    GenerateAccount2p.Text = "Generate " + helper.StorageAccountName2;
                    GenerateAccount3p.Text = "Generate " + helper.StorageAccountName3;
                    CreateMultiSigp.Text = "Create Multisig Address";
                    Transactionp.Text = "Transaction from " + helper.StorageAccountName1 + " to " + helper.StorageAccountName2;
                    MultisigTransactionp.Text = "Send Multisig Transaction to " + helper.StorageAccountName3;





                    // note: multisig sends from acct 1 and 2 if they both sign,
                    // the account receiving funds (acct 3) does not have to be in the multisig,
                    // it could be any account
                    var htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = @"<html><body></body></html>";

                    myWebView.Source = htmlSource;
                    myWebViewp.Source = htmlSource;
                    network = await SecureStorage.GetAsync(helper.StorageNetwork);
                    var nodetype = await SecureStorage.GetAsync(helper.StorageNodeType);

                    NetworkLabel.Text = "Network: " + network + " " + nodetype;


                    buttonstate();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    // Possible that device doesn't support secure storage on device.
                }
            }


        }

        public async void GetAccount1Info_Clicked(System.Object sender, System.EventArgs e)

        {
            await DisplayAccount(helper.StorageAccountName1);
            FundsNeeded1.IsVisible = await ToggleFundButton(network, helper.StorageAccountName1);
            FundsNeeded1p.IsVisible = await ToggleFundButton(network, helper.StorageAccountName1);
        }


        private async Task DisplayAccount(string accountname)
        {
            Account account;
            string mnemonic = "";
            try
            {
                mnemonic = await SecureStorage.GetAsync(accountname);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                // Possible that device doesn't support secure storage on device.
            }

            //restore account from mnemonic
            account = new Account(mnemonic);


            Algorand.Algod.Client.Model.Account accountinfo = await algodApiInstance.AccountInformationAsync(account.Address.ToString());

            Debug.WriteLine("accountinfo: " + accountinfo);

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h3>" + " Address = " + account.Address.ToString() + "</h3>" +
                "<h3>" + "Account amount (micro algos) = " + accountinfo.Amount.ToString() + "</h3>" +
                "<p>Info = " + accountinfo.ToJson() + " </p>" +
                "</body></html>";

            myWebView.Source = htmlSource;
            myWebViewp.Source = htmlSource;
        }

        public async void GetBlock_Clicked(System.Object sender, System.EventArgs e)
        {
            var status = await algodApiInstance.GetStatusAsync();
            long lastround = (long)status.LastRound;
            Block block;
            try
            {
                block = await algodApiInstance.GetBlockAsync(lastround);

                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Last Round = " + lastround.ToString() + "</h3>" +
                    "<h3>" + "Block Info = " + block.ToJson() + "</h3>" +
                    "</body></html>";
                myWebView.Source = htmlSource;
                myWebViewp.Source = htmlSource;
            }
            catch (Exception err)
            {
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3>" + "Error = " + err.Message.ToString() + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
                myWebViewp.Source = htmlSource;
            }
        }




        public async void GetAccount2Info_Clicked(System.Object sender, System.EventArgs e)
        {

            await DisplayAccount(helper.StorageAccountName2);
            FundsNeeded2.IsVisible = await ToggleFundButton(network, helper.StorageAccountName2);
            FundsNeeded2p.IsVisible = await ToggleFundButton(network, helper.StorageAccountName2);
        }

        public async void GetAccount3Info_Clicked(System.Object sender, System.EventArgs e)
        {
            await DisplayAccount(helper.StorageAccountName3);
            FundsNeeded3.IsVisible = await ToggleFundButton(network, helper.StorageAccountName3);
            FundsNeeded3p.IsVisible = await ToggleFundButton(network, helper.StorageAccountName3);
        }
        public async void CreateMultiSig_Clicked(System.Object sender, System.EventArgs e)
        {
            // restore accounts
            var accounts = await helper.RestoreAccounts();
            Account account1 = accounts[0];
            Account account2 = accounts[1];
            Account account3 = accounts[2];


            List<Ed25519PublicKeyParameters> publickeys = new List<Ed25519PublicKeyParameters>();

            publickeys.Add(account1.GetEd25519PublicKey());
            publickeys.Add(account2.GetEd25519PublicKey());
            publickeys.Add(account3.GetEd25519PublicKey());

            MultisigAddress msig = new MultisigAddress(1, 2, publickeys);
            ulong? balance = await helper.GetAccountBalance(helper.StorageMultisig);

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h3>" + "Multisig Address " + msig.ToString() + "</h3>" +
                "<h3>" + "Multisig balance = " + balance.ToString() + "</h3>" +
                "</body></html>";

            myWebView.Source = htmlSource;



            CreateMultiSig.Text = "Multisig created";
            CreateMultiSig.IsEnabled = false;
            GetMultiSig.IsVisible = true;

            myWebViewp.Source = htmlSource;



            CreateMultiSigp.Text = "Multisig created";
            CreateMultiSigp.IsEnabled = false;
            GetMultiSigp.IsVisible = true;


            await SecureStorage.SetAsync(helper.StorageMultisig, msig.ToString());
            buttonstate();

            //     OpenDispenser(helper, network, msig.ToString());


        }

        public async void GetMultiSig_Clicked(System.Object sender, System.EventArgs e)
        {
            var msig = await SecureStorage.GetAsync("Multisig");
            ulong? balance = await helper.GetAccountBalance(helper.StorageMultisig);
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h3>" + "Multisig Address " + msig.ToString() + "</h3>" +
                "<h3>" + "Multisig balance = " + balance.ToString() + "</h3>" +
                "</body></html>";

            myWebView.Source = htmlSource;


            FundsNeededMS.IsVisible = await ToggleFundButton(network, helper.StorageMultisig);
            myWebViewp.Source = htmlSource;


            FundsNeededMSp.IsVisible = await ToggleFundButton(network, helper.StorageMultisig);

        }

        public async void Transaction_Clicked(System.Object sender, System.EventArgs e)
        {
            // restore accounts

            var accounts = await helper.RestoreAccounts();
            Account account1 = accounts[0];
            Account account2 = accounts[1];
            Account account3 = accounts[2];
            HtmlWebViewSource htmlSource;
            // transfer from Account 1 to 2
            TransactionParams transParams = null;
            try
            {
                transParams = algodApiInstance.TransactionParams();
            }
            catch (ApiException err)
            {
                throw new Exception("Could not get params", err);
            }
            var amount = Utils.AlgosToMicroalgos(1);
            var tx = Utils.GetPaymentTransaction(account1.Address, account2.Address, amount, "pay message", transParams);           
            var signedTx = account1.SignTransaction(tx);
            Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);
            TransactionID id = null;
            string wait = "";
            // send the transaction to the network
            try
            {
                id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                wait = Utils.WaitTransactionToComplete(algodApiInstance, id.TxId);
                Console.WriteLine(wait);
            }
            catch (ApiException err)
            {
                // This should give us an informative error message.
                //   await SecureStorage.SetAsync("Transaction", err.Message);
                Console.WriteLine("Exception when calling algod#rawTransaction: " + err.Message);

                if (err.Message.Contains("overspend"))
                {
                    var network = await SecureStorage.GetAsync(helper.StorageNetwork);
                    htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = @"<html><body><h3>" + network + " Account has insuficent funds. " + "</h3>" +
                        "<h3>" + network + " add funds and try again. " + "</h3>" +
                        "<p>Account = " + account1.Address.ToString() + "</p>" +
                        "</body></html>";

                    myWebView.Source = htmlSource;
                    myWebViewp.Source = htmlSource;

                }
                htmlSource = new HtmlWebViewSource();
                htmlSource.Html = @"<html><body><h3> Transaction ID = " + err.Message + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
                myWebViewp.Source = htmlSource;
                //  Entry4.Text = "Transaction ID = " + err.Message;
            }

            if (!(String.IsNullOrEmpty(id.TxId)))
            {
                await SecureStorage.SetAsync(helper.StorageTransaction, id.TxId.ToString());
                GetTransaction.IsVisible = true;
            }


            buttonstate();

            await DisplayAccount(helper.StorageAccountName2);

            var mytx = await SecureStorage.GetAsync(helper.StorageTransaction);
            if (!(mytx == null || mytx == ""))

            {
                Algorand.Algod.Client.Model.Account accountinfo =
                await algodApiInstance.AccountInformationAsync(account2.Address.ToString());

                Debug.WriteLine("Account 2 info: " + accountinfo);
                htmlSource = new HtmlWebViewSource();

                htmlSource.Html = @"<html><body><h3> Transaction ID = " + mytx.ToString() + " </h3>" +
                    "<h3>" + "Account 2 info = " + accountinfo.ToJson() + "</h3>" +
                    "</body></html>";

                myWebView.Source = htmlSource;
                myWebViewp.Source = htmlSource;
            }

        }

        public async void GetTransaction_Clicked(System.Object sender, System.EventArgs e)
        {


            //  await DisplayAccount(helper.StorageAccountName2);
            var txid = await SecureStorage.GetAsync(helper.StorageTransaction);
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h3> Transaction ID = " + txid.ToString() + " </h3>" +
                "</body></html>";

            myWebView.Source = htmlSource;
            myWebViewp.Source = htmlSource;



        }

        public async void GetMultiSigTx_Clicked(System.Object sender, System.EventArgs e)
        {
            //   await DisplayAccount(helper.StorageAccountName3);
            var txid = await SecureStorage.GetAsync(helper.StorageMultisigTransaction);


            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h3> Multisig Transaction ID = " +
                txid.ToString() + " </h3>" +

                "</body></html>";

            myWebView.Source = htmlSource;


            myWebViewp.Source = htmlSource;
        }

        public async void MultisigTransaction_Clicked(System.Object sender, System.EventArgs e)
        {
            //MultisigTransaction

            // List for Pks for multisig account
            List<Ed25519PublicKeyParameters> publicKeys = new List<Ed25519PublicKeyParameters>();

            // restore accounts
            var accounts = await helper.RestoreAccounts();
            Account account1 = accounts[0];
            Account account2 = accounts[1];
            Account account3 = accounts[2];


            publicKeys.Add(account1.GetEd25519PublicKey());
            publicKeys.Add(account2.GetEd25519PublicKey());
            publicKeys.Add(account3.GetEd25519PublicKey());

            // Instantiate the the Multisig Accout

            MultisigAddress msig = new MultisigAddress(1, 2, publicKeys);
            Console.WriteLine("Multisignature Address: " + msig.ToString());
            //   dispense funds to msig account
            string DEST_ADDR = account3.Address.ToString();
            // add some notes to the transaction

            // todo notes
            byte[] notes = Encoding.UTF8.GetBytes("These are some notes encoded in some way!");//.getBytes();

            var amount = Utils.AlgosToMicroalgos(1);
            Transaction tx = null;
            //noteb64 = notes
            try
            {
                tx = Utils.GetPaymentTransaction(new Address(msig.ToString()), new Address(DEST_ADDR), amount, "this is a multisig trans",
                    algodApiInstance.TransactionParams());
            }
            catch (Exception err)
            {
                Console.WriteLine("Could not get params", err.Message);
            }
            // Sign the Transaction for two accounts
            SignedTransaction signedTx = account1.SignMultisigTransaction(msig, tx);
            SignedTransaction completeTx = account2.AppendMultisigTransaction(msig, signedTx);

            // send the transaction to the network
            TransactionID id = null;
            try
            {
                id = Utils.SubmitTransaction(algodApiInstance, completeTx);
                Console.WriteLine("Successfully sent tx with id: " + id);
                var x = Utils.WaitTransactionToComplete(algodApiInstance, id.TxId);
                Console.WriteLine(x);
            }
            catch (ApiException err)
            {

                Console.WriteLine("Exception when calling algod#rawTransaction: " + err.Message);
            }
            await SecureStorage.SetAsync(helper.StorageMultisigTransaction, id.TxId.ToString());
            MultisigTransaction.Text = "Transaction successfully sent";
            GetMultiSigTx.IsVisible = true;

            MultisigTransactionp.Text = "Transaction successfully sent";
            GetMultiSigTxp.IsVisible = true;

            //ulong? balance = await helper.GetAccountBalance(helper.StorageMultisig);
            //var htmlSource = new HtmlWebViewSource();
            //htmlSource.Html = @"<html><body><h3> Multisig balance = " + balance.ToString() + " </h3>" +
            //      "</body></html>";

            //myWebView.Source = htmlSource;





            buttonstate();

            await DisplayAccount(helper.StorageAccountName3);
            //var mytx = await SecureStorage.GetAsync(helper.StorageMultisigTransaction);

            //if (!(mytx == null || mytx == ""))

            //{
            //    htmlSource = new HtmlWebViewSource();
            //    htmlSource.Html = @"<html><body><h3> Transaction ID = " + mytx.ToString() + " </h3>" +
            //          "</body></html>";

            //    myWebView.Source = htmlSource;


            //}

        }

        async void FundsNeeded1_click(System.Object sender, System.EventArgs e)
        {

            await PromptToAddFunds(network, helper.StorageAccountName1);
            buttonstate();
            FundsNeeded1.IsVisible = false;
            FundsNeeded1p.IsVisible = false;
        }

        private async Task PromptToAddFunds(string network, string accountname)
        {
            if (!(String.IsNullOrEmpty(accountname)))
            {
                ulong? amount = await helper.GetAccountBalance(accountname);
                if (amount < helper.MIN_ACCOUNT_BALANCE)
                {
                    // diplay Account1 on network is belown min balance.
                    // would you like to dispense fund to it?

                    string action = await DisplayActionSheet(accountname + " on " + network + " is below min balance would you like to dispense funds to it?", "Cancel", null, "Yes", "No");
                    Debug.WriteLine("Action: " + action);
                    string myaddress = "";
                    if (action == "Yes")
                    {
                        if (accountname != helper.StorageMultisig)
                        {
                            var mnemonic = await SecureStorage.GetAsync(accountname);
                            Account myaccount = new Account(mnemonic);
                            myaddress = myaccount.Address.ToString();
                            await Clipboard.SetTextAsync(myaddress);
                        }
                        else
                        {
                            // get multisig addr
                            myaddress = await SecureStorage.GetAsync(accountname);
                            await Clipboard.SetTextAsync(myaddress);
                        }

                        OpenDispenser(helper, network, myaddress);

                    }
                }
            }
        }

        async void FundsNeeded2_click(System.Object sender, System.EventArgs e)
        {
            await PromptToAddFunds(network, helper.StorageAccountName2);
            buttonstate();
            FundsNeeded2.IsVisible = false;
            FundsNeeded2p.IsVisible = false;
        }

        async void FundsNeeded3_click(System.Object sender, System.EventArgs e)
        {
            await PromptToAddFunds(network, helper.StorageAccountName3);
            buttonstate();
            FundsNeeded3.IsVisible = false;
            FundsNeeded3p.IsVisible = false;
        }

        async void FundsNeededMS_click(System.Object sender, System.EventArgs e)
        {
            await PromptToAddFunds(network, helper.StorageMultisig);
            FundsNeededMS.IsVisible = false;
            FundsNeededMSp.IsVisible = false;
            buttonstate();
            FundsNeededMS.IsVisible = false;
            FundsNeededMSp.IsVisible = false;
        }
    }
}
