namespace CRUD_WebAPI_ViaCEP.Service
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
