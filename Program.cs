using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SistemaVendas
{
    class Program
    {
        static string opcao;
        static string tipoDoc;
        static string doc;
        static Regex rgx = new Regex(@"^\d*$");
        //static bool docValido = false;
        static string docValido;
        static bool arquivoExiste = false;
        static string arquivo = "";
        
        static void Main(string[] args) {
            //imprime menu  
            mostrarMenuPrincipal();
            //exibirListaDeProdutos();
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

        private static string solicitarValidarDocumento(string acao){
            //Escolhe e valida o tipo de cliente
            //tipoDoc -> 1 para CPF e 2 para CNPJ
            do{ 
                mostrarMenuTipoCliente(acao);

                switch(tipoDoc){
                    case "1": docValido = validarCPF(); tipoDoc = "CPF"; break;
                    case "2": docValido = validarCNPJ(); tipoDoc = "CNPJ"; break; 
                }
            } while (docValido == null);
            return docValido;
        }

        /// <summary>
        /// Realiza o cadastro de clientes
        /// </summary>
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
                //escreverCabecalho(clientes, cabecalho.ToArray(typeof(String[])) as String[]);
                escreverCabecalho(clientes, cabecalho.ToArray(typeof(String[])) as String[]);
            }

            //Escreve os dados do cliente no arquivo PessosFisicas.csv ou PessoasJuridicas.csv
            if(docValido != null){
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
            int codProduto = 1;
            string[] campo, produtos;
            StreamWriter swProdutos;

            //Campos para serem cadastrados
            String[] campos = new String[]{ "Nome", "Descrição", "Preço" };
            String[] produto = new String[campos.Length];
            String[] cabecalho = new String[]{ "Código", "Nome", "Descrição", "Preço" };

            //Faz perguntas sobrea os campos
            for(int i = 0; i < campos.Length; i++){
                Console.Write("Digite o " + campos[i] + " do produto: ");
                produto[i] = Console.ReadLine();
            }

            //verifica se já existe o arquivo Produtos.csv
            //Se o arquivo Produtos.csv não foi criado, grava o cabeçalho
            //Define o código do produto
            arquivo = "Produtos.csv";
            if(!File.Exists(arquivo)){
                File.Create(arquivo).Close();
                swProdutos = new StreamWriter(arquivo,true);
                escreverCabecalho(swProdutos, cabecalho);
                codProduto = 1;
                swProdutos.Close();
            } else{
                produtos = File.ReadAllLines(arquivo);
                campo = produtos[produtos.Length-1].Split(';');
                codProduto = int.Parse(campo[0])+1;
            }

            //Cria ou abre o arquivo Clientes.csv
            swProdutos = new StreamWriter(arquivo, true);

            //Escreve os dados do produto no arquivo Produtos.csv
            swProdutos.Write(codProduto + ";");
            for(int i = 0; i < produto.Length; i++){
                if(i == produto.Length -1){
                    swProdutos.Write(produto[i]);
                } else {
                    swProdutos.Write(produto[i] + ";");
                }
            }
            swProdutos.WriteLine("");
            swProdutos.Close();
        }

        private static void realizarVenda(){
            String[] produtosTemp, ultimaLinha; 
            int codProduto, ultimoCodigo;
            string docCliente;
            String[] clienteEncontrado, produtoEncontrado;

            //Chamar no Vendas
            do{
                docCliente = solicitarValidarDocumento("realizará a compra");
                clienteEncontrado = buscarCliente(docCliente);
            } while(clienteEncontrado == null);

            //Captura ultima linha do arquivo e pega o código do produto
            produtosTemp = File.ReadAllLines(arquivo);
            ultimaLinha = produtosTemp[produtosTemp.Length-1].Split(';');
            ultimoCodigo = int.Parse(ultimaLinha[0]);

            exibirListaDeProdutos();

            Console.WriteLine("Escolha um produto pelo código: ");

            do{
                Console.Write("Código do Produto: ");
                codProduto = int.Parse(Console.ReadLine());
            } while (codProduto < 1 || codProduto > ultimoCodigo);

            produtoEncontrado = buscarProduto(codProduto.ToString());

            

            
        }

        private static void exibirExtratoCliente(){

        }

        //Função para validação do dígito verificador do documento
        private static string validarDigito(int[] chave, int tipoDoc){
            string tempdoc;
            int soma = 0, resto = 0;
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

        private static String[] buscarRegistro(string arquivo, string busca){
            String[] registros;
            String[] registroEncontrado = null;

            //Verifica a existência e faz a leitura do arquivo passado
            if(File.Exists(arquivo)){
                registros = File.ReadAllLines(arquivo);
            } else {
                Console.WriteLine("O arquivo " + arquivo + "não existe!");
                registros = null;
            }

            //Realiza a busca pelo termo dentro do arquivo
            foreach(string registro in registros){
                if(registro.Contains(busca)){
                    registroEncontrado = registro.Split(';');
                }
            }
            return registroEncontrado;
        }

        private static String[] buscarCliente(string cliente){
            string arquivo;
            
            //Verifica o tipo do cliente
            if(cliente.Length == 11){
                arquivo = "PessoasFisicas.csv";
            } else {
                arquivo = "PessoasJuridicas.csv";
            }
            
            //busca o cliente
            return buscarRegistro(arquivo, cliente);
        }

        private static String[] buscarProduto(string codigo){
            string arquivo;
            arquivo = "Produtos.csv";
            
            //busca o cliente
            return buscarRegistro(arquivo, codigo);
        }

        /// <summary>
        /// Valida CPF
        /// </summary>
        /// <returns>Validação do CPF</returns>
        private static string validarCPF(){
            int[] chaveCPF = {10,9,8,7,6,5,4,3,2};
            int[] chaveCPF2 = {11,10,9,8,7,6,5,4,3,2};
            string primeiroDigito, segundoDigito;
            
            do{
                Console.Write("Digite o CPF: ");
                doc = limparCaracteresDocumento(Console.ReadLine());
            } while (doc.Length != 11 || !rgx.IsMatch(doc));

            primeiroDigito = validarDigito(chaveCPF, 1);

            if(primeiroDigito != doc.Substring(9, 1)){
                Console.WriteLine("CPF inválido!\n");
                //return false;
                return null;
            } else {
                segundoDigito = validarDigito(chaveCPF2, 1);
                if(doc.EndsWith(segundoDigito)){
                    Console.WriteLine("CPF válido!\n");
                    //return true;
                    return doc;
                } else {
                    Console.WriteLine("CPF inválido!\n");
                    //return false;
                    return null;
                }
            }
        }

        private static string validarCNPJ(){
            int[] chaveCNPJ = {5,4,3,2,9,8,7,6,5,4,3,2};
            int[] chaveCNPJ2 = {6,5,4,3,2,9,8,7,6,5,4,3,2};
            string primeiroDigito, segundoDigito;
            do{
                Console.Write("Digite o CNPJ: ");
                doc = limparCaracteresDocumento(Console.ReadLine());
            } while (doc.Length != 14 || !rgx.IsMatch(doc));

            primeiroDigito = validarDigito(chaveCNPJ, 2);

            if(primeiroDigito != doc.Substring(12, 1)){
                Console.WriteLine("CNPJ inválido!\n");
                //return false;
                return null;
            }else {
                segundoDigito = validarDigito(chaveCNPJ2, 2);
                if(doc.EndsWith(segundoDigito)){
                    Console.WriteLine("CNPJ válido!\n");
                    //return true;
                    return doc;
                } else {
                    Console.WriteLine("CNPJ inválido!\n");
                    //return false;
                    return null;
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
            return doc.Trim().Replace("/","").Replace("-","").Replace(".","");
        }

        private static void exibirListaDeProdutos(){
            //StreamReader arquivoProdutos;
            arquivo = "Produtos.csv";
             StreamReader arquivoProdutos = null;
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
                                Console.Write("| {0,-22} {1,5:N1}", campo, "");
                                
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