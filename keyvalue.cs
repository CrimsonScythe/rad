// from here
// https://stackoverflow.com/questions/18288363/need-advise-for-datastructure-like-tuple-or-keyvaluepair-but-mutable-type

namespace rad {
    public class MutableKeyValuePair<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }

    public MutableKeyValuePair(TKey key, TValue value) {
        this.Key = key;
        this.Value = value;
    }

}
}