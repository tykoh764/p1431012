<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bingTranslator.aspx.cs" Inherits="Task1PartB.bingTranslator" %>

<!DOCTYPE html>
<script src="https://code.jquery.com/jquery-1.10.2.js"></script>
<script>
    $(window).load(function () {
        $('#Button1').click(function (evt) {
            var inputText = $('#TextBox1').val();
            var authToken = $('#tokentxt').val();
            var data = {
                appId: 'Bearer ' + authToken,
                from: 'en',
                to: 'zh-CHS',
                contentType: 'text/plain',
                text: inputText
            };
            $.ajax({
                url: "http://api.microsofttranslator.com/V2/Ajax.svc/Translate",
                dataType: 'jsonp',
                data: data,
                jsonp: 'oncomplete'
            })
            .done(function (result, textStatus, errorThrown) {
                $('#lbl1').html("<span style='font-weight: bold'>Your Translation is: </span>"+result);
            })
            .fail(function (result, textStatus, errorThrown) {
                $('#lbl1').html("We are experiencing technical difficulties<br/><span style='color: red; font-style: italic'>Sorry for the inconvenience caused!</span>");
            });
        });
    });

</script>
<style>
        #tokentxt{
            display: none;
        }
</style>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <h1>My Translator</h1>
    
    </div>
        <p>
            Enter your text in English</p>
        <asp:TextBox ID="TextBox1" runat="server" Height="24px" Width="358px"></asp:TextBox>
        <asp:TextBox ID="tokentxt" runat="server"></asp:TextBox>
        <p>
            <input type = "button" ID="Button1" value="Translate" />
        </p>
        <label id = "lbl1"></label>
    </form>
</body>
</html>
