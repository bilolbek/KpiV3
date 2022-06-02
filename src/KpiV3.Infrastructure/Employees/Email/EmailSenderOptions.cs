namespace KpiV3.Infrastructure.Employees.Email;

public class EmailSenderOptions
{
    public string FromAddress { get; set; } = default!;
    public string FromAddressName { get; set; } = default!;
    public string SmtpServerAddress { get; set; } = default!;
    public int SmtpServerPort { get; set; }
    public bool SmtpServerSsl { get; set; }
    public string Password { get; set; } = default!;
}
