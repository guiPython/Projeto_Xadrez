using System;
using TabuleiroN;
using Xadrez;

namespace Projeto_Xadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while( !partida .Terminada)
                {
                    Console.Clear();
                    Tela.PrintTabuleiro(partida.t);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.ReadPosXadrez().toPosicao();

                    Console.Write("Destino: ");
                    Posicao destino = Tela.ReadPosXadrez().toPosicao();

                    partida.ExecutaMovimeto(origem, destino);
                }
                Tela.PrintTabuleiro(partida.t);
            }
            catch (TabuleiroException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
