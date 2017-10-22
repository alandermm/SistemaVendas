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
        static string doc, op2;
        static string tempdoc;
        static int soma = 0, resto = 0;
        static Regex rgx = new Regex(@"^\d*$");
        static string primeiroDigito, segundoDigito;
        static bool docValido = false;
        static bool arquivoExiste = false;
        static string arquivo = "";
        static void Main(string[] args) {
            //imprime menu  
            mostrarMenuPrincipal();
        }

        private static void mostrarMenuPrincipal(){
            string opt;
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
                    opt = Console.ReadLine();
                    opcao = Int16.Parse(opt);
                } while (opcao < 1 || opcao > 4 && opcao != 9);

                switch(opcao){
                    case 1: cadastrarCliente(); break;
                    case 2: cadastrarProduto(); break;
                    case 3: realizarVenda(); break;
                    case 4: exibirExtratoCliente(); break;
                }
            } while(opcao != 9);
        }

        private static void mostrarMenuCadastroCliente(){
            //string tipoDoc = null;
            //string doc;
            Console.WriteLine("Escola o tipo do cliente:\n"
                        + "1 - Pessoa Física\n"
                        + "2 - Pessoa Jurídica\n");

            do{
                Console.Write("Opção: ");
                tipoDoc = Console.ReadLine();
            } while( tipoDoc != "1" && tipoDoc != "2");
        }

        private static void cadastrarCliente(){
            //Escolhe e valida o tipo de cliente
            //tipoDoc -> 1 para CPF e 2 para CNPJ
            do{ 
                mostrarMenuCadastroCliente();

                switch(tipoDoc){
                    case "1": docValido = validarCPF(); tipoDoc = "CPF"; break;
                    case "2": docValido = validarCNPJ(); tipoDoc = "CNPJ"; break; 
                }
            } while (!docValido);

            //Campos para serem cadastrados
            String[] campos = new String[]{ "Nome Completo", "Email", "Endereço" };
            String[] pessoa = new String[campos.Length];

            //Faz perguntas sobrea os campos
            for(int i = 0; i < campos.Length; i++){
                Console.Write("Digite o " + campos[i] + " do cliente: ");
                pessoa[i] = Console.ReadLine();
            }

            
            switch(tipoDoc){
                case "CPF": arquivo = "PessoasFisicas.csv"; break;
                case "CNPJ": arquivo = "PessoasJuridicas.csv"; break;
            }

            //verifica se já existe o arquivo Clientes.csv
            arquivoExiste = File.Exists(arquivo);

            //Cria ou abre o arquivo Clientes.csv
            StreamWriter clientes = new StreamWriter(arquivo, true);
            
            //Se o arquivo Clientes.csv não foi criado, grava o cabeçalho

            if(!arquivoExiste){
                //cria cabeçaho
                ArrayList cabecalho = new ArrayList();
                cabecalho.Add(tipoDoc);
                for (int i = 0; i < campos.Length; i++){
                    cabecalho.Add(campos[i]);
                }
                cabecalho.Add("Data");
                escreverCabecalho(clientes, cabecalho);
            }

            /*if(!arquivoExiste){
                for(int i = 0; i < campos.Length; i++){
                    if(campos[i] == (campos.Length -1).ToString()){
                        clientes.WriteLine(campos[i]);
                    } else {
                        clientes.Write(campos[i] + ";");
                    }
                }    
            }*/

            //Escreve os dados do cliente no arquivo Cliente.csv
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

        }

        private static void realizarVenda(){

        }

        private static void exibirExtratoCliente(){

        }

        private static string validarDigito(int[] chave, int tipoDoc){
            soma = 0;
            resto = 0;

            tempdoc = doc.Substring(0, chave.Length);
            
            for(int i = 0; i < chave.Length; i++){
                soma += ((int)Char.GetNumericValue(tempdoc[i]) * chave[i]);
            }
            
            resto = soma % 11;
            //if(resto == 0 && (doc.Substring(6,2) == "01" || doc.Substring(6,2) == "02")){
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
        public static void escreverCabecalho(StreamWriter arquivo, ArrayList cabecalho ) {
            if(!arquivoExiste){
                for(int i = 0; i < cabecalho.Count; i++){
                    if (i == (cabecalho.Count - 1))
                        arquivo.WriteLine(cabecalho[i]);
                    else
                        arquivo.Write(cabecalho[i] + ";");
                }    
            }
        }
        private static string limparCaracteresDocumento(string doc){
            return doc.Replace("/","").Replace("-","").Replace(".","");
        }
    }
}