﻿using Aplication.DTO;
using Aplication.Services.Calculators;
using CrossCutting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class StatementService
    {
        private List<PlayDto> Plays = new()
            {
                new PlayDto("Hamlet", 4024, PlayType.tragedy),
                new PlayDto("As You Like It", 2670, PlayType.comedy),
                new PlayDto("Othello", 3560,        PlayType.tragedy),
                new PlayDto("Henry V", 3227, PlayType.history),
                new PlayDto("King John", 2648,   PlayType.history),
                new PlayDto("Richard III", 3718, PlayType.history)
            };
        private List<PerformanceDto> ListAllPerformances()
        {
            List<PerformanceDto> performances = new()
            {
                new(GetPlayByName("Hamlet"), 55),
                new(GetPlayByName("You Like"), 35),
                new(GetPlayByName("Othello"), 40),
                new(GetPlayByName("Henry"), 20),
                new(GetPlayByName("John"), 39)
                };
            return performances;
        }

        private List<PerformanceDto> GetPerformancesByName(params string[] names)
        {
            var AllPerformances = ListAllPerformances();
            var perfs = names.Select(name => AllPerformances.FirstOrDefault(perf => perf.Play.Name.Contains(name)))
                .ToList();
            return perfs!;
        }

        private PlayDto GetPlayByName(string name)
        => Plays.FirstOrDefault(x => x.Name.Contains(name))!;

        public InvoiceDto ObterInvoiceBigCo()
        => new("BigCo", GetPerformancesByName("Hamlet", "As You Like",
                "Othello"));


        public InvoiceDto ObterInvoiceBigCo2()
        => new("BigCo", GetPerformancesByName("Hamlet", "As You Like", "Othello", "Henry", "John", "Henry"));

        public string Imprimir()
        {
            var invoice = ObterInvoiceBigCo2();
            return Print(invoice);
        }

        public static string Print(InvoiceDto invoice)
        {
            CultureInfo cultura = new("en-US");
            var valorTotal = 0; var valorCreditos = 0;
            var resultado = string.Format("Statement for {0}\n", invoice.Customer);

            foreach (var perf in invoice.Performances)
            {
                int lines = perf.Play.Lines;

                if (lines < 1000) lines = 1000;
                if (lines > 4000) lines = 4000;

                var valorPorPerformance = lines * 10;

                var calculadora = PriceCalculatorFactory.GetCalculator(perf.PlayType);

                if (perf.PlayType == PlayType.history)
                    calculadora.ReservedValue = valorPorPerformance;

                valorPorPerformance += calculadora.CalculatePrice(perf.Audience);

                valorCreditos += Math.Max(perf.Audience - 30, 0);

                if (perf.PlayType == PlayType.comedy)
                    valorCreditos += (int)Math.Floor((decimal)perf.Audience / 5);

                var resultadoPerformance = ObterResultado(cultura, perf, valorPorPerformance);
                resultado += resultadoPerformance;
                valorTotal += valorPorPerformance;
            }

            resultado += string.Format(cultura, "Amount owed is {0:C}\n", Convert.ToDecimal(valorTotal) / 100);
            resultado += string.Format("You earned {0} credits\n", valorCreditos);
            return resultado;
        }

        private static string ObterResultado(CultureInfo cultura, PerformanceDto perf, int thisAmount)
            => string.Format(cultura, "  {0}: {1:C} ({2} seats)\n", perf.Play.Name, Convert.ToDecimal(thisAmount) / 100, perf.Audience);
    }
}
