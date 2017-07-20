namespace Grapevine.Blockchain
{
    interface IVerifiable
    {
        bool IsValid(Ledger blockchain);
    }
}
