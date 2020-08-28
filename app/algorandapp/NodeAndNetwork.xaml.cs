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

    public partial class NodeAndNetwork : ContentPage
    {
        // Purestake
        //public const string ALGOD_API_TOKEN_BETANET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";
        //public const string ALGOD_API_ADDR_BETANET = "https://betanet-algorand.api.purestake.io/ps1";
        //public const string ALGOD_API_ADDR_TESTNET = "https://testnet-algorand.api.purestake.io/ps1";
        //public const string ALGOD_API_TOKEN_TESTNET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";

        // Standalone instance
        //public string ALGOD_API_TOKEN_BETANET = "050e81d219d12a0888dafddaeafb5ff8d181bf1256d1c749345995678b16902f";
        //public string ALGOD_API_ADDR_BETANET = "http://betanet-hackathon.algodev.network:8180";
        //public string ALGOD_API_TOKEN_TESTNET = "ef920e2e7e002953f4b29a8af720efe8e4ecc75ff102b165e0472834b25832c1";
        //public string ALGOD_API_ADDR_TESTNET = "http://hackathon.algodev.network:9100";
        public string ALGOD_API_TOKEN_BETANET = "";
        public string ALGOD_API_ADDR_BETANET = "";
        public string ALGOD_API_TOKEN_TESTNET = "";
        public string ALGOD_API_ADDR_TESTNET = "";
        //   default to TESTNET
        public static helper helper = new helper();
        public AlgodApi algodApiInstance = null;

        public int sizeoftoken;
        public int sizeofaddress;
        public string network = "";
        public string nodetype = "";
        
        public NodeAndNetwork()
        {
            InitializeComponent();
            Appearing += NodeAndNetwork_Appearing;
            EntryBetaNetServer.TextChanged += EntryBetaNetServer_TextChanged;
            EntryTestNetServer.TextChanged += EntryTestNetServer_TextChanged;
            EntryBetaNetToken.TextChanged += EntryBetaNetToken_TextChanged;
            EntryTestNetToken.TextChanged += EntryTestNetToken_TextChanged;
            // temp fix for radio button bug not displaying Text on radio button
            if (Device.RuntimePlatform == Device.Android )
            {
                TestNetPurestakeLabel.Text = "Purestake TestNet";
                TestNetHackathonLabel.Text = "Hackathon TestNet";
                TestNetLabel.Text = "Enter your TestNet node";
                PurestakeTestNet.Text = "";
                HackathonTestNet.Text = "";
                myTestNet.Text = "";

                BetaNetPurestakeLabel.Text = "Purestake BetaNet";
                BetaNetHackathonLabel.Text = "Hackathon BetaNet";
                BetaNetLabel.Text = "Enter your BetaNet node";
                PurestakeBetaNet.Text = "";
                HackathonBetaNet.Text = "";
                myBetaNet.Text = "";

            }
            else
            {
                TestNetPurestakeLabel.Text = "";
                TestNetHackathonLabel.Text = "";
                TestNetLabel.Text = "";
                PurestakeTestNet.Text = "Purestake TestNet";
                HackathonTestNet.Text = "Hackathon TestNet";
                myTestNet.Text = "Enter your TestNet node";

                BetaNetPurestakeLabel.Text = "";
                BetaNetHackathonLabel.Text = "";
                BetaNetLabel.Text = "";
                PurestakeBetaNet.Text = "Purestake BetaNet";
                HackathonBetaNet.Text = "Hackathon BetaNet";
                myBetaNet.Text = "Enter your BetaNet node";
            }

            //HackathonTestNet.TextColor = Color.Blue;
            //myTestNet.TextColor = Color.Blue;
            //ios
            //    PurestakeLabel.IsVisible = false;

        }

        private void EntryTestNetToken_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (EntryTestNetToken.Text != null)
                sizeoftoken = EntryTestNetToken.Text.Length;
            if (EntryTestNetServer.Text != null)
                sizeofaddress = EntryTestNetServer.Text.Length;

            checkifTestNetentered();
        }

        private void checkifentered()
        {
            if ((sizeoftoken > 0) && (sizeofaddress > 0))
            {
                SaveBetaNet.IsEnabled = true;
            }
            else
            {
                SaveBetaNet.IsEnabled = false;
            }
        }
        private void checkifTestNetentered()
        {
            if ((sizeoftoken > 0) && (sizeofaddress > 0))
            {
                SaveTestNet.IsEnabled = true;
            }
            else
            {
                SaveTestNet.IsEnabled = false;
            }
        }
        private void EntryBetaNetToken_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EntryBetaNetToken.Text!=null) 
            sizeoftoken = EntryBetaNetToken.Text.Length;
            if (EntryBetaNetServer.Text!=null)
            sizeofaddress = EntryBetaNetServer.Text.Length;
            checkifentered();
        }

        private void EntryTestNetServer_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (EntryTestNetToken.Text != null)
                sizeoftoken = EntryTestNetToken.Text.Length;
            if (EntryTestNetServer.Text != null)
                sizeofaddress = EntryTestNetServer.Text.Length;


            checkifTestNetentered();
        }

        private void EntryBetaNetServer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EntryBetaNetToken.Text != null)
                sizeoftoken = EntryBetaNetToken.Text.Length;
            if (EntryBetaNetServer.Text != null)
                sizeofaddress = EntryBetaNetServer.Text.Length;

            checkifentered();
        }

        public async void NodeAndNetwork_Appearing(object sender, EventArgs e)
        {
            // algodApiInstance = new AlgodApi(ALGOD_API_ADDR_TESTNET, ALGOD_API_TOKEN_TESTNET);

            ALGOD_API_TOKEN_BETANET = await SecureStorage.GetAsync(helper.StorageALGOD_API_TOKEN_BETANET);
            ALGOD_API_TOKEN_TESTNET =  await SecureStorage.GetAsync(helper.StorageALGOD_API_TOKEN_TESTNET);
            ALGOD_API_ADDR_TESTNET = await SecureStorage.GetAsync(helper.StorageALGOD_API_ADDR_TESTNET);
            ALGOD_API_ADDR_BETANET = await SecureStorage.GetAsync(helper.StorageALGOD_API_ADDR_BETANET);

            var savedtest = await SecureStorage.GetAsync(helper.StorageSavedTestNetwork);

            var savedbeta = await SecureStorage.GetAsync(helper.StorageSavedBetaNetwork);
            // todo if empty set to sandbox defaults

            if (savedtest == "true")
            {
                EntryTestNetToken.Text = await SecureStorage.GetAsync(helper.StorageTestNetToken);
                EntryTestNetServer.Text = await SecureStorage.GetAsync(helper.StorageTestNetAddress);

            }
            else
            {
                EntryTestNetToken.Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                EntryTestNetServer.Text = "http://[your ip address]:4001";

            }


            if (savedbeta == "true")
            {
                EntryBetaNetToken.Text = await SecureStorage.GetAsync(helper.StorageBetaNetToken);
                EntryBetaNetServer.Text = await SecureStorage.GetAsync(helper.StorageBetaNetAddress);
            }
            else
            {
                EntryBetaNetToken.Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                EntryBetaNetServer.Text = "http://[your ip address]:4001";
            }


            network = await helper.GetNetwork();


            buttonstate();
        }
        public async void buttonstate()
        {
           // var network = await SecureStorage.GetAsync(helper.StorageNetwork);
            nodetype = await SecureStorage.GetAsync(helper.StorageNodeType);

            if (network == helper.StorageTestNet)
            {
                if (nodetype == helper.StoragePurestake)
                {
                    PurestakeTestNet.IsChecked = true;
                    await PurestakeTestNetClicked();
                }
                else
                     if (nodetype == helper.StorageHackathon)
                {
                    HackathonTestNet.IsChecked = true;
                    await HackathonTestNetClicked();
                }
                else
                { 
                if (nodetype == helper.StoragemyNode)
                {
                    myTestNet.IsChecked = true;
                    myTestNetClicked();
                }
            }
            }
            else if (network == helper.StorageBetaNet)
            {
                if (nodetype == helper.StoragePurestake)
                {
                    PurestakeBetaNet.IsChecked = true;
                    await PurestakeBetaNetClicked();
                }
                else
                     if (nodetype == helper.StorageHackathon)
                {
                    HackathonBetaNet.IsChecked = true;
                    await HackathonTestNetClicked();
                }
                else
                {
                    if (nodetype == helper.StoragemyNode)
                    {
                        myBetaNet.IsChecked = true;
                        myBetaNetClicked();
                    }

                }

            }

            if (String.IsNullOrEmpty(network))
            {
                //default to TestNet
                await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageTestNet);
                network = helper.StorageTestNet;
            }
            if (PurestakeTestNet.IsChecked || PurestakeBetaNet.IsChecked
                || HackathonTestNet.IsChecked || HackathonBetaNet.IsChecked
                || myTestNet.IsChecked || myBetaNet.IsChecked)
            { }
            else
            {
                //default to Purstake
                if (network == helper.StorageTestNet)
                {
                    PurestakeTestNet.IsChecked = true;
                    await PurestakeTestNetClicked();
                }
                else
                {
                    PurestakeBetaNet.IsChecked = true;
                    await PurestakeBetaNetClicked();
                }

            }
            EnableNetworkToggles(network);
        }

        public async void EnableNetworkToggles(string network)
        {


            if (network == helper.StorageTestNet)
            {
                BetaNetToggle.IsToggled = false;
                TestNetToggle.IsToggled = true; 
                BetaNetToggle.IsEnabled = true;
                TestNetToggle.IsEnabled = true;
                BetaNetStack.IsVisible = false;
                TestNetStack.IsVisible = true;

                var savedtest = await SecureStorage.GetAsync(helper.StorageSavedTestNetwork);
                if ((!String.IsNullOrEmpty(savedtest)) && (savedtest == "true"))
                {
                    EntryTestNetToken.Text = await SecureStorage.GetAsync(helper.StorageTestNetToken);
                    EntryTestNetServer.Text = await SecureStorage.GetAsync(helper.StorageTestNetAddress);
                }
            }
            else
            {
                BetaNetToggle.IsToggled = true;
                TestNetToggle.IsToggled = false;
                BetaNetToggle.IsEnabled = true;
                TestNetToggle.IsEnabled = true;
                BetaNetStack.IsVisible = true;
                TestNetStack.IsVisible = false;
                var savedbeta = await SecureStorage.GetAsync(helper.StorageSavedBetaNetwork);
                if ((!String.IsNullOrEmpty(savedbeta)) && (savedbeta == "true"))
                {
                    EntryBetaNetToken.Text = await SecureStorage.GetAsync(helper.StorageBetaNetToken);
                    EntryBetaNetServer.Text = await SecureStorage.GetAsync(helper.StorageBetaNetAddress);
                }

            }
        }



        public async void Switch_Toggled_TestNet(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (TestNetToggle.IsToggled)
            {
                await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageTestNet);
                BetaNetToggle.IsToggled = false;
                BetaNetStack.IsVisible = false;
                TestNetStack.IsVisible = true;
                if (helper.StoragemyNode == await SecureStorage.GetAsync(helper.StorageNodeType))
                {
                    myTestNet.IsChecked = true;
                    myTestNetClicked();
                    TestNetEntry.IsVisible = true;

                }
                if (helper.StoragePurestake == await SecureStorage.GetAsync(helper.StorageNodeType))
                {
                    PurestakeTestNet.IsChecked = true;
                    await PurestakeTestNetClicked();
                    TestNetEntry.IsVisible = false;
                }
                if (helper.StorageHackathon == await SecureStorage.GetAsync(helper.StorageNodeType))
                {
                    HackathonTestNet.IsChecked = true;
                    await HackathonTestNetClicked();
                    TestNetEntry.IsVisible = false;
                }


                //   algodApiInstance = new AlgodApi(ALGOD_API_ADDR_TESTNET, ALGOD_API_TOKEN_TESTNET);
            }
            else
            {
                await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageBetaNet);
                BetaNetToggle.IsToggled = true;
                BetaNetStack.IsVisible = true;
                TestNetStack.IsVisible = false;



            }


        }

        public async void Switch_Toggled_BetaNet(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (BetaNetToggle.IsToggled)
            {
                await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageBetaNet);
                TestNetToggle.IsToggled = false;

                if (helper.StoragemyNode == await SecureStorage.GetAsync(helper.StorageNodeType))
                {
                    myBetaNet.IsChecked = true;
                    myBetaNetClicked();
                    BetaNetEntry.IsVisible = true;
                }
                if (helper.StoragePurestake == await SecureStorage.GetAsync(helper.StorageNodeType))
                {
                    PurestakeBetaNet.IsChecked = true;
                    await PurestakeBetaNetClicked();
                    BetaNetEntry.IsVisible = false ;
                }
                if (helper.StorageHackathon == await SecureStorage.GetAsync(helper.StorageNodeType))
                {
                    HackathonBetaNet.IsChecked = true;
                    await HackathonBetaNetClicked();
                    BetaNetEntry.IsVisible = false;
                }


                //  algodApiInstance = new AlgodApi(ALGOD_API_ADDR_BETANET, ALGOD_API_TOKEN_BETANET);

            }
            else
            {
                await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageTestNet);
                TestNetToggle.IsToggled = true;


            }



        }

        public async void PurestakeBetaNet_click(System.Object sender, System.EventArgs e)
        {
            // Purestake BetaNet
            // mine
            await PurestakeBetaNetClicked();

        }

        private async Task PurestakeBetaNetClicked()
        {
         
            // Purestake 

            ALGOD_API_TOKEN_BETANET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";
            ALGOD_API_ADDR_BETANET = "https://betanet-algorand.api.purestake.io/ps1";

            await SecureStorage.SetAsync(helper.StorageALGOD_API_TOKEN_BETANET, ALGOD_API_TOKEN_BETANET);
            await SecureStorage.SetAsync(helper.StorageALGOD_API_ADDR_BETANET, ALGOD_API_ADDR_BETANET);
            myLabel.Text = "Purestake BetaNet set";
            await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageBetaNet);
            await SecureStorage.SetAsync(helper.StorageNodeType, helper.StoragePurestake);
            BetaNetEntry.IsVisible = false;
        }

        public async void PurestakeTestNet_click(System.Object sender, System.EventArgs e)
        {
            await PurestakeTestNetClicked();

        }

        private async Task PurestakeTestNetClicked()
        {
            // Purestake TestNet
            ALGOD_API_ADDR_TESTNET = "https://testnet-algorand.api.purestake.io/ps1";
            ALGOD_API_TOKEN_TESTNET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";

            await SecureStorage.SetAsync(helper.StorageALGOD_API_TOKEN_TESTNET, ALGOD_API_TOKEN_TESTNET);
            await SecureStorage.SetAsync(helper.StorageALGOD_API_ADDR_TESTNET, ALGOD_API_ADDR_TESTNET);
            await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageTestNet);
            myLabel.Text = "Purestake TestNet set";
            await SecureStorage.SetAsync(helper.StorageNodeType, helper.StoragePurestake);
            TestNetEntry.IsVisible = false;

        }


        public async void HackathonBetaNet_click(System.Object sender, System.EventArgs e)
        {
            await HackathonBetaNetClicked();
        }

        private async Task HackathonBetaNetClicked()
        {

            // Standalone instance
            ALGOD_API_TOKEN_BETANET = "050e81d219d12a0888dafddaeafb5ff8d181bf1256d1c749345995678b16902f";
            ALGOD_API_ADDR_BETANET = "http://betanet-hackathon.algodev.network:8180";
            await SecureStorage.SetAsync(helper.StorageALGOD_API_TOKEN_BETANET, ALGOD_API_TOKEN_BETANET);
            await SecureStorage.SetAsync(helper.StorageALGOD_API_ADDR_BETANET, ALGOD_API_ADDR_BETANET);
            await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageBetaNet);
            myLabel.Text = "Hackathon BetaNet set";
            await SecureStorage.SetAsync(helper.StorageNodeType, helper.StorageHackathon);
            BetaNetEntry.IsVisible = false;
        }

        public async void HackathonTestNet_click(System.Object sender, System.EventArgs e)
        {
            await HackathonTestNetClicked();
        }

        private async Task HackathonTestNetClicked()
        {
            ALGOD_API_TOKEN_TESTNET = "ef920e2e7e002953f4b29a8af720efe8e4ecc75ff102b165e0472834b25832c1";
            ALGOD_API_ADDR_TESTNET = "http://hackathon.algodev.network:9100";

            await SecureStorage.SetAsync(helper.StorageALGOD_API_TOKEN_TESTNET, ALGOD_API_TOKEN_TESTNET);
            await SecureStorage.SetAsync(helper.StorageALGOD_API_ADDR_TESTNET, ALGOD_API_ADDR_TESTNET);
            await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageTestNet);
            myLabel.Text = "Hackathon TestNet set";
            await SecureStorage.SetAsync(helper.StorageNodeType, helper.StorageHackathon);
            TestNetEntry.IsVisible = false;

        }

        void myBetaNet_click(System.Object sender, System.EventArgs e)
        {
            myBetaNetClicked();

        }

        private async void myBetaNetClicked()
        {
            TestNetEntry.IsVisible = false;
            BetaNetEntry.IsVisible = true;
            myLabel.Text = "";
            await SecureStorage.SetAsync(helper.StorageNodeType, helper.StoragemyNode);

        }

        void myTestNet_click(System.Object sender, System.EventArgs e)
        {
            myTestNetClicked();
        }

        private async void myTestNetClicked()
        {
            TestNetEntry.IsVisible = true;
            BetaNetEntry.IsVisible = false;
            myLabel.Text = "";
            await SecureStorage.SetAsync(helper.StorageNodeType, helper.StoragemyNode);
        }

        public async void SaveBetaNetwork_click(System.Object sender, System.EventArgs e)
        {
            await SecureStorage.SetAsync(helper.StorageSavedBetaNetwork, "false");
            if ((sizeoftoken > 0) && (sizeofaddress > 0))
            {
                ALGOD_API_TOKEN_BETANET = EntryBetaNetToken.Text;
                ALGOD_API_ADDR_BETANET = EntryBetaNetServer.Text;

                //store off values for myNode - to populate back if node changed to Purestake or Hackathon

                await SecureStorage.SetAsync(helper.StorageBetaNetToken, ALGOD_API_TOKEN_BETANET);
                await SecureStorage.SetAsync(helper.StorageBetaNetAddress, ALGOD_API_ADDR_BETANET);
                // store off current values of myNode
                await SecureStorage.SetAsync(helper.StorageALGOD_API_TOKEN_BETANET, ALGOD_API_TOKEN_BETANET);
                await SecureStorage.SetAsync(helper.StorageALGOD_API_ADDR_BETANET, ALGOD_API_ADDR_BETANET);

                await SecureStorage.SetAsync(helper.StorageSavedBetaNetwork, "true");
                await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageBetaNet);
                myLabel.Text = "Saved BetaNet Token and Server Address";
            }
            else
            {
                myLabel.Text = "Invalid Token or Address";

            }
        }

        public async void SaveTestNetwork_click(System.Object sender, System.EventArgs e)
        {
            await SecureStorage.SetAsync(helper.StorageSavedTestNetwork, "false");
            if ((sizeoftoken > 0) && ( sizeofaddress > 0))
            {
                ALGOD_API_TOKEN_TESTNET = EntryTestNetToken.Text;
                ALGOD_API_ADDR_TESTNET = EntryTestNetServer.Text;

                //store off values for myNode - to populate back if node changed to Purestake or Hackathon
                await SecureStorage.SetAsync(helper.StorageTestNetToken, ALGOD_API_TOKEN_TESTNET);
                await SecureStorage.SetAsync(helper.StorageTestNetAddress, ALGOD_API_ADDR_TESTNET);
                // store off current values of myNode
                await SecureStorage.SetAsync(helper.StorageALGOD_API_TOKEN_TESTNET, ALGOD_API_TOKEN_TESTNET);
                await SecureStorage.SetAsync(helper.StorageALGOD_API_ADDR_TESTNET, ALGOD_API_ADDR_TESTNET);
                await SecureStorage.SetAsync(helper.StorageNetwork, helper.StorageTestNet);
                await SecureStorage.SetAsync(helper.StorageSavedTestNetwork, "true");

                myLabel.Text = "Saved TestNet Token and Server Address";
            }
        else
            {

            myLabel.Text = "Invalid Token or Address";
            }


        }
    }

}
