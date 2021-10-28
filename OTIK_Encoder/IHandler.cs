namespace OTIK_Encoder
{
    internal interface IHandler
    {
        void SetNextHandler(IHandler nextHandler);
        void Handle(ref FileHandlingStruct handlingStruct);
    }
}