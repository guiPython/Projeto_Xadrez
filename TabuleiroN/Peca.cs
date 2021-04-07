
namespace TabuleiroN
{
    class Peca
    {
        public Peca(Tabuleiro tab, Cor cor)
        {
            Tab = tab;
            Posicao = null;
            Cor = cor;
            QtdMovimentos = 0;
        }

        public Tabuleiro Tab { get; protected set; }
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QtdMovimentos { get; protected set; }

        public void IncrMovimetos() => QtdMovimentos++;
    }
}
