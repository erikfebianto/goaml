<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Logout.aspx.vb" Inherits="Logout" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Log in</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.6 -->
    <link rel="stylesheet" href="styles/bootstrap/css/bootstrap.min.css">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="styles/font-awesome-4.7.0/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="styles/ionicons-2.0.1/css/ionicons.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="styles/dist/css/AdminLTE.min.css">
    <!-- iCheck -->
    <link rel="stylesheet" href="styles/plugins/iCheck/square/blue.css">

</head>
<body class="hold-transition login-page">
    <div class="login-box">


        <div class="login-logo">
        </div>
        <!-- /.login-logo -->
        <div class="login-box-body">
            <div style="text-align: center">
                <img src="images/nawa323.jpg" style="max-width: 450px; max-height: 270px; width: 100%; height: 100%;" />
            </div>
            </br>
            </br>
            
            <form id="form1" runat="server">
                <div style="text-align: center">
                <asp:HyperLink ID="HyperLink1" runat="server" Href="Login.aspx" HrefTarget="_self">You have been successfully signed out</asp:HyperLink>
                    </div>
            </form>


            <!-- /.social-auth-links -->



        </div>
        <!-- /.login-box-body -->
    </div>
    <!-- /.login-box -->

    <!-- jQuery 2.2.3 -->
    <script src="styles/plugins/jQuery/jquery-3.2.1.min.js"></script>
    <!-- Bootstrap 3.3.6 -->
    <script src="styles/bootstrap/js/bootstrap.min.js"></script>
    <!-- iCheck -->
    <script src="styles/plugins/iCheck/icheck.min.js"></script>
    <script>
        $(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue',
                increaseArea: '20%' // optional
            });
        });
    </script>
</body>
</html>
