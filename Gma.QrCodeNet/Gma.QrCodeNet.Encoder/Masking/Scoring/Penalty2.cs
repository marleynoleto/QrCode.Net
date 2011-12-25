namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	/// <summary>
	/// ISO/IEC 18004:2000 Chapter 8.8.2 Page 52
	/// </summary>
	internal class Penalty2 : Penalty
    {
        private Penalty2DecisionTree bitCheckTree = new Penalty2DecisionTree();

        /// <summary>
		/// Calculate penalty value for Second rule.
		/// </summary>
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


        /// <summary>
        /// Search for one horizontal line for penalty 2 scores. 
        /// </summary>
        /// <param name="position">Starting position for horizontal line</param>
        private int SquareBlockSearch(BitMatrix matrix, MatrixPoint position)
        {
            return SquareBlockSearch(matrix, position, 0);
        }

        /// <summary>
        /// Penalty value sum from next check position to end of array. 
        /// </summary>
        /// <param name="position">Current position</param>
        /// <param name="indexJumpValue">Off set for current position indicate next check position</param>
        /// <returns>penalty value from next check position to end</returns>
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

        /// <summary>
        /// Use decision tree to check 2x2 square block. Then decide the next check position for penalty score 
        /// </summary>
        /// <param name="position">current position</param>
        /// <param name="checkNode">decision tree's check node. With compare result and indicate next action</param>
        private int SquareBlockCheck(BitMatrix matrix, MatrixPoint position, BitBinaryTreeNode<Penalty2DecisionNode> checkNode)
        {
            Penalty2DecisionNode checkValue = checkNode.Value;
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

