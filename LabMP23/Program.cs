using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Diagnostics;
namespace LabMP23
{
    class MyThread
    {
        public Thread Thrd;
        int[] a;
        public int positionForSort;
        public MyThread(string name, int[] array)
        {
            a = array;
            Thrd = new Thread(this.Run);
            Thrd.Name = name;
            Thrd.Start();
        }
        void Run()
        {
            Console.WriteLine(Thrd.Name + "начат.");
            a = Program.BubbleSort(a);
            Console.WriteLine(Thrd.Name + "завершён.");
            // Program.Show(a);
        }
        public int[] ReturnSortArray()
        {
            return a;
        }
    }
    class Program
    {
        
        static void Main()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int countOfElements = 100000;
            int countOfThreads = 2;
            int[] mas;
            // mas = GenerateArray(countOfElements);//генерируем нехитрый массив
            mas = CunningGenerateArray(countOfElements);//генерируем хитрый массив
             // Show(mas);
            List<MyThread> listOfThread = new List<MyThread>();
            for (int i = 0; i < countOfThreads; i++)
            {
                int[] tempArray= new int[countOfElements / countOfThreads];
               // for (int j=0; j < countOfElements / countOfThreads; j++)   
                Array.Copy(mas, (countOfElements / countOfThreads) * i, tempArray, 0, countOfElements / countOfThreads);
                string name = "Thread" + i.ToString();
                MyThread temp = new MyThread(name,tempArray);
                listOfThread.Add(temp);
            }
            foreach (MyThread temp in listOfThread)
            {
                temp.Thrd.Join();
            }
            Console.WriteLine( "Потоки завершены.");
            List<int[]> arrays = new List<int[]>();
            foreach (MyThread temp in listOfThread)
            {
                int[] tempArray = new int[countOfElements / countOfThreads];  
                tempArray=temp.ReturnSortArray();
                arrays.Add(tempArray);
            }
            int[] answer= new int[countOfElements];
            answer=Program.MergeArrays(countOfThreads, arrays, countOfElements);
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Console.WriteLine(time);
            // Show(answer);
        }
        public static void Show(int[] array)//вывод массива
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }

        }
        public static int[] MergeArrays(int count,List<int[]> list,int COE)//слияние массивов
        {
            int[] answer= new int[COE];
            int[] positions = new int[count];
            int[] currentSorting= new int[count];
            
            int minIndex=-100;
            for (int i = 0; i < count; i++)
            {
                positions[i] = 0;
            }
            for (int i =0; i<COE;i++)
            {
                for (int j=0;j<count;j++)
                {
                    if (positions[j]<(COE/count))
                    currentSorting[j] = list[j][positions[j]];
                }
                int min = int.MaxValue;
                for (int j = 0; j < count; j++)
                {
                    if (positions[j] < (COE / count))
                    {


                        if (min > currentSorting[j])
                        {
                            min = currentSorting[j];
                            minIndex = j;
                        }
                    }
                }
                positions[minIndex]++;
                answer[i]=min;
    
            }
            return answer;  

            
            
        }
        
        static int[] GenerateArray(int k)//создание массива заданной длины
        {
            int[] array = new int[k];
            Random rand = new Random();
            for (int i = 0; i < array.Length; i++)
                array[i] = rand.Next(); // [0 - 2^31)
            return array;
        }
        static int[] CunningGenerateArray(int k)//создание хитрого массива заданной размерности
        {
            int[] array = new int[k];
            Random rand = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                int t = rand.Next(100);
                if (t > 10)
                {
                    array[i] = 1;
                }
                else
                {
                    array[i] = 1000;
                }

            }


            return array;
        }
        public static int[] BubbleSort(int[] array)//сортировка массива пузырьком
        {
            int temp;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[i] > array[j])
                    {
                        temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
            return array;
        }
    }
}
