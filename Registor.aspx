<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registor.aspx.cs" Inherits="DashBoard.Registor" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    <title>Dashboard</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.min.js"></script>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }

        h1 {
            text-align: center;
            margin-bottom: 20px;
        }

        .chart-container {
            display: flex;
            justify-content: center;
            align-items: center;
            margin: 20px 0;

        }

        #genderChart {
            width: 20rem;
            height: 20rem;
        }

        .statistics, .grid-section {
            margin: 20px 0;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            padding: 4px 2px 2px 2px;
            width:450px;
        }

        .statistics p {
            font-size: 1.2rem;
            text-align: center;
        }
        .statistics h3 {
                text-align: center;
                font-weight:700;
            }

         form {
            max-width: 900px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            background-color: #f9f9f9;
        }
         form h1 {
            margin-bottom: 20px;
            font-size: 2rem;
            color: #333;
        }

         form .statistics, form .chart-container, form .grid-section {
            margin: 20px 0;
        }

    </style>

</head>
         <body>
     <form id="form1" runat="server">
            <div>
                <h3>Search Registration</h3>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by Name or Email or Phone NO." CssClass="form-control" />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="SearchButton_Click" />
            </div>
            <div>
            <h1>Registration Dashboard</h1>
         <!-- Statistics Section -->
            <div class="statistics">
                <h3>Statistics</h3>
                <p>Total Users: <asp:Label ID="lblTotalUsers" runat="server" /></p>
                <p>Total Male: <asp:Label ID="lblTotalMale" runat="server" /></p>
                <p>Total Female: <asp:Label ID="lblTotalFemale" runat="server" /></p>
            </div>
         <!-- Chart Section -->
            <div class="chart-container">
                <canvas id="genderChart"></canvas>
            </div>

        <div class="grid-section">
            <h3>Registration Details</h3>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="StudentId" 
                  OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" 
                  OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting">
        <Columns>
            <%-- Bound fields for data --%>
           <asp:BoundField DataField="StudentId" HeaderText="Student ID" ReadOnly="True" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Gender" HeaderText="Gender" />

            <%-- Edit button --%>
            <asp:CommandField ShowEditButton="True" />

            <%-- Delete button --%>
            <asp:CommandField ShowDeleteButton="True" />
       </Columns>
        </asp:GridView>
        </div>
              
</div>
</form>
    <script>
        // Render Chart.js chart
        document.addEventListener('DOMContentLoaded', function () {
            const ctx = document.getElementById('genderChart').getContext('2d');
            const genderChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: ['Male', 'Female'],
                    datasets: [{
                        data: [maleRegistrations, femaleRegistrations],
                        backgroundColor: ['#3498db', '#e74c3c']
                    }]
                },
                options: {
                    responsive: false,
                    plugins: {
                        legend: {
                            position: 'top'
                        }
                    }
                }
            });
        });
    </script>
</body>
</html>