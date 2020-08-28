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

namespace algorandapp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public Account myAccount;
        public static helper helper = new helper();
        public string network;
        public MainPage()
        {
            InitializeComponent();
            Appearing += MainPage_Appearing;
        }

        private async void MainPage_Appearing(object sender, EventArgs e)
        {
            network = await helper.GetNetwork();
            var nodetype = await SecureStorage.GetAsync(helper.StorageNodeType);
            if (network == null)
                network = "TestNet";
            NetworkLabel.Text = "Network: " + network + " " + nodetype;
            var lastHomebutton = await SecureStorage.GetAsync(helper.StorageLastHomeButton);
            if (string.IsNullOrEmpty(lastHomebutton))
            {
                buttonstate("init");
            }
            else
            {
                buttonstate(lastHomebutton);
            }
        }
        public async void buttonstate(string buttonclicked)
        {
            NetworkLabel.Text = "Network: " + network;
            var account1 = await SecureStorage.GetAsync(helper.StorageAccountName1);
            var account2 = await SecureStorage.GetAsync(helper.StorageAccountName2);
            var account3 = await SecureStorage.GetAsync(helper.StorageAccountName3);
            if ((string.IsNullOrEmpty(account1) || string.IsNullOrEmpty(account2)) || string.IsNullOrEmpty(account3))
            {
                StackGenerateAccount.IsEnabled = true;
                GenerateAccount.Opacity = 1;

                StackASA.IsEnabled = false;
                ASA.Opacity = .4;

                StackASC1.IsEnabled = false;
                ASC1.Opacity = .4;

                StackAtomicTransfers.IsEnabled = false;
                AtomicTransfers.Opacity = .4;
            }
            else
            {
                StackGenerateAccount.IsEnabled = true;
                GenerateAccount.Opacity = 1;

                StackASA.IsEnabled = true;
                ASA.Opacity = 1;

                StackASC1.IsEnabled = true;
                ASC1.Opacity = 1;

                StackAtomicTransfers.IsEnabled = true;
                AtomicTransfers.Opacity = 1;
            }



        }
        public async void NodeNetwork_click(System.Object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new NodeAndNetwork());
        }

        public async void GenerateAccount_click(System.Object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new Accounts());
        }

        public async void ASA_click(System.Object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new ASA());
        }
        public async void AtomicTransfers_click(System.Object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new AtomicTransfers());
        }


        async void ASC1_Clicked(System.Object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new ASC());
        }

        async void Reset_Clicked(System.Object sender, System.EventArgs e)
        {
            string ALGOD_API_ADDR_TESTNET = "https://testnet-algorand.api.purestake.io/ps1";
     
            // purestake hackathon
            string ALGOD_API_TOKEN_TESTNET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";

            await SecureStorage.SetAsync(helper.StorageALGOD_API_TOKEN_TESTNET, ALGOD_API_TOKEN_TESTNET);
            await SecureStorage.SetAsync(helper.StorageALGOD_API_ADDR_TESTNET, ALGOD_API_ADDR_TESTNET);

            await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageTestNet);
    
            await SecureStorage.SetAsync(helper.StorageNodeType, helper.StoragePurestake);

            await SecureStorage.SetAsync(helper.StorageAssetIDName, "");
            await SecureStorage.SetAsync(helper.StorageLastASAButton, "");

            await SecureStorage.SetAsync(helper.StorageAccountName1, "");
            await SecureStorage.SetAsync(helper.StorageAccountName2, "");
            await SecureStorage.SetAsync(helper.StorageAccountName3, "");
            await SecureStorage.SetAsync(helper.StorageMultisig, "");
            await SecureStorage.SetAsync(helper.StorageTransaction, "");
            await SecureStorage.SetAsync(helper.StorageAtomicTransaction, "");
            await SecureStorage.SetAsync(helper.StorageMultisigTransaction, "");
            await SecureStorage.SetAsync(helper.StorageLastHomeButton, "");
            await SecureStorage.SetAsync(helper.StorageSavedBetaNetwork, "");
            await SecureStorage.SetAsync(helper.StorageSavedTestNetwork, "");
            await SecureStorage.SetAsync(helper.StorageTestNetToken, "");
            await SecureStorage.SetAsync(helper.StorageTestNetAddress, "");
            await SecureStorage.SetAsync(helper.StorageBetaNetToken, "");
            await SecureStorage.SetAsync(helper.StorageBetaNetAddress, "");
            ASA.Opacity = .4;
            StackASA.IsEnabled = false;
            AtomicTransfers.Opacity = .4;
            StackAtomicTransfers.IsEnabled = false;
            ASC1.Opacity = .4;
            StackASC1.IsEnabled = false;




        }
    }


}

