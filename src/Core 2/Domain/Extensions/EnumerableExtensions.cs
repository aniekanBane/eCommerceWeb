namespace eCommerceWeb;

public static class EnumerableExtensions
{
    private const string NullCollection = "Enumerable cannot be null or empty";
    private static readonly Random _rng = new();

    public static T RandomChoice<T>(this IEnumerable<T> source)
    {
        Guard.Against.NullOrEmpty(source, nameof(source), NullCollection);

        int sourceCount = source.Count();

        return sourceCount switch
        {
            < 2 => source.First(),
            _ => source.ElementAt(_rng.Next(sourceCount))
        };
    }

    public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> source, int subsetSize = 2)
    {
        Guard.Against.NullOrEmpty(source, nameof(source), NullCollection);
        Guard.Against.NegativeOrZero(subsetSize, nameof(subsetSize));;

        var sourceList = source.ToList();
        int sourceCount = sourceList.Count;

        if (subsetSize > sourceCount)
            throw new ArgumentOutOfRangeException(
                nameof(subsetSize), 
                $"Subset size ({subsetSize}) greater than collection count ({sourceCount})."
            );

        if (sourceCount < 3)
        {
            yield return sourceList.RandomChoice();
            yield break;
        }

        for(int i = 0; i < subsetSize; i++)
        {
            int index = _rng.Next(sourceCount);
            yield return sourceList[index];
            sourceList.RemoveAt(index);
            sourceCount--;
        }
    }
}
