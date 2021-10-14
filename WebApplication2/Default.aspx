<%@ Page Title="Underwriting Priority Redirect Request" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">  
    <br />  
    <div class="header">
        <img src="Images/Platinum-Logo-horizontal_color-no-NMLS.png" alt="logo" />
        <h3>Underwriting Priority Redirect Request</h3>
        <h4>Please select exactly 2 loans to swap</h4>
        <%--<h1>My website name</h1>--%>
    </div>
    <%--<h3>Underwriting Priority Redirect Request</h3>
    <h4>Please select exactly 2 loans to swap</h4>--%>  
    <asp:Label ID="errorLabel" runat="server" Text="Label" BorderStyle="Solid" BorderColor="Red" BackColor="Red" Visible="false"></asp:Label>
    <br />  
    <div class="row">  
        <div class="col-12 tabulator-example">  
            <%--<table class="table table-condensed table-striped table-hover table-sm tabulator_table">--%>  
                <asp:gridview runat="server" ID="Gv1" AutoGenerateColumns="true" HeaderStyle-BackColor="Red"   
    BorderWidth="5" BorderColor="Blue" class="table tabulator_table">  
   </asp:gridview> 
            <%--</table>--%>  
            <asp:Label ID="lblComment" runat="server" Text="Comment"></asp:Label>
            <div>
                <textarea rows="4" cols="50" name="comment" id="txtComment"></textarea>
            </div>

            <asp:Button ID="btnAjax" runat="server" OnClientClick="callAjaxMethod(event)" Text="Send Request" />
        </div>
    </div> 
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
    <script id="scriptTablePrereqDateTimeSort" src="https://unpkg.com/moment/min/moment.min.js" type="text/javascript"></script>
    <script type="text/javascript">  
        var MyTable = {};

        MyTable = new Tabulator(".tabulator_table", {
            columns: [
                //{ formatter: "responsiveCollapse", width: 30, minWidth: 30, align: "center", resizable: false, headerSort: false },
                //{
                //    formatter: "rowSelection", titleFormatter: "rowSelection", hozAlign: "center", headerSort: false, responsive: 0, cellClick: function (e, cell) {
                //        cell.getRow().toggleSelect();
                //    }
                //},
                {
                    formatter: "rowSelection", titleFormatter: "rowSelection", hozAlign: "center", headerSort: false, cellClick: function (e, cell) {
                        cell.getRow().toggleSelect();
                    }
                },
            ],
            //height: "70vh",
            //height: "50vh",
            height: "411px",
            //responsiveLayout: "hide",
            //responsiveLayout: "collapse",
            layout: "fitDataFill", //need for responsive collapse
            //layout: "fitDataStretch",
            //layout: "fitData",
            pagination: "local",
            paginationSize: 25,
            paginationSizeSelector: [10, 20, 30, 50, 100]
        });
        MyTable.hideColumn("loanofficeremail");
        MyTable.hideColumn("loanprocessoremail");
        MyTable.hideColumn("loapaemail");
        MyTable.deleteColumn("loannumber");
        MyTable.addColumn({ title: "Loan Number", field: "loannumber" });
        MyTable.deleteColumn("borrowerlastname");
        MyTable.addColumn({ title: "Borrower Last Name", field: "borrowerlastname" });
        MyTable.deleteColumn("loanprogram");
        MyTable.addColumn({ title: "Loan Program", field: "loanprogram" });
        MyTable.deleteColumn("loantype");
        MyTable.addColumn({ title: "Loan Type", field: "loantype" });
        MyTable.deleteColumn("underwriter");
        MyTable.addColumn({ title: "Underwriter", field: "underwriter" });
        MyTable.deleteColumn("loanprocessor");
        MyTable.addColumn({ title: "Loan Processor", field: "loanprocessor" });
        MyTable.deleteColumn("loanofficer");
        MyTable.addColumn({ title: "Loan Officer", field: "loanofficer" });
        MyTable.deleteColumn("currentmilestone");
        MyTable.addColumn({ title: "Current Milestone", field: "currentmilestone" });
        MyTable.deleteColumn("datesubmitted");
        MyTable.addColumn({ title: "Date Submitted", field: "datesubmitted", sorter: "datetime", sorterParams: { format: "MM/DD/YYYY hh:mm:ssA" }, });
        MyTable.deleteColumn("estimatedclosingdt");
        MyTable.addColumn({title: "Estimated Closing Date", field: "estimatedclosingdt", formatter: "datetime", formatterParams: {
                inputFormat: "MM/DD/YYYY",
                outputFormat: "M/D/YYYY",
                invalidPlaceholder: "(invalid date)",
            },
        });
        MyTable.deleteRow(0);

        function callAjaxMethod(e) {

            //To prevent postback from happening as we are ASP.Net TextBox control

            //If we had used input html element, there is no need to use ' e.preventDefault()' as posback will not happen

            e.preventDefault();
            var tabulatorRows = MyTable.getSelectedRows();
            
            var loanArray = [];
            for (var i = 0; i < tabulatorRows.length; i++) {
                loanArray.push(tabulatorRows[i].getData())
            }

            if (MyTable.getSelectedRows().length != 2) {
                alert('Please select exactly 2 loans to swap.');
                return;
            }

            var commentText = $("#txtComment").val();
            loanArray[0].comment = commentText;
            loanArray[1].comment = commentText;

            $.ajax({
                type: "POST",
                url: "Default.aspx/UWPriorityRedirect",
                data: JSON.stringify({ loans: loanArray }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        alert('Request email has been sent.');
                        //$('input:checkbox:first').prop('checked', false)
                    }
                    else {
                        alert('Request email was not sent.  Please try again.');
                    }
                    },
                failure: function (response) {
                    <%--$('#<%=txtIsLeapYear.ClientID%>').val("Error in calling Ajax:" + response.d);--%>
                    alert('Request email was not sent.  Please try again.');
                }
                });
                }

        //$(document).ready(function () {  
            //var table = new Tabulator(".tabulator_table", {  


            //MyTable = new Tabulator(".tabulator_table", { 
            //    columns: [
            //    {
            //        formatter: "rowSelection", titleFormatter: "rowSelection", hozAlign: "center", headerSort: false, cellClick: function (e, cell) {
            //            cell.getRow().toggleSelect();
            //        }
            //        },

            //        {
            //            title: "EstimatedClosingDt", field: "estimatedclosingdt", formatter: "datetime", formatterParams: {
            //                //inputFormat: "YYYY-MM-DD",
            //                inputFormat: "MM/DD/YYYY",
            //                outputFormat: "M/D/YYYY",
            //                invalidPlaceholder: "(invalid date)",
            //            },
            //        },

            //        {
            //            title: "DateSubmitted", field: "datesubmitted", sorter: "datetime", sorterParams: {format: "MM/DD/YYYY hh:mm:ssA"},
            //        },

            //        //{ title: "Date Of Birth", field: "estimatedclosingdt", sorter: "date", align: "center" },

            //        ],
            //    height: "70vh",  
            //    layout:"fitDataStretch",  
            //    pagination: "local",  
            //    paginationSize: 25,  
            //    paginationSizeSelector: [10, 20, 30, 50, 100]
            //});
            //MyTable.deleteRow(0);
            //MyTable.moveColumn("datesubmitted", "currentmilestone", true);
            //MyTable.moveColumn("estimatedclosingdt", "datesubmitted", true);
        //});  
    </script>
</asp:Content>
