namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	/// <summary>
	/// ISO/IEC 18004:2000 Chapter 8.8.2 Page 52
	/// </summary>
	internal class Penalty3 : Penalty
    {

        private Penalty3DecisionTree bitCheckTree = new Penalty3DecisionTree();

        /// <summary>
		/// Calculate penalty value for Third rule.
		/// </summary>
        internal override int PenaltyCalculate(BitMatrix matrix)
        {
            MatrixSize size = matrix.Size;
            int penaltyValue = 0;
            for (int i = 0; i < size.Height; i++)
            {
                penaltyValue += MidPatternSearch(matrix, new MatrixPoint(0, i), true);
            }

            for (int i = 0; i < size.Width; i++)
            {
                penaltyValue += MidPatternSearch(matrix, new MatrixPoint(i, 0), false);
            }


            return penaltyValue;
        }

        /// <summary>
        /// Mid pattern(oxxxo) search for horizontal or vertical line. 
        /// </summary>
        /// <param name="position">Starting position for horizontal or vertical line</param>
        private int MidPatternSearch(BitMatrix matrix, MatrixPoint position, bool isHorizontal)
        {
            return MidPatternSearch(matrix, position, 0, isHorizontal);
        }


        /// <summary>
        /// Use position offset to search from next position to end. 
        /// </summary>
        private int MidPatternSearch(BitMatrix matrix, MatrixPoint position, int indexJumpValue, bool isHorizontal)
        {
            MatrixSize size = matrix.Size;
            MatrixPoint newPosition;
            if (isInsideMatrix(size, position, indexJumpValue, isHorizontal))
                newPosition = isHorizontal ? position.Offset(indexJumpValue, 0)
                    : position.Offset(0, indexJumpValue);
            else
                return 0;
            return MidPatternCheck(matrix, newPosition, bitCheckTree.Root, isHorizontal);
        }


        /// <summary>
        /// Use decision tree node to check for Mid pattern. According to decision tree to decide next action. 
        /// </summary>
        /// <param name="checkNode">Decision tree from Penalty3DecisionTree. Indicate search action</param>
        private int MidPatternCheck(BitMatrix matrix, MatrixPoint position, BitBinaryTreeNode<Penalty3DecisionNode> checkNode, bool isHorizontal)
        {
            Penalty3DecisionNode checkValue = checkNode.Value;
            MatrixSize size = matrix.Size;
            if( checkValue.IndexJumpValue > 0 )
            {
                return MidPatternSearch(matrix, position, checkValue.IndexJumpValue, isHorizontal);
            }
            else if (checkValue.IndexJumpValue == 0)
            {
                int penaltyValue = PatternCheck(matrix, position, isHorizontal);
                return penaltyValue + MidPatternSearch(matrix, position, 4, isHorizontal);
            }
            else
            {
                MatrixPoint checkIndex;
                if (isInsideMatrix(size, position, checkValue.BitCheckIndex, isHorizontal))
                    checkIndex = isHorizontal ? position.Offset(checkValue.BitCheckIndex, 0)
                    : position.Offset(0, checkValue.BitCheckIndex);
                else
                    return 0;

                return matrix[checkIndex] ? MidPatternCheck(matrix, position, checkNode.One, isHorizontal)
                    : MidPatternCheck(matrix, position, checkNode.Zero, isHorizontal);
                
            }
        }


        /// <summary>
        /// Perform patterncheck after mid pattern(oxxxo) successfully found.
		/// Patterncheck will check at question mark position. (?oxxxo?) 
		/// Return true for if both positions are x		
        /// </summary>
        private int PatternCheck(BitMatrix matrix, MatrixPoint position, bool isHorizontal)
        {
            MatrixPoint FrontOnePos;
            FrontOnePos = isHorizontal ? position.Offset(-1, 0)
                : position.Offset(0, -1);
            MatrixPoint EndOnePos;
            EndOnePos = isHorizontal ? position.Offset(5, 0)
                : position.Offset(0, 5);
            MatrixSize size = matrix.Size;
            if (isOutsideMatrix(size, EndOnePos))
            {
                return 0;
            }
            else if (isOutsideMatrix(size, FrontOnePos))
            {
                return 0;
            }
            else
            {
                if (matrix[FrontOnePos] && matrix[EndOnePos])
                    return LightAreaCheck(matrix, position, isHorizontal);
                else
                    return 0;
            }

        }

        /// <summary>
        /// Perform lightAreaCheck after patterncheck return true. 
        /// LightAreaCheck will check both side of pattern (xoxxxox). 
        /// Return penalty score if it found light area at either side.(oooo)
        /// </summary>
        /// <remarks>This check only available at ISO/IEC 18004:2004. Newest international QrCode standard.
        /// But not on old ISO/IEC18004:2000 standard</remarks>
        private int LightAreaCheck(BitMatrix matrix, MatrixPoint position, bool isHorizontal)
        {
            MatrixSize size = matrix.Size;
            MatrixPoint LeftCheckPoint = isHorizontal ? position.Offset(-5, 0)
                : position.Offset(0, -5);
            MatrixPoint RightCheckPoint = isHorizontal ? position.Offset(9, 0)
                : position.Offset(0, 9);
            int penaltyValue = 0;
            penaltyValue = OneSideWhiteAreaCheck(matrix, LeftCheckPoint, true, isHorizontal) ? penaltyValue + 40 : penaltyValue;
            penaltyValue = OneSideWhiteAreaCheck(matrix, RightCheckPoint, false, isHorizontal) ? penaltyValue + 40 : penaltyValue;

            //penaltyValue = penaltyValue == 80 ? 40 : penaltyValue;
            return penaltyValue;
        }
        
        /// <summary>
        /// LightAreaCheck will call this method for 4 false modules(oooo) check. 
        /// </summary>
        private bool OneSideWhiteAreaCheck(BitMatrix matrix, MatrixPoint checkPoint, bool isLeftSide, bool isHorizontal)
        {
        	int WhiteModuleCount = 0;
        	int offsetValue = isLeftSide ? 1 : -1;
        	MatrixSize size = matrix.Size;
        	if(isInsideMatrix(size, checkPoint))
            {
        		for (int i = 0; i < 4; i++)
            	{
            		if (matrix[checkPoint] == false)
               		{
                 		WhiteModuleCount++;
                    	checkPoint = isHorizontal ? checkPoint.Offset(offsetValue, 0)
                    		: checkPoint.Offset(0, offsetValue);
                	}
                	else
                		break;
           		}
        	}
        	
        	return WhiteModuleCount == 4;
        }


        private bool isOutsideMatrix(MatrixSize size, MatrixPoint position)
        {
            return position.X >= size.Width || position.X < 0 || position.Y >= size.Height || position.Y < 0;
        }

        private bool isInsideMatrix(MatrixSize size, MatrixPoint position)
        {
            return !isOutsideMatrix(size, position);
        }

        private bool isInsideMatrix(MatrixSize size, MatrixPoint position, int indexJumpValue, bool isHorizontal)
        {
            if (isHorizontal)
            {
                return size.Width > (position.X + indexJumpValue);
            }
            else
            {
                return size.Height > (position.Y + indexJumpValue);
            }
        }

    }
}
