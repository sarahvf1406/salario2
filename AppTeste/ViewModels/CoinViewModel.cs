using System;
using System.Windows.Input;
using AppTeste.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using AppTeste.Views;

namespace AppTeste.ViewModels
{
    public partial class CoinViewModel : ObservableObject
    {
        private readonly CoinView _view;

        private string _nome;
        private string _diaSemana;

        // Ajuste: Recebe a View no construtor
        public CoinViewModel(CoinView view)
        {
            _view = view;

            Application.Current.MainPage
                   .DisplayAlert("Mensagem", "Bem-vindo(a) ao COIN FLIP!!", "Ok");

            FlipCommand = new Command(async () => await FlipAsync());
            ReiniciarCommand = new Command(ReiniciarJogo);

            Tentativas = 0;
            Acertos = 0;
            Perdas = 0;
        }

        public ICommand FlipCommand { get; set; }
        public ICommand ReiniciarCommand { get; set; }

        [ObservableProperty]
        public string _ladoEscolhido = string.Empty;

        [ObservableProperty]
        public string _imagem = string.Empty;

        [ObservableProperty]
        public string _resultado = string.Empty;

        [ObservableProperty]
        public int _tentativas;

        [ObservableProperty]
        public int _acertos;

        [ObservableProperty]
        public int _perdas;

        private async Task FlipAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_nome))
                {
                    _nome = await Application.Current.MainPage
                        .DisplayPromptAsync("Identificação", "Digite seu nome");
                }

                if (string.IsNullOrEmpty(_diaSemana))
                {
                    _diaSemana = await Application.Current.MainPage
                        .DisplayActionSheet("Dia da semana",
                            "Cancelar",
                            "Ok",
                            "Domingo",
                            "Segunda",
                            "Terça",
                            "Quarta",
                            "Quinta",
                            "Sexta",
                            "Sábado");
                }

                if (string.IsNullOrEmpty(_ladoEscolhido))
                {
                    throw new Exception("Selecione o lado da moeda");
                }

                // Executa a animação da moeda na View
                await _view.PlayCoinAnimation();

                Coin coin = new Coin();
                string resultadoJogo = coin.Jogar(_ladoEscolhido);
                Imagem = $"{coin.Lado}.png";

                Tentativas++;

                if (resultadoJogo.IndexOf("pena", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Perdas++;
                }
                else
                {
                    Acertos++;
                }

                Resultado = $"{_nome}, Hoje é {_diaSemana}. {resultadoJogo}";

                OnPropertyChanged(nameof(Resultado));
                OnPropertyChanged(nameof(Imagem));
                OnPropertyChanged(nameof(Tentativas));
                OnPropertyChanged(nameof(Acertos));
                OnPropertyChanged(nameof(Perdas));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Mensagem", ex.Message, "Ok");
            }
        }

        private void ReiniciarJogo()
        {
            _nome = string.Empty;
            _diaSemana = string.Empty;
            _ladoEscolhido = string.Empty;
            _resultado = string.Empty;
            _imagem = string.Empty;

            Tentativas = 0;
            Acertos = 0;
            Perdas = 0;

            OnPropertyChanged(nameof(Resultado));
            OnPropertyChanged(nameof(Imagem));
            OnPropertyChanged(nameof(LadoEscolhido));
            OnPropertyChanged(nameof(Tentativas));
            OnPropertyChanged(nameof(Acertos));
            OnPropertyChanged(nameof(Perdas));
        }
    }
}
