namespace Application.Extensions
{
    public static class UngroupExtension
    {
        /// <summary>
        /// Returns a list of the same items ungrouped, or spread out, according
        /// to the result of the supplied function. E.g. {A, A, A, B, C} or
        /// {A, A, B, A, C} becomes something like {A, B, A, C, A}.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="items"></param>
        /// <param name="ungroupKey"></param>
        /// <returns></returns>
        public static IEnumerable<TSource>
            UngroupBy<TSource, TKey>(this IEnumerable<TSource> items,
                    Func<TSource, TKey> ungroupKey)
        {
            if (items.Count() < 3)
                return new List<TSource>(items);

            // basic algorithm: group items by supplied function for spreading out,
            // then build up a list by inserting the items from each group
            // into the list at the appropriate interval, starting with the
            // least populous group and ending with the most populous group.
            // so for {A, A, A, B, C}, we get {B}, then {B C}, then {A B A C A}
            return
                items.GroupBy(item => ungroupKey(item), item => item)
                .OrderBy(group => group.Count())
                .Aggregate(new List<TSource>(),
                    (list, group) =>
                    {
                        if (list.Count == 0 || group.Count() == 1)
                        {
                            list.AddRange(group);
                            return list;
                        }
                        // add 1 to interval to account for each insertion
                        double skip = (double)list.Count / (group.Count() - 1) + 1;
                        double exactIndex = 0.0;
                        int index;
                        foreach (TSource item in group)
                        {
                            index = (int)(Math.Round(exactIndex));
                            if (index > list.Count)
                                index = list.Count;
                            list.Insert(index, item);
                            exactIndex += skip;
                        }
                        return list;
                    });
        }
    }
}
