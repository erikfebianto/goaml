<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" ValidateRequest="true" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
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

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->
</head>
<body class="hold-transition login-page" onload="document.forms[0]['txtUsername'].focus();">
    <div class="login-box">


        <div class="login-logo">
        </div>
        <!-- /.login-logo -->
        <div class="login-box-body">
            <div style="text-align:center;color:red;">PERINGATAN!<br />
                Sistem ini hanya boleh diakses oleh pengguna yang berhak dan digunakan untuk kepentingan Nawa Data.
                Penyalahgunaan dapat dikategorikan sebagai pelanggaran dan/atau tindakan melawan hukum dan dapat berakibat terhadap sanksi
                sesuai dengan peraturan/ketentuan yang berlaku
                <br />
            </div>
            <div style="text-align: center">
                <img src="images/nawa323.jpg" style="max-width: 450px; max-height: 370px; width: 100%; height: 100%;" />
            </div>
            <br/>
            <form id="form1" runat="server" autocomplete="off">
                <ext:ResourceManager ID="ResourceManager1" runat="server" Theme="Crisp" />
                <div class="form-group has-feedback">
                    <%--   <input type="email" id="txtEmail" runat="server" class="form-control" placeholder="User ID" >--%>
                    <asp:TextBox ID="txtUsername" runat="server" class="form-control" placeholder="User ID"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="help-block" ControlToValidate="txtUsername" ErrorMessage="User ID is required"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group has-feedback">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control" placeholder="Password" onkeydown="if (event.keyCode == 13)
 {document.getElementById('btnLogin').click(); return false;}"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="help-block" ControlToValidate="txtPassword" ErrorMessage="Password is required"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage=""></asp:CustomValidator>
                    <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                </div>
                <div class="row" style="text-align: center">

                    <!-- /.col -->
                    <div class="col-xs-12">


                        <button type="button" id="btnLogin" runat="server" style="background-color: #82171d" class="btn btn-primary btn-block ">Sign In</button>
                    </div>
                    <!-- /.col -->
                </div>
                <div id="divMsg" style="display:none;">
    <img src="images/loading.gif" alt="Please wait.." />
</div>
            </form>


            <!-- /.social-auth-links -->



        </div>
        <!-- /.login-box-body -->
    </div>
    <!-- /.login-box -->

    <!-- jQuery 2.2.3 -->
    <script src="styles/plugins/jQuery/jquery-2.2.3.min.js"></script>
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

         $('#btnLogin').click(function(){
             $(this).attr('disabled', 'disabled');
             $(this).html("Please Wait....");
      //   $('#divMsg').show();
         //your client side validation here
            
         if(valid)
            return true;
         else
            {
              $(this).removeAttr('disabled');
              $('#divMsg').hide();     
              return false;
            }
     });
    </script>
</body>
</html>
