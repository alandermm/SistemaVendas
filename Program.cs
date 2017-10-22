using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SistemaVendas
{
    class Program
    {
        static int[] chaveCPF = {10,9,8,7,6,5,4,3,2};
        static int[] chaveCPF2 = {11,10,9,8,7,6,5,4,3,2};
        static int[] chaveCNPJ = {5,4,3,2,9,8,7,6,5,4,3,2};
        static int[] chaveCNPJ2 = {6,5,4,3,2,9,8,7,6,5,4,3,2};
        static string opcao;
        static string tipoDoc;
        static string doc;
        static string tempdoc;
        static int soma = 0, resto = 0;
        static Regex rgx = new Regex(@"^\d*$");
        static string primeiroDigito, segundoDigito;
        static bool docValido = false;
        static bool arquivoExiste = false;
        static string arquivo = "";
        static StreamReader arquivoProdutos;
        static void Main(string[] args) {
            //imprime menu  
            //mostrarMenuPrincipal();
            exibirListaDeProdutos();
        }

        private static void mostrarMenuPrincipal(){
            //string opt;
            int opcao = 9;
            
            do{ 
                Console.WriteLine("Escola uma das opções abaixo\n"
                            + "1 - Cadastrar Clientes\n"
                            + "2 - Cadastrar Produtos\n"
                            + "3 - Realizar Vendas\n"
                            + "4 - Extrato Clientes\n"
                            + "9 - Sair\n");
                
                Console.Write("Opção: ");
                
                do{
                    //opt = Console.ReadLine();
                    opcao = Int16.Parse(Console.ReadLine());
                } while (opcao < 1 || opcao > 4 && opcao != 9);

                switch(opcao){
                    case 1: cadastrarCliente(); break;
                    case 2: cadastrarProduto(); break;
                    case 3: realizarVenda(); break;
                    case 4: exibirExtratoCliente(); break;
                    case 9: Environment.Exit(0); break;
                }
            } while(opcao != 9);
        }

        private static void mostrarMenuTipoCliente(string acao){
            //string tipoDoc = null;
            //string doc;
            Console.WriteLine("Escolha o tipo do cliente que " + acao  + ":\n"
                        + "1 - Pessoa Física\n"
                        + "2 - Pessoa Jurídica\n");

            do{
                Console.Write("Opção: ");
                tipoDoc = Console.ReadLine();
            } while( tipoDoc != "1" && tipoDoc != "2");
        }

        private static void solicitarValidarDocumento(string acao){
            //Escolhe e valida o tipo de cliente
            //tipoDoc -> 1 para CPF e 2 para CNPJ
            do{ 
                mostrarMenuTipoCliente(acao);

                switch(tipoDoc){
                    case "1": docValido = validarCPF(); tipoDoc = "CPF"; break;
                    case "2": docValido = validarCNPJ(); tipoDoc = "CNPJ"; break; 
                }
            } while (!docValido);
        }

        private static void cadastrarCliente(){
            solicitarValidarDocumento("será cadastrado");            

            //Campos para serem cadastrados
            String[] campos = new String[]{ "Nome Completo", "Email", "Endereço" };
            String[] pessoa = new String[campos.Length];

            //Faz perguntas sobrea os campos
            for(int i = 0; i < campos.Length; i++){
                Console.Write("Digite o " + campos[i] + " do cliente: ");
                pessoa[i] = Console.ReadLine();
            }

            //Define o nome do arquivo
            switch(tipoDoc){
                case "CPF": arquivo = "PessoasFisicas.csv"; break;
                case "CNPJ": arquivo = "PessoasJuridicas.csv"; break;
            }

            //verifica se já existe o arquivo PessosFisicas.csv ou PessoasJuridicas.csv
            arquivoExiste = File.Exists(arquivo);

            //Cria ou abre o arquivo Clientes.csv
            StreamWriter clientes = new StreamWriter(arquivo, true);
            
            //Se o arquivoPessosFisicas.csv ou PessoasJuridicas.csv não foi criado, grava o cabeçalho
            if(!arquivoExiste){
                //cria cabeçaho
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add(tipoDoc);
                for (int i = 0; i < campos.Length; i++){
                    cabecalho.Add(campos[i]);
                }
                cabecalho.Add("Data");
                escreverCabecalho(clientes, cabecalho.ToArray(typeof(String[])) as String[]);
            }

            //Escreve os dados do cliente no arquivo PessosFisicas.csv ou PessoasJuridicas.csv
            if(docValido){
                clientes.Write(doc + ";");
                for(int i = 0; i < pessoa.Length; i++){
                    clientes.Write(pessoa[i] + ";");
                }
                //Escreve data e hora do cadastro
                clientes.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                clientes.Close();
            }
        }

        private static void cadastrarProduto(){
            //Campos para serem cadastrados
            String[] campos = new String[]{ "Código do Produto", "Nome do Produto", "Descricao", "Preço" };
            String[] produto = new String[campos.Length];

            //Faz perguntas sobrea os campos
            for(int i = 0; i < campos.Length; i++){
                Console.Write("Digite o " + campos[i] + " do produto: ");
                produto[i] = Console.ReadLine();
            }

            //verifica se já existe o arquivo Produtos.csv
            arquivo = "Produtos.csv";
            arquivoExiste = File.Exists(arquivo);

            //Cria ou abre o arquivo Clientes.csv
            StreamWriter produtos = new StreamWriter(arquivo, true);
            
            //Se o arquivo Produtos.csv não foi criado, grava o cabeçalho
            if(!arquivoExiste){
                //cria cabeçaho
                escreverCabecalho(produtos, campos);
            }

            //Escreve os dados do produto no arquivo Produtos.csv
            for(int i = 0; i < produto.Length; i++){
                if(produto[i] == (produto.Length -1).ToString()){
                    produtos.Write(produto[i]);
                } else {
                    produtos.Write(produto[i] + ";");
                }
            }
            produtos.Close();
        }

        private static void realizarVenda(){
            solicitarValidarDocumento("realizará a compra");

            exibirListaDeProdutos();
        }

        private static void exibirExtratoCliente(){

        }

        //Função para validação do dígito verificador do documento
        private static string validarDigito(int[] chave, int tipoDoc){
            soma = 0;
            resto = 0;

            tempdoc = doc.Substring(0, chave.Length);
            
            for(int i = 0; i < chave.Length; i++){
                soma += ((int)Char.GetNumericValue(tempdoc[i]) * chave[i]);
            }
            
            resto = soma % 11;

            if(resto < 2){
                return "0";
            } else {
                return (11-resto).ToString();
            }
            
        }

        private static bool validarCPF(){
            do{
                Console.Write("Digite o CPF: ");
                doc = limparCaracteresDocumento(Console.ReadLine());
            } while (doc.Length != 11 || !rgx.IsMatch(doc));

            primeiroDigito = validarDigito(chaveCPF, 1);

            if(primeiroDigito != doc.Substring(9, 1)){
                Console.WriteLine("CPF inválido!\n");
                return false;
            } else {
                segundoDigito = validarDigito(chaveCPF2, 1);
                if(doc.EndsWith(segundoDigito)){
                    Console.WriteLine("CPF válido!\n");
                    return true;
                } else {
                    Console.WriteLine("CPF inválido!\n");
                    return false;
                }
            }
        }

        private static bool validarCNPJ(){
            do{
                Console.Write("Digite o CNPJ: ");
                doc = limparCaracteresDocumento(Console.ReadLine());
            } while (doc.Length != 14 || !rgx.IsMatch(doc));

            primeiroDigito = validarDigito(chaveCNPJ, 2);

            if(primeiroDigito != doc.Substring(12, 1)){
                Console.WriteLine("CNPJ inválido!\n");
                return false;
            }else {
                segundoDigito = validarDigito(chaveCNPJ2, 2);
                if(doc.EndsWith(segundoDigito)){
                    Console.WriteLine("CNPJ válido!\n");
                    return true;
                } else {
                    Console.WriteLine("CNPJ inválido!\n");
                    return false;
                }
            }
        }
        public static void escreverCabecalho(StreamWriter arquivo, String[] cabecalho ) {
            if(!arquivoExiste){
                for(int i = 0; i < cabecalho.Length; i++){
                    if (i == cabecalho.Length - 1)
                        arquivo.WriteLine(cabecalho[i]);
                    else
                        arquivo.Write(cabecalho[i] + ";");
                }    
            }
        }
        private static string limparCaracteresDocumento(string doc){
            return doc.Replace("/","").Replace("-","").Replace(".","");
        }

        private static void exibirListaDeProdutos(){
            arquivo = "Produtos.csv";
            //static StreamReader arquivoProdutos;
            if (File.Exists(arquivo)) {
                try {
                    using (arquivoProdutos = new StreamReader(arquivo)){
                        String produto;
                        ArrayList produtos = new ArrayList{};
                        String[] camposProduto;
                        // Lê linha por linha até o final do arquivo
                        while ((produto = arquivoProdutos.ReadLine()) != null){
                            camposProduto = produto.Split(';');
                            produtos.Add(camposProduto);
                        }
                        int registros = 0;
                        foreach(String[] linha in produtos){
                            foreach(string campo in linha){
                                Console.Write("{0,-45} {1,5:N1}", campo, "  |");
                                
                            }
                            
                            Console.WriteLine("");
                            
                            registros++;
                        }

                        /*for (int i = 0; i < produtos.Count; i++){
                            Console.WriteLine(produtos[i]);
                        }*/
                    }
                } catch (Exception ex){
                    Console.WriteLine(ex.Message);
                } finally{
                    arquivoProdutos.Close();
                }
            } else {
                Console.WriteLine(" O arquivo " + arquivo + "não foi localizado !");
            }
            


            /*StreamReader produtos = new StreamReader(arquivo);
            //ArrayList produtos = new ArrayList(File.ReadAllLines("Produtos.csv"));
            
            ArrayList produto = new ArrayList{};
            ArrayList camposProduto = null;

            using while ((linha = sr.ReadLine()) != null){
                produto.Add(
                camposProduto = produto.Split(';');
                Console.WriteLine(camposProduto[0]);
            }
            produtos.Close();*/
            
        }
    }
}