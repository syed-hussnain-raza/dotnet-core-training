namespace MyAssignment.Helper
{
    /// <summary>
    /// Provides standardized HTML email templates for the application.
    /// </summary>
    public static class EmailTemplates
    {
        public static string GetConfirmationEmailTemplate(string confirmationLink)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                    <h2 style='color: #333;'>Welcome to MyAssignment!</h2>
                    <p style='color: #555;'>We are excited to have you on board. Please click the button below to confirm your email address and set your password:</p>
                    <div style='text-align: center; margin: 20px 0;'>
                        <a href='{confirmationLink}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Confirm Email & Set Password</a>
                    </div>
                    <p style='color: #555;'>Or copy and paste this link into your browser:</p>
                    <p style='word-break: break-all; color: #007bff;'><a href='{confirmationLink}'>{confirmationLink}</a></p>
                    <p style='color: #999; font-size: 12px; margin-top: 20px;'>If you did not request this, please ignore this email.</p>
                </div>";
        }

        public static string GetSetPasswordEmailTemplate(string setPasswordLink)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                    <h2 style='color: #333;'>Email Confirmed!</h2>
                    <p style='color: #555;'>Your email has been successfully confirmed. You can now set your new password by clicking the button below:</p>
                    <div style='text-align: center; margin: 20px 0;'>
                        <a href='{setPasswordLink}' style='background-color: #28a745; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; font-weight: bold;'>Set My Password</a>
                    </div>
                    <p style='color: #555;'>Or copy and paste this link into your browser:</p>
                    <p style='word-break: break-all; color: #28a745;'><a href='{setPasswordLink}'>{setPasswordLink}</a></p>
                    <p style='color: #999; font-size: 12px; margin-top: 20px;'>If you did not request this, please ignore this email.</p>
                </div>";
        }
    }
}
