namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	/// <summary>
	/// ISO/IEC 18004:2000 Chapter 8.8.2 Page 52
	/// </summary>
	internal class Penalty1 : Penalty
    {

		/// <summary>
		/// Calculate penalty value for first rule.
		/// </summary>
        internal override int PenaltyCalculate(BitMatrix matrix)
        {
            MatrixSize size = matrix.Size;
            int penaltyValue = 0;

            for (int i = 0; i < size.Height; i++)
            {
                penaltyValue += FiveSameModuleSearch(matrix, new MatrixPoint(0, i), true);
            }

            for (int i = 0; i < size.Width; i++)
            {
                penaltyValue += FiveSameModuleSearch(matrix, new MatrixPoint(i, 0), false);
            }


            return penaltyValue;
        }

		/// <summary>
		/// Check one line of matrix, search for constant five or more than five same color modules 
		/// </summary>
		/// <param name="position">starting position</param>
        private int FiveSameModuleSearch(BitMatrix matrix, MatrixPoint position, bool isHorizontal)
        {
            MatrixSize size = matrix.Size;

            int ModuleCount = FiveSameModuleCheck(matrix, position, isHorizontal);

            if (ModuleCount < 5)
            {
                int NextSearchIndex = 5 - ModuleCount;

                return FiveSameModuleSearch(matrix, position, NextSearchIndex, isHorizontal);
            }
            else if (ModuleCount == 5)
            {
                int ExceedNumber = SameModuleExceedNumberCheck(matrix, position, isHorizontal);
                int NextSearchIndex = 4 + ExceedNumber + 1;

                return 3 + ExceedNumber + FiveSameModuleSearch(matrix, position, NextSearchIndex, isHorizontal);
            }
            else
                return 0;

        }

        /// <summary>
        /// Jump value come from FiveSameModuleCheck. That number indicate next check location. 
        /// If FiveSameModuleCheck return 5, then jump value will larger than 5, it will include exceed same module numbers. 
        /// This method help recursive loop search rapidly during constant color switch modules. 
        /// </summary>
        private int FiveSameModuleSearch(BitMatrix matrix, MatrixPoint position, int indexJumpValue, bool isHorizontal)
        {
            MatrixPoint newPosition;
            if (isInsideMatrix(matrix.Size, position, indexJumpValue, isHorizontal))
            {
                newPosition = isHorizontal ? position.Offset(indexJumpValue, 0)
                    : position.Offset(0, indexJumpValue);
            }
            else
                return 0;
            return FiveSameModuleSearch(matrix, newPosition, isHorizontal);
        }

        /// <summary>
        /// Reverse check for the first module color switch point. 
        /// For reverse check. Last point is current point plus 4. Which include current position should be 5 modules total. 
        /// Use last module compare to first module's bool value to check with other module. 
        /// If same bool value, same module count plus plus. 
        /// At last we want to check if last value same as first value by check bool value we create at start. 
        /// Ex. for pattern o x x x o, it will return 1
        /// pattern o x o x o it will return 1
        /// pattern o x o o o it will return 3.
        /// </summary>
        private int FiveSameModuleCheck(BitMatrix matrix, MatrixPoint position, bool isHorizontal)
        {
            MatrixSize size = matrix.Size;
            MatrixPoint RightCheckPoint = isHorizontal ? position.Offset(4, 0)
                : position.Offset(0, 4);
            if (isOutsideMatrix(size, RightCheckPoint))
                return 0;

            bool compareLastToStart = matrix[position] == matrix[RightCheckPoint];

            int ModuleCount = 1;


            for (int i = 0; i < 3; i++)
            {
                RightCheckPoint = isHorizontal ? RightCheckPoint.Offset(-1, 0)
                    : RightCheckPoint.Offset(0, -1);
                bool compareCurrentToStart = matrix[RightCheckPoint] == matrix[position];
                if (compareCurrentToStart == compareLastToStart)
                    ModuleCount++;
                else
                    break;
            }


            ModuleCount = compareLastToStart ? ModuleCount + 1
                : ModuleCount;
            return ModuleCount;
        }

        /// <summary>
        /// Search for exceed number of modules. Use this method if FiveSameModuleCheck return 5. 
        /// </summary>
        /// <returns>Number of exceed same color modules</returns>
        private int SameModuleExceedNumberCheck(BitMatrix matrix, MatrixPoint position, bool isHorizontal)
        {
            
            MatrixSize size = matrix.Size;
            int ExceedNumber = 0;
            
            int indexJumpValue = 5;

            if (isOutsideMatrix(size, position, indexJumpValue, isHorizontal))
                return ExceedNumber;
			bool startPosition = matrix[position];
            do
            {
                MatrixPoint checkIndex;
                checkIndex = isHorizontal ? position.Offset(indexJumpValue, 0)
                       : position.Offset(0, indexJumpValue);

                if (startPosition == matrix[checkIndex])
                    ExceedNumber++;
                else
                    break;  //First non same module. Stop loop
                indexJumpValue++;
            }
            while (isInsideMatrix(size, position, indexJumpValue, isHorizontal));

            return ExceedNumber;
        }


        private bool isOutsideMatrix(MatrixSize size, MatrixPoint position, int indexJumpValue, bool isHorizontal)
        {
            return !isInsideMatrix(size, position, indexJumpValue, isHorizontal);
        }

        private bool isOutsideMatrix(MatrixSize size, MatrixPoint position)
        {
            return position.X >= size.Width || position.X < 0 || position.Y >= size.Height || position.Y < 0;
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
