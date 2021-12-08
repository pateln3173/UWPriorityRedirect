using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Database;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
//using System.


namespace WebApplication2
{
    [Serializable]
    public partial class _Default : Page
    {
        private Repository repository = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.repository = new Repository();
                GetTableRow();
            }
        }


        /// <summary>  
        /// This method will return the prepare html table body  
        /// </summary>  
        /// <returns>string html table body</returns>  
        public void GetTableRow()
        {
            DataTable dataTable = new DataTable();

            try
            {
                // Get the data from the database repository  
                dataTable = this.repository.GetLoans();
                
                Gv1.DataSource = dataTable;
                Gv1.DataBind();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                errorLabel.Text = "Some Technical Error occurred.  Please visit at a later time.";
                errorLabel.Visible = true;
            }
            finally
            {
                dataTable = null;
            }
        }

        [WebMethod]
        public static bool UWPriorityRedirect(List<UnderwritingPriority> loans)
        {
            //try
            //{
            string to = ConfigurationManager.AppSettings["testEmail"].ToString() == "Y" ? 
                ConfigurationManager.AppSettings["recipientTestEmail"].ToString() : 
                ConfigurationManager.AppSettings["recipientEmail"].ToString();

                string from = ConfigurationManager.AppSettings["fromEmail"].ToString();

                MailMessage message = new MailMessage(from, to);
                message.Subject = "Underwriting Priority Redirect Request";
                message.Body = "<p>Please review the following request to swap loans.</p>";

                var sortedLoans = loans.OrderByDescending(o => o.DateSubmitted).ToList();

                message.Body += "<table border=" + 1 + " cellpadding=" + 1 + " cellspacing=" + 1 + " width = " + 450 + "><tr><td style='text-align:center'>Loan #1</td><td></td><td style='text-align:center'>Loan #2</td></tr><tr><td>" + sortedLoans[0].LoanNumber + "</td><td style='text-align:center'>Loan Number</td><td>" + sortedLoans[1].LoanNumber + "</td></tr>"
                                + "<tr><td>" + sortedLoans[0].BorrowerLastName + "</td><td style='text-align:center'>Borrower Name</td><td>" + sortedLoans[1].BorrowerLastName + "</td></tr>"
                                + "<tr><td>" + sortedLoans[0].LoanProgram + "</td><td style='text-align:center'>Loan Program</td><td>" + sortedLoans[1].LoanProgram + "</td></tr>";
                if (sortedLoans[0].Underwriter == sortedLoans[1].Underwriter)
                {
                    message.Body += "<tr>";
                }
                else
                {
                    message.Body += "<tr style='background-color:red'>";
                }

                message.Body += "<td>" + sortedLoans[0].Underwriter + "</td><td style='text-align:center'>Underwriter</td><td>" + sortedLoans[1].Underwriter + "</td></tr>"
                                + "<tr><td>" + sortedLoans[0].LoanProcessor + "</td><td style='text-align:center'>Loan Processor</td><td>" + sortedLoans[1].LoanProcessor + "</td></tr>"
                                + "<tr><td>" + sortedLoans[0].LoanOfficer + "</td><td style='text-align:center'>Loan Officer</td><td>" + sortedLoans[1].LoanOfficer + "</td></tr>";
                if (sortedLoans[0].CurrentMilestone == sortedLoans[1].CurrentMilestone)
                {
                    message.Body += "<tr>";
                }
                else
                {
                    message.Body += "<tr style='background-color:red'>";
                }
                message.Body += "<td>" + sortedLoans[0].CurrentMilestone + "</td><td style='text-align:center'>Current Milestone</td><td>" + sortedLoans[1].CurrentMilestone + "</td></tr>"
                                + "<tr><td>" + sortedLoans[0].DateSubmitted + "</td><td style='text-align:center'>Date (Re-)Submitted</td><td>" + sortedLoans[1].DateSubmitted + "</td></tr>"
                                + "<tr><td>" + sortedLoans[0].EstimatedClosingDt.ToString("M/d/yyyy") + "</td><td style='text-align:center'>Estimated Closing Date</td><td>" + sortedLoans[1].EstimatedClosingDt.ToString("M/d/yyyy") + "</td></tr>"
                                + "</table>";
                message.Body += "<p>Optional Comment: " + sortedLoans[0].Comment + "</p>";

            var currentName = HttpContext.Current.User.Identity.Name;
            var context = new PrincipalContext(ContextType.Domain);
            UserPrincipal principal = UserPrincipal.FindByIdentity(context, currentName);
            string displayName = principal.DisplayName;
            var emailAddress = principal.EmailAddress;

            message.Body += "<p>Submitted By: " + displayName + " (" + currentName + ")</p>";
            message.IsBodyHtml = true;
            message.ReplyToList.Add(emailAddress);

            if (ConfigurationManager.AppSettings["testEmail"].ToString() != "Y")
            {
                message.CC.Add(ConfigurationManager.AppSettings["recipientCC"].ToString());
                message.Bcc.Add(ConfigurationManager.AppSettings["recipientBCC"].ToString());
                foreach (var item in sortedLoans)
                {
                    message.CC.Add(item.LoanOfficerEmail);
                    message.CC.Add(item.LoanProcessorEmail);
                    if (item.LOAPAEmail != "" && item.LOAPAEmail != "&nbsp;")
                    {
                        message.CC.Add(item.LOAPAEmail);
                    }
                }
            }
            else
            {
                message.CC.Add(ConfigurationManager.AppSettings["recipientTestCC"].ToString());
            }

                string smtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
                SmtpClient client = new SmtpClient(smtpServer);
                client.UseDefaultCredentials = true;

                try
                {
                    client.Send(message);
                    //ExceptionLogging.SendInfoToText("", sortedLoans[0].LoanNumber, sortedLoans[1].LoanNumber);
                    ExceptionLogging.SendInfoToText(displayName, sortedLoans[0].LoanNumber, sortedLoans[1].LoanNumber);
                    return true;
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendErrorToText(ex);
                    return false;
                }
            //}

            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendErrorToText(ex);
            //    return false;
            //}
        }
    }
}