using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using NetOffice.ExcelApi;

/// <summary>
/// Classe Venda
/// </summary>
public class Venda{
    
    public Pessoa cliente {get; set;}
    public Produto produto {get; set;}
    public double valorVenda {get; set;}

    /// <summary>
    /// Método para listar os produtos disponíveis para venda
    /// </summary>
    /// <returns>Lista de códigos dos produtos disponíveis para venda</returns>
    public List<int> listarProdutosDisponiveis(){
        String arquivo = Directory.GetCurrentDirectory() + "\\Produtos.xlsx";
        List<int> codigos = new List<int>();
        Application ex = new Application();
        if (File.Exists(arquivo)){
            ex.Workbooks.Open(arquivo);
            int count = 0, linha = 1, campo = 1;
            while(ex.Cells[1, campo].Value != null){
                Console.Write(ex.Cells[1, campo].Value.ToString() + " | ");
                campo++;
            }
            Console.WriteLine();
            while(ex.Cells[linha, 1].Value != null){
                codigos.Add(Int16.Parse(ex.Cells[linha, 1].Value.ToString()));
                campo = 1;
                count++;
                while(ex.Cells[linha, campo].Value != null){
                    Console.Write(ex.Cells[linha, campo].Value.ToString() + " | ");
                    campo++;
                }
                Console.WriteLine();
                
                linha++;
            }
            Console.WriteLine("\n" + count + " Produtos disponíveis." + "\n\n");
            ex.ActiveWorkbook.Close();
            ex.Quit();
            ex.Dispose();
            return codigos;
        } else {
            Console.WriteLine("O arquivo " + arquivo + " não foi encontrado.\n\n");
            return null;
        }
    }

    /// <summary>
    /// Método para salvar os dados da venda no arquivo de cadastro
    /// </summary>
    /// <param name="arquivo">Path completo para o arquivo de cadastro de vendas</param>
    public void salvar(String arquivo){
        Application ex = new Application();
        int ultimaLinha = new Cadastro().getUltimaLinha(arquivo);
        ex.Workbooks.Open(arquivo);
        ex.Cells[ultimaLinha, 1].Value = this.produto.codigo;
        ex.Cells[ultimaLinha, 2].Value = this.cliente.documento;
        ex.Cells[ultimaLinha, 3].Value = this.valorVenda;
        ex.Cells[ultimaLinha, 4].Value = DateTime.Now;
        ex.ActiveWorkbook.Save();
        ex.ActiveWorkbook.Close();
        ex.Quit();
        ex.Dispose();
    }
}