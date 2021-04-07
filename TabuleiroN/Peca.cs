
namespace TabuleiroN
{
    abstract class Peca
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

        public void DecrMovimentos() => QtdMovimentos--;
        public void IncrMovimetos() => QtdMovimentos++;

        public bool PodeMoverPara(Posicao pos)
        {
            bool[,] mat = MovimentosPossiveis();
            if (mat[pos.Linha, pos.Coluna] == true) return true;
            return false;

        }
        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for( int i = 0; i < Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i, j]) return true;
                }
            }
            return false;
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
