using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoletoNet.Testes
{
    [TestClass]
    public class BancoBanrisulTeste
    {
        private BoletoBancario GerarBoletoCarteira805076()
        {
            DateTime vencimento = new DateTime(2016, 01, 07);

            var cedente = new Cedente("00.000.000/0000-00", "Empresa Teste", "0891", "", "260000120", "5");
            Boleto boleto = new Boleto(vencimento, 1.90M, "8050.76", "29338", cedente);
            boleto.NumeroDocumento = "29331";

            var boletoBancario = new BoletoBancario();
            boletoBancario.CodigoBanco = 041;
            boletoBancario.Boleto = boleto;
            boletoBancario.Boleto.Cedente.Codigo = "0891000012086";

            return boletoBancario;
        }

        [TestMethod]
        public void BanrisulCarteira805076LinhaDigitavel()
        {
            var boletoBancario = GerarBoletoCarteira805076();
            boletoBancario.Boleto.Valida();
            string linhaDigitavelValida = "04192.10893  10000.120005  02933.840023  5  66660000000190";
            Assert.AreEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida);
        }

        [TestMethod]
        public void BanrisulCarteira805076LinhaDigitavelFalha()
        {
            var boletoBancario = GerarBoletoCarteira805076();
            boletoBancario.Boleto.Valida();
            string linhaDigitavelValida = "04192.10894  10000.120005  02933.840023  5  66660000000190";
            Assert.AreNotEqual(boletoBancario.Boleto.CodigoBarra.LinhaDigitavel, linhaDigitavelValida);
        }
    }
}
