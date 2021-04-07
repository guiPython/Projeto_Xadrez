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
                    try
                    {
                        Console.Clear();
                        Tela.PrintPartida(partida);

                        Console.Write("\nOrigem: ");
                        Posicao origem = Tela.ReadPosXadrez().toPosicao();
                        partida.ValidarPosOrigem(origem);
                        bool[,] posicoesPossiveis = partida.t.Peca(origem).MovimentosPossiveis();

                        Console.Clear();
                        Tela.PrintTabuleiro(partida.t, posicoesPossiveis);

                        Console.Write("\nDestino: ");
                        Posicao destino = Tela.ReadPosXadrez().toPosicao();
                        partida.ValidarPosDestino(origem,destino);

                        partida.RealizaJogada(origem, destino);
                    }
                    catch (TabuleiroException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
                Tela.PrintPartida(partida);
            }
            catch (TabuleiroException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
