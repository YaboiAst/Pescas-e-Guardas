public interface IMet
{
    bool Met();
    string MetMessage { get; }
    string NotMetMessage { get; }
}