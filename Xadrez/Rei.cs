using TabuleiroN;

namespace Xadrez
{
    class Rei : Peca
    {
        private PartidaDeXadrez Partida;
        public Rei(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor)
        {
            Partida = partida;
        }

        public override string ToString() => "R";

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.Peca(pos);
            return p == null || p.Cor != Cor;
        }

        private bool testeTorreParaRoque(Posicao pos)
        {
            Peca p = Tab.Peca(pos);
            return p != null && p is Torre && p.Cor == Cor && p.QtdMovimentos == 0;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            //Acima
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) mat[pos.Linha, pos.Coluna] = true;

            //Nordeste
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) mat[pos.Linha, pos.Coluna] = true;

            //Direita
            pos.definirValores(Posicao.Linha, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) mat[pos.Linha, pos.Coluna] = true;

            //Sudeste
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) mat[pos.Linha, pos.Coluna] = true;

            //Abaixo
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) mat[pos.Linha, pos.Coluna] = true;

            //Sudoeste
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) mat[pos.Linha, pos.Coluna] = true;

            //Esquerda
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) mat[pos.Linha, pos.Coluna] = true;

            //Noroeste
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && PodeMover(pos)) mat[pos.Linha, pos.Coluna] = true;

            // #Jogada Especial Roque
            if (QtdMovimentos == 0! && !Partida.Xeque)
            {
                //Roque Pequeno
                Posicao posT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);

                if (testeTorreParaRoque(posT1))
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);
                    if(Tab.Peca(p1) == null && Tab.Peca(p2) == null)
                    {
                        mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                    }
                }

                //Roque Grande
                Posicao posT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);

                if (testeTorreParaRoque(posT2))
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                    Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);
                    if (Tab.Peca(p1) == null && Tab.Peca(p2) == null && Tab.Peca(p3) == null)
                    {
                        mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                    }
                }
            }

                return mat;
        }

    }
}
