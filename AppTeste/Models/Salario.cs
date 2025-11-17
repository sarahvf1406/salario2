using System;

namespace AppTeste.Models
{
    public class Salario
    {
        public decimal ValorHora { get; set; }
        public decimal HorasTrabalhadas { get; set; }

        public decimal CalcularSalario()
        {
            decimal salarioBruto = ValorHora * HorasTrabalhadas * 4.5m * (1 + 1 / 6m);

            decimal aliquota;
            decimal parcelaADeduzir;

            if (salarioBruto <= 1100.00m)
            {
                aliquota = 0.075m;
                parcelaADeduzir = 0;
            }
            else if (salarioBruto <= 2203.48m)
            {
                aliquota = 0.09m;
                parcelaADeduzir = 16.50m;
            }
            else if (salarioBruto <= 3305.22m)
            {
                aliquota = 0.12m;
                parcelaADeduzir = 82.60m;
            }
            else if (salarioBruto <= 6433.57m)
            {
                aliquota = 0.14m;
                parcelaADeduzir = 148.71m;
            }
            else
            {
                return salarioBruto - 751.97m;
            }

            decimal descontoINSS = (salarioBruto * aliquota) - parcelaADeduzir;
            decimal salarioINSS = salarioBruto - descontoINSS;
            decimal salarioLiquido = salarioINSS - CalcularIRRF(salarioINSS);
            return salarioLiquido;
        }

        private decimal CalcularIRRF(decimal salarioINSS)
        {
            decimal IRRF;
            if (salarioINSS <= 2259.20m)
                IRRF = 0;
            else if(salarioINSS <= 2826.65m)
                IRRF = (salarioINSS * 0.075m) - 169.44m;
            else if (salarioINSS <= 3751.05m)
                IRRF = (salarioINSS * 0.15m) - 381.44m;
            else if (salarioINSS <= 4664.68m)
                IRRF = (salarioINSS * 0.225m) - 662.77m;
            else
                IRRF = (salarioINSS * 0.275m) - 869.00m;
            return IRRF;
        }
    }
}