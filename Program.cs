using System;
using System.IO;

namespace SistemaVendas
{
    class Program
    {
        static void Main(string[] args)
        {
            string opcao;
            Console.WriteLine("Escola uma das opções abaixo\n"
                        + "1 - Cadastrar Clientes\n"
                        + "2 - Cadastrar Produtos\n"
                        + "3 - Realizar Vendas\n"
                        + "4 - Extrato Clientes\n"
                        + "0 - Sair\n");
            
            Console.Write("Opção: ");
            int opt = 0;            
            do{
                op2 = Console.ReadLine();
                opt = Int16.Parse(op2);
            } while (opt < 0 || opt > 6);

            switch(op2){
                case "0": Environment.Exit(0); break;
                case "1": validarCPF(); break;
                case "2": validarCNPJ(); break;
                case "3": validarCredito(); break;
                case "4": validarRG(); break;
                case "5": validarTitulo(); break;
                }
            } while(op2 != "0");
        }
    }
}
