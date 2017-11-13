using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Classe Menu
/// </summary>
public class Menu{
    /// <summary>
    /// Método para mostrar o menu principal
    /// </summary>
    public void mostrarMenuPrincipal(){
        String path = Directory.GetCurrentDirectory() + "\\";
        int opt;
        do {
            Console.WriteLine(
                "Escola uma das opções abaixo\n"
                + "1 - Cadastrar Clientes\n"
                + "2 - Cadastrar Produtos\n"
                + "3 - Realizar Vendas\n"
                + "4 - Extrato Clientes\n"
                + "9 - Sair\n"
            );
            Console.Write("Opção: ");
            opt = 0;            
            do{
                opt = Int16.Parse(Console.ReadLine());
            } while (opt != 9);
            switch(opt){
                
                case 1: 
                        Pessoa pessoa = new Pessoa();
                        string tipoDoc = mostrarMenuTipoCliente();
                        pessoa.iniciarDados(tipoDoc);
                        string arquivo;
                        arquivo = tipoDoc.Equals("CPF") ? path + "PessoasFisicas.xlsx" : path + "PessoasJuridicas.xlsx";
                        if (!File.Exists(arquivo)){
                            if(!File.Exists(arquivo) || new Cadastro().getUltimaLinha(arquivo) == 1){
                                String[] cabecalho = new String[]{"Documento", "Nome", "E-mail", "Rua", "Número", "Bairro", "Data Cadastro"};
                                new Cadastro().gerarCabecalho(arquivo, cabecalho);
                            }
                        }
                        pessoa.salvar(arquivo);
                        break;
                case 2: Produto produto = new Produto();
                        arquivo = path + "Produtos.xlsx";
                        if (!File.Exists(arquivo)){
                            if(!File.Exists(arquivo) || new Cadastro().getUltimaLinha(arquivo) == 1){
                                String[] cabecalho = new String[]{"Código", "Nome", "Descrição", "Preço", "Data Cadastro"
                                };
                                new Cadastro().gerarCabecalho(arquivo, cabecalho);
                            }
                        }
                        produto.iniciarDados();
                        produto.salvar(arquivo);
                        break;
                case 3: Venda venda = new Venda();
                        arquivo = path + "Vendas.xlsx";
                        string arquivoProduto = path + "Produtos.xlsx";
                        List<int> codigos = venda.listarProdutosDisponiveis();
                        int codigoProduto = mostrarMenuSelecionarProduto(codigos);
                        if(codigoProduto > 0){
                            venda.produto = new Produto().carregarProduto(codigoProduto);
                            venda.valorVenda = venda.produto.preco;
                            
                            //Selecionar Cliente
                            tipoDoc = mostrarMenuTipoCliente();
                            string arquivoCliente = tipoDoc.Equals("CPF")? path + "PessoasFisicas.xlsx" : path + "PessoasJuridicas.xlsx";
                            string doc = tipoDoc.Equals("CPF") ? new Validacao().pedirCPF() : new Validacao().pedirCNPJ();
                            venda.cliente = new Pessoa().carregarPessoa(Int64.Parse(doc) , arquivoCliente);
                            if (!File.Exists(arquivo)){
                                if(!File.Exists(arquivo) || new Cadastro().getUltimaLinha(arquivo) == 1){
                                    String[] cabecalho = new String[]{"Código venda", "Documento Cliente", "Produto", "Valor Venda", "Data Venda"};
                                    new Cadastro().gerarCabecalho(arquivo, cabecalho);
                                }
                            }
                            venda.salvar(arquivo);
                        } else {
                            Console.WriteLine("No momento não existem produtos disponíveis para venda!");
                        }
                        break;
                case 4: break;
                case 9: Environment.Exit(0); break;
            }
        } while(opt != 0);
    }

    /// <summary>
    /// Método para mostrar o Menu Tipo do Cliente
    /// </summary>
    /// <returns>Retorna "CPF" para pessoas físicas ou "CNPJ" para pessoas jurídicas</returns>
    private string mostrarMenuTipoCliente(){
        string tipoDoc;
        Console.WriteLine("Escolha o tipo do cliente:\n"
                    + "1 - Pessoa Física\n"
                    + "2 - Pessoa Jurídica\n");
        do{
            Console.Write("Opção: ");
            tipoDoc = Console.ReadLine();
        } while( !tipoDoc.Equals("1") && !tipoDoc.Equals("2"));
        return tipoDoc.Equals("1") ? "CPF" : "CNPJ";
    }

    /// <summary>
    /// Método para mostrar o menu para selecionar o produto para venda
    /// </summary>
    /// <param name="resultado">Lista dos códigos dos produtos disponíveis para venda</param>
    /// <returns>retorna inteiro com o código do produto</returns>
    private int mostrarMenuSelecionarProduto(List<int> resultado){
        int opt;
        if(resultado.Count != 0){
            do{
                Console.Write("Digite o código do produto: ");
                opt = Int16.Parse(Console.ReadLine());
            }while(!resultado.Contains(opt));
            return opt;
        }
        return 0;
    }
}