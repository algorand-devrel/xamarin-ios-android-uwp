﻿using Algorand;
using System;
using Algorand.V2;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Account = Algorand.Account;
using Algorand.V2.Model;

namespace algorandapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Rekey : ContentPage
    {
        static helper helper = new helper();
        AlgodApi client;
        Account account1;
        Account account2;
        Account account3;

        public Rekey()
        {
            //  to be completed
            InitializeComponent();
            Appearing += Rekey_Appearing;

        }

        private async void Rekey_Appearing(object sender, EventArgs e)
        {
            client = await helper.CreateApiInstance();
           // network = await helper.GetNetwork();

            // restore accounts
            var accounts = await helper.RestoreAccounts();
            account1 = accounts[0];
            account2 = accounts[1];
            account3 = accounts[2];

            Console.WriteLine("Account 1 = " + account1.Address);
            Console.WriteLine("Account 2 = " + account2.Address);
            Console.WriteLine("Account 3 = " + account3.Address);

        }

 

        void Optin_Clicked(System.Object sender, System.EventArgs e)
        {
            var trans = client.TransactionParams();
            Console.WriteLine("Lastround: " + trans.LastRound.ToString());

            ulong? amount = 0;
            //opt-in send tx to same address as sender and use 0 for amount w rekey account to account 1
            var tx = Utils.GetPaymentTransaction(account3.Address, account3.Address, amount, "pay message", trans);
            tx.RekeyTo = account1.Address;

            var signedTx = account3.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            PostTransactionsResponse id;
            try
            {
                id = Utils.SubmitTransaction(client, signedTx);
                Console.WriteLine("Transaction ID: " + id);
                //waitForTransactionToComplete(algodApiInstance, signedTx.transactionID);
                //Console.ReadKey();
                Console.WriteLine("Confirmed Round is: " +
                    Utils.WaitTransactionToComplete(client, id.TxId).ConfirmedRound);
                DisplayInfo("Account 3 opts in to have Account 1 sign - Confirmed Round is: " +
                     Utils.WaitTransactionToComplete(client, id.TxId).ConfirmedRound);
                OptinBtn.IsEnabled = false;
                AccountSign.IsEnabled = true;
                ResetBack.IsEnabled = true;
            }
            catch (Exception err)
            {
                //e.printStackTrace();
                Console.WriteLine(err.Message);
                DisplayInfo("Error: " + err.Message);

                return;
            }

        }

        void AccountSign_Clicked(System.Object sender, System.EventArgs e)
        {
            var trans = client.TransactionParams();
            Console.WriteLine("Lastround: " + trans.LastRound.ToString());

            var act = client.AccountInformation(account3.Address.ToString());
            Console.WriteLine(act);

            ulong? amount2 = 1000000;
            var tx2 = Utils.GetPaymentTransaction(account3.Address, account2.Address, amount2, "pay message", trans);
            tx2.RekeyTo = account1.Address;
            var signedTx2 = account1.SignTransaction(tx2);
            try
            {
                var id = Utils.SubmitTransaction(client, signedTx2);
                Console.WriteLine("Transaction ID: " + id);
                Console.WriteLine("Confirmed Round is: " +
                    Utils.WaitTransactionToComplete(client, id.TxId).ConfirmedRound);
                DisplayInfo("Account 3 send to Account 2, rekeyed to sign by  Account 1 - Confirmed Round is: " +
                     Utils.WaitTransactionToComplete(client, id.TxId).ConfirmedRound);
                OptinBtn.IsEnabled = false;
                ResetBack.IsEnabled = true;

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                DisplayInfo("Error: " + err.Message);
                return;
            }

        }

        void DisplayInfo(string text)
        {
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"<html><body><h2>" +
                 text + "</h2></body></html>";
            myWebView.Source = htmlSource;
        }

        void ResetBack_Clicked(System.Object sender, System.EventArgs e)
        {
            var trans = client.TransactionParams();
            Console.WriteLine("Lastround: " + trans.LastRound.ToString());

            ulong? amount = 0;
            //opt-in send tx to same address as sender and use 0 for amount w rekey back account to account 3
            var tx = Utils.GetPaymentTransaction(account3.Address, account3.Address, amount, "pay message", trans);
            tx.RekeyTo = account3.Address;

            var signedTx = account1.SignTransaction(tx);
            // send the transaction to the network and
            // wait for the transaction to be confirmed
            try
            {
                var id = Utils.SubmitTransaction(client, signedTx);
                Console.WriteLine("Transaction ID: " + id);
                //waitForTransactionToComplete(algodApiInstance, signedTx.transactionID);
                //Console.ReadKey();
                Console.WriteLine("Confirmed Round is: " +
                    Utils.WaitTransactionToComplete(client, id.TxId).ConfirmedRound);
                DisplayInfo("Account 3 opts in to have Account 3 sign - signed by Account 1 Confirmed Round is: " +
                     Utils.WaitTransactionToComplete(client, id.TxId).ConfirmedRound);
                ResetBack.IsEnabled = false;
                AccountSign.IsEnabled = false;
                OptinBtn.IsEnabled = true;
            }
            catch (Exception err)
            {
                //e.printStackTrace();
                Console.WriteLine(err.Message);
                DisplayInfo("Error: " + err.Message);
                return;
            }
        }
    }
}