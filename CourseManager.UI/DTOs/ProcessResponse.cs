namespace CourseManager.UI.DTOs
{
    public class ProcessResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; } = null!;
    }

    public class ProcessResponse<TData> : ProcessResponse
    {
        public TData? Data { get; set; }
    }
}
