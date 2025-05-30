// Vinyl 2025.

using System.Collections.Generic;

public static class CommonUtility
{
    public static T[] AddToArray<T>(this T[] Source, T Item)
    {
        T[] Result = new T[Source.Length + 1];
        
        for (int Idx = 0; Idx < Source.Length; ++Idx)
        {
            Result[Idx] = Source[Idx];
        }
        
        // ReSharper disable once UseIndexFromEndExpression
        Result[Result.Length - 1] = Item;
        
        return Result;
    }
    
    public static T[] RemoveFromArray<T>(this T[] Source, int Idx)
    {
        T[] Result = new T[Source.Length - 1];

        int SourceIdx = 0;
        int ResultIdx = 0;
        while (SourceIdx < Source.Length)
        {
            if (SourceIdx != Idx)
            {
                Result[ResultIdx] = Source[SourceIdx];
                
                ++ResultIdx;
            }
            
            ++SourceIdx;
        }

        return Result;
    }
    
    public static T[] RemoveFromArray<T>(this T[] Source, T Item)
    {
        T[] Result = new T[Source.Length - 1];

        int FoundIdx = 0;
        for (int Idx = 0; Idx < Source.Length; ++Idx)
        {
            if (!EqualityComparer<T>.Default.Equals(Source[Idx], Item))
            {
                continue;
            }

            FoundIdx = Idx;
            break;
        }
        
        RemoveFromArray(Source, FoundIdx);

        return Result;
    }

    public static T[] RemoveAllFromArray<T>(this T[] Source, T Item)
    {
        T[] Result = new T[Source.Length - 1];

        int SourceIdx = 0;
        int ResultIdx = 0;
        while (SourceIdx < Source.Length)
        {
            if (!EqualityComparer<T>.Default.Equals(Source[SourceIdx], Item))
            {
                Result[ResultIdx] = Source[SourceIdx];
                
                ++ResultIdx;
            }
            
            ++SourceIdx;
        }

        return Result;
    }
}
