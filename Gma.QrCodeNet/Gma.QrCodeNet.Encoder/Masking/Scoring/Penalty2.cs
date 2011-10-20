namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty2 : Penalty
    {
        private Penalty2DecitionTree bitCheckTree = new Penalty2DecitionTree();

        internal override int PenaltyCalculate(BitMatrix matrix)
        {
            MatrixSize size = matrix.Size;
            int penaltyValue = 0;

            if (size.Height < 2)
                return penaltyValue;

            for (int i = 0; i < size.Height; i++)
            {
                penaltyValue += SquareBlockSearch(matrix, new MatrixPoint(0, i));
            }

            return penaltyValue;
        }


        private int SquareBlockSearch(BitMatrix matrix, MatrixPoint position)
        {
            return SquareBlockSearch(matrix, position, 0);
        }

        private int SquareBlockSearch(BitMatrix matrix, MatrixPoint position, int indexJumpValue)
        {
            MatrixSize size = matrix.Size;
            MatrixPoint newPosition;

            if (isInsideMatrix(size, position, indexJumpValue))
            {
                newPosition = position.Offset(indexJumpValue, 0);
            }
            else
                return 0;

            return SquareBlockCheck(matrix, newPosition, bitCheckTree.Root);
        }

        private int SquareBlockCheck(BitMatrix matrix, MatrixPoint position, BitBinaryTreeNode<Penalty2DecitionNode> checkNode)
        {
            Penalty2DecitionNode checkValue = checkNode.Value;
            MatrixSize size = matrix.Size;

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
                MatrixPoint checkIndex = position.Offset(checkValue.BitCheckPoint);

                if (isOutsideMatrix(size, checkIndex))
                    return 0;

                return matrix[position] == matrix[checkIndex] ? SquareBlockCheck(matrix, position, checkNode.One)
                    : SquareBlockCheck(matrix, position, checkNode.Zero);
            }

        }

        private bool isOutsideMatrix(MatrixSize size, MatrixPoint position)
        {
            return position.X >= size.Width || position.X < 0 || position.Y >= size.Height || position.Y < 0;
        }

        private bool isInsideMatrix(MatrixSize size, MatrixPoint position, int indexJumpValue)
        {
            return size.Width > (position.X + indexJumpValue);
        }


    }
}

