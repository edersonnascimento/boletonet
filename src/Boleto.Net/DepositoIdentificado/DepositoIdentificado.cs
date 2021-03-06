﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BoletoNet
{
    public abstract class DepositoIdentificado
    {
        public IBanco Banco { get; private set; }
        public Cedente Cedente { get; set; }
        public Sacado Sacado { get; set; }
        public string Logo { get; set; }
        public abstract string NumeroDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public String InstrucoesPagamento { get; set; }
        public String InstrucoesCaixa { get; set; }

        public DepositoIdentificado(IBanco banco)
        {
            Banco = banco;
        }

        public virtual string MontaHtml()
        {
            string html = Properties.Resources.Html_DI;

            html = Regex.Replace(html, "@LOGO", this.Logo);
            
            html = Regex.Replace(html, "@RAZAO", this.Cedente.Nome);
            html = Regex.Replace(html, "@CNPJ", formataDocumento(this.Cedente.CPFCNPJ));
            html = Regex.Replace(html, "@ENDERECO1", "");
            html = Regex.Replace(html, "@ENDERECO2", string.Format("{0} / {1}", this.Cedente.Endereco.Cidade, this.Cedente.Endereco.UF));

            html = Regex.Replace(html, "@AGENCIA", this.Cedente.ContaBancaria.Agencia);
            html = Regex.Replace(html, "@CONTA", string.Format("{0}-{1}", this.Cedente.ContaBancaria.Conta, this.Cedente.ContaBancaria.DigitoConta));
            html = Regex.Replace(html, "@NUMERO_DOCUMENTO", this.NumeroDocumento);

            html = Regex.Replace(html, "@VENCIMENTO", this.DataVencimento.ToString("dd/MM/yyyy"));
            html = Regex.Replace(html, "@VALOR", this.Valor.ToString("#,##0.#0"));

            html = Regex.Replace(html, "@SACADO", string.Format("{0} - CPF/CNPJ: {1}", this.Sacado.Nome, formataDocumento(this.Sacado.CPFCNPJ)));

            html = Regex.Replace(html, "@INSTRUCOES_PAGAMENTO", geraLista(this.InstrucoesPagamento));
            html = Regex.Replace(html, "@INSTRUCOES_RECEBIMENTO", geraLista(this.InstrucoesCaixa));

            return html;
        }
        private string formataDocumento(string doc)
        {
            if (doc.Length == 11)
                return Regex.Replace(doc, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
            else if (doc.Length == 14)
                return Regex.Replace(doc, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
            else
                return doc;
        }

        private string geraLista(string instrucoes)
        {
            StringReader reader = new StringReader(instrucoes);
            StringBuilder li = new StringBuilder();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                li.AppendLine(string.Format("<li>{0}</li>", line));
            }
            return li.ToString();
        }
    }
}
