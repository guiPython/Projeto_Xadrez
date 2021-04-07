

namespace TabuleiroN
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] Pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;
            Pecas = new Peca[linhas, colunas];
        }

        public Peca Peca(int lin, int col) => Pecas[lin, col];

        public Peca Peca(Posicao pos) {
           return Pecas[pos.Linha, pos.Coluna];
        }

        public bool ExistePeca(Posicao pos)
        {
            ValidarPosicao(pos);
            return Peca(pos) != null;
        }

        public void AddPeca( Peca p , Posicao pos)
        {
            if (ExistePeca(pos)) throw new TabuleiroException("Já existe uma peça nessa posição!");
            Pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }

        public Peca RemovePeca(Posicao pos)
        {
            if (Peca(pos) == null) return null;
            Peca aux = Peca(pos);
            aux.Posicao = null;
            Pecas[pos.Linha,pos.Coluna] = null;
            return aux;
        }

        public bool PosicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Linha >= Linhas || pos.Coluna < 0 || pos.Coluna >= Colunas) return false;
            return true;
        }

        public void ValidarPosicao(Posicao pos)
        {
            if (!PosicaoValida(pos)) throw new TabuleiroException("Posição Inválida!");
        }
    }
}
