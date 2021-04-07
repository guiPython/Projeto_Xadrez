using TabuleiroN;
using Xadrez;
using System;
using System.Collections.Generic;

namespace Projeto_Xadrez
{
    class Tela
    {
        public static void PrintPartida(PartidaDeXadrez partida)
        {
            PrintTabuleiro(partida.t);
            Console.WriteLine();
            PrintPecasCapturadas(partida);
            Console.WriteLine($"\n\nTurno: {partida.turno}");
            Console.WriteLine($"Aguardando a jogada: {partida.JogadorAtual}");
        }
        public static void PrintTabuleiro(Tabuleiro t)
        {
            for( int i = 0; i < t.Linhas; i++)
            {
                Console.Write($"{8-i} ");
                for( int j = 0; j < t.Colunas; j++)
                {
                    PrintPeca(t.Peca(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void PrintTabuleiro(Tabuleiro t, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;
            for (int i = 0; i < t.Linhas; i++)
            {
                Console.Write($"{8 - i} ");
                for (int j = 0; j < t.Colunas; j++)
                {
                    if (posicoesPossiveis[i, j]) Console.BackgroundColor = fundoAlterado;
                    else Console.BackgroundColor = fundoOriginal;
                    PrintPeca(t.Peca(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = fundoOriginal;
        }

        public static void PrintPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("Peças Capturadas: ");
            Console.Write("Brancas: ");
            PrintConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.WriteLine();
            Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;
        }

        public static void PrintConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            foreach (Peca x in conjunto) Console.Write($"{x} ");
            Console.Write("]");
        }

        public static void PrintPeca(Peca p)
        {
            if (p == null) Console.Write("- ");
            else
            {
                if (p.Cor == Cor.Branca) Console.Write(p);
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(p);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static PosicaoXadrez ReadPosXadrez()
        {
            string s = Console.ReadLine();
            char col = s[0];
            int lin = int.Parse(s[1] + "");

            return new PosicaoXadrez(col, lin);

        }
    }
}
