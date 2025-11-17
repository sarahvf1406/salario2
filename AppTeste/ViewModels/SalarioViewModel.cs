// SalarioViewModel.cs (versão recomendada)
using System;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AppTeste.Models;

namespace AppTeste.ViewModels
{
    public class SalarioViewModel : ObservableObject
    {
        private readonly Salario _salario = new Salario();
        private decimal _salarioCalculado;

        public IRelayCommand CalcularSalarioCommand { get; }

        public SalarioViewModel()
        {
            CalcularSalarioCommand = new RelayCommand(
                execute: () => CalcularSalario(),
                canExecute: () => PodeCalcular()
            );
            // inicializações opcionais
            ValorHoraString = string.Empty;
            HorasTrabalhadasString = string.Empty;
        }

        // Propriedades string usadas pelo Entry (TwoWay)
        private string _valorHoraString;
        public string ValorHoraString
        {
            get => _valorHoraString;
            set
            {
                if (SetProperty(ref _valorHoraString, value))
                {
                    // tenta atualizar o model (sem lançar)
                    if (TryParseDecimal(value, out var parsed))
                    {
                        _salario.ValorHora = parsed;
                    }
                    else
                    {
                        _salario.ValorHora = 0m;
                    }
                    CalcularSalarioCommand.NotifyCanExecuteChanged();
                    OnPropertyChanged(nameof(SalarioCalculado));
                }
            }
        }

        private string _horasTrabalhadasString;
        public string HorasTrabalhadasString
        {
            get => _horasTrabalhadasString;
            set
            {
                if (SetProperty(ref _horasTrabalhadasString, value))
                {
                    if (TryParseDecimal(value, out var parsed))
                    {
                        _salario.HorasTrabalhadas = parsed;
                    }
                    else
                    {
                        _salario.HorasTrabalhadas = 0m;
                    }
                    CalcularSalarioCommand.NotifyCanExecuteChanged();
                    OnPropertyChanged(nameof(SalarioCalculado));
                }
            }
        }

        public decimal SalarioCalculado
        {
            get => _salarioCalculado;
            private set => SetProperty(ref _salarioCalculado, value);
        }

        private void CalcularSalario()
        {
            SalarioCalculado = _salario.CalcularSalario();
        }

        private bool PodeCalcular()
        {
            return _salario.ValorHora > 0m && _salario.HorasTrabalhadas > 0m;
        }

        // tenta parsear considerando vírgula/ponto de acordo com cultura
        private bool TryParseDecimal(string s, out decimal result)
        {
            result = 0m;
            if (string.IsNullOrWhiteSpace(s))
                return false;

            // tenta com CurrentCulture e InvariantCulture (aceita 1,23 e 1.23)
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out result))
                return true;
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
                return true;

            // remover espaços e tentar novamente
            var cleaned = s.Trim();
            return decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.CurrentCulture, out result)
                || decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
        }
    }
}
