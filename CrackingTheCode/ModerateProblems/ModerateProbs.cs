﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeepDiveTechnicals.CrackingTheCode
{
    
    public static class ModerateProbs
    {
        /// <summary>
        /// Problem : 16.1
        /// Description : Write a function to swap a number in place (that is, without temporary variables).
        /// </summary>
        public static Tuple<int,int> NumberSwapper(int a,int b)
        {
            //Approach to swap elements (in position) without temps
            // var a -> b and var b - > a
            // a = 3 , b = 5
            // a = 3 + 5 = 8
            // b = a - b = 8 - 5 = 3
            // a = a - b = 8 - 3 = 5

            a = a + b;
            b = a - b;
            a = a - b;

            return new Tuple<int, int>(a, b);
        }

        /// <summary>
        /// Problem : 16.2
        /// Description : Design a method to find the frequency of occurrences of any given word in a
        /// book.What if we were running this algorithm multiple times?
        /// </summary>
        /// Solution :
        /// Just do pre-processing with cost of extra time and memory, design a HashMap and the lookup
        /// will be executed in O(1). If you have to check this for a single time then you have to go 
        /// through every word of the text and this will cost O(n) time.
        /// 

        /// <summary>
        /// Problem : 16.6
        /// Description : Given two arrays of integers, compute the pair of values (one value in each
        /// array) with the smallest(non-negative) difference.Return the difference.
        /// EXAMPLE
        /// Input: {l, 3, 15, 11, 2}, {23, 127, 235, 19, 8}
        /// Output: 3. That is, the pair(11, 8).
        /// Explanation : Sort two int[] arrays and set pointer1 to arr1[0] and pointer2 to arr2[0].
        /// Start moving pointers alternatively if pointer1<pointer2 .
        /// increase pointer1 to decrease the difference and vice versa.
        /// Sorting O(ALogA+BloGB) , Searching O(A+B) A = arr1.Count and B = arr2.Count
        /// So Overall Runtime -> O(ALogA + BLogB)
        public static void SmallestDifference(List<int> l1, List<int> l2)
        {
            l1.Sort(); //nlogn n = l1.Count-1
            l2.Sort(); //mlogm m = l2.Count-1

            //1,2,3,11,15     8,19,23,127,235

            if (l1.Count==0 || l1==null || l2.Count==0 || l2== null)
            {
                Console.WriteLine("Invalid Input");
                return;
            }    

            int pointer1 = l1[0];
            int pointer2 = l2[0];

            int finalMinPointer1 = pointer1;
            int finalMinPointer2 = pointer2;
            
            int min = Math.Abs(pointer1-pointer2);

            int pos1 = 0;
            int pos2 = 0;

            while (pos1<l1.Count-1 && pos2<l2.Count-1)
            {
                if (pointer1<=pointer2)
                {
                    pos1++;
                    pointer1 = l1[pos1];
                }
                else
                {
                    pos2++;
                    pointer2 = l2[pos2];
                }
                int tempMin =  Math.Abs(pointer1 - pointer2);
                if (tempMin < min)
                { 
                    min = tempMin;
                    finalMinPointer1 = pointer1;
                    finalMinPointer2 = pointer2;
                }
                if (min == 0) break;
            }

            Console.WriteLine("The minimum pair is : " + min + $" and it's produced by ({finalMinPointer1},{finalMinPointer2})");
        }

        /// <summary>
        /// Problem : 16.10
        /// Description : Given a list of people with their birth and death years, implement a method to
        /// compute the year with the most number of people alive.You may assume that all people were born
        /// between 1900 and 2000 (inclusive). If a person was alive during any portion of that year, they should
        /// be included in that year's count. For example, Person (birth= 1908, death= 1909) is included in the
        /// counts for both 1908 and 1909.
        /// Time : O(nlogn) where n count of persons
        /// 

        public static int LivingPeople(List<Person> persons)
        {
            //birth: 01 10 10 12 13 20 23 75 83 90
            //death: 15 72 82 90 94 98 98 98 98 99
            
            var births = SortIt(persons, true);
            var deaths = SortIt(persons, false);

            int maxAlive = 0;
            int yearOccured = 0;
            int birthIndex = 0;
            int deathIndex = 0;
            int currentAlive = 0;

            while (birthIndex<births.Count)
            {
                if (births[birthIndex]<=deaths[deathIndex])
                {
                    currentAlive++;
                    if (currentAlive > maxAlive)
                    {
                        maxAlive = currentAlive;
                        yearOccured = births[birthIndex];
                    }
                    birthIndex++;
                }
                else if (births[birthIndex]>deaths[deathIndex])
                {
                    currentAlive--;
                    deathIndex++;
                }
            }
            return yearOccured;
        }

        public class Person 
        {
            public int birthDate;
            public int deathDate;
        }

        public static List<int> SortIt(List<Person> persons,bool births)
        {
            var list = (births) ? persons.OrderBy(it => it.birthDate)?.Select(it=>it.birthDate).ToList() : persons.OrderBy(it => it.deathDate)?.Select(it=>it.deathDate).ToList();
            return list;
        }

        /// <summary>
        /// Problem : 16.11
        /// Description : You are building a diving board by placing a bunch of planks of wood end-to-end.
        /// There are two types of planks, one of length shorter and one of length longer.You must use
        /// exactly K planks of wood.Write a method to generate all possible lengths for the diving board.
        /// Runtime O(totalPlanks^2)
        public static void DivingBoard(int totalPlanks, int shorter, int longer) 
        {
            var memoSet = SetPlanksMemo(totalPlanks, 0, shorter, longer, new HashSet<int>(), new Dictionary<Tuple<int, int>, HashSet<int>>());
            foreach (var item in memoSet)
            {
                Console.WriteLine("Available combinations with Memoiazation lead to length -> " + item);
            }

            var set = SetPlanks(totalPlanks, 0, shorter, longer, new HashSet<int>());
            foreach (var item in set)
            {
                Console.WriteLine("Available combinations lead to length -> " + item);
            }

            
        }

        public static HashSet<int> SetPlanks(int totalPlanks, int totalLength, int shorter, int longer, HashSet<int> lengths)
        {
            if (totalPlanks ==0)
            {
                lengths.Add(totalLength);
            }
            else
            {
                SetPlanks(totalPlanks - 1, totalLength + shorter, shorter, longer, lengths);
                SetPlanks(totalPlanks - 1, totalLength + longer, shorter, longer, lengths);
            }
            return lengths;
        }

        public static HashSet<int> SetPlanksMemo(int totalPlanks, int totalLength, int shorter, int longer, HashSet<int> lengths,
            Dictionary<Tuple<int, int>, HashSet<int>> complexHash)
        {
            
            if (complexHash.ContainsKey(new Tuple<int,int>(totalPlanks,totalLength)))
                return complexHash[new Tuple<int, int>(totalPlanks, totalLength)];
            if (totalPlanks == 0)
            {
                lengths.Add(totalLength);
                complexHash.Add(new Tuple<int, int>(totalPlanks, totalLength), lengths);
            }
            else
            {
                SetPlanksMemo(totalPlanks - 1, totalLength + shorter, shorter, longer, lengths, complexHash);
                SetPlanksMemo(totalPlanks - 1, totalLength + longer, shorter, longer, lengths, complexHash);
            }
            return lengths;
        }

        /// <summary>
        /// Problem : 16.15
        /// Description : The computer has four slots, and each slot will contain a ball that is red (R), yellow (Y), green (G) or
        /// blue(B). For example, the computer might have RGGB(Slot #1 is red, Slots #2 and #3 are green, Slot
        /// #4 is blue).
        /// You, the user, are trying to guess the solution. You might, for example, guess YRGB.
        /// When you guess the correct color for the correct slot, you get a "hit:' If you guess a color that exists
        /// but is in the wrong slot, you get a "pseudo-hit:' Note that a slot that is a hit can never count as a
        ///pseudo-hit.
        /// For example, if the actual solution is RGBY and you guess GGRR , you have one hit and one pseudohit
        /// Write a method that, given a guess and a solution, returns the number of hits and pseudo-hits
        /// </summary>
        public static void MasterMind(string guess)
        {
            //YRGB 2 pseudo-hit, 1 hit
            int position = 0;
            int pseudo = 0;
            int hit = 0;
            foreach (var ch in guess)
            {
                if (ComputersChoice.Contains(ch))
                {
                    bool flg = false;
                    int first = ComputersChoice.IndexOf(ch);
                    if (first == position) hit++;
                    else 
                    {
                        
                        while (first != -1)
                        {
                            first = ComputersChoice.IndexOf(ch, first + 1);
                            if (first == position) flg = true;
                        }
                    }
                    if (flg) hit++; else pseudo++;
                }
                position++;
            }
            Console.WriteLine("Hits : " + hit + " Pseudo-Hits : " + pseudo);
        }
        public static string ComputersChoice = "RGGB";

        /// <summary>
        /// Problem : 16.16
        /// Description : Given an array of integers, write a method to find indices m and n such that if you sorted
        /// elements m through n, the entire array would be sorted.Minimize n - m(that is, find the smallest
        /// such sequence).
        /// EXAMPLE
        /// Input: 1, 2, 4, 7, 10, 11, 7, 12, 6, 7, 16, 18, 19
        /// Output: (3, 9)
        /// TODO WITH BOOK APPROACH WHICH IS OPTIMIZED. THIS IS MY APPROACH USING RECURSION 
        /// </summary>
        /// 
        public static int MinIndex = -1;
        public static int MaxIndex = 100;
        public static List<int> GlobalLs = new List<int>();
        public static void SubSort()
        {
            List<int> ls = new List<int>
            { 1,2,4, 7, 10, 11, 7, 12, 6, 7, 16, 18, 19};
            foreach (var item in ls)
            {
                GlobalLs.Add(item);
            }
            GlobalLs.Sort();

            SubSortHelperMinIndex(ls, 0);
            SubSortHelperMaxIndex(ls, ls.Count - 1);
            Console.WriteLine($"Our Range is [{MinIndex},{MaxIndex})."+Environment.NewLine+$"Start at {MinIndex} and finish at {MaxIndex - 1}");
        }
        public static void SubSortHelperMinIndex(List<int> ls, int indexStart)
        {
            if (indexStart > ls.Count) return;
            List<int> temp = new List<int>();
            temp = ls.Skip(indexStart).ToList();
            temp.Sort();
            List<int> tempFinal = new List<int>();
            tempFinal.AddRange(ls.Take(indexStart));
            tempFinal.AddRange(temp);
            if (GlobalLs.SequenceEqual(tempFinal))
            {
                if (indexStart > MinIndex)
                    MinIndex = indexStart;
                indexStart++;
                SubSortHelperMinIndex(ls, indexStart);
            }
            else
                return;
        }

        public static void SubSortHelperMaxIndex(List<int> ls, int indexEnd)
        {
            if (indexEnd > ls.Count) return;
            List<int> temp = new List<int>();
            temp = ls.Take(indexEnd).ToList();
            temp.Sort();
            List<int> tempFinal = new List<int>();
            
            tempFinal.AddRange(temp);
            tempFinal.AddRange(ls.Skip(indexEnd));
            if (GlobalLs.SequenceEqual(tempFinal))
            {
                if (indexEnd < MaxIndex)
                    MaxIndex = indexEnd;
                indexEnd--;
                SubSortHelperMaxIndex(ls, indexEnd);
            }
            else
                return;
        }
        //TODO
        public static void SubSortHelperOptimized(List<int> ls)
        {
        ///left: 1, 2, 4, 7, 10, 11 -> max = 11
        ///middle: 8, 12
        ///right: 5, 6, 16, 18, 19 -> min = 5
        ///
        ///min(middle) > end(left)
        ///max(middle) < start(right)
        }

        /// <summary>
        /// Problem : 16.17
        /// Description : You are given an array of integers (both positive and negative). Find the
        /// contiguous sequence with the largest sum.Return the sum.
        /// EXAMPLE
        /// Input: 2, -8, 3, -2, 4, -10
        /// Output: 5 (i.e • , { 3, -2, 4} )
        /// </summary>
        public static void ContiguousSequence()
        {
            //2 3 -8 -1 2 4 -2 3
            int[] arr = new int[8] { 2, 3, -8, -1, 2, 4, -2, 3 };
            int sum = 0;
            int maxSum = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                sum += arr[i];
                if (sum > maxSum)
                    maxSum = sum;
                else if (sum < 0)
                    sum = 0;
            }
            Console.WriteLine($"The largest sum of the contiguous sequence is {maxSum}");
        }

        /// <summary>
        /// Problem : 16.18
        /// Description : You are given two strings, pattern and value. The pattern string consists of
        /// just the letters a and b, describing a pattern within a string. For example, the string catcatgocatgo
        /// matches the pattern aabab(where cat is a and go is b). It also matches patterns like a, ab, and b.
        /// Write a method to determine if value matches pattern.
        /// </summary>
        /// TODO
        public static void PatternMatching()
        {

        }

        /// <summary>
        /// Problem : 16.19
        /// Description : You have an integer matrix representing a plot of land, where the value at that location
        /// represents the height above sea level.A value of zero indicates water.A pond is a region of water
        /// connected vertically, horizontally, or diagonally. The size of the pond is the total number of
        /// connected water cells.Write a method to compute the sizes of all ponds in the matrix.
        /// EXAMPLE
        /// Input:
        /// 0 2 1 0
        /// 0 1 0 1
        /// 1 1 0 1
        /// 0 1 0 1
        /// Output: 2, 4, 1 (in any order)
        /// <Time>O(WH) where W width and H height</Time>
        /// Another way to compute this is to think about how many times each cell is "touched" by either call. Each cell
        /// will be touched once by the c omputePondSizes function.Additionally, a cell might be touched once by
        /// each of its adjacent cells.This is still a constant number of touches per cell. Therefore, the overall runtime is
        /// O(N2) on an NxN matrix or, more generally, O(WH).
        public static List<int> PondSizes()
        {
            int[,] land = new int[5, 4]
            {
                {0,2,1,0 },
                {0,1,0,1 },
                {1,1,0,1 }
                ,{0,1,0,1}
                ,{0,0,0,0}
            };
            int rowSize = land.GetLength(0);
            int colSize = land.GetLength(1);
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < colSize; j++)
                {
                    if (land[i, j] == 0)
                    {
                        int size = CalculatePondSize(i, j, land);
                        Ponds.Add(size);
                    }
                }
            }

            return Ponds;
        }

        public static List<int> Ponds = new List<int>();

        public static int CalculatePondSize(int row, int col, int[,] land)
        {
            int rowSize = land.GetLength(0)-1;
            int colSize = land.GetLength(1)-1;
            if (row > rowSize || row < 0 || col > colSize || col < 0 || land[row, col] != 0)
                return 0;

            land[row, col] = -1;
            int size = 1;
            for (int dr=-1;dr<=1;dr++)
            {
                for (int dc=-1;dc<=1;dc++)
                {
                    size += CalculatePondSize(row + dr, col + dc, land);
                }
            }
            
            return size;
        }

        /// <summary>
        /// Problem : 16.20
        /// Description : T9: On old cell phones, users typed on a numeric keypad and the phone would provide a list of words
        /// that matched these numbers.Each digit mapped to a set of O - 4 letters.Implement an algorithm
        /// to return a list of matching words, given a sequence of digits.You are provided a list of valid words
        /// (provided in whatever data structure you'd like). The mapping is shown in the diagram below:
        /// </summary>
        /// <Time>O(4^n) where n is the length of the string and n is the available letters for each number in the input</Time>
        //This is the mobile mapping for each number
        public static Dictionary<int, char[]> NumToWord = new Dictionary<int, char[]>
        {
            {8 , new char[3]{'t','u','v' } },{7, new char[4]{'p','q','r','s' } }, {3, new char[3]{'d','e','t'} }
        };
        //The list with the possible words
        public static List<string> availableWords = new List<string>();
        //The set with the valid words
        public static HashSet<string> validWords = new HashSet<string> { "tree", "used" };
        public static List<string> T9Mobile(string input)
        {
            
            char[] words = NumToWord[Convert.ToInt32(input[0].ToString())];
            for (var i=0;i<words.Length;i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(words[i]);
                //T9MobileHelper(input.Substring(1),sb,1);
                T9MobileHelper(input.ToCharArray(), sb, 1);
            }
            var itemsToReturn = availableWords.Where(word => validWords.Contains(word))?.ToList();
            return itemsToReturn;
        }

        /*public static void T9MobileHelper(string input, StringBuilder sb, int index)
        {
            if (index > input.Length-1)
            {
                availableWords.Add(sb.ToString());
                return;
            }
            
            char[] words = NumToWord[Convert.ToInt32(input[0].ToString())];
            for (var i = 0; i < words.Length; i++)
            {
                sb.Append(words[i]);
                T9MobileHelper(input.Substring(index), sb, index + 1);
                sb.Remove(sb.Length - 1, sb.Length - 1);
            }
        }*/
        public static void T9MobileHelper(char[] input, StringBuilder sb, int index)
        {
            if (index > input.Length - 1)
            {
                availableWords.Add(sb.ToString());
                return;
            }

            char[] words = NumToWord[Convert.ToInt32(input[index].ToString())];
            for (var i = 0; i < words.Length; i++)
            {
                sb.Append(words[i]);
                T9MobileHelper(input, sb, index + 1);
                string temp = sb.ToString();
                sb.Clear();
                sb.Append(temp.Substring(0, temp.Length - 1)); 
            }
        }
        /// <summary>
        /// Problem : 16.26
        /// Description : Given an array of integers, write a method to find indices m and n such that if you sorted
        /// elements m through n, the entire array would be sorted.Minimize n - m(that is, find the smallest
        /// such sequence).
        /// EXAMPLE
        /// Input: 1, 2, 4, 7, 10, 11, 7, 12, 6, 7, 16, 18, 19
        /// Output: (3, 9)
        /// </summary>
        public static HashSet<char> NewSet = new HashSet<char> { '*', '-', '+', '/' };
        public static int Calculator(string input)
        {
            if (input.Length == 0) return 0;

            // [2 * 3] +[ 5 / 6 * 3 ]- 15
            //2*3+5/6*3+15
            //15
            //3 +
            //6 *
            //5 /
            //3 +
            //2 *
            Stack<int> numbersStack = new Stack<int>();
            Stack<char> signStack = new Stack<char>();

            foreach(var ch in input)
            {
                if (NewSet.Contains(ch))
                    signStack.Push(ch);
                else
                    numbersStack.Push(Convert.ToInt32(ch));
            }
            
            int finalNumber = numbersStack.Pop();
            int tempNum;
            char tempSign;
            var sequentialFlag = false;
            for (var i=0;i<numbersStack.Count;i++)
            {
                tempNum = numbersStack.Pop();
                tempSign = signStack.Pop();
                if (tempSign == '+' && (signStack.Peek() == '+' || signStack.Peek() =='-'))
                    finalNumber += tempNum;
                else if (tempSign == '-' && (signStack.Peek() == '+' || signStack.Peek() == '-'))
                    finalNumber -= tempNum;
                else
                {
                    while (tempSign != '+' || tempSign != '-')
                    {
                        if (tempSign == '*')
                        {
                            tempNum = tempNum * numbersStack.Pop();
                        }
                        else // '/'
                        {
                            tempNum = tempNum / numbersStack.Pop();
                        }
                        tempSign = signStack.Pop();
                    }
                    //finalNumber +=
                }
            }
            return finalNumber;
        }
        
    }
}