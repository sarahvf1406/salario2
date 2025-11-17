using AppTeste.ViewModels;

namespace AppTeste.Views;

public partial class CoinView : ContentPage
{
    public CoinView()
    {
        InitializeComponent();

        // Passa a própria view para a ViewModel no construtor
        this.BindingContext = new CoinViewModel(this);
    }

    // Método que executa a animação trocando as imagens
    public async Task PlayCoinAnimation()
    {
        string[] sequence = new string[]
        {
            "logojogo.png",  // 0
            "cara.png",      // 1
            "lado.png",      // 2
            "coroa.png",     // 3
            "cara.png",      // 1
            "lado.png",      // 2
            "coroa.png"      // 3
        };

        foreach (var img in sequence)
        {
            CoinImage.Source = img;
            await Task.Delay(150);
        }
    }
}
