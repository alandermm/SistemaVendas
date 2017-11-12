using System;
using System.IO;
using NetOffice.ExcelApi;

/// <summary>
/// Classe Carro
/// </summary>
public class Produto{
    public int codigo {get; set;}
    public string nome {get; set;}
    public string descricao {get; set;}
    public double preco {get; set;}

    /// <summary>
    /// Método para inicar dados do objeto Produto
    /// </summary>
    public void iniciarDados(){
        string arquivo = Directory.GetCurrentDirectory() + "\\Produtos.xlsx";
        this.codigo = (new Cadastro().getUltimaLinha(arquivo)) - 1;
        Console.Write("Nome do produto: ");
        this.nome = Console.ReadLine();
        Console.Write("Breve descrição do produto: ");
        this.descricao = Console.ReadLine();
        Console.Write("Valor do produto: ");
        this.preco = double.Parse(Console.ReadLine());
    }

    /// <summary>
    /// Método para salvar os dados do produto no arquivo de cadastro
    /// </summary>
    /// <param name="arquivo">Path completo para o arquivo de cadastro do produto</param>
    public void salvar(String arquivo){
        Application ex = new Application();
        int ultimaLinha = new Cadastro().getUltimaLinha(arquivo);
        ex.Workbooks.Open(arquivo);
        ex.Cells[ultimaLinha, 1].Value = this.codigo;
        ex.Cells[ultimaLinha, 2].Value = this.nome;
        ex.Cells[ultimaLinha, 3].Value = this.descricao;
        ex.Cells[ultimaLinha, 4].Value = this.preco;
        ex.Cells[ultimaLinha, 5].Value = DateTime.Now;
        ex.ActiveWorkbook.Save();
        ex.ActiveWorkbook.Close();
        ex.Quit();
        ex.Dispose();
    }

    /// <summary>
    /// Método para carregar um objeto produto
    /// </summary>
    /// <param name="codigoCarro">Código do produto a ser carregado no objeto</param>
    /// <returns>Retorna o objeto Produto</returns>
    public Produto carregarCarro(int codigoProduto){
        String arquivo = Directory.GetCurrentDirectory() + "\\Carros.xlsx";
        Application ex = new Application();
        ex.Workbooks.Open(arquivo);
        Produto carro = new Produto();
        int linha = codigoProduto + 1;
        /*while(!ex.Cells[linha, 1].Value.ToString().Contains(codigoProduto.ToString()) && ex.Cells[linha,1].Value != null ){
            linha++;
        }*/
        carro.codigo = Int16.Parse(ex.Cells[linha, 1].Value.ToString());
        carro.nome = ex.Cells[linha, 2].Value.ToString();
        carro.descricao = ex.Cells[linha, 3].Value.ToString();
        carro.preco = double.Parse(ex.Cells[linha, 4].Value.ToString());
        ex.ActiveWorkbook.Close();
        ex.Quit();
        ex.Dispose();
        return carro;
    }

    /// <summary>
    /// Método para alterar o valor do campo Disponível para "Não"
    /// </summary>
    /// <param name="codigo">Código do carro vendido</param>
    public void vender(int codigo){
        Application ex = new Application();
        String arquivo = Directory.GetCurrentDirectory() + "\\Carros.xlsx";
        ex.Workbooks.Open(arquivo);
        ex.Cells[codigo + 1, 13].Value = "Não";
        ex.ActiveWorkbook.Save();
        ex.ActiveWorkbook.Close();
        ex.Quit();
        ex.Dispose();
    }
}