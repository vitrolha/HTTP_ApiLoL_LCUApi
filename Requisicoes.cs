using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AppLoL_v1._0_
{
    internal class Requisicoes
    {
        ConectarLCU lcu = new ConectarLCU();
        
        private string search_state_url = "/lol-lobby/v2/lobby/matchmaking/search-state";
        private string aceitar_url = "/lol-matchmaking/v1/ready-check/accept";
        private string gameflow_phase_url = "/lol-gameflow/v1/gameflow-phase";

        private  string Base_URL()
        {
            
            string port = lcu.LCUPassPort().Split(':')[1];
            return $"https://127.0.0.1:{port}";
        }
        private string Senha()
        {
            string pass = lcu.LCUPassPort().Split(':')[0];
            return pass;
        }

        public async Task<string> RequisicaoSearch_State()
        {
            if (lcu.IsLCUOpen() == false) MessageBox.Show("O LOL está fechado");
            try
            {
                string faseRequest_url = Base_URL() + search_state_url;
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                var httpClient = new HttpClient(httpClientHandler);
                httpClient.BaseAddress = new Uri(Base_URL());
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{Senha()}"))); 

                var response = await httpClient.GetAsync(faseRequest_url);
                var responseContent = await response.Content.ReadAsStringAsync();
                var fase = JsonConvert.DeserializeObject<LCUGettersSetters>(responseContent);
                //MessageBox.Show($"Resposta: {fase.searchState.ToString()}");
                return fase.searchState.ToString();               
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return ex.Message;
            }


        }

        //Funciona
        public async void AceitarPartida()
        {
            if (lcu.IsLCUOpen() == false) MessageBox.Show("O LOL está fechado");
            try
            {
                string aceitarRequest_url = Base_URL() + aceitar_url;

                var httpsClientHandler = new HttpClientHandler();
                httpsClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                var httpsClient = new HttpClient(httpsClientHandler);
                httpsClient.BaseAddress = new Uri(Base_URL());
                httpsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{Senha()}")));

                var stringContent = new StringContent(aceitarRequest_url, Encoding.UTF8, "application/json");
                var response = await httpsClient.PostAsync(aceitarRequest_url, stringContent);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public async Task<string> RequisicaoGameFlow_Phase()
        {
            if (lcu.IsLCUOpen() == false) MessageBox.Show("O LOL está fechado");
            try
            {
                string champSelect_Url = Base_URL() + gameflow_phase_url;
                var httpsClientHandler = new HttpClientHandler();
                httpsClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                var httpsClient = new HttpClient(httpsClientHandler);
                httpsClient.BaseAddress = new Uri(Base_URL());
                httpsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{Senha()}")));

                var response = await httpsClient.GetAsync(champSelect_Url);
                var champSelectInfo = await response.Content.ReadAsStringAsync();
                //var champSelect = JsonConvert.DeserializeObject<LCUGettersSetters>(responseContent);
                var champSelectInfoSemAspas = champSelectInfo.Replace("\"", "");
                return champSelectInfoSemAspas;
                
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
                return ex.Message;
            }
        }
    }

}
