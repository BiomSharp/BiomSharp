// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Imaging.Wsq.Tree
{
    internal class WTreeElement
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int LenX { get; set; }
        public int LenY { get; set; }
        public int InvR { get; set; }
        public int InvC { get; set; }

        public WTreeElement() { }
    }

    internal class WTree
    {
        public const int Length = 20;
        public WTreeElement[] W { get; private set; }

        public WTree(int width, int height)
        {
            W = Enumerable
                .Range(0, Length)
                .Select(w => new WTreeElement())
                .ToArray();
            W[2].InvR = 1;
            W[4].InvR = 1;
            W[7].InvR = 1;
            W[9].InvR = 1;
            W[11].InvR = 1;
            W[13].InvR = 1;
            W[16].InvR = 1;
            W[18].InvR = 1;
            W[3].InvC = 1;
            W[5].InvC = 1;
            W[8].InvC = 1;
            W[9].InvC = 1;
            W[12].InvC = 1;
            W[13].InvC = 1;
            W[17].InvC = 1;
            W[18].InvC = 1;

            WTree4(0, 1, width, height, 0, 0, 1);

            int lenx, lenx2, leny, leny2;
            if (W[1].LenX % 2 == 0)
            {
                lenx = W[1].LenX / 2;
                lenx2 = lenx;
            }
            else
            {
                lenx = (W[1].LenX + 1) / 2;
                lenx2 = lenx - 1;
            }

            if (W[1].LenY % 2 == 0)
            {
                leny = W[1].LenY / 2;
                leny2 = leny;
            }
            else
            {
                leny = (W[1].LenY + 1) / 2;
                leny2 = leny - 1;
            }

            WTree4(4, 6, lenx2, leny, lenx, 0, 0);
            WTree4(5, 10, lenx, leny2, 0, leny, 0);
            WTree4(14, 15, lenx, leny, 0, 0, 0);

            W[19].X = 0;
            W[19].Y = 0;
            if (W[15].LenX % 2 == 0)
            {
                W[19].LenX = W[15].LenX / 2;
            }
            else
            {
                W[19].LenX = (W[15].LenX + 1) / 2;
            }
            if (W[15].LenY % 2 == 0)
            {
                W[19].LenY = W[15].LenY / 2;
            }
            else
            {
                W[19].LenY = (W[15].LenY + 1) / 2;
            }
        }

        public WTreeElement this[int i] => W[i];

        private void WTree4(
            int p1,
            int p2,
            int lx,
            int ly,
            int x,
            int y,
            int stop1)
        {
            W[p1].X = x;
            W[p1].Y = y;
            W[p1].LenX = lx;
            W[p1].LenY = ly;

            W[p2].X = x;
            W[p2 + 2].X = x;
            W[p2].Y = y;
            W[p2 + 1].Y = y;

            if (lx % 2 == 0)
            {
                W[p2].LenX = lx / 2;
                W[p2 + 1].LenX = W[p2].LenX;
            }
            else
            {
                if (p1 == 4)
                {
                    W[p2].LenX = (lx - 1) / 2;
                    W[p2 + 1].LenX = W[p2].LenX + 1;
                }
                else
                {
                    W[p2].LenX = (lx + 1) / 2;
                    W[p2 + 1].LenX = W[p2].LenX - 1;
                }
            }
            W[p2 + 1].X = W[p2].LenX + x;
            if (stop1 == 0)
            {
                W[p2 + 3].LenX = W[p2 + 1].LenX;
                W[p2 + 3].X = W[p2 + 1].X;
            }
            W[p2 + 2].LenX = W[p2].LenX;


            if (ly % 2 == 0)
            {
                W[p2].LenY = ly / 2;
                W[p2 + 2].LenY = W[p2].LenY;
            }
            else
            {
                if (p1 == 5)
                {
                    W[p2].LenY = (ly - 1) / 2;
                    W[p2 + 2].LenY = W[p2].LenY + 1;
                }
                else
                {
                    W[p2].LenY = (ly + 1) / 2;
                    W[p2 + 2].LenY = W[p2].LenY - 1;
                }
            }
            W[p2 + 2].Y = W[p2].LenY + y;
            if (stop1 == 0)
            {
                W[p2 + 3].LenY = W[p2 + 2].LenY;
                W[p2 + 3].Y = W[p2 + 2].Y;
            }
            W[p2 + 1].LenY = W[p2].LenY;
        }
    }
}
