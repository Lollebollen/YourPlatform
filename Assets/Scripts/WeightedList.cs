public class WeightedList<type>
{
    public int count;
    public int max;

    type[] array;
    int[] weights;
    int[] CDF; // comulative density function, ..i weights added to i

    public WeightedList() { }
    public WeightedList(type[] array, int[] weights)
    {
        if (array.Length != weights.Length) { throw new ("Lenghts of arrays must match"); }
        this.array = array;
        this.weights = weights;
        count = array.Length - 1;
        CalculateCDF();
    }

    public void Add(type element, int weight)
    {
        int num = array.Length;
        type[] newArray = new type[num];
        int[] newIntArray = new int[num];
        newIntArray[num - 1] = weight;
        newArray[num - 1] = element;
        for (int i = 0; i < num; i++)
        {
            array[i] = newArray[i];
            newIntArray[i] = weights[i];
        }
        count = num;
        CalculateCDF();
    }

    public void Sort()
    {
        QuickSort(ref weights, ref array, 0, weights.Length - 1);
        CalculateCDF();
    }

    public type Get(int weight)
    {
        weight++;
        int low = 0;
        int high = array.Length - 1;
        int i = 0;
        int value;

        while (low <= high)
        {
            i = low + (high - low) / 2;
            value = CDF[i];
            if (value == weight) { return array[i]; }
            if (value > weight) { high = i - 1; }
            else { low = i + 1; }
        }
        i = low + (high - low) / 2;
        if (i > array.Length - 1) { throw new ("Weight out of bounds"); } 
        else { return array[i]; }
    }

    private void CalculateCDF()
    {
        int num = weights.Length;
        int tailingWeight = 0;
        CDF = new int[num];
        for (int i = 0; i < num; i++)
        {
            CDF[i] = weights[i] + tailingWeight;
            tailingWeight += weights[i];
        }
        max = CDF[num - 1];
    }

    private void QuickSort(ref int[] arr, ref type[] arr2, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partision(ref arr, ref arr2, low, high);
            QuickSort(ref arr, ref arr2, low, pivotIndex);
            QuickSort(ref arr, ref arr2, pivotIndex + 1, high);
        }
    }

    private int Partision(ref int[] arr, ref type[] arr2, int low, int high)
    {
        int i = low - 1;
        int j = high + 1;
        int pivot = arr[low];
        int temp;
        type temp2;

        while (true)
        {
            do { i++; }
            while (arr[i] < pivot);
            do { j--; }
            while (arr[j] > pivot);

            if (j <= i) { return j; }

            temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;

            temp2 = arr2[i];
            arr2[i] = arr2[j];
            arr2[j] = temp2;
        }
    }
}