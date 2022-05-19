namespace MeetMVC.Infrastructure
{
    public interface IUnitOfWork
    {
        void UploadImage(IFormFile file);
    }
}
