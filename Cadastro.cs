using System;
using System.IO;
using System.Collections;
using NetOffice.ExcelApi;

/// <summary>
/// Classe Cadastro
/// </summary>
public class Cadastro{
    /// <summary>
    /// Método que entrega a ultima linha do arquivo passado como referência
    /// </summary>
    /// <param name="arquivo">Path completo do arquivo de cadastro</param>
    /// <returns>Retorna o número da última linha do arquivo de cadastro</returns>
    public int getUltimaLinha(String arquivo){
        int contador = 0;
        Application ex = new Application();
        if(File.Exists(arquivo)){
            ex.Workbooks.Open(arquivo);
            do{
                contador++;
            } while (ex.Cells[contador,1].Value != null);
            ex.ActiveWorkbook.Close();
            ex.Quit();
            ex.Dispose();
        } else {
            contador = 1;
        }
        return contador;
    }

    /// <summary>
    /// Método para gerar o cabeçalho dos arquivos de cadastro
    /// </summary>
    /// <param name="arquivo">Path completo do arquivo de cadastro </param>
    /// <param name="cabecalho"></param>
    public void gerarCabecalho(String arquivo, String[] cabecalho){
            Application ex = new Application();
            bool existeArquivo = File.Exists(arquivo);
            if(!existeArquivo){
                ex.Workbooks.Add();
            } else {
                ex.Workbooks.Open(arquivo);
            }
            if(!File.Exists(arquivo) || getUltimaLinha(arquivo) == 1){
                for (int i = 0; i < cabecalho.Length; i++){
                    ex.Cells[1,i+1].Value = cabecalho[i];
                }
            }
            if(existeArquivo){
                ex.ActiveWorkbook.Save();
            } else {
                ex.ActiveWorkbook.SaveAs(arquivo);
            }
            ex.ActiveWorkbook.Close();
            ex.Quit();
            ex.Dispose();
        }

    /*public ArrayList buscar(String arquivo, String campo, String busca ){
        if(File.Exists(arquivo)){
            ArrayList codigos = new ArrayList();
            Application ex = new Application();
            ex.Workbooks.Open(arquivo);
            int numCampo = 1;
            String cabecalho = null, resultado = null;
            while(!ex.Cells[1,numCampo].Value.ToString().Equals(campo)){
                numCampo++;
            }
            int linha = 0;
            do{
                linha++;
                if(ex.Cells[linha, numCampo].Value.ToString().Equals(busca)){
                    numCampo = 1;
                    while(!ex.Cells[linha, numCampo].Value.Equals(null)){
                        if(numCampo == 1){
                            codigos.Add(ex.Cells[linha, numCampo].Value);
                        } 
                        resultado += ex.Cells[linha, numCampo].Value.ToString() + " | ";
                        numCampo++;
                    }
                    if(!resultado.Equals(null)){
                        resultado += "\n";
                    }
                }
            } while (ex.Cells[linha,1].Value != null);
            if(!resultado.Equals(null)){
                numCampo = 1;
                while(!ex.Cells[linha, numCampo].Value.Equals(null)){
                    cabecalho += ex.Cells[1, numCampo].Value.ToString() + " | ";
                    numCampo++;
                }
                Console.WriteLine("Resultado(s) encontrado(s): ");
                Console.WriteLine(cabecalho);
                Console.WriteLine(resultado);
                return codigos;
            } else {
                Console.WriteLine("O termo buscado não foi encontrado");
                return null;
            } 
            ex.Quit();
        } else {
            Console.WriteLine("O arquivo " + arquivo + " não foi encontrado!");
            return null;
        }
    }*/
    /*public void ler(String arquivo){
        if(File.Exists(arquivo)){
            Application ex = new Application();
            ex.Workbooks.Open(arquivo);
            int linha = 1;
            int campo = 1;
            string resultado = null, cabecalho = null;
            while(!ex.Cells[linha, campo].Value.Equals(null)){
                campo = 1;
                while(!ex.Cells[linha, campo].Value.Equals(null)){
                    resultado += ex.Cells[linha, campo].Value.ToString() + " | ";
                    campo++;
                }
                resultado += "\n";
                linha++;
            }
            if(!resultado.Equals(null)){
                campo = 1;
                while(!ex.Cells[linha, campo].Value.Equals(null)){
                    cabecalho += ex.Cells[1, campo].Value.ToString() + " | ";
                    campo++;
                }
                Console.WriteLine("Resultado(s) encontrado(s): ");
                Console.WriteLine(cabecalho);
                Console.WriteLine(resultado);
            } else {
                Console.WriteLine("O termo buscado não foi encontrado");
            } 
            ex.Quit();
        } else {
            Console.WriteLine("O arquivo " + arquivo + " não foi encontrado!");
        }
    }*/
}