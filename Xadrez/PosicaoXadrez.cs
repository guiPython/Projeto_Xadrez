using TabuleiroN;

namespace Xadrez
{
    class PosicaoXadrez
    {
        public PosicaoXadrez(char coluna, int linha)
        {
            Coluna = coluna;
            Linha = linha;
        }

        public char Coluna { get; set; }
        public int Linha { get; set; }

        public Posicao toPosicao()
        {
            return new Posicao(8 - Linha, Coluna - 'a');
        }
        public override string ToString() => $"{Coluna}{Linha}";
    }
}
