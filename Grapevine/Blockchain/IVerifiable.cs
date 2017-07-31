namespace Grapevine.Blockchain
{
    public interface IVerifiable
    {
        bool IsValid(Ledger blockchain);
    }
}
