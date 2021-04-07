using TabuleiroN;

namespace Xadrez
{
    class Torre : Peca
    {
        public Torre(Tabuleiro t, Cor cor) : base(t, cor) { }

        public override string ToString() => "T";

    }
}
