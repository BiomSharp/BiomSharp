// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Imaging.Wsq.Tree
{
    internal class QTreeElement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int LenX { get; set; }
        public int LenY { get; set; }
        public QTreeElement() { }
    }

    internal class QTree
    {
        public const int Length = 64;

        public QTreeElement[] Q { get; private set; }

        public QTree(WTree wtree)
        {
            Q = Enumerable
                .Range(0, Length)
                .Select(q => new QTreeElement())
                .ToArray();
            QTree16(3, wtree[14].LenX, wtree[14].LenY, wtree[14].X, wtree[14].Y, 0, 0);
            QTree16(19, wtree[4].LenX, wtree[4].LenY, wtree[4].X, wtree[4].Y, 0, 1);
            QTree16(48, wtree[0].LenX, wtree[0].LenY, wtree[0].X, wtree[0].Y, 0, 0);
            QTree16(35, wtree[5].LenX, wtree[5].LenY, wtree[5].X, wtree[5].Y, 1, 0);
            QTree4(0, wtree[19].LenX, wtree[19].LenY, wtree[19].X, wtree[19].Y);
        }

        public QTreeElement this[int i] => Q[i];

        private void QTree16(
            int p,
            int lx,
            int ly,
            int x,
            int y,
            int rw,
            int cl)
        {
            int evenx = lx % 2;
            int eveny = ly % 2;
            int tempx, temp2x;
            int tempy, temp2y;

            if (evenx == 0)
            {
                tempx = lx / 2;
                temp2x = tempx;
            }
            else
            {
                if (cl != 0)
                {
                    temp2x = (lx + 1) / 2;
                    tempx = temp2x - 1;
                }
                else
                {
                    tempx = (lx + 1) / 2;
                    temp2x = tempx - 1;
                }
            }

            if (eveny == 0)
            {
                tempy = ly / 2;
                temp2y = tempy;
            }
            else
            {
                if (rw != 0)
                {
                    temp2y = (ly + 1) / 2;
                    tempy = temp2y - 1;
                }
                else
                {
                    tempy = (ly + 1) / 2;
                    temp2y = tempy - 1;
                }
            }

            evenx = tempx % 2;
            eveny = tempy % 2;

            Q[p].X = x;
            Q[p + 2].X = x;
            Q[p].Y = y;
            Q[p + 1].Y = y;
            if (evenx == 0)
            {
                Q[p].LenX = tempx / 2;
                Q[p + 1].LenX = Q[p].LenX;
                Q[p + 2].LenX = Q[p].LenX;
                Q[p + 3].LenX = Q[p].LenX;
            }
            else
            {
                Q[p].LenX = (tempx + 1) / 2;
                Q[p + 1].LenX = Q[p].LenX - 1;
                Q[p + 2].LenX = Q[p].LenX;
                Q[p + 3].LenX = Q[p + 1].LenX;
            }
            Q[p + 1].X = x + Q[p].LenX;
            Q[p + 3].X = Q[p + 1].X;
            if (eveny == 0)
            {
                Q[p].LenY = tempy / 2;
                Q[p + 1].LenY = Q[p].LenY;
                Q[p + 2].LenY = Q[p].LenY;
                Q[p + 3].LenY = Q[p].LenY;
            }
            else
            {
                Q[p].LenY = (tempy + 1) / 2;
                Q[p + 1].LenY = Q[p].LenY;
                Q[p + 2].LenY = Q[p].LenY - 1;
                Q[p + 3].LenY = Q[p + 2].LenY;
            }
            Q[p + 2].Y = y + Q[p].LenY;
            Q[p + 3].Y = Q[p + 2].Y;

            evenx = temp2x % 2;

            Q[p + 4].X = x + tempx;
            Q[p + 6].X = Q[p + 4].X;
            Q[p + 4].Y = y;
            Q[p + 5].Y = y;
            Q[p + 6].Y = Q[p + 2].Y;
            Q[p + 7].Y = Q[p + 2].Y;
            Q[p + 4].LenY = Q[p].LenY;
            Q[p + 5].LenY = Q[p].LenY;
            Q[p + 6].LenY = Q[p + 2].LenY;
            Q[p + 7].LenY = Q[p + 2].LenY;
            if (evenx == 0)
            {
                Q[p + 4].LenX = temp2x / 2;
                Q[p + 5].LenX = Q[p + 4].LenX;
                Q[p + 6].LenX = Q[p + 4].LenX;
                Q[p + 7].LenX = Q[p + 4].LenX;
            }
            else
            {
                Q[p + 5].LenX = (temp2x + 1) / 2;
                Q[p + 4].LenX = Q[p + 5].LenX - 1;
                Q[p + 6].LenX = Q[p + 4].LenX;
                Q[p + 7].LenX = Q[p + 5].LenX;
            }
            Q[p + 5].X = Q[p + 4].X + Q[p + 4].LenX;
            Q[p + 7].X = Q[p + 5].X;


            eveny = temp2y % 2;

            Q[p + 8].X = x;
            Q[p + 9].X = Q[p + 1].X;
            Q[p + 10].X = x;
            Q[p + 11].X = Q[p + 1].X;
            Q[p + 8].Y = y + tempy;
            Q[p + 9].Y = Q[p + 8].Y;
            Q[p + 8].LenX = Q[p].LenX;
            Q[p + 9].LenX = Q[p + 1].LenX;
            Q[p + 10].LenX = Q[p].LenX;
            Q[p + 11].LenX = Q[p + 1].LenX;
            if (eveny == 0)
            {
                Q[p + 8].LenY = temp2y / 2;
                Q[p + 9].LenY = Q[p + 8].LenY;
                Q[p + 10].LenY = Q[p + 8].LenY;
                Q[p + 11].LenY = Q[p + 8].LenY;
            }
            else
            {
                Q[p + 10].LenY = (temp2y + 1) / 2;
                Q[p + 11].LenY = Q[p + 10].LenY;
                Q[p + 8].LenY = Q[p + 10].LenY - 1;
                Q[p + 9].LenY = Q[p + 8].LenY;
            }
            Q[p + 10].Y = Q[p + 8].Y + Q[p + 8].LenY;
            Q[p + 11].Y = Q[p + 10].Y;


            Q[p + 12].X = Q[p + 4].X;
            Q[p + 13].X = Q[p + 5].X;
            Q[p + 14].X = Q[p + 4].X;
            Q[p + 15].X = Q[p + 5].X;
            Q[p + 12].Y = Q[p + 8].Y;
            Q[p + 13].Y = Q[p + 8].Y;
            Q[p + 14].Y = Q[p + 10].Y;
            Q[p + 15].Y = Q[p + 10].Y;
            Q[p + 12].LenX = Q[p + 4].LenX;
            Q[p + 13].LenX = Q[p + 5].LenX;
            Q[p + 14].LenX = Q[p + 4].LenX;
            Q[p + 15].LenX = Q[p + 5].LenX;
            Q[p + 12].LenY = Q[p + 8].LenY;
            Q[p + 13].LenY = Q[p + 8].LenY;
            Q[p + 14].LenY = Q[p + 10].LenY;
            Q[p + 15].LenY = Q[p + 10].LenY;
        }

        private void QTree4(
            int p,
            int lx,
            int ly,
            int x,
            int y)
        {
            int evenx = lx % 2;
            int eveny = ly % 2;

            Q[p].X = x;
            Q[p + 2].X = x;
            Q[p].Y = y;
            Q[p + 1].Y = y;
            if (evenx == 0)
            {
                Q[p].LenX = lx / 2;
                Q[p + 1].LenX = Q[p].LenX;
                Q[p + 2].LenX = Q[p].LenX;
                Q[p + 3].LenX = Q[p].LenX;
            }
            else
            {
                Q[p].LenX = (lx + 1) / 2;
                Q[p + 1].LenX = Q[p].LenX - 1;
                Q[p + 2].LenX = Q[p].LenX;
                Q[p + 3].LenX = Q[p + 1].LenX;
            }
            Q[p + 1].X = x + Q[p].LenX;
            Q[p + 3].X = Q[p + 1].X;
            if (eveny == 0)
            {
                Q[p].LenY = ly / 2;
                Q[p + 1].LenY = Q[p].LenY;
                Q[p + 2].LenY = Q[p].LenY;
                Q[p + 3].LenY = Q[p].LenY;
            }
            else
            {
                Q[p].LenY = (ly + 1) / 2;
                Q[p + 1].LenY = Q[p].LenY;
                Q[p + 2].LenY = Q[p].LenY - 1;
                Q[p + 3].LenY = Q[p + 2].LenY;
            }
            Q[p + 2].Y = y + Q[p].LenY;
            Q[p + 3].Y = Q[p + 2].Y;
        }
    }
}
