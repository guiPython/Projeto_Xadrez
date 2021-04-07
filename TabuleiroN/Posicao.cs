
namespace TabuleiroN
{
    class Posicao
    {
        public Posicao(int linha, int coluna)
        {
            Linha = linha;
            Coluna = coluna;
        }

        public int Linha { get;  set; }
        public int Coluna { get;  set; }

        public override string ToString() => $"{Linha}, {Coluna}";
        public void definirValores(int linha, int coluna)
        {
            Linha = linha;
            Coluna = coluna;
        }
    }
}
