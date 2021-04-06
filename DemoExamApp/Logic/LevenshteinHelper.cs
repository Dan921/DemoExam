using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExamApp.Logic
{
    public class LevenshteinHelper
    {
        public bool IsLevenshteinDistanceLess1(string firstWord, string secondWord)
        {
            if (LevenshteinDistance(firstWord, secondWord) > 1)
            {
                return false;
            }
            return true;
        }

        public bool IsLevenshteinDistanceLess3(string firstWord, string secondWord)
        {
            if(LevenshteinDistance(firstWord, secondWord) > 3)
            {
                return false;
            }
            return true;
        }

        private int LevenshteinDistance(string firstWord, string secondWord)
        {
            if(firstWord == null)
            {
                firstWord = "";
            }
            if (secondWord == null)
            {
                secondWord = "";
            }

            var n = firstWord.Length + 1;
            var m = secondWord.Length + 1;
            var matrixD = new int[n, m];

            const int deletionCost = 1;
            const int insertionCost = 1;

            for (var i = 0; i < n; i++)
            {
                matrixD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                matrixD[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var substitutionCost = firstWord[i - 1] == secondWord[j - 1] ? 0 : 1;

                    matrixD[i, j] = Minimum(matrixD[i - 1, j] + deletionCost, 
                        matrixD[i, j - 1] + insertionCost, 
                        matrixD[i - 1, j - 1] + substitutionCost);
                }
            }

            return matrixD[n - 1, m - 1];
        }

        private int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;
    }
}
