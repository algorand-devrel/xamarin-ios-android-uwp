using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace algorandapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatefulContracts : ContentPage
    {
        public StatefulContracts()
        {
            InitializeComponent();
            StatefulExamples();
        }
        void StatefulExamples()
        {
            //string ALGOD_API_ADDR = "https://testnet-algorand.api.purestake.io/idx2";
            //string ALGOD_API_TOKEN = "GeHdp7CCGt7ApLuPNppXN4LtrW07Mm1kaFNJ5Ovr";
            string ALGOD_API_ADDR = "http://localhost:4001";
            string ALGOD_API_TOKEN = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        }
    }
        
}