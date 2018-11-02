using System;

namespace SN_Maths_Eval
{
    class Program
        
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Startin!");
            MathsEvaluator mEval = new MathsEvaluator();
            System.Diagnostics.Debug.WriteLine(mEval.Evaluate("(1 + (12 * 2)"));
            Console.WriteLine("Hello world");
        }
    }
}
