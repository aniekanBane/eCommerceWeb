namespace SharedKernel.Extensions;

/// <summary>
/// Extension methods for IEnumerable collections.
/// </summary>
public static class EnumerableExtensions
{
    private static readonly ThreadLocal<Random> _rng = new(() => new Random());

    /// <summary>
    /// Returns a random element from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <returns>A random element from the collection.</returns>
    /// <exception cref="ArgumentException">When the collection is null or empty.</exception>
    public static T RandomChoice<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sourceList = source as IList<T> ?? [.. source];
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sourceList.Count);

        return sourceList.Count switch
        {
            1 => sourceList[0],
            _ => sourceList[_rng.Value!.Next(sourceList.Count)]
        };
    }

    /// <summary>
    /// Returns a random subset of elements from the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="subsetSize">The size of the subset to return.</param>
    /// <param name="allowDuplicates">Whether to allow duplicate elements in the result.</param>
    /// <returns>A random subset of elements.</returns>
    /// <exception cref="ArgumentException">When the collection is null or empty, or subsetSize is invalid.</exception>
    public static IEnumerable<T> RandomSubset<T>(
        this IEnumerable<T> source,
        int subsetSize,
        bool allowDuplicates = false)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(subsetSize);

        var sourceList = source as IList<T> ?? [.. source];
        if (!sourceList.Any())
        {
            throw new ArgumentException("Enumerable cannot be null or empty", nameof(source));
        }

        if (subsetSize > sourceList.Count && !allowDuplicates)
        {
            throw new ArgumentException(
                $"Subset size ({subsetSize}) cannot be greater than collection count ({sourceList.Count}) when duplicates are not allowed.",
                nameof(subsetSize));
        }

        return allowDuplicates 
            ? GetRandomSubsetWithDuplicates(sourceList, subsetSize)
            : GetRandomSubsetWithoutDuplicates(sourceList, subsetSize);
    }

    /// <summary>
    /// Chunks the collection into smaller collections of specified size.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <param name="chunkSize">The size of each chunk.</param>
    /// <returns>A collection of chunks.</returns>
    public static IEnumerable<IEnumerable<T>> Chunk<T>(
        this IEnumerable<T> source,
        int chunkSize)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(chunkSize);

        return ChunkIterator(source, chunkSize);
    }

    /// <summary>
    /// Determines whether the collection is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        => source is null || !source.Any();

    private static IEnumerable<T> GetRandomSubsetWithoutDuplicates<T>(IList<T> source, int subsetSize)
    {
        var indices = Enumerable.Range(0, source.Count).ToList();
        for (int i = 0; i < subsetSize; i++)
        {
            int index = _rng.Value!.Next(indices.Count);
            yield return source[indices[index]];
            indices.RemoveAt(index);
        }
    }

    private static IEnumerable<T> GetRandomSubsetWithDuplicates<T>(IList<T> source, int subsetSize)
    {
        for (int i = 0; i < subsetSize; i++)
        {
            yield return source[_rng.Value!.Next(source.Count)];
        }
    }

    private static IEnumerable<IEnumerable<T>> ChunkIterator<T>(
        IEnumerable<T> source,
        int chunkSize)
    {
        var chunk = new List<T>(chunkSize);
        foreach (var item in source)
        {
            chunk.Add(item);
            if (chunk.Count == chunkSize)
            {
                yield return chunk;
                chunk = new List<T>(chunkSize);
            }
        }

        if (chunk.Count > 0)
        {
            yield return chunk;
        }
    }
} 