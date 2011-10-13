namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty2 : Penalty
    {
        private Penalty2CheckTree bitCheckTree = new Penalty2CheckTree();

        internal override int PenaltyCalculate(BitMatrix matrix)
        {
            Size size = matrix.Size;
            int penaltyValue = 0;

            if (size.Height < 2)
                return penaltyValue;

            for (int i = 0; i < size.Height; i++)
            {
                penaltyValue += SquareBlockSearch(matrix, new Point(0, i));
            }

            return penaltyValue;
        }


        private int SquareBlockSearch(BitMatrix matrix, Point position)
        {
            return SquareBlockSearch(matrix, position, 0);
        }

        private int SquareBlockSearch(BitMatrix matrix, Point position, int indexJumpValue)
        {
            Size size = matrix.Size;
            Point newPosition;

            if (isInsideMatrix(size, position, indexJumpValue))
            {
                newPosition = position.Offset(indexJumpValue, 0);
            }
            else
                return 0;

            return SquareBlockCheck(matrix, newPosition, bitCheckTree.Root);
        }

        private int SquareBlockCheck(BitMatrix matrix, Point position, BitBinaryTreeNode<Penalty2NodeValue> checkNode)
        {
            Penalty2NodeValue checkValue = checkNode.Value;
            Size size = matrix.Size;

            if (checkValue.IndexJumpValue > 0)
            {
                return SquareBlockSearch(matrix, position, checkValue.IndexJumpValue);
            }
            else if (checkValue.IndexJumpValue == 0)
            {
                return 3 + SquareBlockSearch(matrix, position, 1);
            }
            else
            {
                Point checkIndex = position.Offset(checkValue.BitCheckPoint);

                if (isOutsideMatrix(size, checkIndex))
                    return 0;

                return matrix[position] == matrix[checkIndex] ? SquareBlockCheck(matrix, position, checkNode.One)
                    : SquareBlockCheck(matrix, position, checkNode.Zero);
            }

        }

        private bool isOutsideMatrix(Size size, Point position)
        {
            return position.X >= size.Width || position.X < 0 || position.Y >= size.Height || position.Y < 0;
        }

        private bool isInsideMatrix(Size size, Point position, int indexJumpValue)
        {
            return size.Width > (position.X + indexJumpValue);
        }


    }
}

