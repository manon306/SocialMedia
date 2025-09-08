public class Reply
{
    protected Reply() { }
    public int Id { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public string CreatedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }
    public string? UpdatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // navigation property
    public int ParentCommentID { get; private set; }
    public Comment ParentComment { get; private set; }

    // Methods
    public void Update(string updatedBy, string content)
    {
        if (!string.IsNullOrEmpty(content)) Content = content;
        UpdatedAt = DateTime.Now;
        UpdatedBy = updatedBy;
    }

    public void Delete(string deletedBy)
    {
        IsDeleted = true;
        DeletedAt = DateTime.Now;
        DeletedBy = deletedBy;
    }

    // Constructor
    public Reply(string content, int parentCommentId, string createdBy)
    {
        Content = content;
        ParentCommentID = parentCommentId;
        CreatedBy = createdBy;
    }
}
