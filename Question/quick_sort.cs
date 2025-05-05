using System;
using System.Diagnostics;

namespace QuicksortLargeArray
{
    class Program
    {
        static void Main(string[] args)
        {
            const int ARRAY_SIZE = 1000;
            
            // Create an array with 1000 elements
            int[] array = new int[ARRAY_SIZE];
            
            // Fill array with random numbers
            Random random = new Random();
            for (int i = 0; i < ARRAY_SIZE; i++)
            {
                array[i] = random.Next(1, 10000); // Random numbers between 1 and 10000
            }
            
            Console.WriteLine($"Original array:");
            PrintArray(array, ARRAY_SIZE);      
            
            // Sort
            Quicksort(array, 0, array.Length - 1);
            
            Console.WriteLine($"\nSorted array:");
            PrintArray(array, ARRAY_SIZE);             
            // Verify array is sorted
            bool isSorted = IsSorted(array);
            Console.WriteLine($"\nArray is sorted: {isSorted}");

        }
        
        // Quicksort implementation
        static void Quicksort(int[] array, int low, int high)
        {
            if (low < high)
            {
                // Get partition index
                int partitionIndex = Partition(array, low, high);
                
                // Recursively sort elements before and after partition
                Quicksort(array, low, partitionIndex - 1);
                Quicksort(array, partitionIndex + 1, high);
            }
        }
        
        // Partition the array and return the partition index
        static int Partition(int[] array, int low, int high)
        {
            int pivot = array[high];
            int i = low - 1;
            
            for (int j = low; j < high; j++)
            {
                if (array[j] <= pivot)
                {
                    i++;
                    
                    // Swap elements
                    Swap(array, i, j);
                }
            }
            
            // Swap pivot element with element at i+1
            Swap(array, i + 1, high);
            
            // Return the partition index
            return i + 1;
        }
        
        // Improved pivot selection (median-of-three)
        // This can improve performance for large arrays
        static int MedianOfThree(int[] array, int low, int high)
        {
            int mid = low + (high - low) / 2;
            
            // Sort low, mid, high
            if (array[low] > array[mid])
                Swap(array, low, mid);
                
            if (array[low] > array[high])
                Swap(array, low, high);
                
            if (array[mid] > array[high])
                Swap(array, mid, high);
                
            // Place pivot at high-1
            Swap(array, mid, high - 1);
            return array[high - 1];
        }
        

        static void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        

        static void PrintArray(int[] array, int n)
        {
            n = Math.Min(n, array.Length);
            for (int i = 0; i < n; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine("...");
        }
        
        static bool IsSorted(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] > array[i + 1])
                    return false;
            }
            return true;
        }
    }
}