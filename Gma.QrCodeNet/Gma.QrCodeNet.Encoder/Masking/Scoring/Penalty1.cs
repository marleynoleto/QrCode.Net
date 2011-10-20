namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty1 : Penalty
    {

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

        
        private int SameModuleExceedNumberCheck(BitMatrix matrix, MatrixPoint position, bool isHorizontal)
        {
            
            MatrixSize size = matrix.Size;
            int ExceedNumber = 0;
            
            int indexJumpValue = 5;

            if (isOutsideMatrix(size, position, indexJumpValue, isHorizontal))
                return ExceedNumber;

            do
            {
                MatrixPoint checkIndex;
                checkIndex = isHorizontal ? position.Offset(indexJumpValue, 0)
                       : position.Offset(0, indexJumpValue);

                if (matrix[position] == matrix[checkIndex])
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
