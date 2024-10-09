namespace CourseManager.UI.Models
{
    public class PINConfirmationModel
    {
        public int CourseId { get; set; }
        public int? OtherId { get; set; }
        public bool IsDialogOk { get; set; }
        public string? InsertedPIN { get; set; }
        public string AfterDialogActionType { get; set; } = null!;

    }
}
