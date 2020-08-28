using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Account = Algorand.Account;
using System.Threading.Tasks;
using System.Diagnostics;
using Algorand.Algod.Client.Api;


// Purestake 
//public const string ALGOD_API_TOKEN_BETANET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";
//public const string ALGOD_API_ADDR_BETANET = "https://betanet-algorand.api.purestake.io/ps1";
//public const string ALGOD_API_ADDR_TESTNET = "https://testnet-algorand.api.purestake.io/ps1";
//public const string ALGOD_API_TOKEN_TESTNET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";

// Standalone hackathon instance
//public string ALGOD_API_TOKEN_BETANET = "050e81d219d12a0888dafddaeafb5ff8d181bf1256d1c749345995678b16902f";
//public string ALGOD_API_ADDR_BETANET = "http://betanet-hackathon.algodev.network:8180";
//public string ALGOD_API_TOKEN_TESTNET = "ef920e2e7e002953f4b29a8af720efe8e4ecc75ff102b165e0472834b25832c1";
//public string ALGOD_API_ADDR_TESTNET = "http://hackathon.algodev.network:9100";




namespace algorandapp
{
    public class helper
    {
        public ulong? MIN_ACCOUNT_BALANCE = (ulong?)100000;
        public string StorageAccountName1 = "Account 1";
        public string StorageAccountName2 = "Account 2";
        public string StorageAccountName3 = "Account 3";
        public string StorageALGOD_API_TOKEN_BETANET = "ALGOD_API_TOKEN_BETANET";
        public string StorageALGOD_API_ADDR_BETANET = "ALGOD_API_ADDR_BETANET";
        public string StorageALGOD_API_TOKEN_TESTNET = "ALGOD_API_TOKEN_TESTNET";
        public string StorageALGOD_API_ADDR_TESTNET = "ALGOD_API_ADDR_TESTNET";
        public string StorageNetwork = "Network";
        public string StorageMultisig = "Multisig";
        public string StorageTransaction = "Transaction";
        public string StorageAtomicTransaction = "AtomicTransaction";
        public string StorageMultisigTransaction = "MultisigTransaction";
        public string StorageAssetIDName = "AssetID";
        public string StorageTestNet = "TestNet";
        public string StorageBetaNet = "BetaNet";

        public string StoragePurestake = "Purestake";
        public string StorageHackathon = "Hackathon";
        public string StoragemyNode = "myNode";
        public string StorageSavedBetaNetwork = "SavedBetaNetwork";
        public string StorageSavedTestNetwork = "SavedTestNetwork";
        public string StorageTestNetToken = "TestNetToken";
        public string StorageTestNetAddress = "TestNetAddress";
        public string StorageBetaNetToken = "BetaNetToken";
        public string StorageBetaNetAddress = "BetaNetAddress";
        public string StorageNodeType = "NodeType";
        public string StorageLastASAButton = "LastASAButton";
        public string StorageLastHomeButton = "LastHomeButton";
        public helper()
        {

        }

        public async void openurl(string url, string account)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Launcher.OpenAsync(url + "?account=" + account);

            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                await Launcher.OpenAsync(url + "?account=" + account);
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                await Launcher.OpenAsync(url + "?account=" + account);
            }
        }

        public async Task<string[]> CreateAccount(string accountname)
        {
            string[] accountinfo = new string[2];
            Account myAccount = new Account();
            var myMnemonic = myAccount.ToMnemonic();
            Console.WriteLine(accountname.ToString() + " Address = " + myAccount.Address.ToString());
            Console.WriteLine(accountname.ToString() + " Mnemonic = " + myMnemonic.ToString());

            accountinfo[0] = myAccount.Address.ToString();
            accountinfo[1] = myMnemonic.ToString();
            return accountinfo;
        }

        public async Task<Account[]> RestoreAccounts()
        {
            Account[] accounts = new Account[3];

            string mnemonic1 = "";
            string mnemonic2 = "";
            string mnemonic3 = "";

            try
            {
                mnemonic1 = await SecureStorage.GetAsync(StorageAccountName1);
                mnemonic2 = await SecureStorage.GetAsync(StorageAccountName2);
                mnemonic3 = await SecureStorage.GetAsync(StorageAccountName3);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                // Possible that device doesn't support secure storage on device.
            }
            // restore accounts
            accounts[0] = new Account(mnemonic1);
            accounts[1] = new Account(mnemonic2);
            accounts[2] = new Account(mnemonic3);
            return accounts;


        }
        public AlgodApi algodApiInstance;

        public async Task<ulong?> GetAccountBalance(string accountname)
        {
            Account account = null;
            string myaddress = "";
            string network = await SecureStorage.GetAsync(StorageNetwork);
            if (!(accountname == StorageMultisig))
            {
                string mnemonic = await SecureStorage.GetAsync(accountname);
                if (mnemonic != null)
                {
                    account = new Account(mnemonic);
                    myaddress = account.Address.ToString();
                }
            }
            else
            {
                // multisig address
                myaddress = await SecureStorage.GetAsync(accountname);
            }
            algodApiInstance = await CreateApiInstance();
            if (algodApiInstance != null)
            {
                if (!(String.IsNullOrEmpty(myaddress)))
                {
                    Algorand.Algod.Client.Model.Account accountinfo = await algodApiInstance.AccountInformationAsync(myaddress);

                    if (accountinfo != null)
                    {
                        return accountinfo.Amount;
                    }
                }
            }
            return (ulong?)0;
        }

        public async Task<AlgodApi> CreateApiInstance()
        {
            // creatapi instance
            string ALGOD_API_TOKEN_BETANET = await SecureStorage.GetAsync(StorageALGOD_API_TOKEN_BETANET);
            string ALGOD_API_TOKEN_TESTNET = await SecureStorage.GetAsync(StorageALGOD_API_TOKEN_TESTNET);
            string ALGOD_API_ADDR_TESTNET = await SecureStorage.GetAsync(StorageALGOD_API_ADDR_TESTNET);
            string ALGOD_API_ADDR_BETANET = await SecureStorage.GetAsync(StorageALGOD_API_ADDR_BETANET);
            string network = await GetNetwork();
            if (string.IsNullOrEmpty(network))
            {
                //first time - default to TestNet/Purestake
                ALGOD_API_ADDR_TESTNET = "https://testnet-algorand.api.purestake.io/ps1";
                ALGOD_API_TOKEN_TESTNET = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab";
                await SecureStorage.SetAsync(StorageALGOD_API_TOKEN_TESTNET, ALGOD_API_TOKEN_TESTNET);
                await SecureStorage.SetAsync(StorageALGOD_API_ADDR_TESTNET, ALGOD_API_ADDR_TESTNET);

                algodApiInstance = new AlgodApi(ALGOD_API_ADDR_TESTNET, ALGOD_API_TOKEN_TESTNET);
                await SecureStorage.SetAsync("Network", StorageTestNet);
            }
            else
            {
                if (network == "TestNet")
                {

                    algodApiInstance = new AlgodApi(ALGOD_API_ADDR_TESTNET, ALGOD_API_TOKEN_TESTNET);
                }
                else
                {

                    algodApiInstance = new AlgodApi(ALGOD_API_ADDR_BETANET, ALGOD_API_TOKEN_BETANET);
                }
            }
            return algodApiInstance;
        }

        public async Task<string> GetNetwork()
        {
            string network = await SecureStorage.GetAsync("Network");
            return network;

        }
    }
}
